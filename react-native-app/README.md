# React Nativeサンプルアプリ

React Nativeのサンプルアプリです。

## 技術メモ

### run-iosでのビルドエラー

以下のエラーが出る場合、iosプロジェクトで `pod install` が正常にできていない可能性があるので確認する。（※1記事参照）

```txt
error Failed to build iOS project. We ran "xcodebuild" command but it exited with error code 65. To debug build logs further, consider building your app with Xcode.app, by opening sample.xcworkspace.
```

## 参考文献

- [React(TypeScript)の復習として簡単なTodoアプリを作ってみた話](https://zenn.dev/grazie/articles/cfb43e4b81a152)
- ※1: [React Native iOS で run-ios 出来ない時のエラー - ねこさんのぶろぐ](https://www.neko3cs.net/entry/error-on-react-native-ios-and-cocoapods)
