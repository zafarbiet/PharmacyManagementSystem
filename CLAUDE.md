# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Status

This project is in the **requirements and design phase**. Documentation lives in `docs/Pharmacy Management/`. No source code exists yet — this file describes the intended architecture based on the team's established template.

## Project Overview

A pharmacy management system for Indian pharmaceutical retail/distribution businesses covering inventory, invoicing, purchase orders, quotations, vendor relationships, and expired drug handling.

## Business Domain

**Core modules (from `docs/Pharmacy Management/`):**

- **Inventory Management** — stock tracking, reorder levels, batch/lot management
- **Invoice Management** — billing, GST calculations, profit margins
- **Purchase Order Flow** — PO creation, vendor communication, receiving
- **Quotation Management** — quote generation and tracking
- **Expired Drug Management** — expiry tracking, return/disposal workflows
- **User Roles** — role-based access control

**Indian pharmacy specifics:**
- Drug inventory tracked by **batch number + expiry date**, not just SKU
- GST (CGST/SGST/IGST) is a first-class concern on all invoices
- Profit margin uses MRP-based calculation — see `docs/Pharmacy Management/Calculation Methods/ProfitMargin.xlsx`
- Vendor bill format reference: `docs/Pharmacy Management/Vendor Bills/`

---

## Solution Architecture

This project follows the same layered architecture as KamaruddinTraders.

### Layer Structure

```
Common (Domain)  →  Server (Business Logic)  →  Data.SqlServer (Data Access)  →  Host (API)
```

| Layer | Project Suffix | Purpose |
|-------|---------------|---------|
| Domain | `.Common` | Entities, exceptions, filter models |
| Business Logic | `.Server` | Actions, repositories, interfaces |
| Data Access | `.Server.Data.SqlServer` | Dapper SQL implementations, migrations |
| API | `.Server.Host` | Minimal APIs, DI wiring, startup |
| Tests | `.Server.Unit` | Unit tests (MSTest + NSubstitute + FluentAssertions) |

### Database Schema

All tables use schema `PMS` (Pharmacy Management System). All tables include: `Id` (Guid PK), `UpdatedAt` (DateTimeOffset), `UpdatedBy` (string), `IsActive` (bool). Soft deletes only — never `DELETE`, set `IsActive = false`.

---

## Technology Stack

- **.NET 9.0** — `net9.0`
- **C# 12+** — primary constructors, collection expressions, `required`, nullable reference types
- **ASP.NET Core Minimal APIs**
- **Dapper** — SQL Server queries
- **FluentMigrator** — migrations (timestamp version: `yyyyMMddHHmmss`)
- **Microsoft.Data.SqlClient**
- **Serilog** — structured logging (Serilog.AspNetCore, Serilog.Sinks.Async)
- **MSTest v3** + **NSubstitute** + **FluentAssertions** + **Coverlet**

---

## Commands

```bash
# Build
dotnet build

# Run API
dotnet run --project src/PharmacyManagementSystem.Server.Host

# Run all tests
dotnet test

# Run single test class
dotnet test --filter "ClassName=TestGetDrugAction"

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

---

## Coding Conventions

1. **Primary constructors** for all DI classes; assign to `private readonly` fields.
2. **`ConfigureAwait(false)`** on all `await` calls in library/server code.
3. **`ArgumentNullException.ThrowIfNull`** at every public method entry point.
4. **`CancellationToken`** as last parameter on every async public method.
5. **Nullable reference types** enabled everywhere.
6. **GUIDs** generated in SQL insert (not in C# code).
7. **Validation** in Action layer only, not in endpoints.
8. **Logging** via `ILogger<T>`; debug/trace level on happy paths.

### Exception Hierarchy

```
BaseException
├── BadRequestException    (validation — HTTP 422)
├── ConflictException      (duplicate/conflict — HTTP 409)
└── DataAccessException    (data layer — HTTP 500)
```

### DI Lifetimes

- `Singleton` — Actions (stateless orchestrators)
- `Scoped` — Repositories, StorageClients, DbClient (one per request)

---

## Checklist When Adding a New Entity

- [ ] Entity in `*.Common/EntityName/EntityName.cs` (extends `BaseObject`)
- [ ] Filter in `*.Common/EntityName/EntityNameFilter.cs` (extends `FilterBase`)
- [ ] `IGetEntityAction`, `ISaveEntityAction`, `IEntityRepository`, `IEntityStorageClient` interfaces in `*.Server/EntityName/`
- [ ] `GetEntityAction`, `SaveEntityAction`, `EntityRepository` implementations in `*.Server/EntityName/`
- [ ] `SqlServerEntityStorageClient` + `EntityDatabaseCommandText` in `*.Server.Data.SqlServer/EntityName/`
- [ ] FluentMigrator migration in `*.Server.Data.SqlServer/Migrations/`
- [ ] Register all in `DependencyExtensions.cs`
- [ ] GET/POST/PUT/DELETE endpoints in `ControllerExtensions.cs`
- [ ] Unit tests + test data in `*.Server.Unit/EntityName/`

### Test Naming Convention

`MethodName_When[Condition]_[ExpectedResult]`

### Test Folder Layout

```
test/
└── PharmacyManagementSystem.Server.Unit/
    └── EntityName/
        ├── TestGetEntityAction.cs
        ├── TestSaveEntityAction.cs
        └── Data/
            ├── GetEntityActionData.cs
            └── SaveEntityActionData.cs
```
