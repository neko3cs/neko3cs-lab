# Agent Instructions - Apollo GraphQL Learning Project

This document provides essential information for AI agents operating in this repository. Follow these guidelines strictly to maintain consistency and quality.

## 1. Build, Lint, and Test Commands

This project uses Node.js, TypeScript, and **pnpm** as the package manager.

| Action                   | Command                       |
| :----------------------- | :---------------------------- |
| **Install Dependencies** | `pnpm install`                |
| **Development Mode**     | `pnpm run dev`                |
| **Build Project**        | `pnpm run build`              |
| **Lint Code**            | `pnpm run lint`               |
| **Fix Lint Issues**      | `pnpm run lint -- --fix`      |
| **Run All Tests**        | `pnpm test`                   |
| **Run Single Test**      | `pnpm test -- <path-to-file>` |
| **Type Check**           | `pnpm run type-check`         |

## 2. Code Style Guidelines

### Imports and Exports

- Use ESM `import/export` syntax.
- Group imports: 1. Built-in, 2. External libraries, 3. Internal modules.
- Use absolute paths if configured (e.g., `@/server/...`), otherwise use relative paths.
- Prefer named exports over default exports for better discoverability and refactoring.

### TypeScript and Types

- **Strict Mode:** Always assume `strict: true` in `tsconfig.json`.
- **Explicit Types:** Provide explicit return types for functions, especially resolvers and API calls.
- **Interfaces vs Types:** Use `interface` for object structures that might be extended, and `type` for unions, intersections, and primitives.
- **GraphQL Types:** Use tools like `graphql-codegen` to generate TypeScript types from the GraphQL schema. Do not manually mirror schema types.

### Naming Conventions

- **Files/Directories:** `kebab-case` (e.g., `user-service.ts`, `schema-loader.ts`).
- **Variables/Functions:** `camelCase` (e.g., `getUserById`, `isLoaded`).
- **Classes/Interfaces/Types:** `PascalCase` (e.g., `UserResolver`, `AuthPayload`).
- **Constants:** `UPPER_SNAKE_CASE` (e.g., `MAX_RETRY_COUNT`).
- **React Components:** `PascalCase` (e.g., `UserProfile.tsx`).

### Error Handling

- **Server-side:** Use `ApolloError` or specialized subclasses (e.g., `UserInputError`, `AuthenticationError`) from `apollo-server`.
- **Async/Await:** Use `try/catch` blocks for asynchronous operations.
- **Result Pattern:** For complex mutations, consider returning a union type or a response object with a `success` boolean and an `errors` array.

### GraphQL Best Practices

- **Schema-first:** 閲覧性を高めるため、スキーマ定義とリゾルバーは `server/main.ts` 内で完結させるか、最小限の分割に留めてください。
- **Thin Resolvers:** ロジックが複雑になる場合は外部関数に切り出しますが、基本的には `main.ts` 内で見通しが良いように配置してください。
- **Data Source:** **Do not use a database.** Import raw JSON files directly for data. All data JSON files must be placed in the `src/server/data` directory.
- **Mutations:** Always return the affected object or a clear result payload after a mutation. Use specific input types for mutation arguments.
- **Pagination:** Follow the Relay Cursor Connections specification for list fields that require pagination.
- **Enums:** Use GraphQL Enums for fields with a fixed set of values to improve type safety and discoverability.

### Schema Design Patterns

- **Types:** Use descriptive names for types. Use `PascalCase`.
- **Fields:** Use `camelCase` for field names.
- **Input Objects:** Use `Input` suffix for mutation input types (e.g., `CreateUserInput`).
- **Payloads:** Use `Payload` suffix for mutation return types (e.g., `CreateUserPayload`).
- **Interfaces:** Use GraphQL Interfaces to share common fields across different types.

### Resolver Structure

Resolvers should follow a consistent signature: `(parent, args, context, info)`.

- **Context:** Use the `context` object to access shared resources like data loaders and authentication info.
- **Args:** Destructure `args` for better readability.
- **Parent:** Use the `parent` object to resolve fields for nested types.

