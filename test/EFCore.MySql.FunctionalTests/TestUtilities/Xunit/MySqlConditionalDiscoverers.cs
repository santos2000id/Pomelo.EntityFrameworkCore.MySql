// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit
{
    public class MySqlConditionalFactDiscoverer : FactDiscoverer
    {
        protected override IXunitTestCase CreateTestCase(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            IXunitTestMethod testMethod,
            IFactAttribute factAttribute)
            => MySqlConditionalTestCase.Wrap(base.CreateTestCase(discoveryOptions, testMethod, factAttribute));
    }

    public class MySqlConditionalTheoryDiscoverer : TheoryDiscoverer
    {
        protected override ValueTask<IReadOnlyCollection<IXunitTestCase>> CreateTestCasesForTheory(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            IXunitTestMethod testMethod,
            ITheoryAttribute theoryAttribute)
            => WrapAsync(base.CreateTestCasesForTheory(discoveryOptions, testMethod, theoryAttribute));

        protected override ValueTask<IReadOnlyCollection<IXunitTestCase>> CreateTestCasesForDataRow(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            IXunitTestMethod testMethod,
            ITheoryAttribute theoryAttribute,
            ITheoryDataRow dataRow,
            object[] testMethodArguments,
            string index)
            => WrapAsync(base.CreateTestCasesForDataRow(discoveryOptions, testMethod, theoryAttribute, dataRow, testMethodArguments, index));

        private static async ValueTask<IReadOnlyCollection<IXunitTestCase>> WrapAsync(ValueTask<IReadOnlyCollection<IXunitTestCase>> testCases)
        {
            var result = await testCases;
            var wrapped = new List<IXunitTestCase>(result.Count);

            foreach (var testCase in result)
            {
                wrapped.Add(MySqlConditionalTestCase.Wrap(testCase));
            }

            return wrapped;
        }
    }
}
