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

var dirPath = flag.String("dir", "", "JPGイメージのあるフォルダパス")

func convertJPG2PNG(jpgFilePath string) {
	jpgFile, err := os.Open(jpgFilePath)
	if err != nil {
		log.Fatal(err)
	}
	defer jpgFile.Close()

	image, _, err := image.Decode(jpgFile)
	if err != nil {
		log.Fatal(err)
	}

	pngFilePath := strings.Replace(jpgFilePath, ".JPG", ".PNG", -1)
	fmt.Println("PNG filepath: " + pngFilePath)

	pngFile, err := os.Create(pngFilePath)
	if err != nil {
		log.Fatal(err)
	}
	defer pngFile.Close()

	png.Encode(pngFile, image)
}

func getJPGFilePaths(targetDirPath string) []string {
	jpgFilePaths := []string{}

	err := filepath.Walk(*dirPath,
		func(path string, info os.FileInfo, err error) error {
			if filepath.Ext(path) == ".JPG" {
				jpgFilePaths = append(jpgFilePaths, path)
			}
			return nil
		})
	if err != nil {
		log.Fatal(err)
	}

	return jpgFilePaths
}

func main() {
	flag.Parse()

	jpgFilePaths := getJPGFilePaths(*dirPath)
	for _, path := range jpgFilePaths {
		convertJPG2PNG(path)
	}
}
