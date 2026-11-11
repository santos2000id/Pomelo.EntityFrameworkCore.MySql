using Microsoft.EntityFrameworkCore.BulkUpdates.Inheritance;
using Microsoft.EntityFrameworkCore.Query.Inheritance;
﻿namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.Query;

public class TPCFiltersInheritanceQueryMySqlFixture : TPCInheritanceQueryMySqlFixture
{
    public override bool EnableFilters
        => true;
}
