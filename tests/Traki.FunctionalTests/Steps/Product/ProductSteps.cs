using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Data;

namespace Traki.FunctionalTests.Steps.Product
{
    [Binding]
    public class ProductSteps
    {
        private IWebDriver driver;
        private static Dictionary<string, string> testingData = new Dictionary<string, string>();

        public ProductSteps(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        [Given(@"I have logged in as product manager")]
        public void GivenNotEnoughProductsInStock()
        {
            driver.Navigate().GoToUrl("https://localhost:3000/login");
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys(ExampleData.ProductManagerEmail);
            driver.FindElement(By.Id("password")).Click();
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys(ExampleData.ProductManagerPassword);
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

        [Given(@"I have opened product page")]
        public void IHaveOpenedProductPage()
        {
            WhenIPresOnProduct();
            ThenProductNameShouldBePresent();
        }


        [When(@"I press on product item")]
        public void WhenIPresOnProduct()
        {
            driver.ElementShouldBePresent(By.Id("1-products-0"));
            string productName = driver.FindElement(By.Id("1-products-0")).Text;
            testingData.Add("productName", productName);
            driver.FindElement(By.Id("1-products-0")).Click();
            driver.ElementShouldBePresent(By.Id("product-name"));
        }

        [When(@"I open edit product page")]
        public void WhenIOpenEditProductPage()
        {
            driver.ElementShouldBePresent(By.Id("edit-product"));
            driver.FindElement(By.Id("edit-product")).Click();
            driver.ElementShouldBePresent(By.Id("product-name"));
        }

        [When(@"update product name")]
        public void WhenIUpdatefields()
        {
            string productName = Any<string>();
            driver.ElementShouldBePresent(By.Id("product-name"));
            testingData.Add("newProductName", productName);
            driver.WriteNewText(By.Id("product-name"), productName);
            driver.ElementShouldBePresent(By.Id("update-product"));
            driver.FindElement(By.Id("update-product")).Click();
            driver.FindElement(By.Id("product-link")).Click();
        }

        [When(@"update product name with invalid characters")]
        public void WhenIUpdatefieldsWithInvalidCharacter()
        {
            string productName = Any<string>();
            driver.ElementShouldBePresent(By.Id("product-name"));
            testingData.Add("newProductName", productName);
            driver.WriteNewText(By.Id("product-name"), "!@#$@$%#&*&#*");
            driver.ElementShouldBePresent(By.Id("update-product"));
        }

        [Then(@"product name should be updated")]
        public void ProductNameShouldBeUpdated()
        {
            driver.ElementShouldBePresent(By.Id("product-name"));
            testingData["newProductName"].Should().BeEquivalentTo(driver.FindElement(By.Id("product-name")).Text);
        }

        [Then(@"I should not be allowed to update product")]
        public void IShouldNotBeAllowedToUpdateField()
        {
            driver.FindElement(By.Id("product-name-helper-text")).Text.Should().StartWith("Special");
            var button = FindUpdateButton();
            button.Enabled.Should().BeFalse();
        }

        [Then(@"I should be navigated to product page")]
        public void ThenProductNameShouldBePresent()
        {
            driver.ElementShouldBePresent(By.Id("product-name"));
            testingData["productName"].Should().BeEquivalentTo(driver.FindElement(By.Id("product-name")).Text);
        }

        private IWebElement FindUpdateButton()
        {
            return driver.FindElement(By.Id("update-product"));
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
