// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;
using Xunit.v3;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit
{
    public class MySqlTestCollectionOrderer : ITestCollectionOrderer
    {
        public IReadOnlyCollection<TTestCollection> OrderTestCollections<TTestCollection>(IReadOnlyCollection<TTestCollection> testCollections)
            where TTestCollection : ITestCollection
            => testCollections
                .OrderBy(c => c.TestCollectionDisplayName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(c => c.TestCollectionDisplayName, StringComparer.Ordinal)
                .ThenBy(c => c.UniqueID)
                .ToArray();
    }
}
