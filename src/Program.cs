using OpenQA.Selenium;
using System;

namespace SeleniumToolSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new WebDriverFactory();
            var driver = factory.Create(WebDriverKind.ChromeHeadless);

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
}
