package main

import (
	"flag"
	"log"
	"os"

	"neko3cs/jpg2png/imgconv"
	"neko3cs/jpg2png/util"
)

/*
	変換方法フラグです。

		対応フォーマットは以下の通り。
		- 'JPG2PNG'
		- 'PNG2JPG'
*/
var convertFrom = flag.String("c", "JPG2PNG", "変換先フォーマット。'JPG2PNG', 'PNG2JPG'から指定する。")

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
		switch *convertFrom {
		case "JPG2PNG":
			imgconv.ConvertJPG2PNG(path)
		case "PNG2JPG":
			imgconv.ConvertPNG2JPG(path)
		default:
			log.Fatal("unsupported format specified.")
		}
	}
}
