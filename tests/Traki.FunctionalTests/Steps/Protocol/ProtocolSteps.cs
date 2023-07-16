using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Extensions;

namespace Traki.FunctionalTests.Steps.Protocol
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

        [Given(@"I have created protocol")]
        public void IHaveCreatedProtocol()
        {
            IEnterNewProtocolNameAndSubmit();
            ProtocolsShouldBeAdded();
            _driver.FindElements(By.XPath("//div[@id='protocol-name']/span")).Last().Click();
        }

        [Given("I have navigated to protocol templates page")]
        [When(@"I press on Templates tab")]
        public void WhenINavigateToTemplatesTab()
        {
            _driver.ElementShouldBePresent(By.Id("protocols-drawer"));
            _driver.FindElement(By.Id("protocols-drawer")).Click();
            _driver.ElementShouldBePresent(By.Id("protocol-name"));
        }

        [When(@"I open create section page")]
        public void IHaveOpenedCreateSectionPage()
        {
            _driver.ElementShouldBePresent(By.Id("create-section"));
            _driver.FindElement(By.Id("create-section")).Click();
            _driver.ElementShouldBePresent(By.Id("section-name"));
            Thread.Sleep(1000);
        }

        [When(@"I open edit section page")]
        public void IHaveOpenedEditSectionPage()
        {
            _driver.ElementShouldBePresent(By.Id("section-summary"));
            _driver.FindElement(By.Id("section-summary")).Click();
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("edit-section"));
            _driver.FindElement(By.Id("edit-section")).Click();
            _driver.ElementShouldBePresent(By.Id("section-name"));
            Thread.Sleep(1000);
        }


        [When(@"I enter new protocol name and submit")]
        public void IEnterNewProtocolNameAndSubmit()
        {
            string newProtocolName = Any<string>();
            _scenarioContext.Add("newProtocolName", newProtocolName);
            _driver.ElementShouldBePresent(By.Id("new-protocol-name"));
            _driver.WriteNewText(By.Id("new-protocol-name"), newProtocolName);
            _driver.FindElement(By.Id("create-protocol")).Click();
            Thread.Sleep(1000);
        }

        [When(@"I press delete button and confirm deletion")]
        public void WhenIPressDeleteButton()
        {
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("delete-protocol"));
            _driver.FindElement(By.Id("delete-protocol")).Click();
            _driver.ElementShouldBePresent(By.Id("confirm"));
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("confirm")).Click();
        }

        [When(@"add text input question")]
        public void WhenIAddTextInputQuestion()
        {
            _driver.ElementShouldBePresent(By.Id("add-question"));
            _driver.FindElement(By.Id("add-question")).Click();
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("question-menu"));
            _driver.FindElements(By.Id("question-menu")).Last().Click();
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("text-input"));
            _driver.FindElements(By.Id("text-input")).Last().Click();
            Thread.Sleep(1000);
        }

        [When(@"add multiple choice question")]
        public void WhenIAddMultipleChoiceQuestion()
        {
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("add-question"));
            _driver.FindElement(By.Id("add-question")).Click();
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("question-menu"));
            _driver.FindElements(By.Id("question-menu")).Last().Click();
            Thread.Sleep(1000);
            _driver.ElementShouldBePresent(By.Id("multiple-choice"));
            _driver.FindElements(By.Id("multiple-choice")).Last().Click();
            Thread.Sleep(1000);
        }

        [When(@"press section creation button")]
        public void WhenIPressCreateSectionButton()
        {
            _driver.ElementShouldBePresent(By.Id("create-section"));
            _driver.FindElement(By.Id("create-section")).Click();
        }

        [When(@"update question names")]
        public void WhenIUpdateQuestionNames()
        {
            var question = Any<string>();
            _scenarioContext.Add("question", question);

            _driver.ElementShouldBePresent(By.Id("section-item"));
            _driver.WriteNewText(By.Id("section-item"), question);
            _driver.FindElement(By.Id("update-section")).Click();
        }

        [When(@"update question names with invalid characters")]
        public void WhenIUpdateQuestionNamesWithInvalid()
        {
            var question = "!@@$%^&$&*(*()";

            _driver.ElementShouldBePresent(By.Id("section-item"));
            _driver.WriteNewText(By.Id("section-item"), question);
        }

        [When(@"press delete button")]
        public void WhenIPressSectionDeleteButton()
        {
            _scenarioContext.Add("currentUrl", _driver.Url);
            _driver.FindElement(By.Id("delete-section")).Click();
        }

        [Then(@"section should be deleted")]
        public void SectionShouldBeDeleted()
        {
            _driver.Navigate().GoToUrl(_scenarioContext.Get<string>("currentUrl"));
            _driver.ElementShouldBePresent(By.Id("not-found"));
        }

        [Then(@"I should not be allowed to update section")]
        public void ShouldNotBeAllowedToUpdate()
        {
            _driver.FindElement(By.Id("update-section")).Enabled.Should().BeFalse();
        }

        [Then(@"template section should be updated")]
        public void SectionShouldBeUpdated()
        {
            _driver.Navigate().Refresh();
            _driver.ElementShouldBePresent(By.Id("section-name"));
            _driver.ElementShouldBePresent(By.Id("section-item"));
            _driver.FindElement(By.Id("section-item")).Text.Should().BeEquivalentTo(_scenarioContext.Get<string>("question"));
        }

        [Then(@"section should be created")]
        public void SectionShouldBeCreated()
        {
            _driver.Navigate().Back();
            _driver.ElementShouldBePresent(By.Id("protocol-name"));
            _driver.ElementShouldBePresent(By.Id("section-summary"));
            _driver.FindElements(By.Id("section-summary")).Last().Click();
            var questions = _driver.FindElements(By.Id("question-name"));
        }


        [Then(@"protocol should be deleted")]
        public void ProtocolsShouldBeDeleted()
        {
            _driver.Navigate().Refresh();
            _driver.ElementShouldBePresent(By.Id("protocol-name"));

            var nextButton = _driver.FindElements(By.XPath("//nav[@id='pagination']/ul/li")).Reverse().Skip(1).First();
            nextButton.Click();

            var protocols = _driver.FindElements(By.Id("protocol-name")).Select(x => x.Text);
            protocols.Should().NotContain(_scenarioContext.Get<string>("newProtocolName"));
        }

        [Then(@"protocols should be displayed")]
        public void ProtocolsShouldBeDisplayed()
        {
            _driver.ElementShouldBePresent(By.Id("protocol-name"));
        }

        [Then(@"protocol should be added to the list")]
        public void ProtocolsShouldBeAdded()
        {
            // need to navigate to the list end
            var nextButton = _driver.FindElements(By.XPath("//nav[@id='pagination']/ul/li")).Reverse().Skip(1).First();
            nextButton.Click();

            _driver.ElementShouldBePresent(By.Id("protocol-name"));
            var protocols = _driver.FindElements(By.Id("protocol-name")).Select(x=> x.Text);
            protocols.Should().Contain(_scenarioContext.Get<string>("newProtocolName"));
        }
    }
}
