// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Pomelo.EntityFrameworkCore.MySql.Tests;
using Xunit.Sdk;
using Xunit.v3;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities.Xunit
{
    /// <summary>
    /// Test case for <see cref="ConditionalFactAttribute"/> and <see cref="ConditionalTheoryAttribute"/> data rows.
    /// Evaluates <see cref="ITestCondition"/> attributes on the test class and method before running, and reports
    /// failures caused by features that the target server does not support as skipped (see <see cref="ShouldSkipFailedTest"/>).
    /// </summary>
    public sealed class MySqlConditionalTestCase : XunitTestCase, ISelfExecutingXunitTestCase
    {
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public MySqlConditionalTestCase()
        {
        }

        public MySqlConditionalTestCase(XunitTestCase source)
            : base(
                source.TestMethod,
                source.TestCaseDisplayName,
                source.UniqueID,
                source.Explicit,
                source.TestLabel,
                source.DisableParallelization,
                source.SkipExceptions,
                source.SkipReason,
                source.SkipType,
                source.SkipUnless,
                source.SkipWhen,
                source.Traits,
                source.TestMethodArguments,
                source.SourceFilePath,
                source.SourceLineNumber,
                source.Timeout)
        {
        }

        /// <summary>
        /// Wraps plain test cases produced by the base discoverers. Other test case types are returned unchanged.
        /// </summary>
        public static IXunitTestCase Wrap(IXunitTestCase testCase)
            => testCase is XunitTestCase plain && plain.GetType() == typeof(XunitTestCase)
                ? new MySqlConditionalTestCase(plain)
                : testCase;

        public async ValueTask<RunSummary> Run(
            ExplicitOption explicitOption,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource,
            ParallelMode parallelMode,
            ExecutionScheduler scheduler,
            FixtureMappingManager methodFixtureMappings)
        {
            var skipReason = GetUnmetConditionSkipReason() ?? SkipReason;
            var tests = await CreateTests();

            var failureFilter = new SkipFailedTestMessageBus(messageBus);
            var summary = await XunitTestCaseRunner.Instance.Run(
                this,
                tests,
                failureFilter,
                aggregator,
                cancellationTokenSource,
                parallelMode,
                scheduler,
                TestCaseDisplayName,
                skipReason,
                explicitOption,
                constructorArguments,
                methodFixtureMappings);

            summary.Failed -= failureFilter.ConvertedFailures;
            summary.Skipped += failureFilter.ConvertedFailures;

            return summary;
        }

        private string GetUnmetConditionSkipReason()
        {
            var conditions = new List<ITestCondition>();

            conditions.AddRange(TestClass.Class.GetCustomAttributes(true).OfType<ITestCondition>());
            conditions.AddRange(TestMethod.Method.GetCustomAttributes(true).OfType<ITestCondition>());

            foreach (var condition in conditions)
            {
                if (!condition.IsMetAsync().AsTask().GetAwaiter().GetResult())
                {
                    return condition.SkipReason;
                }
            }

            return null;
        }

        /// <summary>
        /// Mark failed tests as 'Skipped', if they failed because they use an expression that is not supported by the underlying database
        /// server version.
        /// </summary>
        public static bool ShouldSkipFailedTest(IReadOnlyList<string> exceptionTypes, IReadOnlyList<string> exceptionMessages)
        {
            var supports = AppConfig.ServerVersion.Supports;
            var skip = true;
            var count = 0;

            for (var i = 0; i < exceptionTypes.Count; i++)
            {
                if (exceptionTypes[i] == typeof(AggregateException).FullName)
                {
                    continue;
                }

                count++;
                var message = exceptionMessages[i] ?? string.Empty;

                if (!skip || exceptionTypes[i] != typeof(InvalidOperationException).FullName)
                {
                    return false;
                }

                if (message.StartsWith("The LINQ expression '", StringComparison.Ordinal) ||
                    message.Contains("' could not be translated.", StringComparison.Ordinal))
                {
                    skip &= !supports.OuterApply && message.Contains("OUTER APPLY") ||
                            !supports.CrossApply && message.Contains("CROSS APPLY") ||
                            !supports.WindowFunctions && message.Contains("ROW_NUMBER() OVER") ||
                            !supports.ExceptIntercept &&
                            (message.Contains("EXCEPT") || message.Contains("INTERSECT")) ||
                            !supports.JsonTable && (message.Contains("JSON_TABLE") ||
                                                    message.Contains("JsonTable")) ||
                            message.Contains("Primitive collections support has not been enabled.") ||
                            message.Contains("MySqlBipolarExpression");
                }
                else
                {
                    skip &= (!supports.JsonTable ||
                             !supports.JsonTableImplementationWithAggregate) && (message.Contains("JSON_TABLE") ||
                                                                                 message.Contains("JsonTable"));
                }
            }

            return count > 0 && skip;
        }

        /// <summary>
        /// Converts <see cref="ITestFailed"/> messages into <see cref="TestSkipped"/> messages when
        /// <see cref="ShouldSkipFailedTest"/> considers the failure expected. Disposing this wrapper does not dispose
        /// the inner message bus, because the caller owns it.
        /// </summary>
        private sealed class SkipFailedTestMessageBus : IMessageBus
        {
            private readonly IMessageBus _inner;

            public SkipFailedTestMessageBus(IMessageBus inner)
                => _inner = inner;

            public int ConvertedFailures { get; private set; }

            public bool QueueMessage(IMessageSinkMessage message)
            {
                if (message is ITestFailed failed &&
                    ShouldSkipFailedTest(failed.ExceptionTypes.Select(t => t ?? string.Empty).ToArray(), failed.Messages))
                {
                    ConvertedFailures++;

                    return _inner.QueueMessage(new TestSkipped
                    {
                        AssemblyUniqueID = failed.AssemblyUniqueID,
                        ExecutionTime = failed.ExecutionTime,
                        FinishTime = failed.FinishTime,
                        Output = failed.Output,
                        Reason = failed.Messages.FirstOrDefault() ?? string.Empty,
                        TestCaseUniqueID = failed.TestCaseUniqueID,
                        TestClassUniqueID = failed.TestClassUniqueID,
                        TestCollectionUniqueID = failed.TestCollectionUniqueID,
                        TestMethodUniqueID = failed.TestMethodUniqueID,
                        TestUniqueID = failed.TestUniqueID,
                        Warnings = failed.Warnings,
                    });
                }

                return _inner.QueueMessage(message);
            }

            public void Dispose()
            {
            }
        }
    }
}
