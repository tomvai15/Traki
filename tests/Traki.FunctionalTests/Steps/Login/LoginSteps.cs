
using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Data;
using Traki.FunctionalTests.Hooks;

namespace Traki.FunctionalTests.Steps.Login
{

    [Binding]
    public class LoginSteps
    {
        private IWebDriver driver;
        private static Dictionary<string, string> testingData = new Dictionary<string, string>();

        public LoginSteps(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        [Given(@"I have navigated to login page")]
        public void IHaveNavigatedToLoginPage()
        {
            driver.Navigate().GoToUrl($"{ConfigurationAccessor.WebUrl}/login");
        }

        [When(@"I enter wrong credentials")]
        public void WhenIEnterInvalidCredentials()
        {
            driver.ElementShouldBePresent(By.Id("email"));
            driver.WriteNewText(By.Id("email"), Any<string>());
            driver.WriteNewText(By.Id("password"), Any<string>());
            driver.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            driver.ElementShouldBePresent(By.Id("email"));
            driver.WriteNewText(By.Id("email"), ExampleData.ProjectManagerEmail);
            driver.WriteNewText(By.Id("password"), ExampleData.ProjectManagerPassword);
            driver.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);
        }

        [Then(@"I should be redirected to home page")]
        public void IShouldBeRedirectedToHomePage()
        {
            driver.Url.Should().Contain("home");
        }

        [Then(@"error message should be presented")]
        public void ThenErrorMessagePresented()
        {
            driver.ElementShouldBePresent(By.Id("error"));
            driver.FindElement(By.Id("error")).Text.Should().Be("Email or password is incorrect");
        }
    }
}
