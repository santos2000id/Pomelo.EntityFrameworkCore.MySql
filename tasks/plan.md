# Implementation Plan: EF Core 11 / .NET 11

## Overview
Upgrade this fork's mainline from EF Core 9 / .NET 8-9 to EF Core 11 / .NET 11, including provider code compatibility, test/build infrastructure, and user-facing compatibility metadata.

## Decisions
- Retarget the existing single framework to `net11.0`; do not add a multi-target matrix in this change.
- Keep all EF Core packages on the 11.0 line and align .NET/ASP.NET packages and the `dotnet-ef` tool with .NET 11.
- Upgrade MySqlConnector to a patched release because baseline restore currently fails: 2.4.0 triggers GHSA-473q-m89c-ghf8; release 2.6.1 fixes it (use latest 2.6.2).
- Keep provider behavior unchanged except for compatibility fixes required by EF Core 11; don't publish a release or open a PR to upstream.

## Task List

### Phase 1: SDK, target frameworks, and dependencies
- [ ] Move SDK, library/test TFMs, EF Core package range, EF tool, and aligned package versions to 11.
- [ ] Restore successfully without NuGet audit errors.
- [ ] Build all projects and capture compile failures from EF Core API changes.

### Phase 2: EF Core 11 compatibility
- [ ] Fix compile/runtime incompatibilities in provider code.
- [ ] Add or update regression tests where behavior changed.
- [ ] Run focused unit and functional tests.

### Phase 3: CI and compatibility information
- [ ] Update CI-related framework references and package/version metadata.
- [ ] Update README compatibility/getting-started information for EF Core 11 / .NET 11.
- [ ] Run Debug and Release builds plus all locally runnable tests; document any environment-only gaps.

## Risks
- EF Core 11 may have API changes that require behavior-sensitive provider work; use the official EF Core 11 migration documentation and tests.
- Integration tests depend on MySQL/MariaDB; local database availability may limit verification.
- NuGet restore treats warnings as errors, and the baseline MySqlConnector version is known vulnerable.

## References
- https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-11.0/whatsnew
- https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-11.0/breaking-changes
- https://github.com/advisories/GHSA-473q-m89c-ghf8
