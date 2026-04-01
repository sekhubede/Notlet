# ADR 0002: Error Handling and Exit Code Policy

- **Status:** Accepted
- **Date:** 2026-03-27
- **Owners:** Notlet team

## Context

Notlet is a console application where users must receive clear, friendly feedback without internal exception details.  
At the same time, developers need sufficient diagnostics in logs for troubleshooting.  
A consistent process exit policy is also needed for local usage and future automation (CI/scripts).

## Decision

Adopt a two-level error handling model:

1. **Application-level handling (`NotletApplication`)**
   - Catch known/expected failures (for example validation or format errors).
   - Show user-friendly messages.
   - Return a validation/usage exit code.

2. **Global handling (`Program`)**
   - Catch all unhandled/unexpected exceptions.
   - Log full technical details.
   - Show a generic user-safe message.
   - Return an unexpected-error exit code.

Use standardized exit codes via `AppExitCode`:

- `Success = 0`
- `UnexpectedError = 1`
- `ValidationOrUsageError = 2`
- `Cancelled = 3`

### Storage Error Boundary

`JsonNoteStore` logs storage failures and rethrows unexpected exceptions so they are handled by the global application error boundary (`Program`), preserving a consistent user-facing error policy.

## Consequences

### Positive

- Consistent user experience across commands.
- No raw exception output shown to end users.
- Reliable automation behavior through stable exit codes.
- Better observability due to structured logging in error paths.

### Negative

- Requires discipline to classify expected vs unexpected failures correctly.
- New failure types may require policy updates over time.

## Alternatives Considered

1. **Single global catch only**
   - Rejected: expected user-input failures would be treated as generic unexpected errors.

2. **Expose exception messages directly to users**
   - Rejected: may leak internal details and produce inconsistent UX.

3. **No exit code standardization**
   - Rejected: makes scripts/CI behavior ambiguous and harder to automate.

## Follow-up

- Review and refine expected failure categories as features are added.
- Add tests for global catch behavior when practical.
- Consider documenting user-facing error message guidelines in coding standards.
- Expand tests for storage failure paths and cancellation behavior.
