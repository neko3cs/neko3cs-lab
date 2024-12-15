package util

import (
	"log"
	"os"
	"path/filepath"
)

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
func GetFilePaths(root string) []string {
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
