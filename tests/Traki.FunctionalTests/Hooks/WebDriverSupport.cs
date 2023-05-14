using BoDi;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Traki.FunctionalTests.Hooks
{
    [Binding]
    public class WebDriverSupport
    {
        private readonly IObjectContainer objectContainer;

        public WebDriverSupport(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            var webDriver = BuildDriver();
            objectContainer.RegisterInstanceAs<IWebDriver>(webDriver);
        }
    }
}
