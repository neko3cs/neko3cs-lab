# Angular + Electron (TypeScript) Setup

Angularアプリを **Electronデスクトップアプリ**として動作させる最小構成。
開発モードでは **Angular dev server + Electron を同時起動**します。

ElectronはWebアプリをデスクトップアプリとして実行できるフレームワークです。([GeeksforGeeks][1])

---

# Requirements

- Node.js 20+
- pnpm
- Angular CLI

```bash
pnpm add -g @angular/cli
```

---

# 1. Angular プロジェクト作成

```bash
ng new electron-angular-app
cd electron-angular-app
```

---

# 2. Electron関連パッケージ

```bash
pnpm add -D \
electron \
electron-builder \
concurrently \
wait-on \
cross-env \
typescript \
@types/node
```

---

# 3. Electron用ディレクトリ作成

```text
src-electron/
  main.ts
  preload.ts
```

---

# 4. Electron main process

`src-electron/main.ts`

```ts
import { app, BrowserWindow, ipcMain } from 'electron';
import path from 'path';

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
    },
  });

  if (process.env.NODE_ENV === 'dev') {
    win.loadURL('http://localhost:4200');
  } else {
    win.loadFile(path.join(__dirname, '../dist/electron-angular-app/browser/index.html'));
  }
}

app.whenReady().then(createWindow);

ipcMain.handle('ping', async () => {
  return 'pong';
});
```

---

# 5. preload

`src-electron/preload.ts`

```ts
import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
  ping: () => ipcRenderer.invoke('ping'),
});
```

---

# 6. Electron TypeScript config

`tsconfig.electron.json`

```json
{
  "compilerOptions": {
    "outDir": "dist-electron",
    "module": "commonjs",
    "target": "es2020",
    "types": ["node"],
    "esModuleInterop": true
  },
  "include": ["src-electron/**/*.ts"]
}
```

---

# 7. Angular側 型定義

`src/types/preload.d.ts`

```ts
export {};

declare global {
  interface Window {
    electronAPI: {
      ping: () => Promise<string>;
    };
  }
}
```

---

# 8. package.json scripts

```json
{
  "main": "dist-electron/main.js",

  "scripts": {
    "start": "ng serve",

    "build:electron": "tsc -p tsconfig.electron.json",

    "electron:dev": "concurrently \"ng serve\" \"wait-on http://localhost:4200 && cross-env NODE_ENV=dev pnpm build:electron && electron .\"",

    "build": "ng build && pnpm build:electron",

    "electron:build": "pnpm build && electron-builder"
  }
}
```

---

# 9. Electron Builder設定

`package.json`

```json
{
  "build": {
    "appId": "com.example.electronangular",
    "files": ["dist/**/*", "dist-electron/**/*", "package.json"],
    "directories": {
      "buildResources": "build"
    },
    "mac": {
      "target": "dmg"
    },
    "win": {
      "target": "nsis"
    },
    "linux": {
      "target": "AppImage"
    }
  }
}
```

---

# 10. 開発起動

```bash
pnpm electron:dev
```

起動フロー

```text
Angular dev server
        ↓
http://localhost:4200
        ↓
Electron BrowserWindow
```

---

# 11. ビルド

```bash
pnpm electron:build
```

生成

```text
dist/
dist-electron/
release/
```

OSごとの実行ファイル

```text
Mac    → dmg
Windows → exe
Linux   → AppImage
```

---

# プロジェクト構造

```text
.
├─ src/                # Angular
├─ src-electron/       # Electron main/preload
│   ├─ main.ts
│   └─ preload.ts
├─ dist/               # Angular build
├─ dist-electron/      # Electron build
├─ angular.json
├─ package.json
└─ tsconfig.electron.json
```

---

# IPC Example

Angular

```ts
const result = await window.electronAPI.ping();
```

Electron

```ts
ipcMain.handle('ping', async () => 'pong');
```

---

# 開発構成

```text
Renderer  → Angular
Main      → Electron
Bridge    → preload
```

---
