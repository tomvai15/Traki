using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
