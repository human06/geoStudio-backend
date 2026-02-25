# geoStudio Backend

AI Visibility Platform — .NET 10 Clean Architecture Backend

## Architecture

```
geoStudio.Backend/
├── geoStudio.API/              → ASP.NET Core Web API (entry point)
├── geoStudio.Application/      → Business logic, services, DTOs, validators
├── geoStudio.Domain/           → Core entities, enums, value objects
├── geoStudio.Infrastructure/   → EF Core, PostgreSQL, Redis, JWT
├── geoStudio.BackgroundJobs/   → Hangfire jobs, hosted services
└── geoStudio.Tests/            → Unit & integration tests
```

## Prerequisites

| Tool | Version |
|------|---------|
| .NET SDK | 10.0+ |
| Docker & Docker Compose | latest |
| PostgreSQL | 16+ (or via Docker) |
| Redis | 7+ (or via Docker) |

## Quick Start

### 1. Clone and setup

```bash
git clone https://github.com/yourusername/geoStudio-Backend.git
cd geoStudio.Backend
```

### 2. Start infrastructure (PostgreSQL + Redis)

```bash
docker compose up postgres redis -d
```

### 2. Configure secrets (development)

The `appsettings.Development.json` already has working local values.
For production, use User Secrets or environment variables:

```bash
cd geoStudio.API
dotnet user-secrets set "Jwt:SecretKey" "your-super-secret-key-min-32-chars"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;"
```

### 3. Apply database migrations

```bash
dotnet ef database update --project geoStudio.Infrastructure --startup-project geoStudio.API
```

### 4. Run the API

```bash
dotnet run --project geoStudio.API
```

API is available at `http://localhost:5000`
Swagger UI at `http://localhost:5000/` (development only)
Hangfire Dashboard at `http://localhost:5000/hangfire`

## Running with Docker

```bash
docker compose up --build
```

## Running Tests

```bash
dotnet test
```

With coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Creating Migrations

```bash
dotnet ef migrations add <MigrationName> \
  --project geoStudio.Infrastructure \
  --startup-project geoStudio.API \
  --output-dir Persistence/Migrations
```

## Environment Variables (Production)

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string |
| `ConnectionStrings__RedisConnection` | Redis connection string |
| `Jwt__SecretKey` | JWT signing key (≥ 32 chars) |
| `Jwt__Issuer` | Token issuer |
| `Jwt__Audience` | Token audience |
| `AllowedOrigins` | Comma-separated CORS origins |

## API Endpoints

### Auth
| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/v1/auth/register` | Register new user |
| POST | `/api/v1/auth/login` | Login |
| POST | `/api/v1/auth/refresh-token` | Rotate tokens |
| POST | `/api/v1/auth/logout` | Logout (requires auth) |

### Businesses
| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/v1/businesses` | Create business profile |
| GET | `/api/v1/businesses/{id}` | Get by ID |
| GET | `/api/v1/businesses/my` | Get current user's businesses |
| PATCH | `/api/v1/businesses/{id}` | Update |
| DELETE | `/api/v1/businesses/{id}` | Delete |

### SignalR
| Hub | Path | Description |
|-----|------|-------------|
| AuditHub | `/hubs/audit` | Business-scoped audit notifications |
| AuditProgressHub | `/hubs/audit-progress` | Real-time per-audit progress tracking |

## Tech Stack

- **Runtime**: .NET 10 / ASP.NET Core 10
- **ORM**: Entity Framework Core 10 + Npgsql
- **Auth**: JWT Bearer tokens + BCrypt password hashing
- **Cache**: Redis (StackExchange.Redis)
- **Jobs**: Hangfire + PostgreSQL storage
- **Real-time**: SignalR
- **Logging**: Serilog (console + rolling file)
- **Docs**: Swagger / OpenAPI
- **Validation**: FluentValidation
- **Testing**: xUnit + Moq + FluentAssertions
