# Plan: 基本的なファイル操作の実装

## フェーズ 1: ファイル読み込み機能の実現 [checkpoint: fb9280d]

- [x] Task: `ipcMain.on` ハンドラの準備 (f96f8eb)
    - [ ] メインプロセス (`main/index.ts`) にて、ファイルのオープン要求 (`open-file`) をリッスンする`ipcMain.on`ハンドラを定義する。
    - [ ] ハンドラ内で`dialog.showOpenDialog`を呼び出し、ユーザーにファイル選択を促す。
    - [ ] 選択されたファイルのパスを返す。
- [x] Task: ファイル読み込みロジックの実装 (f96f8eb)
    - [ ] `ipcMain.on`ハンドラ内で、Node.jsの`fs`モジュールを使用して選択されたファイルを非同期で読み込む。
    - [ ] 読み込んだファイルの内容をレンダラープロセスに安全に返送する。
- [x] Task: レンダラープロセスからのファイルオープン呼び出し (f96f8eb)
    - [ ] レンダラープロセス (`renderer/src/App.tsx`など) に、`ipcRenderer.invoke('open-file')`を呼び出すボタンまたはメニュー項目を実装する。
    - [ ] 返されたファイル内容をReactのステートに保存し、エディタコンポーネントに表示する。
- [x] Task: Conductor - User Manual Verification 'ファイル読み込み機能の実現' (Protocol in workflow.md)

## フェーズ 2: テキスト編集と新規保存機能の実装 [checkpoint: f53544e]

- [x] Task: エディタコンポーネントの準備 (37e81df)
    - [ ] テキスト内容を保持し、変更をリアルタイムで反映するReactのエディタコンポーネントを実装する。
- [x] Task: `ipcMain.on` ハンドラ（保存）の準備 (20b2912)
    - [ ] メインプロセス (`main/index.ts`) にて、ファイルの保存要求 (`save-file`) をリッスンする`ipcMain.on`ハンドラを定義する。
    - [ ] ハンドラ内で`dialog.showSaveDialog`を呼び出し、ユーザーに保存場所とファイル名選択を促す。
- [x] Task: ファイル書き込みロジックの実装 (20b2912)
    - [ ] `ipcMain.on`ハンドラ内で、Node.jsの`fs`モジュールを使用してレンダラープロセスから受け取った内容を指定されたファイルに非同期で書き込む。
    - [ ] 書き込みの成功または失敗をレンダラープロセスに安全に返送する。
- [x] Task: レンダラープロセスからのファイル保存呼び出し (aa11d3e)
    - [ ] レンダラープロセス (`renderer/src/App.tsx`など) に、`ipcRenderer.invoke('save-file', content, filePath)`を呼び出すボタンまたはメニュー項目を実装する。
    - [ ] 保存が成功した場合のUIフィードバック（例: 「保存しました」メッセージ）を表示する。
- [x] Task: Conductor - User Manual Verification 'テキスト編集と新規保存機能の実装' (Protocol in workflow.md)
