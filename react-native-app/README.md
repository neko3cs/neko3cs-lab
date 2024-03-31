# React Nativeサンプルアプリ

React Nativeのサンプルアプリです。

## TODO

### react-native-macosでエラーが出る

以下のエラーが出る。
`yarn clean-all && yarn install-all`を実行してみてもダメ。

```txt
No bundle URL present.

Make sure you're running a packager server or have included a .jsbundle file in your application bundle.

RCTFatal
__28-[RCTCxxBridge handleError:]_block_invoke
_dispatch_call_block_and_release
_dispatch_client_callout
_dispatch_main_queue_drain
_dispatch_main_queue_callback_4CF
__CFRUNLOOP_IS_SERVICING_THE_MAIN_DISPATCH_QUEUE__
__CFRunLoopRun
CFRunLoopRunSpecific
RunCurrentEventLoopInMode
ReceiveNextEventCommon
_BlockUntilNextEventMatchingListInModeWithFilter
_DPSNextEvent
-[NSApplication(NSEventRouting) _nextEventMatchingEventMask:untilDate:inMode:dequeue:]
-[NSApplication run]
NSApplicationMain
main
start
```

## 技術メモ

### run-iosでのビルドエラー

以下のエラーが出る場合、iosプロジェクトで `pod install` が正常にできていない可能性があるので確認する。（※1記事参照）

```txt
error Failed to build iOS project. We ran "xcodebuild" command but it exited with error code 65. To debug build logs further, consider building your app with Xcode.app, by opening sample.xcworkspace.
```

### React Native Elementsの導入

公式の手順（※2、※3記事参照）にしたがって依存パッケージも必ず導入する。

- `yarn add react-native-vector-icons`
- `yarn add react-native-safe-area-context`

react-native-vector-iconsはインストール後各プラットフォームのフォルダーに対してFontsの配置をおこなう必要がある。

### react-native-macOSの追加

以下のコマンドでmacOSに対応させる。

```zsh
npx react-native-macos-init --overwrite
```

| オプション     | 説明                                                          |
| ------------- | ------------------------------------------------------------ |
| `--overwrite` | 既存プロジェクトにmacOS対応用のソースやパッケージを追加するようにする。 |

### react-native-windowsの追加

以下のコマンドでWindowsに対応させる。

```zsh
npx react-native-windows-init --language 'cs' --namespace '<NameSpace>' --overwrite
```

| オプション     | 説明                                                                                           |
| ------------- | --------------------------------------------------------------------------------------------- |
| `--language`  | React NativeのWindows対応にはUWPが使われている。このオプションでUWPプロジェクトの言語を指定できる。       |
| `--namespace` | 名前空間を指定できる。デフォルトだとなぜかプロジェクト名の全部小文字になるのでここで改めて指定するといいと思う。 |
| `--overwrite` | 既存プロジェクトにWindows対応用のソースやパッケージを追加するようにする。                                |

## 参考文献

- [React(TypeScript)の復習として簡単なTodoアプリを作ってみた話](https://zenn.dev/grazie/articles/cfb43e4b81a152)
- ※1: [React Native iOS で run-ios 出来ない時のエラー - ねこさんのぶろぐ](https://www.neko3cs.net/entry/error-on-react-native-ios-and-cocoapods)
- ※2: [React Native Elements](https://reactnativeelements.com/docs/installation)
- ※3: [oblador/react-native-vector-icons: Customizable Icons for React Native with support for image source and full styling.](https://github.com/oblador/react-native-vector-icons?tab=readme-ov-file#installation)
- [React Native for Windows + macOS · Build native Windows & macOS apps with Javascript and React](https://microsoft.github.io/react-native-windows/)
