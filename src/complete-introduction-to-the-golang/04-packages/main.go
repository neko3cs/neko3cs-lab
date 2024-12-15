package main

import (
	"fmt"
	"time"

	// 先にgo get {URL}でパッケージを取得しておく
	// go mod tidyはビルド前に行う
	"github.com/tenntenn/greeting/v2"
)

func main() {
	fmt.Println(greeting.Do(time.Now()))
}
