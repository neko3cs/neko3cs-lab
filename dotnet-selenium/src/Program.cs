using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace DotnetSelenium;

internal class Program
{
    private static void Main(string[] args)
    {
        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        var version = new ChromeConfig().GetMatchingBrowserVersion();
        using var service = ChromeDriverService.CreateDefaultService($"./Chrome/{version}/X64");

        var options = new ChromeOptions();
        options.AddArguments("--headless", "--log-level=3");
        using var driver = new ChromeDriver(service, options);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

        try
        {
            driver.Navigate().GoToUrl("https://stocks.finance.yahoo.co.jp/stocks/detail/?code=998407.O");
            wait.Until(d => d
                .FindElement(By.XPath("((//div[@id='root']/main/div/div/div)[1]/div)[2]/div/h2"))
                .Displayed
            );

            var price = driver
                .FindElement(By.XPath("(((((//div[@id='root']/main/div/div/div)[1]/div)[2]/div/div)[2]/div)[2]/p)[2]/span"))
                .Text;
            var comparedToTheDayBeforePlusMinus = driver
                .FindElement(By.XPath("(((((//div[@id='root']/main/div/div/div)[1]/div)[2]/div/div)[2]/div)[3]/p/span/span/span)[1]"))
                .Text;
            var comparedToTheDayBefore = driver
                .FindElement(By.XPath("(((((//div[@id='root']/main/div/div/div)[1]/div)[2]/div/div)[2]/div)[3]/p/span/span/span)[2]"))
                .Text;
            var beginPrice = driver
                .FindElement(By.XPath("(((((//div[@id='root']/main/div/div/div)[1]/div)[2]/div)[4]/div/div/div/dl)[2]/dd/span)[1]"))
                .Text;
            var highestPrice = driver
                .FindElement(By.XPath("(((((//div[@id='root']/main/div/div/div)[1]/div)[2]/div)[4]/div/div/div/dl)[3]/dd/span)[1]"))
                .Text;
            var lowestPrice = driver
                .FindElement(By.XPath("(((((//div[@id='root']/main/div/div/div)[1]/div)[2]/div)[4]/div/div/div/dl)[4]/dd/span)[1]"))
                .Text;

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"日経平均株価\t: {price}");
            Console.WriteLine($"前日比\t\t:   {comparedToTheDayBeforePlusMinus}{comparedToTheDayBefore}");
            Console.WriteLine($"始値\t\t: {beginPrice}");
            Console.WriteLine($"高値\t\t: {highestPrice}");
            Console.WriteLine($"安値\t\t: {lowestPrice}");
            Console.WriteLine(Environment.NewLine);
        }
        finally
        {
            driver.Close();
            driver.Quit();
        }
    }
}