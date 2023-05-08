using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;

namespace Traki.FunctionalTestsNunit.Common
{
    public static class Driver
    {
        public static IWebDriver BuildDriver()
        {
            var path = DownloadDriverMatchingCurrentMachineBrowser();


            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--ignore-certificate-errors");
            

            var driver =  new ChromeDriver(Path.GetDirectoryName(path), options);

            Thread.Sleep(1000);
            return driver;
        }

        public static string DownloadDriverMatchingCurrentMachineBrowser()
            => new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
    }
}
