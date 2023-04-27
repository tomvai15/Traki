
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Pages;
using static Traki.FunctionalTests.TestsSetup;

namespace Traki.FunctionalTests.Steps.Login
{

    [Binding]
    public class LoginSteps
    {
        private readonly ScenarioContext _context;
        private readonly LoginPage _loginPage;

        public LoginSteps(ScenarioContext context)
        {
            _context = context;
            _loginPage = new LoginPage(Driver);
        }

        [When(@"I enter my credentials")]
        public void GivenNotEnoughProductsInStock()
        {
            _loginPage.Login("Bybys", "Kiausiai");
        }

        [Then(@"I should be redirected to home page")]
        public void GivenEnoughProductsInStock()
        {
            var a = Driver.PageSource;
            return;
        }
    }
}
