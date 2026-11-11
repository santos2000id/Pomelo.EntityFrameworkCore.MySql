using Microsoft.EntityFrameworkCore.BulkUpdates.Inheritance;
using Microsoft.EntityFrameworkCore.Query.Inheritance;
using Xunit;
﻿using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.Query;

public class TPHInheritanceQueryMySqlTest : TPHInheritanceQueryTestBase<TPHInheritanceQueryMySqlFixture>
{
    public TPHInheritanceQueryMySqlTest(TPHInheritanceQueryMySqlFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }
}
