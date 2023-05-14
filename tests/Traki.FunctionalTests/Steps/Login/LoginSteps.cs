
using FluentAssertions;
using TechTalk.SpecFlow;


namespace Traki.FunctionalTests.Steps.Login
{

    [Binding]
    public class LoginSteps
    {
        private readonly ScenarioContext _context;

        public LoginSteps(ScenarioContext context)
        {
            _context = context;
        }

        [When(@"I enter wrong credentials")]
        public void GivenNotEnoughProductsInStock()
        {
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            Thread.Sleep(1000);
        }

        [Then(@"I should be redirected to home page")]
        public void GivenEnoughProductsInStock()
        {
        }

        [Then(@"error message should be presented")]
        public void ThenErrorMessagePresented()
        {
        }
    }
}
