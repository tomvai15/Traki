using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traki.FunctionalTests.Pages
{
    public class LoginPage : BasePage
    {
        private const string Url = "login";

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void Login(string email, string password)
        {
            FindEmailInput().SendKeys(email);
            FindPasswordInput().SendKeys(password);
            return;
        }

        public IWebElement FindEmailInput()
             => Driver.FindElement(By.Id(".email"));

        public IWebElement FindPasswordInput()
            => Driver.FindElement(By.Id(".password"));
    }
}
