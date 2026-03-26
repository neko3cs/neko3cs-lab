# C++ 11 to 23 Travel

このプロジェクトは、C++11からC++23までの新機能や文法を章ごとに学ぶためのサンプルコード集です。

## 開発環境

- **ビルドツール**: [xmake](https://xmake.io/)
- **コンパイラ**: LLVM/Clang (C++23対応）
- **言語標準**: C++23

## 章ごとの解説

### Chapter 1: 標準出力の進化 (C++23)

`std::print` および `std::println` を使用した、型安全で直感的なフォーマット出力。

- `std::println("Hello, {}!", "World");` のように、Pythonの `format` やRustの `println!` に近い記述が可能です。

### Chapter 2: Ranges と Views (C++20/23)

コンテナーに対する宣言的な操作。

- パイプ演算子 (`|`) を用いて、`filter` や `transform` を繋げた処理が記述できます。
- コピーを伴わない遅延評価（View）による効率的なデータ処理が可能です。

### Chapter 3: std::optional のモナド操作 (C++23)

`std::optional` に対する関数型プログラミング的なアプローチ。

- `transform`: 値が存在する場合に変換処理を適用。
- `and_then`: 値が存在する場合に次の `optional` を返す処理を連結。
- `value_or`: 値がない場合のデフォルト値を指定。

### Chapter 4: std::expected (C++23)

「値」または「エラー」を保持する新しいエラーハンドリング機構。

- 従来の例外やエラーコードに代わる、明示的で安全なエラー処理を可能にします。
- `std::unexpected` を用いてエラー状態を表現します。

### Chapter 5, 6, 7

（現在実装準備中）

### Chapter 8: C++23 の高度な文法

- **Deduce this (Explicit object parameters)**: メンバー関数の最初の引数に `this` を明示的に受け取ることで、CRTPなどのパターンを簡潔に記述できます。
- **多次元添字演算子**: `operator[]` が複数の引数を受け取れるようになり、`matrix[1, 2]` のような自然な記述が可能になりました。

### Chapter 9: プリプロセッサの改善 (C++23)

- `#elifdef` および `#elifndef`: プラットフォーム依存の記述がより簡潔に、安全に記述できるようになりました。

### Chapter 10: Modules (C++20)

従来のヘッダーファイル (`#include`) に代わる新しいコード分割機構。

- `import std;` による高速な標準ライブラリの利用。
- `export module` によるインターフェイスの公開。
- ビルド時間の短縮とシンボルのカプセル化が図られています。

## ビルド・実行方法

xmakeを使用してビルドおよび実行を行います。

```bash
# ビルド
xmake

# 実行
xmake run
```

## 設定ファイル (`xmake.lua`)

C++23およびモジュールを有効にするための設定が含まれています。

- `set_languages("c++23")`
- `set_policy("build.c++.modules", true)`
