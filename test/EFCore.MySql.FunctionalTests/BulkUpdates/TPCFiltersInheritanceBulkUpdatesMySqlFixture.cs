using Microsoft.EntityFrameworkCore.BulkUpdates.Inheritance;
using Microsoft.EntityFrameworkCore.Query.Inheritance;
﻿namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.BulkUpdates;

public class TPCFiltersInheritanceBulkUpdatesMySqlFixture : TPCInheritanceBulkUpdatesMySqlFixture
{
    protected override string StoreName
        => "TPCFiltersInheritanceBulkUpdatesTest";

    public override bool EnableFilters
        => true;
}
