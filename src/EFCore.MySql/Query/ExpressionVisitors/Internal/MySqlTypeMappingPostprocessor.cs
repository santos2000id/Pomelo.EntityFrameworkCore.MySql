// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;

namespace Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;

public class MySqlTypeMappingPostprocessor : RelationalTypeMappingPostprocessor
{
    public MySqlTypeMappingPostprocessor(
        QueryTranslationPostprocessorDependencies dependencies,
        RelationalQueryTranslationPostprocessorDependencies relationalDependencies,
        RelationalQueryCompilationContext queryCompilationContext)
        : base(dependencies, relationalDependencies, queryCompilationContext)
    {
    }

    protected override Expression VisitExtension(Expression expression)
    {
        if (expression is MySqlJsonTableExpression { JsonExpression: SqlParameterExpression { TypeMapping: null } parameter } jsonTable)
        {
            var mappingSource = RelationalDependencies.TypeMappingSource;
            var collectionMapping = TryGetInferredTypeMapping(jsonTable.Alias, "value", out var elementMapping)
                ? mappingSource.FindMapping(parameter.Type, QueryCompilationContext.Model, elementMapping)
                : mappingSource.FindMapping(parameter.Type, QueryCompilationContext.Model);

            if (collectionMapping?.ElementTypeMapping is not RelationalTypeMapping mappedElement)
            {
                throw new InvalidOperationException($"Unable to determine a collection type mapping for '{parameter.Name}'.");
            }

            var columns = new[]
            {
                new MySqlJsonTableExpression.ColumnInfo
                {
                    Name = "value",
                    TypeMapping = mappedElement,
                    Path = new[] { new PathSegment(RelationalDependencies.SqlExpressionFactory.Constant(0, mappingSource.FindMapping(typeof(int)))) }
                }
            };

            return jsonTable.Update(parameter.ApplyTypeMapping(collectionMapping), jsonTable.Path, columns);
        }

        return base.VisitExtension(expression);
    }
}
