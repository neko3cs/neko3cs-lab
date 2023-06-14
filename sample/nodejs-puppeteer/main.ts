import puppeteer from "puppeteer";
import { setTimeout } from "timers/promises";

(async () => {
  const browser = await puppeteer.launch({ headless: false });
  const page = await browser.newPage();

  await page.goto("https://cgi-lib.berkeley.edu/ex/fup.html");

  const [fileChooser] = await Promise.all([
    page.waitForFileChooser(),
    page.click("input[type='file'][name='upfile']")
  ]);
  await fileChooser.accept(['/Users/neko3cs/Desktop/test.txt']);

  await page.click("input[type='submit'][value='Press']");

  await setTimeout(3000);
  await browser.close();
})();
