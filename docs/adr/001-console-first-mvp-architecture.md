# ADR 0001: Console-First MVP Architecture

- **Status:** Accepted
- **Date:** 2026-03-27
- **Owners:** Notlet team

## Context

The project goal is to demonstrate a repeatable team workflow using GitHub issues, branches, pull requests, and quality gates while shipping an MVP quickly.  
A split architecture (API + client) is planned in the future, but implementing multiple projects in day-one MVP would increase setup and integration overhead.

## Decision

For v1, Notlet will use a **single .NET 8 console application** with:

- Generic Host for composition and dependency injection
- Console logging for observability
- Friendly error handling for user-facing failures
- Standardized application exit codes
- xUnit tests for startup and validation/failure paths

The project remains structured to support future extraction of an API layer without major disruption.

## Consequences

### Positive

- Faster MVP delivery with less plumbing.
- Easier onboarding for team workflow demo.
- Clear baseline for coding standards and test discipline.
- Reduced risk of over-engineering in early phase.

### Negative

- No HTTP/API boundary in v1.
- Some abstractions may need refactoring during API extraction.
- Integration behavior between client and API is not tested in v1.

## Alternatives Considered

1. **Two-project start (API + Console Client)**
   - Rejected for v1 due to added setup complexity and slower delivery.

2. **Single project with no host/DI/logging baseline**
   - Rejected because it weakens architecture learning and quality standards demonstration.

## Follow-up

- Revisit this ADR when Notes CRUD + storage are stable.
- Evaluate adding `Notlet.Api` in a later milestone.
- Define migration plan for storage abstraction when API/cloud support is introduced.
