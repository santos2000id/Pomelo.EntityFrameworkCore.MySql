// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit.Sdk;
using Xunit.v3;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit
{
    public class MySqlTestCaseOrderer : ITestCaseOrderer, ITestClassOrderer
    {
#if SPECIFIC_TEST_ORDER
        private static readonly bool _isSpecificTestCaseOrderingEnabled = true;
#else
        private static readonly bool _isSpecificTestCaseOrderingEnabled = false;
#endif

        private static string[] _specificTestCaseDisplayNamesInOrder;

        public static string[] SpecificTestCaseDisplayNamesInOrder
            => _specificTestCaseDisplayNamesInOrder ??= _isSpecificTestCaseOrderingEnabled &&
                                                        Path.GetFullPath(@"..\..\..\TestResults\SpecificTestOrder.txt") is var path &&
                                                        File.Exists(path)
                ? File.ReadLines(path)
                    .Select(s => Regex.Match(s, @"^(?:\W*)([^\u200B]+)").Groups[1].Value)
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToArray()
                : [];

        private readonly Dictionary<string, int> _specificTestCaseDisplayNamesWithIndex;

        public MySqlTestCaseOrderer()
        {
            _specificTestCaseDisplayNamesWithIndex = SpecificTestCaseDisplayNamesInOrder
                .Select((s, i) => (s, i))
                .ToDictionary(t => t.s, t => t.i);
        }

        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
            where TTestCase : ITestCase
            => testCases
                .OrderBy(c => _specificTestCaseDisplayNamesWithIndex.GetValueOrDefault(c.TestCaseDisplayName, int.MaxValue))
                .ThenBy(c => c.TestCaseDisplayName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(c => c.TestCaseDisplayName, StringComparer.Ordinal)
                .ThenBy(c => c.UniqueID)
                .ToArray();

        public IReadOnlyCollection<TTestClass> OrderTestClasses<TTestClass>(IReadOnlyCollection<TTestClass> testClasses)
            where TTestClass : ITestClass
            => testClasses.OrderBy(c => c.TestClassName, StringComparer.Ordinal).ToArray();
    }
}
