# Spec: mainプロセスのmockにおける型エラーの修正

## 概要
`src/main/__mocks__/electron.ts` において、Vitestの `vi.fn()` によって推論された型が原因で発生しているTypeScriptエラー（TS2742）を修正します。

## ゴール
- `src/main/__mocks__/electron.ts` 内のすべてのエクスポートに対して明示的な型アノテーションを追加する。
- `pnpm typecheck` がエラーなしでパスすることを確認する。

## 詳細
- `app`, `shell`, `BrowserWindow`, `ipcMain`, `dialog` の各オブジェクトおよび関数に対して型を定義する。
- 必要に応じて `vitest` から `Mock` などの型をインポートして使用する。
