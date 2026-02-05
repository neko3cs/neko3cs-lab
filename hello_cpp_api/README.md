# Hello C++ API Project

## 1. このプロジェクトについて

このプロジェクトは、C++の高性能Webフレームワークである [Drogon](https://github.com/drogonframework/drogon) を使用したWeb APIのサンプル実装です。
ビルドシステムには [xmake](https://xmake.io/) を採用し、C++20標準で記述されています。

コード全体を通して **Google C++ Style Guide** に準拠するように構成されています。

主な機能は以下の通りです：

- **Hello API**: シンプルなGETリクエストに対する応答 (`/hello`)
- **Todo API**: TodoリストのCRUD操作を提供するRESTful API (`/todos`)
  - データはメモリ上 (`std::map`) に保持されます（永続化はされません）。
- **統合テスト**: [GoogleTest](https://github.com/google/googletest) を用いて、実際にサーバーを起動してリクエストを送信するテストコードが含まれています。

### ファイル構成

Google Style Guideに従い、ファイル名は `snake_case`、拡張子は `.cc` および `.h` を使用しています。

- `src/main.cc`: アプリケーションのエントリポイント。サーバーの設定と起動を行います。
- `src/hello_api.{h,cc}`: `HttpSimpleController` を継承したシンプルなコントローラー。
- `src/todo_api.{h,cc}`: `HttpController` を継承したRESTfulコントローラー。
- `test/test_main.cc`: テスト用のカスタムエントリポイント。
- `test/hello_api_test.cc`: Hello APIの統合テスト。
- `test/todo_api_test.cc`: Todo APIの統合テスト。
- `xmake.lua`: ビルド設定ファイル。

## 2. 統合テストとDrogonのライフサイクル

Drogonの `drogon::app()` はプロセス内でシングルトンとして振る舞い、一度 `run()` して `quit()` させると、同じプロセス内での再起動ができません。

そのため、テストターゲットを1つに統合した本プロジェクトでは、`test/test_main.cc` において **GoogleTestのグローバル環境 (`testing::Environment`)** を使用しています。

- テスト実行の開始時に一度だけDrogonサーバーを別スレッドで起動します。
- すべてのテストスイートが完了した後に `quit()` を呼び出します。
- これにより、各テストファイル内で個別にサーバーを管理することによる「二重起動」や「再起動不可」のエラーを回避しています。

## 3. xmakeの設定について

### コマンド例

- **ビルド**:

  ```bash
  xmake
  ```

- **実行**:

  ```bash
  xmake run hello_cpp_api
  ```

- **テスト実行**:

  ```bash
  xmake run test
  ```

- **コンパイルデータベース生成** (LSP用）:

  ```bash
  xmake project -k compile_commands
  ```

## 4. Drogonの使い方について

### コントローラーの種類

1. **HttpSimpleController** (`HelloApi`で使用）
   - 単一のパスに対して処理を割り当てるのに適しています。
   - `PATH_LIST_BEGIN` ～ `PATH_LIST_END` 内でルーティングを定義します。

2. **HttpController** (`TodoApi`で使用）
   - RESTful APIのように、同じリソースに対して複数のメソッド（GET, POST, PUT, DELETE）やパスパラメーターを定義するのに適しています。
   - `METHOD_LIST_BEGIN` ～ `METHOD_LIST_END` 内で `ADD_METHOD_TO` マクロを使用します。

### コーディング規約

Google Style Guideに基づき、以下の命名規則を採用しています。

- **メソッド名**: `PascalCase` (`GetTodos`, `Create` など）
- **変数名**: `snake_case`
- **プライベートメンバー変数**: 末尾アンダースコア付き `snake_case_` (`todo_storage_` など）
- **名前空間**: `using namespace` を避け、明示的に `drogon::` などを使用。
