# AGENTS.md

This document provides essential information for agentic coding agents working on this repository.

## Project Overview

This is an Electron application built with React and TypeScript, using `electron-vite` for development and building.

- **Main Process**: `src/main/`
- **Preload Script**: `src/preload/`
- **Renderer Process**: `src/renderer/` (React-based)

## Build, Lint, and Test Commands

The project uses `pnpm` as the package manager.

### Development

- `pnpm dev`: Starts the development server with HMR.
- `pnpm start`: Previews the production build.

### Building

- `pnpm build`: Runs type checks and builds the application.
- `pnpm build:win`: Builds for Windows.
- `pnpm build:mac`: Builds for macOS.
- `pnpm build:linux`: Builds for Linux.

### Quality Control

- `pnpm lint`: Runs ESLint for code quality.
- `pnpm format`: Runs Prettier to format the codebase.
- `pnpm typecheck`: Runs TypeScript compiler checks for both main and web processes.

### Testing

- `pnpm test`: Runs all tests using Vitest.
- **Run a single test**: `npx vitest run <path/to/file.test.ts>`
- `pnpm test:watch`: Runs tests in watch mode.
- `pnpm test:coverage`: Generates test coverage report.

## Code Style Guidelines

### 1. General Principles

- Use TypeScript for all new code.
- Strictly adhere to the project's Prettier and ESLint configurations.
- Prefer functional programming patterns over classes.

### 2. Naming Conventions

- **Components**: PascalCase (e.g., `Button.tsx`, `AppHeader.tsx`).
- **Files/Folders**: PascalCase for components, camelCase for utility files and directories (except component categories).
- **Functions/Variables**: camelCase.
- **Constants**: UPPER_SNAKE_CASE.
- **Interfaces/Types**: PascalCase, preferably prefixed with nothing or suffixed with `Props` for components.

### 3. React Components (Renderer)

- Use functional components.
- Use `React.FC<Props>` or define the return type as `React.JSX.Element`.
- Follow **Atomic Design** principles for organizing components:
  - `src/renderer/src/components/atoms/`: Smallest building blocks (Button, Input).
  - `src/renderer/src/components/molecules/`: Simple groups of atoms (SearchBar, MenuItem).
  - `src/renderer/src/components/organisms/`: Complex UI sections (AppHeader, EditorPanel).
  - `src/renderer/src/pages/`: Full pages.
- Use Tailwind CSS for styling via the `className` prop.

### 4. Imports

- Use named imports where possible.
- Order: Built-in modules (e.g., `path`), external libraries (e.g., `electron`, `react`), internal components/utils.
- Use relative paths as established in the codebase.

### 5. Electron IPC (Inter-Process Communication)

- Define IPC handlers in `src/main/index.ts`.
- Use `ipcMain.on` or `ipcMain.handle` in the main process.
- Use the bridge pattern in `src/preload/index.ts` to expose APIs to the renderer.
- Ensure the renderer accesses Electron features only through the `window.electron` or custom context bridge APIs.
- Prefer `ipcMain.handle` for asynchronous operations to provide return values.

### 6. Error Handling

- Use `try...catch` blocks for asynchronous operations and IPC handlers.
- Always check the type of caught errors:
  ```typescript
  try {
    // ... logic
  } catch (err) {
    const message = err instanceof Error ? err.message : String(err);
    console.error('Operation failed:', message);
  }
  ```
- Send error messages back to the renderer via IPC when appropriate.

### 7. Testing (Vitest)

- Place tests alongside the source file with the `.test.ts` or `.test.tsx` extension.
- Use `describe` blocks to group tests for a component or module.
- Use `@testing-library/react` for component tests.
- Mock Electron modules when testing main process logic or components that interact with the bridge.
- Mocking example:
  ```typescript
  import { vi, describe, it, expect } from 'vitest';
  const handleClick = vi.fn();
  ```

### 8. Styling (Tailwind CSS)

- Use Tailwind CSS utility classes exclusively for styling.
- Avoid inline styles.
- Use the `clsx` or `tailwind-merge` utility if available for dynamic classes (check `package.json`).
- Adhere to the project's color palette and spacing defined in `tailwind.config.js`.

### 9. State Management

- Use React Hooks (`useState`, `useReducer`, `useContext`) for local and shared state.
- Keep state as close to where it's used as possible.
- Avoid complex global state if simpler alternatives suffice.

## Existing Rules

(No `.cursorrules` or `.github/copilot-instructions.md` were found in this repository at the time of creation.)

## Development Workflow for Agents

1. **Analyze**: Read relevant files and understand the context.
2. **Plan**: Describe the proposed changes clearly.
3. **Implement**: Modify code following the style guidelines above.
4. **Verify**:
   - Run `pnpm typecheck` to ensure no type regressions.
   - Run `pnpm lint` and `pnpm format`.
   - Run relevant tests with `npx vitest run <path>`.
   - If tests don't exist, create them.
5. **Finalize**: Ensure the application builds correctly with `pnpm build`.

---

_This file is maintained for automated agents to ensure consistency and quality._
