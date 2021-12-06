using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;

namespace SeleniumToolSample
{
    public class WebDriverFactory
    {
        public IWebDriver Create(WebDriverKind kind)
        {
            switch (kind)
            {
                case WebDriverKind.IE:
                    var ieOptions = new InternetExplorerOptions();
                    ieOptions.BrowserCommandLineArguments = "--silent";
                    return new InternetExplorerDriver(ieOptions);

                case WebDriverKind.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--log-level=3");
                    return new ChromeDriver(chromeOptions);

                case WebDriverKind.ChromeHeadless:
                    var chromeHeadlessOptions = new ChromeOptions();
                    chromeHeadlessOptions.AddArguments("--headless", "--log-level=3");
                    return new ChromeDriver(chromeHeadlessOptions);

                default:
                    throw new ArgumentException("Invalid WebDriverKind.");
            }
        }
    }
}
