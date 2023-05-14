using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Traki.FunctionalTests.Utils
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

        public static void ElementShouldBePresent(this IWebDriver driver, By by)
        {
            const int maxWaitTime = 5;
            for (int second = 0; ; second++)
            {
                if (second >= maxWaitTime) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(driver, by)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }

        private static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
