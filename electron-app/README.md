# Electron Memo App

ReactとTypeScriptで構築された、シンプルで高機能なデスクトップメモアプリケーションです。

## 特徴

- **高速な開発**: `electron-vite` を使用したHMR (Hot Module Replacement) 対応。
- **モダンな UI**: Tailwind CSS 4を使用したクリーンでレスポンシブなデザイン。
- **堅牢な設計**: TypeScriptによる型安全性の確保と、Atomic Designに基づいたコンポーネント設計。
- **セキュアな IPC**: Context Bridgeを利用した安全なメイン・レンダラープロセス間通信。

## 技術スタック

- **Framework**: Electron
- **Frontend**: React 19
- **Styling**: Tailwind CSS 4
- **Language**: TypeScript
- **Build Tool**: electron-vite
- **Package Manager**: pnpm
- **Testing**: Vitest, React Testing Library
- **Linting/Formatting**: ESLint, Prettier

## はじめに

### 依存関係のインストール

```bash
pnpm install
```

### 開発モードでの起動

```bash
pnpm dev
```

### プレビュー (ビルド後の動作確認)

```bash
pnpm start
```

## テスト

### すべてのテストを実行

```bash
pnpm test
```

### 特定のファイルのテストを実行

```bash
npx vitest run <ファイルのパス>
```

### カバレッジの確認

```bash
pnpm test:coverage
```

## ビルド

アプリケーションをパッケージ化するには、OSに合わせたコマンドを実行します。

```bash
# Windows
pnpm build:win

# macOS
pnpm build:mac

# Linux
pnpm build:linux
```

## プロジェクト構造

- `src/main`: メインプロセスのロジック（ウィンドウ管理、IPCハンドリングなど）。
- `src/preload`: プリロードスクリプト（レンダラーへの安全なAPI公開）。
- `src/renderer`: レンダラープロセス（React UIコンポーネント）。
- `resources`: アイコンなどの静的アセット。
