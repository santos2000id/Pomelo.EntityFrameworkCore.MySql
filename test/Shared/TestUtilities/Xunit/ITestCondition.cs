// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    /// <summary>
    /// A test condition attached to a test class or test method. When <see cref="IsMetAsync"/> returns false, the test is
    /// reported as skipped with <see cref="SkipReason"/>.
    /// </summary>
    public interface ITestCondition
    {
        ValueTask<bool> IsMetAsync();

        string SkipReason { get; }
    }
}
