using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using MySqlConnector;
using Xunit;

namespace Pomelo.EntityFrameworkCore.MySql.UpgradeTests;

public class MySqlIntegrationTests
{
    [Fact]
    public async Task Create_query_update_and_delete_against_mysql()
    {
        var connectionString = Environment.GetEnvironmentVariable("POMELO_TEST_CONNECTION");
        Assert.SkipWhen(string.IsNullOrWhiteSpace(connectionString), "Set POMELO_TEST_CONNECTION to run MySQL integration tests.");

        var cancellationToken = TestContext.Current.CancellationToken;
        var builder = new MySqlConnectionStringBuilder(connectionString) { Database = string.Empty };
        var database = "pomelo_ef11_smoke_" + Guid.NewGuid().ToString("N");
        await using var admin = new MySqlConnection(builder.ConnectionString);
        await admin.OpenAsync(cancellationToken);
        await using (var command = new MySqlCommand($"CREATE DATABASE `{database}`", admin))
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        try
        {
            builder.Database = database;
            var options = new DbContextOptionsBuilder<QueryCompatibilityTests.TestContext>()
                .UseMySql(builder.ConnectionString, ServerVersion.AutoDetect(builder.ConnectionString),
                    mysql => mysql.EnableIndexOptimizedBooleanColumns())
                .Options;
            await using var context = new QueryCompatibilityTests.TestContext(options);
            await context.Database.MigrateAsync(cancellationToken);
            Assert.Contains("20261111000000_CreateItems", await context.Database.GetAppliedMigrationsAsync(cancellationToken));
            context.Items.AddRange(
                new QueryCompatibilityTests.Item { Name = "a%b", Enabled = true },
                new QueryCompatibilityTests.Item { Name = "axb", Enabled = true },
                new QueryCompatibilityTests.Item { Name = "abc", Enabled = false });
            await context.SaveChangesAsync(cancellationToken);

            var prefix = "a%";
            var matches = await context.Items.Where(item => item.Enabled && item.Name.StartsWith(prefix))
                .Select(item => item.Name).ToListAsync(cancellationToken);
            Assert.Equal(new[] { "a%b" }, matches);

            prefix = "ax";
            matches = await context.Items.Where(item => item.Enabled && item.Name.StartsWith(prefix))
                .Select(item => item.Name).ToListAsync(cancellationToken);
            Assert.Equal(new[] { "axb" }, matches);

            var compiledQuery = EF.CompileQuery((QueryCompatibilityTests.TestContext db, string value)
                => db.Items.Where(item => item.Enabled && item.Name.StartsWith(value)).Select(item => item.Name));
            Assert.Equal(new[] { "a%b" }, compiledQuery(context, "a%"));
            Assert.Equal(new[] { "axb" }, compiledQuery(context, "ax"));

            var projected = await context.Items.OrderBy(item => item.Id)
                .Select(item => new { item.Name, item.Enabled, Label = item.Enabled ? "yes" : "no" })
                .ToListAsync(cancellationToken);
            Assert.Equal(new[] { "yes", "yes", "no" }, projected.Select(item => item.Label));
            Assert.Equal(new[] { true, true, false }, projected.Select(item => item.Enabled));

            var groups = await context.Items.GroupBy(item => item.Enabled)
                .Select(group => new { Enabled = group.Key, Count = group.Count() })
                .ToListAsync(cancellationToken);
            Assert.Equal(2, groups.Single(group => group.Enabled).Count);
            Assert.Equal(1, groups.Single(group => !group.Enabled).Count);

            Assert.Equal(1, await context.Items.Where(item => !item.Enabled)
                .ExecuteUpdateAsync(setters => setters.SetProperty(item => item.Name, "updated"), cancellationToken));
            Assert.Equal(1, await context.Items.Where(item => !item.Enabled).ExecuteDeleteAsync(cancellationToken));

            await context.GetService<IMigrator>().MigrateAsync("0", cancellationToken);
            Assert.Empty(await context.Database.GetAppliedMigrationsAsync(cancellationToken));
        }
        finally
        {
            await using var command = new MySqlCommand($"DROP DATABASE `{database}`", admin);
            command.ExecuteNonQuery();
        }
    }
}

[DbContext(typeof(QueryCompatibilityTests.TestContext))]
[Migration("20261111000000_CreateItems")]
public class CreateItemsMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Items",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Enabled = table.Column<bool>(nullable: false),
                Name = table.Column<string>(nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_Items", item => item.Id));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable("Items");
}
