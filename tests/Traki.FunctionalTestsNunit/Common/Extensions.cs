using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Traki.FunctionalTestsNunit.Common
{
    public static class Extensions
    {
        public static void ClearText(this IWebDriver webDriver)
        {
            Actions actions = new Actions(webDriver);
            actions.KeyDown(Keys.Control).SendKeys("a")
                .KeyUp(Keys.Control)
                .SendKeys(Keys.Delete).Perform();
        }

        public static void WriteNewText(this IWebDriver webDriver, By by, string text)
        {
            webDriver.FindElement(by).Click();
            webDriver.ClearText();
            webDriver.FindElement(by).SendKeys(text);
        }
    }
}
