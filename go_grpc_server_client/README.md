# Go gRPC 学習サンプル

このプロジェクトは、Go言語を用いたgRPCの実装方法（Hello WorldおよびToDoアプリ）を学習するためのサンプルコードです。

## 目次

- [gRPCとは](#grpcとは)
- [gRPCをGoで使うためには](#grpcをgoで使うためには)
- [protoファイルの書き方](#protoファイルの書き方)
- [protocの使い方](#protocの使い方)
- [サーバーサイドの実装方法](#サーバーサイドの実装方法)
- [クライアントサイドの実装方法](#クライアントサイドの実装方法)
- [実行方法](#実行方法)

---

## gRPCとは

gRPC (Google Remote Procedure Call) は、Googleが開発したオープンソースの高性能なRPCフレームワークです。

- **Protocol Buffers**: データのシリアライズにProtocol Buffers (.proto) を使用します。
- **HTTP/2**: 通信プロトコルにHTTP/2を採用しており、双方向ストリーミングやヘッダー圧縮などにより高速な通信が可能です。
- **言語中立**: IDL（インターフェイス定義言語）を用いてサービスを定義するため、異なるプログラミング言語間での連携が容易です。

## gRPCをGoで使うためには

GoでgRPCを開発するには、以下のツールが必要です。

1. **Protocol Buffers コンパイラ (`protoc`)**: `.proto` ファイルをコンパイルする本体。
2. **Go用プラグイン**:

   ```bash
   go install google.golang.org/protobuf/cmd/protoc-gen-go@latest
   go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@latest
   ```

3. **依存ライブラリ**:

   ```bash
   go get google.golang.org/grpc
   ```

## protoファイルの書き方

`.proto` ファイルで「どんなサービスがあり、どんなデータをやり取りするか」を定義します。

```proto
syntax = "proto3"; // バージョンの指定

package todo; // パッケージ名

// 生成されるGoコードのインポートパスを指定
option go_package = "example.com/hello-grpc/src/proto/todo";

// サービス（関数群）の定義
service TodoService {
  rpc CreateTodo (CreateTodoRequest) returns (CreateTodoResponse);
}

// メッセージ（データ構造）の定義
message Todo {
  string id = 1;
  string title = 2;
  bool done = 3;
}
```

## protocの使い方

定義した `.proto` からGoのコードを生成します。

```bash
protoc --go_out=. --go_opt=paths=source_relative \
       --go-grpc_out=. --go-grpc_opt=paths=source_relative \
       src/proto/todo/todo.proto
```

- `--go_out`: メッセージ（型定義）の出力先。
- `--go-grpc_out`: サービス（サーバー/クライアントの雛形）の出力先。

## サーバーサイドの実装方法

1. 生成された `UnimplementedXXXServer` を埋め込んだ構造体を作成します。
2. サービスメソッドを実装します。
3. `grpc.NewServer()` でサーバー本体を作成します。
4. `RegisterXXXServer(s, &myServer{})` で実装を登録します。
5. TCPリスナーを作成し、`s.Serve(lis)` で待ち受けを開始します。

## クライアントサイドの実装方法

1. `grpc.NewClient` でサーバーへのコネクションを作成します。
2. 生成された `NewXXXClient(conn)` でクライアントインスタンスを作成します。
3. リクエストごとに `context.WithTimeout` でタイムアウト付きのコンテキストを作成します。
4. **重要**: `defer cancel()` を用いて、関数終了時にコンテキストを確実に破棄します（リソースリーク防止）。
5. クライアントのメソッドを呼び出します。

## 実行方法

### 1. ビルド

プロジェクトルートにある `build.sh` を使用してバイナリを生成します。

```bash
./build.sh
```

バイナリは `build/` ディレクトリに生成されます。

### 2. サーバーの起動

```bash
./build/server
```

### 3. クライアントの実行

別のターミナルで実行します。

```bash
./build/client
```

### 4. テストの実行

```bash
go test ./src/...
```
