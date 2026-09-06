# BuildHub

A marketplace for home renovation in Oman. Homeowners post jobs, vendors (contractors,
designers and stores) bid on them, the homeowner accepts one bid, the payment is held in
a mocked escrow until the work is done, and the homeowner rates the vendor afterwards.

> The frontend is being reworked and is not documented yet.

## Tech stack

| | |
|---|---|
| API | ASP.NET Core Web API, .NET 10 |
| Data | Entity Framework Core 10, SQL Server (code-first migrations) |
| Auth | JWT bearer tokens, role-based, PBKDF2 password hashing |
| Docs | OpenAPI + Swagger UI, Postman collection |

## Repository structure

```
Backend/Backend/        The API project (see below)
DOCS/                   ERD - Build_Hub.mmd (Mermaid source) and Build_Hub.png
FrontEnd/               Being reworked
```

### Backend

```
Program.cs              DI registration, auth, request pipeline
Controllers/            HTTP surface - routing, role gates, status codes
Services/               All business rules, ownership checks, transactions
Repositories/           EF Core queries only, no rules
Data/                   DbContext, delete behaviour, sample-data seeder
Models/
  Entities/             The 9 EF entities
  Dtos/                 Request and response shapes
  Enums.cs              Status enums
Exceptions/             Domain exceptions mapped to HTTP statuses
Middleware/             Exception handler producing ProblemDetails
Migrations/             EF Core migrations
Configuration/          JWT settings, validated at startup
```

Requests flow `Controller -> Service -> Repository -> DbContext`. Controllers stay thin,
services hold every rule, repositories only query. Entities are never serialized - each
resource has its own request and response DTOs.

## Getting started

**Prerequisites:** .NET 10 SDK, SQL Server on `localhost`, and
`dotnet tool install --global dotnet-ef` for migrations.

```bash
dotnet run --project Backend/Backend --launch-profile http
```

Runs on `http://localhost:5158` with Swagger UI at `/swagger`.

In Development, `SeedData` is enabled: the app applies pending migrations and fills an
empty database with sample categories, accounts and jobs at every stage of the flow.
Every seeded account uses the password `Password123!`; sign in as
`admin@buildhub.om` (admin), `salim@example.om` (homeowner) or `aisha@example.om`
(vendor).

Change the connection string in `Backend/Backend/appsettings.json`. The JWT signing key
is not committed - the development value lives in `appsettings.Development.json`, and
any deployment must supply its own via environment variable or secret store.

To manage the schema manually:

```bash
dotnet ef database update --project Backend/Backend
```

## API

All routes are under `/api`, and every endpoint requires a token unless marked otherwise.

| Resource | Endpoints | Notes |
|---|---|---|
| `Auth` | `POST /login` | Public. Returns the bearer token |
| `Users` | CRUD | Registration is public; listing is admin-only |
| `Categories` | CRUD | Public to read, admin to manage |
| `VendorProfiles` | CRUD | Public to read; one profile per vendor account |
| `Jobs` | CRUD | Public to read; homeowners post |
| `Offers` | CRUD | Vendors bid on open jobs, one offer per job |
| `Agreements` | CRUD | Created by accepting an offer; holds the escrow state |
| `Products` | CRUD | Public to read; store listings for price comparison |
| `Reviews` | CRUD | Public to read; only after a completed agreement |
| `Notifications` | CRUD | Recipients read their own |

Send the token as `Authorization: Bearer <token>`. Role checks live on the controllers;
ownership checks ("this job is not yours") live in the services and return `403`.

`Backend/Backend/BuildHub.postman_collection.json` covers every endpoint and rule, and
is safe to run top to bottom against a freshly seeded database.

## Core flow

1. A homeowner posts a job - it starts `Open`.
2. Vendors submit offers while it is open - each starts `Pending`.
3. The homeowner accepts one. In a single transaction the offer becomes `Accepted`, the
   rest become `NotSelected`, the job becomes `Hired`, and an agreement opens with the
   payment `Held`.
4. The work happens off-platform.
5. The homeowner releases the escrow: the vendor's balance is credited and the job is
   `Completed`. An admin can refund instead, which cancels both.
6. The homeowner reviews the vendor, and the vendor's average rating is recalculated.

## License

[MIT](LICENSE)
