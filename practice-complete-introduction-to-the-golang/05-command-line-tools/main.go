package main

import (
	"bufio"
	"flag"
	"fmt"
	"log"
	"os"
	"strconv"
)

var filePath = flag.String("filePath", "", "ファイルパス")
var showLineNumber = flag.Bool("n", false, "行番号を表示する")

func main() {
	flag.Parse()

	sf, err := os.Open(*filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer sf.Close()

	line := 1
	scanner := bufio.NewScanner(sf)
	for scanner.Scan() {
		if *showLineNumber {
			fmt.Fprintln(os.Stdout, strconv.Itoa(line)+": "+scanner.Text())
		} else {
			fmt.Fprintln(os.Stdout, scanner.Text())
		}
		line++
	}
}
