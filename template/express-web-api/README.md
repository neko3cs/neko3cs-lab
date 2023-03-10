# express-sample

自分用のexpressサンプルテンプレートです。

## 構築方法

以下の通りコマンドを実行する。

```sh
yarn init &&
yarn add express cors express-validator node-fetch &&
yarn add --dev typescript vite vite-plugin-node @types/node @types/express @types/cors @types/node-fetch &&
mkdir ./routes &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/tsconfig.json -o ./tsconfig.json &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/vite.config.ts -o ./vite.config.ts &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/main.ts -o ./main.ts &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/routes/coffee.ts -o ./routes/coffee.ts
```

各パッケージは以下の目的となっている。

|パッケージ名|目的|
|:--|:--|
|[express](https://github.com/expressjs/express)|Webアプリケーションフレームワークとして利用するため|
|[cors](https://github.com/expressjs/cors)|CORS対策のため|
|[express-validator](https://github.com/express-validator/express-validator)|バリデーションチェックのため|
|[node-fetch](https://github.com/node-fetch/node-fetch)|サンプルAPI呼出のため|
|[typescript](https://github.com/microsoft/TypeScript)|開発言語として利用するため|
|[vite](https://github.com/vitejs/vite)|ビルドツールとして利用するため|
|[vite-plugin-node](https://github.com/axe-me/vite-plugin-node)|viteを用いてNode.jsアプリケーションをホストするため|

※@typesは型情報なため省略

その他必要な処理は [Express middleware](https://expressjs.com/en/resources/middleware) の **Replaces built-in function** を参照して追加すること。
