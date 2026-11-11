// Ported from dotnet/efcore v9.0.0 (EFCore.Specification.Tests_BulkUpdates_ComplexTypeBulkUpdatesFixtureBase.cs). The test base was removed upstream in EF Core 11.
// Kept so the MySQL test classes that derive from it keep their test coverage.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;
﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.BulkUpdates;

#nullable disable

public abstract class ComplexTypeBulkUpdatesFixtureBase : ComplexTypeQueryFixtureBase
{
    protected override string StoreName
        => "ComplexTypeBulkUpdatesTest";

    public abstract override void UseTransaction(DatabaseFacade facade, IDbContextTransaction transaction);
}
