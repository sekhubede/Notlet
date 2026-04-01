# Notlet

Notlet is a .NET 8 console note-taking app used to demonstrate a disciplined GitHub workflow and coding standards. The current version includes MVP bootstrap foundations plus local JSON note storage with tested persistence behavior.

## Purpose

- Demonstrate team workflow: issue-driven development, branch discipline, and PR quality gates.
- Build a clean MVP foundation that can expand to Notes CRUD and storage in later issues.
- Keep architecture simple now, while remaining ready for future API extraction.

## Architecture (v1)

- **Runtime:** .NET 8
- **App type:** Console application
- **Composition:** Generic Host + Dependency Injection
- **Logging:** Console logging
- **Error handling:** Friendly user messages + structured technical logs
- **Exit codes:** Standardized application exit codes via `AppExitCode`

## Storage (v1)

- **Abstraction:** `INoteStore`
- **Implementation:** `JsonNoteStore`
- **Provider config:** `Notlet:Storage:Provider` (currently `Json`)
- **File path config:** `Notlet:Storage:FilePath` (default `data/notes.json`)
- Missing storage file is treated as an empty note set.
- Storage read/write failures are logged and surfaced to global error handling.

## Project Structure

- `Notlet.sln`
- `src/Notlet.App` - Console app
- `tests/Notlet.App.Tests` - xUnit tests
- `docs/adr` - Architecture decision records
- `src/Notlet.App/Abstractions` - Contracts (e.g., `INoteStore`)
- `src/Notlet.App/Domain` - Domain models (e.g., `Note`)
- `src/Notlet.App/Infrastructure/Storage` - JSON storage implementation/options

## Run the App

```bash
dotnet run --project src/Notlet.App
```

## Run Tests

```bash
dotnet test
```

## Workflow Rules

- No code without an issue.
- One issue = one branch = one PR.
- All feature work targets `staging` first.
- `main` remains releasable/stable.
- PRs must include test evidence and behavior notes.

## Definition of Done (Issue Level)

- Build and tests pass locally.
- At least one happy-path and one validation/failure test per feature.
- User-facing errors are friendly (no raw exception dumps).
- Logging captures enough detail for debugging.
- PR includes summary, test evidence, and linked issue.
