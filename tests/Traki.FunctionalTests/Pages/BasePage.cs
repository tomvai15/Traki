using OpenQA.Selenium;
using static Traki.FunctionalTests.TestsSetup.Config;

namespace Traki.FunctionalTests.Pages
{
    public abstract class BasePage
    {
        public IWebDriver Driver { get; }

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        protected void NavigateTo(string relativeUrl)
        {
            if (Driver.Url == relativeUrl) return;

            Driver.Navigate().GoToUrl(RootUrl + relativeUrl);
        }
    }
}
