// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Pomelo.EntityFrameworkCore.MySql.Tests.TestUtilities.Attributes;
using Xunit.Sdk;
using Xunit.v3;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit
{
    /// <summary>
    /// Enables discovery-time filtering of whole test classes, e.g. with <see cref="SupportedServerVersionConditionAttribute"/>.
    /// </summary>
    public class MySqlXunitTestFramework : XunitTestFramework
    {
        private readonly string _configFileName;

        public MySqlXunitTestFramework()
            : this(null)
        {
        }

        public MySqlXunitTestFramework(string configFileName)
            : base(configFileName)
            => _configFileName = configFileName;

        protected override ITestFrameworkDiscoverer CreateDiscoverer(Assembly assembly)
        {
            var testAssembly = new XunitTestAssembly(assembly, _configFileName);

            return new MySqlXunitTestFrameworkDiscoverer(testAssembly, new CollectionPerAssemblyTestCollectionFactory(testAssembly));
        }
    }

    public class MySqlXunitTestFrameworkDiscoverer : XunitTestFrameworkDiscoverer
    {
        public MySqlXunitTestFrameworkDiscoverer(IXunitTestAssembly testAssembly, IXunitTestCollectionFactory collectionFactory)
            : base(testAssembly, collectionFactory)
        {
        }

        protected override ValueTask<bool> FindTestsForType(
            IXunitTestClass testClass,
            ITestFrameworkDiscoveryOptions discoveryOptions,
            Func<ITestCase, ValueTask<bool>> discoveryCallback)
        {
            // Filter whole classes whose conditions are not met, instead of reporting each of their tests as skipped.
            if (!IsValidTestClassType(testClass.Class))
            {
                return new ValueTask<bool>(true);
            }

            return base.FindTestsForType(testClass, discoveryOptions, discoveryCallback);
        }

        public static bool IsValidTestClassType(Type type)
            => IsTestConditionMet<SupportedServerVersionConditionAttribute>(type) &&
               IsTestConditionMet<SupportedServerVersionLessThanConditionAttribute>(type);

        private static bool IsTestConditionMet<TType>(Type type) where TType : ITestCondition
            => type == null ||
               type.GetCustomAttributes(typeof(TType), true)
                   .Cast<ITestCondition>()
                   .All(condition => condition.IsMetAsync().AsTask().GetAwaiter().GetResult());
    }
}
