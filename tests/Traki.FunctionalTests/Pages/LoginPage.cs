using OpenQA.Selenium;
using Selenium.WebDriver.WaitExtensions;
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
            EmailInput.SendKeys(email);
            PasswordInput.SendKeys(password);
            SubmitButton.Click();
            Thread.Sleep(2000);
        }

        public string GetErrorMessage()
        {
            return ErrorField.Text;
        }

        public IWebElement EmailInput
             => Driver.FindElement(By.Id("email"));

        public IWebElement PasswordInput
            => Driver.FindElement(By.Id("password"));

        public IWebElement SubmitButton
            => Driver.FindElement(By.Id("submit"));

        public IWebElement ErrorField
            => Driver.FindElement(By.Id("error"));
    }
}
