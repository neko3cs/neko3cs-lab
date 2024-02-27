using PuppeteerSharp;

using var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync();
await using var browser = await Puppeteer.LaunchAsync(
  new LaunchOptions { Headless = true }
);
await using var page = await browser.NewPageAsync();
await page.GoToAsync("https://cgi-lib.berkeley.edu/ex/fup.html");

File.WriteAllText(path: "text.txt", contents: "hoge");
var waitTask = page.WaitForFileChooserAsync();
await Task.WhenAll(
  waitTask,
  page.ClickAsync("input[type='file'][name='upfile']")
);
await waitTask.Result.AcceptAsync("text.txt");
await page.TypeAsync("input[type='text'][name='note']", "fuga");

await page.ClickAsync("input[type='submit'][value='Press']");
await page.ScreenshotAsync("screenshot.jpg");
