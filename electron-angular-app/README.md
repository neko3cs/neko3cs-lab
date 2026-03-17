# Angular + Electron Setup

`ng new` 直後のAngularプロジェクトを、Electronで動かせるようにする手順です。

このREADMEでは、Angularアプリを先にビルドし、その成果物をElectronから読み込む構成を作ります。`concurrently` や `wait-on` を使ったdev server連携ではなく、構成が単純なビルドベース方式です。

## 前提条件

- Node.js 20以上
- pnpm
- Angular CLI

## 1. Angular プロジェクトを作成する

```bash
ng new electron-angular-app --package-manager pnpm
cd electron-angular-app
```

以降は、この `electron-angular-app` をベースに説明します。

## 2. Electron 関連パッケージを追加する

```bash
pnpm add -D electron electron-builder typescript @types/node @types/electron
```

必要に応じてElectronのバイナリ取得用スクリプトも使えるようにしておくと便利です。

## 3. Electron 用ディレクトリを作成する

```bash
mkdir -p src-electron
touch src-electron/main.ts src-electron/preload.ts src-electron/preload.d.ts
```

最終的な構成イメージは以下です。

```text
.
├─ src/
├─ src-electron/
│  ├─ main.ts
│  ├─ preload.ts
│  └─ preload.d.ts
├─ angular.json
├─ electron-builder.yaml
├─ package.json
└─ tsconfig.electron.json
```

## 4. `src-electron/main.ts` を作成する

Electronのメインプロセスです。ビルド済みのAngularアプリを読み込みます。

```ts
import { app, BrowserWindow, ipcMain } from 'electron';
import * as path from 'path';

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      contextIsolation: true,
    },
  });

  win.loadFile(path.join(__dirname, '../dist/electron-angular-app/browser/index.html'));
}

function configureIPCHandlers() {
  ipcMain.handle('ping', () => {
    return 'pong';
  });
}

app.whenReady().then(() => {
  configureIPCHandlers();
  createWindow();
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});
```

`loadFile()` のパスにある `electron-angular-app` は、`ng new` で作成したプロジェクト名に合わせてください。

## 5. `src-electron/preload.ts` を作成する

レンダラープロセスへ安全にAPIを公開するpreloadです。

```ts
import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
  ping: () => ipcRenderer.invoke('ping'),
});
```

## 6. `src-electron/preload.d.ts` を作成する

Angular側で `window.electronAPI` を型付きで使えるようにします。

```ts
declare global {
  interface Window {
    electronAPI: {
      ping: () => Promise<string>;
    };
  }
}

export {};
```

## 7. `tsconfig.electron.json` を作成する

Electron側コードを `dist-electron/` へ出力するためのTypeScript設定です。

```json
{
  "compilerOptions": {
    "outDir": "./dist-electron",
    "module": "commonjs",
    "target": "es2020",
    "types": ["node", "electron"]
  },
  "include": ["src-electron/**/*.ts"]
}
```

## 8. `tsconfig.app.json` を更新する

Angularアプリ側からpreloadの型定義を読めるように、`include` に `src-electron/**/*.d.ts` を追加します。

```json
{
  "extends": "./tsconfig.json",
  "compilerOptions": {
    "outDir": "./out-tsc/app",
    "types": []
  },
  "include": ["src/**/*.ts", "src-electron/**/*.d.ts"],
  "exclude": ["src/**/*.spec.ts"]
}
```

## 9. `package.json` を更新する

`main` とElectron用スクリプトを追加します。既存の `start` や `test` は残したままで構いません。

```json
{
  "main": "dist-electron/main.js",
  "scripts": {
    "start": "ng serve",
    "build": "ng build",
    "test": "ng test",
    "install:electron": "node ./node_modules/electron/install.js",
    "build:angular": "ng build --base-href ./",
    "build:electron": "tsc -p tsconfig.electron.json",
    "electron:dev": "pnpm build:angular && pnpm build:electron && electron .",
    "electron:build": "pnpm build:angular && pnpm build:electron && electron-builder"
  }
}
```

ポイント:

- `build:angular` で `--base-href ./` を付けると、Electronがローカルファイルとして読み込みやすくなります。
- `electron:dev` はAngular dev serverを使わず、ビルド済みファイルを読み込んで起動します。

## 10. `electron-builder.yaml` を作成する

配布ビルド設定を `package.json` ではなく別ファイルに切り出します。

```yaml
appId: com.example.angular-electron
productName: AngularElectronApp
directories:
  output: release
files:
  - dist/**/*
  - dist-electron/**/*
win:
  target: nsis
mac:
  target: dmg
linux:
  target: AppImage
```

## 11. Angular 側から Electron API を呼ぶ

任意のAngularコンポーネントから、以下のようにpreload経由のAPIを呼べます。

```ts
const response = await window.electronAPI.ping();
console.log(response); // "pong"
```

## 12. 動作確認する

### Angular 単体で確認

```bash
pnpm start
```

### Electron アプリとして起動

```bash
pnpm electron:dev
```

実行順:

1. Angularをビルド
2. Electron用TypeScriptをビルド
3. Electronを起動

### 配布物を作成

```bash
pnpm electron:build
```

主な出力先:

```text
dist/
dist-electron/
release/
```

## 補足

- この手順は、Angular dev serverとElectronの同時起動構成ではありません。
- `main.ts` の `dist/electron-angular-app/browser/index.html` は、Angularのプロジェクト名に応じて読み替えてください。
- Electron Builderの設定は `electron-builder.yaml` に置いています。
