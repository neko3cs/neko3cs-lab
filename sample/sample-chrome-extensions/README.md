# sample-chrome-extensions

Chrome拡張機能勉強用のサンプルアプリ

## メモ

- `content_scripts`
  - Webページの中に挿入するCSS, JavaScriptのこと
  - セキュリティの観点からDOMアクセスは可能だが既存の変数や関数にはアクセス出来ない？
  - Chrome APIも使えるものは限られている
  - manifest.jsonに定義する
- `background`
  - 常にバックグラウンドで動いているページ
  - 拡張機能を一元管理する目的で使う
  - Chrome APIをフルで使える
  - manifest.jsonに定義する
- 特定のサイトに処理を拡張する場合は `content_scripts` を使う
  - `matches` プロパティで対象のサイトのURLを指定する
  - `js` プロパティで実行するスクリプトを指定する
    - 処理は画面描画後（window#load）に実行されるため、`window.addEventListener('load', event);` で初期処理を追加する
  - ボタンの処理を中断するには `Event#stopPropagation` 関数を使ってバブリングを止める
    - `addEventListener` で設定したイベントをリッスンしているタイミングでは `document` までバブルアップしてしまう
    - このため、呼び出すイベント関数は `capture: true` を指定して設定しておく必要がある

## 参考サイト

- [API Reference - Chrome Developers](https://developer.chrome.com/docs/extensions/reference/)
- [Chrome拡張機能の作り方。誰でもかんたんに開発できる！](https://original-game.com/how-to-make-chrome-extensions/)
- [Chrome拡張でとっても役立つAPIのまとめ - Qiita](https://qiita.com/Yuta_Fujiwara/items/daf41429f95caec82982)
- [javascript-Chrome拡張機能のコンテンツスクリプトでstopPropagationが機能しない-スタックオーバーフロー](https://stackoverflow.com/questions/60551603/stoppropagation-not-working-in-content-script-of-chrome-extension)
