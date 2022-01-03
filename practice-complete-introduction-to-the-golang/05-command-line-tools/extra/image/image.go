package image

import (
	"image"
	"image/jpeg"
	"image/png"
	"io"
	"log"
	"os"
	"strings"
)

type Img struct {
	SrcPath string
	Value   image.Image
}

/*
	指定のパスの画像を取得します。
*/
func GetImage(src string) Img {
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
	return Img{SrcPath: src, Value: image}
}

/*
	指定のJPGファイルをPNGファイルに変換します。
*/
func (img *Img) ConvertJPG2PNG() {
	file, err := os.Create(strings.Replace(img.SrcPath, ".JPG", ".PNG", -1))
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	png.Encode(file, img.Value)
}

/*
	指定のPNGファイルをJPGファイルに変換します。
*/
func (img *Img) ConvertPNG2JPG() {
	file, err := os.Create(strings.Replace(img.SrcPath, ".PNG", ".JPG", -1))
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	jpeg.Encode(file, img.Value, &jpeg.Options{Quality: 100})
}
