# ADR Index

This directory stores Architecture Decision Records (ADRs) for Notlet.

## Purpose

ADRs capture important technical decisions, why they were made, and their trade-offs.  
They help the team keep a shared language and avoid re-discussing settled decisions without new context.

## ADR List

- [000 Template](./000-template.md) - ADR writing template
- [001 Console-First MVP Architecture](./001-console-first-mvp-architecture.md)
- [002 Error Handling and Exit Code Policy](./002-error-handling-and-exit-code-policy.md)

## How to Add a New ADR

1. Copy `000-template.md`.
2. Name the file with the next number and short kebab-case title:
   - `003-some-decision-title.md`
3. Set status (`Accepted`, `Proposed`, `Superseded`, etc.).
4. Fill in context, decision, consequences, and alternatives.
5. Add the new ADR link to this index.
6. Reference the ADR in the related PR.

## Lightweight Team Rules

- Prefer one meaningful decision per ADR.
- Keep ADRs concise and practical.
- Update status if a decision changes.
- If a decision replaces another ADR, link both directions.
