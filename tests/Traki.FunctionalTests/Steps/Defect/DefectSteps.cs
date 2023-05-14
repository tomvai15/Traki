using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;

namespace Traki.FunctionalTests.Steps.Defect
{
    internal class DefectSteps
    {
        [Binding]
        public class ProjectSteps
        {
            private IWebDriver driver;
            private static Dictionary<string, string> testingData = new Dictionary<string, string>();

            public ProjectSteps(IWebDriver webDriver)
            {
                driver = webDriver;
            }

            [Given(@"I have opened defects page")]
            [When(@"I open defects page")]
            public void WhenIOpenCreateProductPage()
            {
                driver.ElementShouldBePresent(By.Id("defect-details"));
                driver.FindElement(By.Id("defect-details")).Click();
                driver.ElementShouldBePresent(By.Id("defect-title"));
            }

            [When(@"I select new defect tab")]
            public void WhenSelectNewDefectTab()
            {
                driver.ElementShouldBePresent(By.Id("tab-1"));
                driver.FindElement(By.Id("tab-1")).Click();
            }

            [When(@"I select region")]
            public void WhenSelectRegion()
            {
                driver.ElementShouldBePresent(By.Id("drawing"));
                var drawing = driver.FindElement(By.Id("drawing"));

                // Create an instance of the Actions class
                Actions actions = new Actions(driver);

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

                testingData.Add("title", title);
                testingData.Add("description", description);

                driver.ElementShouldBePresent(By.Id("new-defect-title"));
                driver.ElementShouldBePresent(By.Id("new-defect-description"));
                driver.WriteNewText(By.Id("new-defect-title"), title);
                driver.WriteNewText(By.Id("new-defect-description"), description);
                driver.FindElement(By.Id("create-defect")).Click();
            }


            [When(@"I write a comment and submit")]
            public void WhenIWriteAComment()
            {
                string comment = Any<string>().Substring(0, 10);
                testingData.Add("comment", comment);

                driver.ElementShouldBePresent(By.Id("comment-field"));
                driver.WriteNewText(By.Id("comment-field"), comment);
                driver.FindElement(By.Id("submit-comment")).Click();
                Thread.Sleep(2000);
            }

            [Then(@"defects information is displayed")]
            public void DefectsInformationIsDisplayed()
            {
                driver.ElementShouldBePresent(By.Id("defect-title"));
                driver.FindElement(By.Id("defect-title")).Text.Should().BeEquivalentTo(testingData["title"]);
                driver.FindElement(By.Id("defect-description")).Text.Should().BeEquivalentTo(testingData["description"]);
            }

            [Then(@"new comment should be displayed")]
            public void NewCommentShouldBeCreated()
            {
                var comments = driver.FindElements(By.Id("activity-comment-field")).Select(x=> x.Text);
                comments.Should().Contain(testingData["comment"]);
            }
        }
    }
}
