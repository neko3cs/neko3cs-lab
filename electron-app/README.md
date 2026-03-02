# electron-app

ReactとTypeScriptを使用したElectronアプリケーションのサンプルプロジェクトです。
`electron-vite` を使用して、モダンな開発環境が構築されています。

## はじめに

このプロジェクトは、Electronにおけるメインプロセスとレンダラープロセスの連携（IPC通信）や、Viteを用いた高速なフロントエンド開発手法を学習するためのテンプレートです。

## 使用技術

- **Runtime**: [Electron](https://www.electronjs.org/) (v39.7.0)
- **Frontend Framework**: [React](https://react.dev/) (v19.2.4)
- **Build Tool**: [electron-vite](https://electron-vite.org/) (v5.0.0)
- **Language**: [TypeScript](https://www.typescriptlang.org/)
- **Package Manager**: [pnpm](https://pnpm.io/)
- **Utilities**: [@electron-toolkit/utils](https://github.com/alex8088/electron-toolkit)

## プロジェクト構造

```text
├── src/
│   ├── main/          # メインプロセス (Node.js)
│   │   └── index.ts   # アプリのライフサイクルとIPCハンドラの定義
│   ├── preload/       # プリロードスクリプト (Main と Renderer の橋渡し)
│   │   ├── index.ts   # contextBridge を用いた API 公開
│   │   └── index.d.ts # TypeScript 用の型定義
│   └── renderer/      # レンダラープロセス (React)
│       ├── index.html
│       └── src/       # フロントエンドのソースコード (App.tsx など)
```

## IPC (Inter-Process Communication) の実装メモ

Electronではセキュリティのため、レンダラープロセスから直接Node.jsのAPIを叩くことはできません。`ipcMain`, `preload`, `ipcRenderer` を組み合わせて通信を行います。

### 1. メインプロセス (バックエンド) でのハンドラー定義
`src/main/index.ts` にて、レンダラーからの要求を待ち受ける処理を記述します。

```typescript
// src/main/index.ts
import { ipcMain } from 'electron'

function configureIpcHandlers(): void {
  // 'ping' というイベント名で受信し、'pong' とログ出力する
  ipcMain.on('ping', () => console.log('pong'))

  // 引数を受け取る例
  ipcMain.on('say-hello', (_event, name = 'World') => {
    console.log(`Hello, ${name}!`)
  })
}
```

### 2. プリロードスクリプトでの API 公開
`src/preload/index.ts` にて、メインプロセスへの通信を `api` オブジェクトとして定義し、レンダラーの `window` オブジェクトに公開します。

```typescript
// src/preload/index.ts
import { contextBridge, ipcRenderer } from 'electron'

const api = {
  ping: () => ipcRenderer.send('ping'),
  sayHello: (name?: string) => ipcRenderer.send('say-hello', name)
}

// レンダラープロセスの window.api として公開
contextBridge.exposeInMainWorld('api', api)
```

### 3. TypeScript 用の型定義 (重要)
`src/preload/index.d.ts` で `window` オブジェクトを拡張し、公開した `api` の型を定義します。**これを行わないと、React コンポーネントから `window.api` を呼び出す際に TypeScript のコンパイルエラーが発生します。**

```typescript
// src/preload/index.d.ts
import { ElectronAPI } from '@electron-toolkit/preload'

declare global {
  interface Window {
    electron: ElectronAPI
    api: {
      ping: () => void
      sayHello: (name?: string) => void
      // ... その他の API の型定義
    }
  }
}
```

### 4. レンダラープロセス (フロントエンド) からの利用
Reactコンポーネントなどから、`window.api` を通じて呼び出します。型定義があるため、エディターの補完も効くようになります。

```tsx
// src/renderer/src/App.tsx
function App() {
  const handlePing = () => {
    // window.api.ping() が型安全に呼び出せる
    window.api.ping()
  }

  return (
    <button onClick={handlePing}>Send IPC</button>
  )
}
```

## 開発とビルド

### 推奨環境
- [VSCode](https://code.visualstudio.com/)
- [ESLint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)
- [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)

### セットアップ
```bash
$ pnpm install
```

### 開発モード
```bash
$ pnpm dev
```

### ビルド
OSに合わせて以下のコマンドを実行します。
```bash
# Windows
$ pnpm build:win

# macOS
$ pnpm build:mac

# Linux
$ pnpm build:linux
```

## 参考リンク
- [electron-vite documentation](https://electron-vite.org/)
- [Electron Toolkit](https://github.com/alex8088/electron-toolkit)