## 3. Apollo Client Guidelines (Frontend)

### Data Fetching

- Use the `useQuery` and `useMutation` hooks for most data fetching needs.
- Handle loading and error states explicitly in components.
- Avoid using the Apollo Client directly (e.g., `client.query`) inside components unless necessary.

### Cache Management

- **Normalization:** Ensure every type in the schema has an `id` or `_id` field for efficient cache normalization.
- **Updates:** Use the `update` function in `useMutation` to manually update the cache when necessary, especially for deletions or additions to lists.
- **Policies:** Define `TypePolicies` in the `InMemoryCache` constructor for complex pagination or field-level cache logic.

### Local State

- Use Reactive Variables (`makeVar`) for global client-side state that doesn't belong in the GraphQL server.
- Avoid mixing local state management libraries (like Redux or Zustand) with Apollo Client if possible; try to use Apollo's built-in capabilities first.

## 4. Testing Procedures

このプロジェクトは学習用のサンプルコードであるため、**原則としてテストコードを書く必要はありません。** 動作確認は手動または、開発用サーバーでの実機確認を優先してください。

## 5. Project Structure

閲覧性とシンプルさを重視し、プロダクトコードのような複雑なディレクトリ分割は行いません。可能な限りファイルをまとめ、コードの全容を把握しやすくしてください。

```text
apollo-graphql/
├── src/
│   ├── client/           # Apollo Client / Frontend
│   │   ├── index.html    # エントリーポイントとなるHTML
│   │   └── App.tsx       # 全てのコンポーネント、Query/Mutation、キャッシュ設定をここに集約
│   └── server/           # Apollo Server / Backend
│       ├── main.ts       # サーバー起動、スキーマ定義、リゾルバーをここに集約
│       └── data/         # 生のJSONデータ（DBの代わり）
├── .gitignore
├── package.json
├── tsconfig.json
└── pnpm-lock.yaml
```

## 6. Documentation & Communication

- **Language:** Always communicate with the user in Japanese. (ユーザーとの対話は常に日本語で行ってください。)
- **Schema Comments:** Use triple-quote descriptions in `.graphql` files for all types and fields.
- **README:** Keep the root `README.md` updated with setup instructions and high-level architecture.
- **Inline Comments:** Add high-value comments describing *why* a particular approach was taken, especially for complex cache updates or resolver logic.

## 7. Development Workflow

Agents should follow a systematic workflow to ensure code quality and project stability.

1. **Exploration:** Use `grep` and `glob` to find relevant files and understand existing patterns.
2. **Planning:** Create a brief plan and share it with the user if the task is complex.
3. **Implementation:** Write code in small, incremental steps.
4. **Self-Correction:** If a command fails or a lint error occurs, fix it immediately before proceeding.
5. **Verification:** Run tests and linting as the final step of every task.

## 8. Git Best Practices

When performing git operations, adhere to the following:

- **Permissions:** **NEVER** run `git commit` or `git push` without explicit permission from the user. (ユーザーの許可なく `git commit` または `git push` を行わないでください。)
- **Branching:** Work on feature branches if requested.
- **Commit Messages:** Use descriptive, present-tense commit messages (e.g., "Add user authentication service").
- **Atomicity:** Keep commits small and focused on a single change.
- **Cleanup:** Do not leave temporary files or debug logs in the codebase.

## 9. Agent Safety & Protocol

- **Git Safety:** Do not commit or push changes unless the user explicitly requests it.
- **Verification:** After making changes, run `pnpm run lint` and `pnpm run type-check`.
- **Testing:** このプロジェクトではテストコードの作成は不要です。
- **Secrets:** Never commit `.env` files or hardcode API keys. Use the `.env.example` template.
- **Proactiveness:** If you notice a small, obvious improvement (like a typo or a missing type), fix it along with your primary task.
- **Communication:** Be concise. Use tool calls for actions and text only for essential updates.
