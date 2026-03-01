# Spec: Fix remaining ESLint warnings in src/main/__mocks__/electron.ts

## Overview
Address the remaining ESLint warnings related to `@typescript-eslint/no-explicit-any` in `src/main/__mocks__/electron.ts` to achieve a clean codebase.

## Goals
- Eliminate all `no-explicit-any` warnings in `src/main/__mocks__/electron.ts`.
- Ensure `pnpm lint` passes without errors or warnings.

## Functional Requirements
- Replace `any` types with more specific types or `unknown` where appropriate.
- Use type casting with `as unknown as ...` if necessary for complex mock objects.
