using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;

namespace Traki.FunctionalTests.Utils
{
    public static class CustomDriver
    {
        public static IWebDriver BuildDriver()
        {
            var path = DownloadDriverMatchingCurrentMachineBrowser();


            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--ignore-certificate-errors");
           // options.AddArguments("headless");


            var driver =  new ChromeDriver(Path.GetDirectoryName(path), options);

            Thread.Sleep(1000);
            return driver;
        }

        public static string DownloadDriverMatchingCurrentMachineBrowser()
            => new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
    }
}
