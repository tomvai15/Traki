using FluentAssertions;
using Microsoft.VisualBasic;
using OpenQA.Selenium;
using System.Text;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Data;

namespace Traki.FunctionalTests.Steps.Project
{
    [Binding]
    public class ProjectSteps
    {
        private IWebDriver driver;
        private static Dictionary<string, string> testingData = new Dictionary<string, string>();

        public ProjectSteps()
        {
            driver = BuildDriver();
        }

        [Given(@"I have logged in as project manager")]
        public void GivenNotEnoughProductsInStock()
        {
            driver.Navigate().GoToUrl("https://localhost:3000/login");
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys("vainoristomas9@gmail.com");
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

        [Then(@"I navigate back")]
        [When(@"I navigate back")]
        public void Then_INavigateback()
        {
            driver.Navigate().Back();
            driver.Navigate().Refresh();
        }

        [When(@"I press on Projects tab")]
        public void WhenIEnterValidCredentials()
        {
            driver.FindElement(By.XPath("//div[@id='root']/div/div/div/nav/div[2]/div[2]/span")).Click();
        }

        [Given(@"I have navigated to projects page")]
        public void Given_IHaveNavigatedToProjectPage()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.XPath("//div[@id='projects-drawer']/div[2]/span"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            driver.FindElement(By.XPath("//div[@id='projects-drawer']/div[2]/span")).Click();
        }

        [When(@"I open edit project page")]
        public void Given_IHaveOpenedEditProjectPage()
        {
            driver.Navigate().GoToUrl("https://localhost:3000/projects");
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("0-edit-project"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("0-edit-project")).Click();
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("project-name"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }

        [When(@"I open create project page")]
        public void Given_IHaveOpenedCreateProjectPage()
        {
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("add-project"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("add-project")).Click();
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("project-name"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }

        [When(@"I update all project fields")]
        public void When_IUpdateAllField()
        {
            string randomValue = Any<string>().Substring(0, 10);
            testingData.Add("RandomValue", randomValue);

            driver.FindElement(By.Id("project-name")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-name")).SendKeys(randomValue);

            driver.FindElement(By.Id("project-address")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-address")).SendKeys(randomValue);

            driver.FindElement(By.Id("project-client")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-client")).SendKeys(randomValue);

            driver.FindElement(By.XPath("//div[@id='root']/div/main/div[2]/div/div[2]/div/div[2]/button")).Click();
        }

        [When(@"I add all project fields")]
        public void When_UpdateAllFields()
        {
            string randomValue = Any<string>().Substring(0, 10);
            testingData.Add("NewProject", randomValue);

            driver.FindElement(By.Id("project-name")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-name")).SendKeys(randomValue);

            driver.FindElement(By.Id("project-address")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-address")).SendKeys(randomValue);

            driver.FindElement(By.Id("project-client")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-client")).SendKeys(randomValue);

            driver.FindElement(By.Id("upload-image")).Click();
            //   driver.FindElement(By.XPath("//input[@type='file']")).Clear();
            driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(ExampleData.ExampleImage);

            FindCreateButton().Click();
            return;
        }


        [When(@"I update all project fields with invalid characters")]
        public void When_IUpdateAllFieldsWithInvalidCharacter()
        {
            string randomValue = "@@#@%#$%T($*%($$$%Y&())^*(<>";

            driver.FindElement(By.Id("project-name")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-name")).SendKeys(randomValue);

            driver.FindElement(By.Id("project-address")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-address")).SendKeys(randomValue);

            driver.FindElement(By.Id("project-client")).Click();
            driver.ClearText();
            driver.FindElement(By.Id("project-client")).SendKeys(randomValue);
        }

        [Then(@"I should not be allowed to update project information")]
        public void Then_I_ShouldNotBeAllowed_ToUpdateProject()
        {
            driver.FindElement(By.Id("project-name-helper-text")).Text.Should().StartWith("Special");
            driver.FindElement(By.Id("project-name-helper-text")).Text.Should().StartWith("Special");
            driver.FindElement(By.Id("project-name-helper-text")).Text.Should().StartWith("Special");
            var button = FindUpdateButton();
            button.Enabled.Should().BeFalse();
        }

        [Then(@"project information should be updated")]
        public void Then_ProjectInfromationShouldBeUpdated()
        {
            Then_INavigateback();
            Given_IHaveOpenedEditProjectPage();
            var randomValue = testingData["RandomValue"];

            Assert.Equal(randomValue, driver.FindElement(By.Id("project-name")).GetAttribute("value"));
            Assert.Equal(randomValue, driver.FindElement(By.Id("project-address")).GetAttribute("value"));
            Assert.Equal(randomValue, driver.FindElement(By.Id("project-client")).GetAttribute("value"));
        }

        [Then(@"project is created")]
        public void Then_ProjectIsCreated()
        {
            Then_INavigateback();
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("project-name"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            var elements = driver.FindElements(By.Id("project-name")).Select(x=> x.Text).ToList();
            var projectName = testingData["NewProject"];

            elements.Should().NotBeEmpty();
        }

        [Then(@"projects should be displayed")]
        public void GivenEnoughProductsInStock()
        {
            Assert.Equal("Projects", driver.FindElement(By.XPath("//div[@id='root']/div/main/div[2]/div/div/nav/ol/li/p")).Text);
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
