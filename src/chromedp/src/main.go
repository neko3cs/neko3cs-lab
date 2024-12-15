package main

import (
	"context"
	"fmt"
	"log"
	"runtime"

	"github.com/chromedp/chromedp"
)

func main() {
	var chromePath string
	switch runtime.GOOS {
	case "windows":
		chromePath = `C:\Program Files\Google\Chrome\Application\chrome.exe`
	case "darwin":
		chromePath = `/Applications/Google Chrome.app/Contents/MacOS/Google Chrome`
	default:
		log.Fatal("unsupported OS")
	}

	opts := append(chromedp.DefaultExecAllocatorOptions[:],
		chromedp.ExecPath(chromePath),
		chromedp.Flag("headless", true),
		chromedp.Flag("disable-gpu", true),
	)
	allocCtx, cancel := chromedp.NewExecAllocator(context.Background(), opts...)
	defer cancel()
	ctx, cancel := chromedp.NewContext(allocCtx, chromedp.WithLogf(log.Printf))
	defer cancel()

	var results []string
	err := chromedp.Run(ctx,
		chromedp.Navigate(`https://www.google.com/`),
		chromedp.SendKeys(`textarea[name=q]`, "Golang chromedp tutorial", chromedp.ByQuery),
		chromedp.SendKeys(`input[name=btnK]`, "\n", chromedp.ByQuery),
		chromedp.WaitVisible(`#search`, chromedp.ByID),
		chromedp.Evaluate(`Array.from(document.querySelectorAll('h3')).map(e => e.innerText)`, &results),
	)
	if err != nil {
		log.Fatalf("Chrome起動エラー: %v", err)
	}

	fmt.Println("検索結果:")
	for i, result := range results {
		fmt.Printf("%d: %s\n", i+1, result)
	}
}
