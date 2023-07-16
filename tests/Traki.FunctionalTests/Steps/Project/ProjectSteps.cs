using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Data;
using Traki.FunctionalTests.Extensions;

namespace Traki.FunctionalTests.Steps.Project
{
    [Binding]
    public class ProjectSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;

        public ProjectSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = _scenarioContext.GetRequiredService<IWebDriver>();
        }

        [Given(@"I have logged in as project manager")]
        public void GivenNotEnoughProductsInStock()
        {
            _driver.Navigate().GoToUrl($"{Configuration.WebUrl}/login");
            _driver.FindElement(By.Id("email")).Click();
            _driver.FindElement(By.Id("email")).Clear();
            _driver.FindElement(By.Id("email")).SendKeys(ExampleData.ProjectManagerEmail);
            _driver.FindElement(By.Id("password")).Click();
            _driver.FindElement(By.Id("password")).Clear();
            _driver.FindElement(By.Id("password")).SendKeys(ExampleData.ProjectManagerPassword);
            _driver.FindElement(By.Id("submit")).Click();
            _driver.ElementShouldBePresent(By.XPath("//div[@id='root']/div/div/div/nav/div[2]"));
        }

        [Then(@"I navigate back")]
        [When(@"I navigate back")]
        public void Then_INavigateback()
        {
            _driver.Navigate().Back();
            _driver.Navigate().Refresh();
        }

        [When(@"I press on Projects tab")]
        public void WhenIEnterValidCredentials()
        {
            _driver.FindElement(By.XPath("//div[@id='root']/div/div/div/nav/div[2]/div[2]/span")).Click();
        }

        [Given(@"I have navigated to projects page")]
        public void Given_IHaveNavigatedToProjectPage()
        {
            _driver.ElementShouldBePresent(By.XPath("//div[@id='projects-drawer']/div[2]/span"));
            _driver.FindElement(By.XPath("//div[@id='projects-drawer']/div[2]/span")).Click();
        }

        [When(@"I open edit project page")]
        public void Given_IHaveOpenedEditProjectPage()
        {
            _driver.Navigate().GoToUrl($"{Configuration.WebUrl}/projects");
            _driver.ElementShouldBePresent(By.Id("0-edit-project"));

            _driver.FindElement(By.Id("0-edit-project")).Click();
            _driver.ElementShouldBePresent(By.Id("project-name"));
        }

        [When(@"I open create project page")]
        public void Given_IHaveOpenedCreateProjectPage()
        {
            _driver.ElementShouldBePresent(By.Id("add-project"));
            _driver.FindElement(By.Id("add-project")).Click();
            _driver.ElementShouldBePresent(By.Id("project-name"));
        }

        [When(@"I update all project fields")]
        public void When_IUpdateAllField()
        {
            string randomValue = Any<string>().Substring(0, 10);
            _scenarioContext.Add("RandomValue", randomValue);

            _driver.FindElement(By.Id("project-name")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-name")).SendKeys(randomValue);

            _driver.FindElement(By.Id("project-address")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-address")).SendKeys(randomValue);

            _driver.FindElement(By.Id("project-client")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-client")).SendKeys(randomValue);

            _driver.FindElement(By.XPath("//div[@id='root']/div/main/div[2]/div/div[2]/div/div[2]/button")).Click();
        }

        [When(@"I add all project fields")]
        public void When_UpdateAllFields()
        {
            string randomValue = Any<string>().Substring(0, 10);
            _scenarioContext.Add("NewProject", randomValue);

            _driver.FindElement(By.Id("project-name")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-name")).SendKeys(randomValue);

            _driver.FindElement(By.Id("project-address")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-address")).SendKeys(randomValue);

            _driver.FindElement(By.Id("project-client")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-client")).SendKeys(randomValue);

            _driver.FindElement(By.Id("upload-image")).Click();
            //   driver.FindElement(By.XPath("//input[@type='file']")).Clear();
            _driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(ExampleData.ExampleImage);

            FindCreateButton().Click();
            return;
        }


        [When(@"I update all project fields with invalid characters")]
        public void When_IUpdateAllFieldsWithInvalidCharacter()
        {
            string randomValue = "@@#@%#$%T($*%($$$%Y&())^*(<>";

            _driver.FindElement(By.Id("project-name")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-name")).SendKeys(randomValue);

            _driver.FindElement(By.Id("project-address")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-address")).SendKeys(randomValue);

            _driver.FindElement(By.Id("project-client")).Click();
            _driver.ClearText();
            _driver.FindElement(By.Id("project-client")).SendKeys(randomValue);
        }

        [Then(@"I should not be allowed to update project information")]
        public void Then_I_ShouldNotBeAllowed_ToUpdateProject()
        {
            _driver.FindElement(By.Id("project-name-helper-text")).Text.Should().StartWith("Special");
            _driver.FindElement(By.Id("project-name-helper-text")).Text.Should().StartWith("Special");
            _driver.FindElement(By.Id("project-name-helper-text")).Text.Should().StartWith("Special");
            var button = FindUpdateButton();
            button.Enabled.Should().BeFalse();
        }

        [Then(@"project information should be updated")]
        public void Then_ProjectInfromationShouldBeUpdated()
        {
            Then_INavigateback();
            Given_IHaveOpenedEditProjectPage();
            var randomValue = _scenarioContext.Get<string>("RandomValue");

            Assert.Equal(randomValue, _driver.FindElement(By.Id("project-name")).GetAttribute("value"));
            Assert.Equal(randomValue, _driver.FindElement(By.Id("project-address")).GetAttribute("value"));
            Assert.Equal(randomValue, _driver.FindElement(By.Id("project-client")).GetAttribute("value"));
        }

        [Then(@"project is created")]
        public void Then_ProjectIsCreated()
        {
            Then_INavigateback();
            _driver.ElementShouldBePresent(By.Id("project-name"));
            var elements = _driver.FindElements(By.Id("project-name")).Select(x=> x.Text).ToList();
            var projectName = _scenarioContext.Get<string>("NewProject");

            elements.Should().NotBeEmpty();
        }

        [Then(@"projects should be displayed")]
        public void GivenEnoughProductsInStock()
        {
            Assert.Equal("Projects", _driver.FindElement(By.XPath("//div[@id='root']/div/main/div[2]/div/div/nav/ol/li/p")).Text);
        }

        private IWebElement FindUpdateButton()
        {
            return _driver.FindElement(By.Id("update-button"));
        }

        private IWebElement FindCreateButton()
        {
            return _driver.FindElement(By.Id("create-button"));
        }
    }
}
