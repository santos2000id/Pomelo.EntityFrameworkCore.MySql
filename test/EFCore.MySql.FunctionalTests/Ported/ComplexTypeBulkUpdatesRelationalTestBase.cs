// Ported from dotnet/efcore v9.0.0 (EFCore.Relational.Specification.Tests_BulkUpdates_ComplexTypeBulkUpdatesRelationalTestBase.cs). The test base was removed upstream in EF Core 11.
// Kept so the MySQL test classes that derive from it keep their test coverage.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.BulkUpdates;

#nullable disable

public abstract class ComplexTypeBulkUpdatesRelationalTestBase<TFixture> : ComplexTypeBulkUpdatesTestBase<TFixture>
    where TFixture : ComplexTypeBulkUpdatesRelationalFixtureBase, new()
{
    protected ComplexTypeBulkUpdatesRelationalTestBase(TFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture)
    {
        ClearLog();
        Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
    }

    public override Task Delete_complex_type(bool async)
        => AssertTranslationFailed(
            RelationalStrings.ExecuteDeleteOnNonEntityType,
            () => base.Delete_complex_type(async));

    public override Task Update_projected_complex_type_via_OrderBy_Skip(bool async)
        => AssertTranslationFailed(
            RelationalStrings.ExecuteUpdateSubqueryNotSupportedOverComplexTypes("Customer.ShippingAddress#Address"),
            () => base.Update_projected_complex_type_via_OrderBy_Skip(async));

    protected static async Task AssertTranslationFailed(string details, Func<Task> query)
        => Assert.Contains(
            "' could not be translated. Additional information: " + details + " See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.",
            (await Assert.ThrowsAsync<InvalidOperationException>(query)).Message);

    private void ClearLog()
        => Fixture.TestSqlLoggerFactory.Clear();
}
