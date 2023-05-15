using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Traki.FunctionalTests.Steps.Protocol
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

        [Given(@"I have created protocol")]
        public void IHaveCreatedProtocol()
        {
            IEnterNewProtocolNameAndSubmit();
            ProtocolsShouldBeAdded();
            driver.FindElements(By.XPath("//div[@id='protocol-name']/span")).Last().Click();
        }

        [Given("I have navigated to protocol templates page")]
        [When(@"I press on Templates tab")]
        public void WhenINavigateToTemplatesTab()
        {
            driver.ElementShouldBePresent(By.Id("protocols-drawer"));
            driver.FindElement(By.Id("protocols-drawer")).Click();
            driver.ElementShouldBePresent(By.Id("protocol-name"));
        }

        [When(@"I open create section page")]
        public void IHaveOpenedCreateSectionPage()
        {
            driver.ElementShouldBePresent(By.Id("create-section"));
            driver.FindElement(By.Id("create-section")).Click();
            driver.ElementShouldBePresent(By.Id("section-name"));
            Thread.Sleep(1000);
        }

        [When(@"I open edit section page")]
        public void IHaveOpenedEditSectionPage()
        {
            driver.ElementShouldBePresent(By.Id("section-summary"));
            driver.FindElement(By.Id("section-summary")).Click();
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("edit-section"));
            driver.FindElement(By.Id("edit-section")).Click();
            driver.ElementShouldBePresent(By.Id("section-name"));
            Thread.Sleep(1000);
        }


        [When(@"I enter new protocol name and submit")]
        public void IEnterNewProtocolNameAndSubmit()
        {
            string newProtocolName = Any<string>();
            testingData.Add("newProtocolName", newProtocolName);
            driver.ElementShouldBePresent(By.Id("new-protocol-name"));
            driver.WriteNewText(By.Id("new-protocol-name"), newProtocolName);
            driver.FindElement(By.Id("create-protocol")).Click();
            Thread.Sleep(1000);
        }

        [When(@"I press delete button and confirm deletion")]
        public void WhenIPressDeleteButton()
        {
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("delete-protocol"));
            driver.FindElement(By.Id("delete-protocol")).Click();
            driver.ElementShouldBePresent(By.Id("confirm"));
            Thread.Sleep(1000);
            driver.FindElement(By.Id("confirm")).Click();
        }

        [When(@"add text input question")]
        public void WhenIAddTextInputQuestion()
        {
            driver.ElementShouldBePresent(By.Id("add-question"));
            driver.FindElement(By.Id("add-question")).Click();
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("question-menu"));
            driver.FindElements(By.Id("question-menu")).Last().Click();
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("text-input"));
            driver.FindElements(By.Id("text-input")).Last().Click();
            Thread.Sleep(1000);
        }

        [When(@"add multiple choice question")]
        public void WhenIAddMultipleChoiceQuestion()
        {
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("add-question"));
            driver.FindElement(By.Id("add-question")).Click();
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("question-menu"));
            driver.FindElements(By.Id("question-menu")).Last().Click();
            Thread.Sleep(1000);
            driver.ElementShouldBePresent(By.Id("multiple-choice"));
            driver.FindElements(By.Id("multiple-choice")).Last().Click();
            Thread.Sleep(1000);
        }

        [When(@"press section creation button")]
        public void WhenIPressCreateSectionButton()
        {
            driver.ElementShouldBePresent(By.Id("create-section"));
            driver.FindElement(By.Id("create-section")).Click();
        }

        [When(@"update question names")]
        public void WhenIUpdateQuestionNames()
        {
            var question = Any<string>();
            testingData.Add("question", question);

            driver.ElementShouldBePresent(By.Id("section-item"));
            driver.WriteNewText(By.Id("section-item"), question);
            driver.FindElement(By.Id("update-section")).Click();
        }

        [When(@"update question names with invalid characters")]
        public void WhenIUpdateQuestionNamesWithInvalid()
        {
            var question = "!@@$%^&$&*(*()";

            driver.ElementShouldBePresent(By.Id("section-item"));
            driver.WriteNewText(By.Id("section-item"), question);
        }

        [Then(@"I should not be allowed to update section")]
        public void ShouldNotBeAllowedToUpdate()
        {
            driver.FindElement(By.Id("update-section")).Enabled.Should().BeFalse();
        }

        [Then(@"template section should be updated")]
        public void SectionShouldBeUpdated()
        {
            driver.Navigate().Refresh();
            driver.ElementShouldBePresent(By.Id("section-name"));
            driver.ElementShouldBePresent(By.Id("section-item"));
            driver.FindElement(By.Id("section-item")).Text.Should().BeEquivalentTo(testingData["question"]);
        }

        [Then(@"section should be created")]
        public void SectionShouldBeCreated()
        {
            driver.Navigate().Back();
            driver.ElementShouldBePresent(By.Id("protocol-name"));
            driver.ElementShouldBePresent(By.Id("section-summary"));
            driver.FindElements(By.Id("section-summary")).Last().Click();
            var questions = driver.FindElements(By.Id("question-name"));
        }


        [Then(@"protocol should be deleted")]
        public void ProtocolsShouldBeDeleted()
        {
            driver.Navigate().Refresh();
            driver.ElementShouldBePresent(By.Id("protocol-name"));

            var nextButton = driver.FindElements(By.XPath("//nav[@id='pagination']/ul/li")).Reverse().Skip(1).First();
            nextButton.Click();

            var protocols = driver.FindElements(By.Id("protocol-name")).Select(x => x.Text);
            protocols.Should().NotContain(testingData["newProtocolName"]);
        }

        [Then(@"protocols should be displayed")]
        public void ProtocolsShouldBeDisplayed()
        {
            driver.ElementShouldBePresent(By.Id("protocol-name"));
        }

        [Then(@"protocol should be added to the list")]
        public void ProtocolsShouldBeAdded()
        {
            // need to navigate to the list end
            var nextButton = driver.FindElements(By.XPath("//nav[@id='pagination']/ul/li")).Reverse().Skip(1).First();
            nextButton.Click();

            driver.ElementShouldBePresent(By.Id("protocol-name"));
            var protocols = driver.FindElements(By.Id("protocol-name")).Select(x=> x.Text);
            protocols.Should().Contain(testingData["newProtocolName"]);
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
