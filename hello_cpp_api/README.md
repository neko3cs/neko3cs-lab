# Hello C++ API Project

## 1. このコードについて

このプロジェクトは、C++の高性能Webフレームワークである [Drogon](https://github.com/drogonframework/drogon) を使用したWeb APIのサンプル実装です。
ビルドシステムには [xmake](https://xmake.io/) を採用し、C++20標準で記述されています。

主な機能は以下の通りです：

- **Hello API**: シンプルなGETクエストに対する応答 (`/hello`)
- **Todo API**: TodoリストのCRUD操作を提供するRESTful API (`/todos`)
  - データはメモリ上 (`std::map`) に保持されます（永続化はされません）。
- **統合テスト**: [GoogleTest](https://github.com/google/googletest) を用いて、実際にサーバーを起動してリクエストを送信するテストコードが含まれています。

### ファイル構成

- `src/main.cpp`: アプリケーションのエントリポイント。サーバーの設定と起動を行います。
- `src/HelloApi.{h,cpp}`: `HttpSimpleController` を継承したシンプルなコントローラー。
- `src/TodoApi.{h,cpp}`: `HttpController` を継承したRESTfulコントローラー。
- `test/`: APIの統合テストコード。
- `xmake.lua`: ビルド設定ファイル。

## 2. xmakeの設定について

このプロジェクトでは、Luaベースのモダンなビルドツール `xmake` を使用しています。
`xmake.lua` の主な設定ポイントは以下の通りです。

### 依存関係の管理

`add_requires` を使用して、パッケージマネージャーからライブラリを自動的にダウンロード・ビルドします。

```lua
add_requires("drogon", "gtest")
```

### 共通設定

ターゲット間で共通の設定（C++標準、警告レベル、最適化オプションなど）をグローバルスコープ（ターゲット定義の外）に記述することで、記述の重複を避けています。

```lua
set_languages("c++20")
set_warnings("all", "extra")
-- リリース/デバッグモードに応じた最適化設定
if is_mode("release") then ... end
```

### ターゲット定義

アプリケーション本体とテスト用のターゲットを分けて定義しています。

- **`hello_cpp_api`**: メインサーバーアプリケーション。`src/*.cpp` をコンパイルします。
- **`test_...`**: テスト用実行バイナリ。`gtest_main` をリンクすることで、テストコード内に `main` 関数を書く必要をなくしています。

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
  xmake run test_hello_cpp_api
  xmake run test_todo_api
  ```

- **コンパイルデータベース生成** (LSP用）:

  ```bash
  xmake project -k compile_commands
  ```

## 3. Drogonの使い方について

このプロジェクトで見られるDrogonの主な使用パターンについて解説します。

### コントローラーの種類

Drogonにはいくつかコントローラーの基底クラスがありますが、ここでは2種類使用しています。

1. **HttpSimpleController** (`HelloApi`で使用）
   - 単一のパスに対して処理を割り当てるのに適しています。
   - `PATH_LIST_BEGIN` ～ `PATH_LIST_END` 内で `PATH_ADD("/path", ...)` を使ってルーティングを定義します。

2. **HttpController** (`TodoApi`で使用）
   - RESTful APIのように、同じリソースに対して複数のメソッド（GET, POST, PUT, DELETE）やパスパラメーターを定義するのに適しています。
   - `METHOD_LIST_BEGIN` ～ `METHOD_LIST_END` 内で `ADD_METHOD_TO` マクロを使用します。
   - 例: `ADD_METHOD_TO(TodoApi::getOne, "/todos/{id}", Get);` と定義すると、`getOne` メソッドの引数として `int id` を受け取ることができます。

### JSONレスポンス

Drogonは `JsonCpp` を統合しており、簡単にJSONを扱えます。

```cpp
// JSONオブジェクトの作成
Json::Value ret;
ret["message"] = "Hello";

// レスポンスの生成
auto resp = HttpResponse::newHttpJsonResponse(ret);
callback(resp);
```

### 非同期処理

Drogonのハンドラーは非同期スタイルで記述されます。処理が完了したら、引数で渡された `callback` 関数を呼び出してレスポンスを返します。

```cpp
void handler(const HttpRequestPtr &req, std::function<void(const HttpResponsePtr &)> &&callback) {
    // ... 処理 ...
    callback(response);
}
```

### サーバーの起動 (`main.cpp`)

`drogon::app()` を使用してリスナーを設定し、イベントループを開始します。

```cpp
int main() {
    drogon::app().addListener("0.0.0.0", 8080);
    drogon::app().run();
    return 0;
}
```
