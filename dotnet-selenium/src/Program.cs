using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace DotnetSelenium;

internal class Program
{
    private static void Main(string[] args)
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        var chromeHeadlessOptions = new ChromeOptions();
        chromeHeadlessOptions.AddArguments("--headless", "--log-level=3");
        var driver = new ChromeDriver(chromeHeadlessOptions);

        try
        {
            driver.Navigate().GoToUrl("https://stocks.finance.yahoo.co.jp/stocks/detail/?code=998407.O");

            var price = driver
                .FindElements(By.ClassName("stoksPrice"))[1]
                .Text;

            var comparedToTheDayBefore = driver
                .FindElement(By.ClassName("change"))
                .FindElements(By.TagName("span"))[1]
                .Text;

            var latestUpdateDate = driver
                .FindElement(By.ClassName("nikkeireal"))
                .FindElement(By.TagName("span"))
                .Text;

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"日経平均株価\t：{price}");
            Console.WriteLine($"前日比\t\t：{comparedToTheDayBefore}");
            Console.WriteLine($"最終更新日\t：{latestUpdateDate}");
            Console.WriteLine(Environment.NewLine);
        }
        finally
        {
            driver.Close();
            driver.Quit();
        }
    }
}