using Xunit;
﻿using Microsoft.EntityFrameworkCore.Query;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.Query
{
    public class ManyToManyQueryMySqlTest : ManyToManyQueryRelationalTestBase<ManyToManyQueryMySqlFixture>
    {
        public ManyToManyQueryMySqlTest(ManyToManyQueryMySqlFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
        }
    }
}
