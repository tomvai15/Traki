using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Traki.FunctionalTests.Steps.Product
{
    [Binding]
    public class ProductSteps
    {
        private IWebDriver driver;
        private static Dictionary<string, string> testingData = new Dictionary<string, string>();

        public ProductSteps()
        {
            driver = BuildDriver();
        }

        [Given(@"I have logged in as product manager")]
        public void GivenNotEnoughProductsInStock()
        {
            driver.Navigate().GoToUrl("https://localhost:3000/login");
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys("vainoristomas@gmail.com");
            driver.FindElement(By.Id("password")).Click();
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("password");
            driver.FindElement(By.Id("submit")).Click();
            //driver.Navigate().GoToUrl("https://localhost:3000/home");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.XPath("//div[@id='root']/div/div/div/nav/div[2]"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }

        [When(@"I press on Projects tab")]
        public void WhenIEnterValidCredentials()
        {
            driver.FindElement(By.XPath("//div[@id='root']/div/div/div/nav/div[2]/div[2]/span")).Click();
        }

        private IWebElement FindUpdateButton()
        {
            return driver.FindElement(By.Id("update-button"));
        }

        private IWebElement FindCreateButton()
        {
            return driver.FindElement(By.Id("create-button"));
        }

        private bool IsElementPresent(By by)
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
