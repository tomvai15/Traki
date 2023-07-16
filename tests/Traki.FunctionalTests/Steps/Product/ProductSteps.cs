using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Data;
using Traki.FunctionalTests.Extensions;

namespace Traki.FunctionalTests.Steps.Product
{
    [Binding]
    public class ProductSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;

        public ProductSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = _scenarioContext.GetRequiredService<IWebDriver>();
        }

        [Given(@"I have logged in as product manager")]
        public void GivenNotEnoughProductsInStock()
        {
            _driver.Navigate().GoToUrl($"{Configuration.WebUrl}/login");
            _driver.FindElement(By.Id("email")).Click();
            _driver.FindElement(By.Id("email")).Clear();
            _driver.FindElement(By.Id("email")).SendKeys(ExampleData.ProductManagerEmail);
            _driver.FindElement(By.Id("password")).Click();
            _driver.FindElement(By.Id("password")).Clear();
            _driver.FindElement(By.Id("password")).SendKeys(ExampleData.ProductManagerPassword);
            _driver.FindElement(By.Id("submit")).Click();
            _driver.ElementShouldBePresent(By.XPath("//div[@id='root']/div/div/div/nav/div[2]"));
        }

        [Given(@"I have opened product page")]
        public void IHaveOpenedProductPage()
        {
            WhenIPresOnProduct();
            ThenProductNameShouldBePresent();
        }

        [Given(@"I have created product")]
        public void IHaveCreatedProdtc()
        {
            WhenIOpenCreateProductPage();
            WhenIAddProductName();
            ProductIsCreated();
        }


        [When(@"I press on product item")]
        public void WhenIPresOnProduct()
        {
            _driver.ElementShouldBePresent(By.Id("1-products-0"));
            string productName = _driver.FindElement(By.Id("1-products-0")).Text;
            _scenarioContext.Add("productName", productName);
            _driver.FindElement(By.Id("1-products-0")).Click();
            _driver.ElementShouldBePresent(By.Id("product-name"));
        }

        [When(@"I open edit product page")]
        public void WhenIOpenEditProductPage()
        {
            _driver.ElementShouldBePresent(By.Id("edit-product"));
            _driver.FindElement(By.Id("edit-product")).Click();
            _driver.ElementShouldBePresent(By.Id("product-name"));
        }

        [When(@"update product name")]
        public void WhenIUpdatefields()
        {
            string productName = Any<string>();
            _driver.ElementShouldBePresent(By.Id("product-name"));
            _scenarioContext.Add("newProductName", productName);
            _driver.WriteNewText(By.Id("product-name"), productName);
            _driver.ElementShouldBePresent(By.Id("update-product"));
            _driver.FindElement(By.Id("update-product")).Click();
            _driver.FindElement(By.Id("product-link")).Click();
        }

        [When(@"update product name with invalid characters")]
        public void WhenIUpdatefieldsWithInvalidCharacter()
        {
            string productName = Any<string>();
            _driver.ElementShouldBePresent(By.Id("product-name"));
            _scenarioContext.Add("newProductName", productName);
            _driver.WriteNewText(By.Id("product-name"), "!@#$@$%#&*&#*");
            _driver.ElementShouldBePresent(By.Id("update-product"));
        }


        [When(@"I open create product page")]
        public void WhenIOpenCreateProductPage()
        {
            _driver.ElementShouldBePresent(By.Id("add-product"));
            _driver.FindElement(By.Id("add-product")).Click();
            _driver.ElementShouldBePresent(By.Id("product-name"));
        }

        [When(@"I add product name")]
        public void WhenIAddProductName()
        {
            string productName = Any<string>();
            _scenarioContext.Add("newProductName", productName);

            _driver.ElementShouldBePresent(By.Id("product-name"));
            _driver.WriteNewText(By.Id("product-name"), productName);
            _driver.FindElement(By.Id("create-product")).Click();
            Thread.Sleep(1000);
        }

        [When(@"press delete button and confirm deletion")]
        public void DeleteProduct()
        {
            _driver.ElementShouldBePresent(By.Id("product-name"));
            _driver.FindElement(By.Id("delete-product")).Click();
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("confirm"));
            _driver.FindElement(By.Id("confirm")).Click();
            Thread.Sleep(1000);
        }


        [When(@"I open protocol import window")]
        public void IOpenImportProtocolWindow()
        {
            _driver.ElementShouldBePresent(By.Id("add-protocol"));
            _driver.FindElement(By.Id("add-protocol")).Click();
        }

        [When(@"select protocol from the list")]
        public void SelectProtocolFromList()
        {
            _driver.ElementShouldBePresent(By.Id("protocol-item"));
            _driver.FindElement(By.Id("protocol-item")).Click();
        }

        [When(@"I open fill protocol page")]
        public void OpenFillProtocolPage()
        {
            _driver.ElementShouldBePresent(By.Id("fill-protocol"));
            _driver.FindElement(By.Id("fill-protocol")).Click();
        }

        [When(@"fill section and save changes")]
        public void FillSectionAndSave()
        {
            string randomText = Any<string>();
            _scenarioContext.Add("randomText", randomText);

            _driver.ElementShouldBePresent(By.Id("question-comment"));
            _driver.WriteNewText(By.Id("question-comment"), randomText);
            _driver.ElementShouldBePresent(By.Id("save-section"));
            _driver.FindElement(By.Id("save-section")).Click();
            Thread.Sleep(1000);
        }

        [Then(@"section should be updated")]
        public void SectionShouldBeUpdated()
        {
            _driver.Navigate().Refresh();
            _driver.ElementShouldBePresent(By.Id("question-comment"));
            _driver.FindElement(By.Id("question-comment")).Text.Should().BeEquivalentTo(_scenarioContext.Get<string>("randomText"));
        }


        [Then(@"protocol should be added for product")]
        public void ProtocolIsAdded()
        {
            _driver.ElementShouldBePresent(By.Id("product-protocol"));
        }

        [Then(@"product is deleted")]
        public void ProductIsDeleted()
        {
            var productUrl = _scenarioContext.Get<string>("newProductUrl");
            _driver.Navigate().GoToUrl(productUrl);
            _driver.ElementShouldBePresent(By.Id("not-found"));
        }

        [Then(@"product is created")]
        public void ProductIsCreated()
        {
            _scenarioContext.Add("newProductUrl", _driver.Url);
            _driver.ElementShouldBePresent(By.Id("product-name"));
            _scenarioContext.Get<string>("newProductName").Should().BeEquivalentTo(_driver.FindElement(By.Id("product-name")).Text);
        }

        [Then(@"product name should be updated")]
        public void ProductNameShouldBeUpdated()
        {
            _driver.ElementShouldBePresent(By.Id("product-name"));
            _scenarioContext.Get<string>("newProductName").Should().BeEquivalentTo(_driver.FindElement(By.Id("product-name")).Text);
        }

        [Then(@"I should not be allowed to update product")]
        public void IShouldNotBeAllowedToUpdateField()
        {
            _driver.FindElement(By.Id("product-name-helper-text")).Text.Should().StartWith("Special");
            var button = FindUpdateButton();
            button.Enabled.Should().BeFalse();
        }

        [Then(@"I should be navigated to product page")]
        public void ThenProductNameShouldBePresent()
        {
            _driver.ElementShouldBePresent(By.Id("product-name"));
            _scenarioContext.Get<string>("productName").Should().BeEquivalentTo(_driver.FindElement(By.Id("product-name")).Text);
        }

        private IWebElement FindUpdateButton()
        {
            return _driver.FindElement(By.Id("update-product"));
        }
    }
}
