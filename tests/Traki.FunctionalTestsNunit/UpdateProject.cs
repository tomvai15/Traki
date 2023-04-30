﻿using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using Traki.FunctionalTestsNunit.Common;

namespace Traki.FunctionalTestsNunit
{
    [TestFixture]
    public class UpdateProject
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = BuildDriver();
            baseURL = "https://www.google.com/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheUpdateProjectTest()
        {
            driver.Navigate().GoToUrl("https://localhost:3000/login");
            driver.FindElement(By.Id("email")).Click();
            driver.FindElement(By.Id("email")).Clear();
            driver.FindElement(By.Id("email")).SendKeys("vainoristomas9@gmail.com");
            driver.FindElement(By.Id("password")).Click();
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("password");
            driver.FindElement(By.Id("submit")).Click();
        //    driver.Navigate().GoToUrl("https://localhost:3000/home");
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

            string randomValue = Any<string>().Substring(0,10);

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
            driver.FindElement(By.XPath("//div[@id='projects-drawer']/div[2]/span")).Click();
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
                    if (IsElementPresent(By.XPath("//div[@id='root']/div/main/div[2]/div/div[2]/div/div/div/p"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            Assert.AreEqual(randomValue, driver.FindElement(By.Id("project-name")).GetAttribute("value"));
            Assert.AreEqual(randomValue, driver.FindElement(By.Id("project-address")).GetAttribute("value"));
            Assert.AreEqual(randomValue, driver.FindElement(By.Id("project-client")).GetAttribute("value"));
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

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}