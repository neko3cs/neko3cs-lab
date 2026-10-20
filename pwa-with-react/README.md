# BeforeInstallPrompt 動作確認PWA

React と Vite で作った、`beforeinstallprompt` を使う PWA のインストールとオフライン動作を試す小さなアプリです。

## 今回の学習要点

**一番確かめたかったことは、Web ページ内の独自ボタンから PWA のインストール画面を開けるか**です。対応するブラウザーでは、次の流れで実現できます。

1. ブラウザーが manifest などを見てインストール可能と判断すると、`beforeinstallprompt` イベントを発火します。アプリ側からこのイベントを強制的に発火させることはできません。
2. アプリはイベントで `preventDefault()` を呼び、イベントオブジェクトを保存して独自ボタンを有効にします。
3. ユーザーがボタンを押したとき、保存したイベントの `prompt()` を呼ぶとブラウザーのインストール確認画面が開きます。実際にインストールするかはユーザーが選びます。`prompt()` を呼べるのは、そのイベントにつき一度だけです。

`beforeinstallprompt` が発火しないことは、「インストール済み」を意味しません。未対応のブラウザーやインストール条件を満たさない場合にも発火しないためです。対応ブラウザーでは `getInstalledRelatedApps()` を使い、確認できた場合だけ「アプリインストール済み」と表示します。

**Service Worker が有効であることと、オフラインで使えることは別です。** `pnpm dev` ではインストール操作を試せますが、オフラインでの再読み込みは `pnpm build` → `pnpm preview` で確かめます。カードの枚数は `localStorage` に保存し、リセットでは値を 0 に書き換える代わりにキーを削除します。

参考: [MDN: インストール画面を開く](https://developer.mozilla.org/en-US/docs/Web/Progressive_web_apps/How_to/Trigger_install_prompt)、[Vite PWA: 開発モード](https://vite-pwa-org.netlify.app/guide/development)、[Chrome: インストール済みアプリの検知](https://developer.chrome.com/docs/capabilities/get-installed-related-apps)

## 起動

Node.js と pnpm を用意し、このディレクトリで実行します。

```sh
pnpm install
pnpm dev
```

表示された `http://localhost:5173/` を Chrome または Edge で開いてください。`pnpm dev` でも manifest と開発用 Service Worker を配信する設定です。インストール操作を試せます。

オフライン動作は本番ビルドで確認します。

```sh
pnpm build
pnpm preview
```

この場合は `http://localhost:4173/` を開いてください。開発用 Service Worker のキャッシュは、本番ビルドのオフライン動作を再現しません。

## 確認手順

1. 初回表示後、「Service Worker」が「有効」になるまで待つか、一度再読み込みします。
2. 「アプリをインストール」が有効なら押して、ブラウザーのインストール画面を確認します。Edge ではアドレスバーのアプリのアイコン、または「…」→「その他のツール」→「アプリ」からもインストールできます。インストール後は OS のアプリ一覧から開き、「表示モード」が「アプリ画面」になることを確認します。
3. 「カードを追加」を押します。ブラウザーの開発者ツールの Network で Offline を選ぶか、ネットワークを切って通常の再読み込みをします。画面とカードの数が残ることを確認します。
4. 「カードをリセット」を押し、表示が 0 枚になり、開発者ツールの Application → Local storage から `pwa-lab-card-count` キーが消えることを確認します。

`beforeinstallprompt` は対応ブラウザーで、ブラウザーがインストール可能と判断したときだけ発火します。既にインストール済みの場合など、ボタンが有効にならないこともあります。その場合はブラウザーのメニューにある「インストール」や「ホーム画面に追加」を確認してください。iPhone/iPad では共有メニューからホーム画面に追加します。スマートフォンから試す場合は HTTPS で公開する必要があります（`localhost` へのアクセスは例外）。

インストール済みかどうかは、対応ブラウザーでは `getInstalledRelatedApps()` で確認します。デスクトップの Edge/Chrome 140 以降で利用できます。対応していない場合、取得に失敗した場合、または結果が空の場合は、未インストールと断定せず「インストール状態を確認できません」と表示します。検証用の manifest は既定の `localhost:5173`（開発）と `localhost:4173`（プレビュー）に合わせているため、別のホスト名やポートでは判定できないことがあります。

カードの数は `localStorage` に保存されます。リセットでは `pwa-lab-card-count` キーを削除します。画面自体のオフライン表示は Service Worker のキャッシュによるものです。
