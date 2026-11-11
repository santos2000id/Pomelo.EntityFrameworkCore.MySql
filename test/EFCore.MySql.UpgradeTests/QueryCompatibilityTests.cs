using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;
using Xunit;

namespace Pomelo.EntityFrameworkCore.MySql.UpgradeTests;

public class QueryCompatibilityTests
{
    [Fact]
    public void Model_and_create_script_are_generated()
    {
        using var context = CreateContext();

        var script = context.Database.GenerateCreateScript();

        Assert.Contains("CREATE TABLE `Items`", script);
        Assert.Contains("`Enabled`", script);
    }

    [Fact]
    public void Migration_history_create_script_uses_provider_model()
    {
        using var context = CreateContext();

        var script = context.GetService<IHistoryRepository>().GetCreateScript();

        Assert.Contains("CREATE TABLE `__EFMigrationsHistory`", script);
        Assert.Contains("`MigrationId`", script);
        Assert.Contains("`ProductVersion`", script);
    }

    [Fact]
    public void Boolean_predicates_are_optimized_for_indexes()
    {
        using var context = CreateContext();

        var enabled = context.Items.Where(item => item.Enabled).ToQueryString();
        var disabled = context.Items.Where(item => !item.Enabled).ToQueryString();

        Assert.Contains("`i`.`Enabled` = TRUE", enabled);
        Assert.Contains("`i`.`Enabled` = FALSE", disabled);

        var nested = context.Items.Where(item => item.Enabled && item.Name != "").ToQueryString();
        Assert.Contains("`i`.`Enabled` = TRUE", nested);
    }

    [Fact]
    public void Captured_string_pattern_is_escaped_for_like()
    {
        using var context = CreateContext();
        var pattern = "a%b_c";

        var sql = context.Items.Where(item => item.Name.StartsWith(pattern)).ToQueryString();

        Assert.Contains("LIKE", sql);
        Assert.Contains("a\\\\%b\\\\_c%", sql);
    }

    [Fact]
    public void Parameterized_primitive_collection_translates_to_json_table_when_enabled()
    {
        var options = new DbContextOptionsBuilder<TestContext>()
            .UseMySql("Server=localhost;Database=test;User ID=test", new MySqlServerVersion(new Version(8, 0, 40)),
                mysql => mysql.EnablePrimitiveCollectionsSupport()
                    .UseParameterizedCollectionMode(ParameterTranslationMode.Parameter))
            .Options;
        using var context = new TestContext(options);
        var ids = new[] { 1, 2 };

        var sql = context.Items.Where(item => ids.Contains(item.Id)).ToQueryString();

        Assert.Contains("JSON_TABLE", sql);
        Assert.Contains("[1,2]", sql);
    }

    [Fact]
    public void Json_table_translates_string_collections_with_boolean_optimization()
    {
        var options = new DbContextOptionsBuilder<TestContext>()
            .UseMySql("Server=localhost;Database=test;User ID=test", new MySqlServerVersion(new Version(8, 0, 40)),
                mysql => mysql.EnablePrimitiveCollectionsSupport()
                    .EnableIndexOptimizedBooleanColumns()
                    .UseParameterizedCollectionMode(ParameterTranslationMode.Parameter))
            .Options;
        using var context = new TestContext(options);
        var names = new[] { "a", "b" };

        var sql = context.Items.Where(item => item.Enabled && names.Contains(item.Name)).ToQueryString();

        Assert.Contains("JSON_TABLE", sql);
        Assert.Contains("`i`.`Enabled` = TRUE", sql);
    }

    [Fact]
    public void Json_table_parameter_uses_invariant_name_and_disables_sql_caching()
    {
        using var context = CreateContext();
        var factory = context.GetService<ISqlExpressionFactory>();
        var mapping = context.GetService<IRelationalTypeMappingSource>().FindMapping(typeof(string));
        var parameter = new SqlParameterExpression("queryValue", "sql_name", typeof(string), true, null, mapping);
        var jsonTable = new MySqlJsonTableExpression("j", parameter);
        var parameters = new ParametersCacheDecorator(new Dictionary<string, object> { ["queryValue"] = "[1,2]" });
        var visitor = new MySqlParameterInliningExpressionVisitor(
            context.GetService<IRelationalTypeMappingSource>(), factory, context.GetService<IMySqlOptions>());

        var result = (MySqlJsonTableExpression)visitor.Process(jsonTable, parameters);

        Assert.NotSame(jsonTable.JsonExpression, result.JsonExpression);
        Assert.False(parameters.CanCache);
    }

    [Fact]
    public void Parameterized_collections_default_to_constants()
    {
        using var context = CreateContext();

        var extension = context.GetService<IDbContextOptions>().FindExtension<Infrastructure.Internal.MySqlOptionsExtension>();

        Assert.Equal(ParameterTranslationMode.Constant, extension.ParameterizedCollectionMode);
    }

    [Fact]
    public void Complex_type_collection_is_mapped_to_json_column()
    {
        var options = new DbContextOptionsBuilder<ComplexCollectionContext>()
            .UseMySql("Server=localhost;Database=test;User ID=test", new MySqlServerVersion(new Version(8, 0, 40)))
            .Options;
        using var context = new ComplexCollectionContext(options);

        var script = context.Database.GenerateCreateScript();

        Assert.Contains("`Tags` json", script);
    }

    private static TestContext CreateContext()
        => new(new DbContextOptionsBuilder<TestContext>()
            .UseMySql("Server=localhost;Database=test;User ID=test", new MySqlServerVersion(new Version(8, 0, 40)),
                options => options.EnableIndexOptimizedBooleanColumns())
            .Options);

    public class TestContext(DbContextOptions<TestContext> options) : DbContext(options)
    {
        public DbSet<Item> Items => Set<Item>();
    }

    public class Item
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
    }

    public class ComplexCollectionContext(DbContextOptions<ComplexCollectionContext> options) : DbContext(options)
    {
        public DbSet<Holder> Holders => Set<Holder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Holder>().ComplexCollection(h => h.Tags, b => b.ToJson());
    }

    public class Holder
    {
        public int Id { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }

    public class Tag
    {
        public string Label { get; set; }
    }
}
