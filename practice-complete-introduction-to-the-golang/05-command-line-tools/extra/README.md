# 学習メモ

## `image\*` を使って画像をデコードする時

対応先のパッケージも読み込んでおかないとエラーするので注意。

```go
import (
  // ... 中略
  // 👇このコード内では image/jpeg パッケージは使ってないが、 `image.Decode` 時に内部で使ってるっぽいので読み込む必要がある
  _ "image/jpeg"
  "image/png"
  // ... 中略
)

// ... 中略

image, _, err := image.Decode(file)
if err != nil {
  log.Fatal(err)
}

file, err := os.Create(strings.Replace(src, ".JPG", ".PNG", -1))
if err != nil {
  log.Fatal(err)
}

defer file.Close()
png.Encode(file, getImage(src))  // ここで使うため image/png パッケージは読み込んでる

// ... 中略
``
