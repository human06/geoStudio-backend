# Development Guide

## Getting Started

1. Set up your development environment (see [README.md](README.md))
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes
4. Test your changes: `dotnet test`
5. Commit with clear messages: `git commit -m "feat(scope): description"`
6. Push to your branch: `git push origin feature/your-feature-name`
7. Submit a Pull Request into `develop`

---

## Code Style

- Follow standard C# naming conventions (PascalCase for types/members, camelCase for locals)
- Use Clean Architecture layer boundaries — no skipping layers
- Prefer primary constructors and collection expressions (C# preview features are enabled)
- Add XML doc comments (`/// <summary>`) on all public members
- Keep methods small and focused — single responsibility
- Use `CancellationToken` on every async method that crosses I/O boundaries

---

## Testing

- Write unit tests for all service methods (`geoStudio.Tests/Unit/`)
- Write integration tests for API endpoints (`geoStudio.Tests/Integration/`)
- Aim for ≥ 80% code coverage
- Run all tests before committing:

```bash
dotnet test
```

With coverage report:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## Database

- Use EF Core migrations for **all** schema changes — never edit the DB manually
- Name migrations descriptively: `Add_AuditStatusColumn`, `Create_TeamMembers_Table`
- Never modify an already-applied migration — add a new one instead
- Always test migrations in both directions (up & down)

### Create a migration

```bash
dotnet ef migrations add <MigrationName> \
  --project geoStudio.Infrastructure \
  --startup-project geoStudio.API \
  --output-dir Persistence/Migrations
```

### Apply migrations

```bash
dotnet ef database update \
  --project geoStudio.Infrastructure \
  --startup-project geoStudio.API
```

### Roll back one migration

```bash
dotnet ef database update <PreviousMigrationName> \
  --project geoStudio.Infrastructure \
  --startup-project geoStudio.API
```

---

## Commit Convention

Format: `<type>(<scope>): <description>`

| Type | When to use |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `refactor` | Code change with no behaviour change |
| `test` | Adding or fixing tests |
| `chore` | Build system, dependencies, CI |
| `perf` | Performance improvement |

**Examples:**

```
feat(auth): add refresh-token rotation endpoint
fix(audit): handle null AuditStatus on completion
docs(readme): update Docker quick-start instructions
refactor(cache): replace byte[] serialisation with System.Text.Json
test(business): add unit tests for BusinessService.CreateAsync
chore(deps): upgrade Npgsql to 9.0.4
```

---

## Branching Strategy

| Branch | Purpose |
|--------|---------|
| `master` | Stable, production-ready code |
| `develop` | Integration branch for completed features |
| `feature/*` | Individual feature work |
| `fix/*` | Bug fixes |
| `release/*` | Release stabilisation |

Always branch from `develop` and merge back into `develop` via Pull Request.

---

## Project Layer Rules

| From → To | Allowed? |
|-----------|---------|
| API → Application | ✅ |
| API → Infrastructure | ✅ (DI wiring only) |
| Application → Domain | ✅ |
| Application → Infrastructure | ✅ (via interfaces) |
| Infrastructure → Domain | ✅ |
| Domain → anything | ❌ (Domain has no outward dependencies) |

---

## Questions?

Open a GitHub issue or ask in the development channel.
