# practive-react-native-windows

React Native Windows 学習用 Repository

適当に簡単なアプリでも作って試してみる。

## メモ

### プロジェクトの生成方法

以下のコマンドで ReactNative アプリケーションを生成する。

```pwsh
npx react-native init <projectName> `
--directory <sourceDirectoryName> `
--template react-native-template-typescript
```

| オプション    | 説明                                                                                          |
| ------------- | --------------------------------------------------------------------------------------------- |
| `--directory` | プロジェクトのディレクトリ名を指定できる。`src` とかにしたいときに使う。                      |
| `--template`  | テンプレート指定する。TypeScript を使う場合は `react-native-template-typescript` を指定する。 |

デフォルトだと iOS と Android のみなので、以下のコマンドで Windows に対応させる。

```pwsh
npx react-native-windows-init `
--language "cs" `
--namespace "<NameSpace>" `
--overwrite
```

| オプション    | 説明                                                                                                             |
| ------------- | ---------------------------------------------------------------------------------------------------------------- |
| `--language`  | ReactNative の Windows 対応には UWP が使われている。このオプションで UWP プロジェクトの言語を指定できる。        |
| `--namespace` | 名前空間を指定できる。デフォルトだとなぜかプロジェクト名の全部小文字になるのでここで改めて指定するといいと思う。 |
| `--overwrite` | 既存プロジェクトに Windows 対応用のソースやパッケージを追加するようにする。                                      |

### RNDatetimePicker

導入時、WindowsではAutolinkがサポートされていないため、手動で追加する必要がある。

[datetimepicker/manual-installation.md at master · react-native-datetimepicker/datetimepicker](https://github.com/react-native-datetimepicker/datetimepicker/blob/master/docs/manual-installation.md)
