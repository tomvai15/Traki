using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Data;
using Traki.FunctionalTests.Extensions;

namespace Traki.FunctionalTests.Steps.Login
{
    [Binding]
    public class LoginSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = _scenarioContext.GetRequiredService<IWebDriver>();
        }

        [Given(@"I have navigated to login page")]
        public void IHaveNavigatedToLoginPage()
        {
            _driver.Navigate().GoToUrl($"{Configuration.WebUrl}/login");
        }

        [When(@"I enter wrong credentials")]
        public void WhenIEnterInvalidCredentials()
        {
            _driver.ElementShouldBePresent(By.Id("email"));
            _driver.WriteNewText(By.Id("email"), Any<string>());
            _driver.WriteNewText(By.Id("password"), Any<string>());
            _driver.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            _driver.ElementShouldBePresent(By.Id("email"));
            _driver.WriteNewText(By.Id("email"), ExampleData.ProjectManagerEmail);
            _driver.WriteNewText(By.Id("password"), ExampleData.ProjectManagerPassword);
            _driver.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);
        }

        [Then(@"I should be redirected to home page")]
        public void IShouldBeRedirectedToHomePage()
        {
            _driver.Url.Should().Contain("home");
        }

        [Then(@"error message should be presented")]
        public void ThenErrorMessagePresented()
        {
            _driver.ElementShouldBePresent(By.Id("error"));
            _driver.FindElement(By.Id("error")).Text.Should().Be("Email or password is incorrect");
        }
    }
}
