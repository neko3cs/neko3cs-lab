package imgconv

import (
	"image"
	"image/jpeg"
	"image/png"
	"io"
	"log"
	"os"
	"strings"
)

/*
	指定のパスの画像を取得します。
*/
func getImage(src string) image.Image {
	file, err := os.Open(src)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	file.Seek(0, io.SeekStart)
	image, _, err := image.Decode(file)
	if err != nil {
		log.Fatal(err)
	}
	return image
}

/*
	指定のJPGファイルをPNGファイルに変換します。
*/
func ConvertJPG2PNG(src string) {
	file, err := os.Create(strings.Replace(src, ".JPG", ".PNG", -1))
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	png.Encode(file, getImage(src))
}

/*
	指定のPNGファイルをJPGファイルに変換します。
*/
func ConvertPNG2JPG(src string) {
	file, err := os.Create(strings.Replace(src, ".PNG", ".JPG", -1))
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	jpeg.Encode(file, getImage(src), &jpeg.Options{Quality: 100})
}
