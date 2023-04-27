using OpenQA.Selenium;

namespace Traki.FunctionalTests.Extensions
{
    public static class WebDriverExtensions
    {
        public static string GetLocalUrl(this IWebDriver driver)
        {
            return driver.Url.Replace(TestsSetup.Config.RootUrl, "");
        }
    }
}
