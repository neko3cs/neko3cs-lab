package main

import (
	"flag"
	"fmt"
	"image"
	_ "image/jpeg" // image/jpegをimportしないとimage.Decodeでコケる
	"image/png"
	"log"
	"os"
	"path/filepath"
	"strings"
)

var filePath = flag.String("filePath", "", "ファイルパス")

func main() {
	flag.Parse()

	jpgFile, err := os.Open(*filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer jpgFile.Close()

	image, _, err := image.Decode(jpgFile)
	if err != nil {
		log.Fatal(err)
	}

	pngFileName := strings.Replace(filepath.Base(*filePath), ".JPG", ".PNG", -1)
	fmt.Println("PNG Name: " + pngFileName)

	pngFile, err := os.Create(pngFileName)
	if err != nil {
		log.Fatal(err)
	}
	defer pngFile.Close()

	png.Encode(pngFile, image)
}
