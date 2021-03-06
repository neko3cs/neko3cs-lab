package main

import (
	"flag"
	"log"
	"os"

	"neko3cs/jpg2png/image"
	"neko3cs/jpg2png/util"
)

/*
	変換方法フラグです。

		対応フォーマットは以下の通り。
		- 'JPG2PNG'
		- 'PNG2JPG'
*/
var convertFrom = flag.String("c", "JPG2PNG", "変換先フォーマット。'JPG2PNG', 'PNG2JPG'から指定する。") // FIXME: なぜかコマンドラインからフラグ取得出来ない

/*
	エントリーポイントです。
*/
func main() {
	flag.Parse()
	root := flag.Arg(0)

	if _, err := os.Stat(root); os.IsNotExist(err) {
		log.Fatal("directory specified can not loaded.")
	}

	filePaths := util.GetFilePaths(root)
	for _, path := range filePaths {
		img := image.GetImage(path)

		switch *convertFrom {
		case "JPG2PNG":
			img.ConvertJPG2PNG()
		case "PNG2JPG":
			img.ConvertPNG2JPG()
		default:
			log.Fatal("unsupported format specified.")
		}
	}
}
