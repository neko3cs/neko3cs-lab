# Plan: Fix remaining ESLint warnings in src/main/__mocks__/electron.ts

## Phase 1: Lint Fix

- [x] Task: Fix explicit any in electron mock (5757c4f)
    - [ ] Read `src/main/__mocks__/electron.ts`.
    - [ ] Replace `any` with specific types or `unknown`.
- [x] Task: Verify Lint (5757c4f)
    - [ ] Run `pnpm lint` and confirm no warnings.
- [ ] Task: Conductor - User Manual Verification 'Lint Fix' (Protocol in workflow.md)
