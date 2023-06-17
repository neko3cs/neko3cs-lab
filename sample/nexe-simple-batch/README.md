# nexe simple batch

nexeの使い方を確認するサンプルコード。

## メモ

importでモジュールを利用するTypeScriptコードをコンパイルして生成したJavaScriptファイルをnexeにかけるとエラーする。

なんかimport形式でのモジュール呼び出しがよくないっぽいので、nexeを使う場合はJavaScript直接書くか、TypeScriptでもrequireを使ってモジュールを利用する必要があるっぽい。

他に何かやりようがあるかもしれないけど、分からんかった。

## 参考

- [nexe/nexe: 🎉 create a single executable out of your node.js apps](https://github.com/nexe/nexe#compiling-node)
