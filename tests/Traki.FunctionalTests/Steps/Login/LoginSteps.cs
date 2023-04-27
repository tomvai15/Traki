
using FluentAssertions;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Extensions;
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

        [When(@"I enter wrong credentials")]
        public void GivenNotEnoughProductsInStock()
        {
            _loginPage.Login("Bybys", "Kiausiai");
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            _loginPage.Login("vainoristomas@gmail.com", "password");
        }

        [Then(@"I should be redirected to home page")]
        public void GivenEnoughProductsInStock()
        {
            var url = _loginPage.Driver.GetLocalUrl();
            url.Should().Be("home");
        }

        [Then(@"error message should be presented")]
        public void ThenErrorMessagePresented()
        {
            var errorMessage = _loginPage.GetErrorMessage();
            errorMessage.Should().Be("Email or password is incorrect");
        }
    }
}
