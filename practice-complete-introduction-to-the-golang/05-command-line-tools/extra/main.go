package main

import (
	"flag"
	"image"
	"image/jpeg"
	"image/png"
	"log"
	"os"
	"path/filepath"
	"strings"
)

/*
	変換方法フラグです。

		対応フォーマットは以下の通り。
		- 'JPG2PNG'
		- 'PNG2JPG'
*/
var convertFrom = flag.String("c", "JPG2PNG", "変換先フォーマット。'JPG2PNG', 'PNG2JPG'から指定する。")

/*
	指定のパスの画像を取得します。
*/
func getImage(src string) image.Image {
	file, err := os.Open(src)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	image, _, err := image.Decode(file)
	if err != nil {
		log.Fatal(err)
	}
	return image
}

/*
	指定のJPGファイルをPNGファイルに変換します。
*/
func convertJPG2PNG(src string) {
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
func convertPNG2JPG(src string) {
	file, err := os.Create(strings.Replace(src, ".PNG", ".JPG", -1))
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	jpeg.Encode(file, getImage(src), &jpeg.Options{Quality: 100})
}

/*
	指定のスライスに指定の文字列が含まれる場合、trueを返します。
*/
func contains(slice []string, str string) bool {
	for _, v := range slice {
		if v == str {
			return true
		}
	}
	return false
}

/*
	指定のフォルダからファイルを取得します。
*/
func getFilePaths(root string) []string {
	paths := []string{}

	err := filepath.Walk(root,
		func(path string, info os.FileInfo, err error) error {
			if contains([]string{".JPG", ".PNG"}, filepath.Ext(path)) {
				paths = append(paths, path)
			}
			return nil
		})
	if err != nil {
		log.Fatal(err)
	}

	return paths
}

/*
	エントリーポイントです。
*/
func main() {
	flag.Parse()
	root := flag.Arg(0)

	if _, err := os.Stat(root); os.IsNotExist(err) {
		log.Fatal("directory specified can not loaded.")
	}

	filePaths := getFilePaths(root)
	for _, path := range filePaths {
		switch *convertFrom {
		case "JPG2PNG":
			convertJPG2PNG(path)
		case "PNG2JPG":
			convertPNG2JPG(path)
		default:
			log.Fatal("unsupported format specified.")
		}
	}
}
