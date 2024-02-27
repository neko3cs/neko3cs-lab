using PuppeteerSharp;

const string chromePath = @"/Applications/Google\ Chrome.app/Contents/MacOS/Google\ Chrome";

var launchOptions = new LaunchOptions() { Headless = true };
if (!File.Exists(chromePath))
{
#if DEBUG
  Console.WriteLine("DEBUG: Cannot find chrome! Donwloading chromium...");
#endif
  using var browserFetcher = new BrowserFetcher();
  await browserFetcher.DownloadAsync();
  launchOptions.ExecutablePath = chromePath;
}
else
{
#if DEBUG
  Console.WriteLine("DEBUG: Finded chrome! Don't donwload chromium.");
#endif
}

await using var browser = await Puppeteer.LaunchAsync(
  new LaunchOptions { Headless = true }
);
await using var page = await browser.NewPageAsync();
await page.GoToAsync("https://cgi-lib.berkeley.edu/ex/fup.html");

#if DEBUG
Console.WriteLine("DEBUG: Sent page.");
#endif

File.WriteAllText(path: "text.txt", contents: "hoge");
var waitTask = page.WaitForFileChooserAsync();
await Task.WhenAll(
  waitTask,
  page.ClickAsync("input[type='file'][name='upfile']")
);
await waitTask.Result.AcceptAsync("text.txt");
await page.TypeAsync("input[type='text'][name='note']", "fuga");

#if DEBUG
Console.WriteLine("DEBUG: Uploaded file.");
#endif

await page.ClickAsync("input[type='submit'][value='Press']");
await page.WaitForNavigationAsync();

await page.ScreenshotAsync("screenshot.jpg");

#if DEBUG
Console.WriteLine("DEBUG: Navigated and took a screenshot.");
#endif
