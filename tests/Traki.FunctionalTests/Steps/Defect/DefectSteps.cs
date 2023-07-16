using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Extensions;

namespace Traki.FunctionalTests.Steps.Defect
{
    internal class DefectSteps
    {
        [Binding]
        public class ProjectSteps
        {
            private const string TitleKey = "Title";

            private readonly ScenarioContext _scenarioContext;
            private readonly IWebDriver _driver;

            public ProjectSteps(ScenarioContext scenarioContext)
            {
                _scenarioContext = scenarioContext;
                _driver = _scenarioContext.GetRequiredService<IWebDriver>();
            }

            [Given(@"I have opened defects page")]
            [When(@"I open defects page")]
            public void WhenIOpenCreateProductPage()
            {
                _driver.ElementShouldBePresent(By.Id("defect-details"));
                _driver.FindElement(By.Id("defect-details")).Click();
                _driver.ElementShouldBePresent(By.Id("defect-title"));
            }

            [When(@"I select new defect tab")]
            public void WhenSelectNewDefectTab()
            {
                _driver.ElementShouldBePresent(By.Id("tab-1"));
                _driver.FindElement(By.Id("tab-1")).Click();
            }

            [When(@"I select region")]
            public void WhenSelectRegion()
            {
                _driver.ElementShouldBePresent(By.Id("drawing"));
                var drawing = _driver.FindElement(By.Id("drawing"));

                // Create an instance of the Actions class
                Actions actions = new Actions(_driver);

                // Click and hold the element
                actions.ClickAndHold(drawing);

                // Move the mouse to the desired location while holding it
                actions.MoveByOffset(300, 300); // Adjust the offset values as per your requirements

                // Release the mouse click
                actions.Release();

                // Perform the actions
                actions.Build().Perform();
            }

            [When(@"fill defect information")]
            public void WhenFillDefectInformation()
            {
                string title = Any<string>().Substring(0, 10);
                string description = Any<string>();

                _scenarioContext.Add("title", title);
                _scenarioContext.Add("description", description);

                _driver.ElementShouldBePresent(By.Id("new-defect-title"));
                _driver.ElementShouldBePresent(By.Id("new-defect-description"));
                _driver.WriteNewText(By.Id("new-defect-title"), title);
                _driver.WriteNewText(By.Id("new-defect-description"), description);
                _driver.FindElement(By.Id("create-defect")).Click();
            }


            [When(@"I write a comment and submit")]
            public void WhenIWriteAComment()
            {
                string comment = Any<string>().Substring(0, 10);
                _scenarioContext.Add("comment", comment);

                _driver.ElementShouldBePresent(By.Id("comment-field"));
                _driver.WriteNewText(By.Id("comment-field"), comment);
                _driver.FindElement(By.Id("submit-comment")).Click();
                Thread.Sleep(2000);
            }

            [When(@"I change defect status")]
            public void WhenIChangeDefectStatus()
            {
                _driver.ElementShouldBePresent(By.Id("defect-status"));
                _driver.FindElement(By.Id("defect-status")).Click();
                _driver.ElementShouldBePresent(By.Id("fixed"));
                _driver.FindElement(By.Id("fixed")).Click();
                Thread.Sleep(2000);
            }

            [Then(@"defect status change activity is displayed")]
            public void DefectStatusChangeIsDisplayed()
            {
                _driver.ElementShouldBePresent(By.Id("activity-status-field-to"));
                _driver.FindElement(By.Id("defect-status")).Text.Should().BeEquivalentTo("Fixed");
            }

            [Then(@"defects information is displayed")]
            public void DefectsInformationIsDisplayed()
            {
                _driver.ElementShouldBePresent(By.Id("defect-title"));
                _driver.FindElement(By.Id("defect-title")).Text.Should().BeEquivalentTo(_scenarioContext.Get<string>("title"));
                _driver.FindElement(By.Id("defect-description")).Text.Should().BeEquivalentTo(_scenarioContext.Get<string>("description"));
            }

            [Then(@"new comment should be displayed")]
            public void NewCommentShouldBeCreated()
            {
                var comments = _driver.FindElements(By.Id("activity-comment-field")).Select(x=> x.Text);
                comments.Should().Contain(_scenarioContext.Get<string>("comment"));
            }
        }
    }
}
