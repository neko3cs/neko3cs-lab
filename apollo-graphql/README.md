# Apollo Server & Client - GraphQL Learning Project

このプロジェクトは、GraphQLの基礎と、Apollo Server / Apollo Clientを使用したモダンな開発手法を学ぶためのサンプルアプリケーションです。
コーヒーショップのメニュー管理を題材に、スキーマ定義からフロントエンドでのデータ表示までの一連の流れを体験できます。

## 🚀 クイックスタート

このプロジェクトは `pnpm` を使用したモノレポ構成になっています。

### 1. 依存関係のインストール

```bash
pnpm install
```

### 2. 開発サーバーの起動

サーバー（バックエンド）とクライアント（フロントエンド）の両方が同時に起動します。

```bash
pnpm run dev
```

- **Frontend**: [http://localhost:3000](http://localhost:3000)
- **Backend (Apollo Sandbox)**: [http://localhost:4000](http://localhost:4000)

---

## 📖 学習ガイド

### 1. GraphQL の基本構造

GraphQLは、APIのためのクエリ言語であり、型システムを用いてデータを定義します。

#### 基本文法

- **型定義 (Type Definition)**: データの形を定義します。

  ```graphql
  type Coffee {
    id: ID! # ! は必須項目（Non-nullable）
    name: String! # 文字列型
    price: Int! # 整数型
    options: [String] # リスト型
  }
  ```

- **クエリ (Query)**: データを取得するためのリクエストです。クライアントが必要なフィールドだけを指定します。

  ```graphql
  query GetCoffee {
    coffees {
      # 取得したいフィールドを指定
      name
      price
    }
  }
  ```

- **引数 (Arguments)**: 特定のデータを絞り込む際に使用します。

  ```graphql
  query {
    coffee(id: "c001") {
      name
    }
  }
  ```

#### GraphQL のメリット

- **Overfetchingの防止**: RESTではエンドポイントが返すデータが決まっていますが、GraphQLでは必要な項目（例：名前だけ）だけを取得できるため、通信量を削減できます。
- **Underfetchingの防止**: 1回の工程で複数の関連データ（例：コーヒーとそのオプション詳細）を取得できるため、何度もリクエストを送る必要がありません。
- **型安全**: スキーマによってデータの型が保証されるため、開発時のエラーを減らすことができます。

### 2. サーバーサイド (Apollo Server)

`src/server` ディレクトリに実装されています。

- **スキーマ定義 (`schema.graphql`)**:
  GraphQLの型定義を行います。コメント（`"""`）を記述することで、Apollo Sandbox上でドキュメントとして表示されます。
- **リゾルバーの実装 (`main.ts`)**:
  スキーマで定義したフィールドと、実際のデータ（`src/server/data/coffee.json`）を紐付けます。

### 3. クライアントサイド (Apollo Client)

`src/client` ディレクトリに実装されています。

- **Clientの設定 (`App.tsx`)**:
  `ApolloClient` インスタンスを作成し、接続先URL（`http://localhost:4000`）を指定します。`ApolloProvider` でアプリをラップすることで、コンポーネント内からGraphQLを扱えるようになります。
- **データ取得 (`queries.ts` & `App.tsx`)**:
  `gql` タグを使用してクエリを定義し、`useQuery` フックでデータを取得します。loadingやerrorの状態を簡単に扱えるのが特徴です。

---

## 📁 ディレクトリ構造

```text
apollo-graphql/
├── src/
│   ├── client/           # フロントエンド (React + Vite)
│   │   ├── App.tsx       # メインコンポーネント (ApolloProvider, useQuery)
│   │   ├── queries.ts    # GraphQL クエリ定義
│   │   └── style.css     # スタイリング (Tailwind CSS)
│   └── server/           # バックエンド (Apollo Server)
│       ├── main.ts       # サーバー起動設定・リゾルバー実装
│       ├── schema.graphql # GraphQL スキーマ定義
│       └── data/         # JSONデータ (データベースの代わり)
└── package.json          # 全体の起動スクリプト
```

## 🛠 使用技術

- **Language**: TypeScript
- **Backend**: Apollo Server, GraphQL
- **Frontend**: React, Apollo Client, Vite, Tailwind CSS
- **Package Manager**: pnpm
