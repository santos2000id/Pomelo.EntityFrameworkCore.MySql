// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.v3;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    /// <summary>
    /// A fact whose <see cref="ITestCondition"/> attributes are evaluated before the test runs. Unmet conditions are reported as skipped.
    /// </summary>
    [XunitTestCaseDiscoverer(typeof(Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit.MySqlConditionalFactDiscoverer))]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConditionalFactAttribute : FactAttribute
    {
        public ConditionalFactAttribute(
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = -1)
            : base(sourceFilePath, sourceLineNumber)
        {
        }
    }

    /// <summary>
    /// A theory whose <see cref="ITestCondition"/> attributes are evaluated before the test runs. Unmet conditions are reported as skipped.
    /// </summary>
    [XunitTestCaseDiscoverer(typeof(Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit.MySqlConditionalTheoryDiscoverer))]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConditionalTheoryAttribute : TheoryAttribute
    {
        public ConditionalTheoryAttribute(
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = -1)
            : base(sourceFilePath, sourceLineNumber)
        {
        }
    }
}
