using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace TestsWithoutFramework
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;
        private string _url = "https://rozetka.com.ua/ua/hudojestvennaya-literatura/c4326593/";

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(_url);            
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(d => d.Url == _url);
        }

        [TestCleanup]
        public void TestFinalize()
        {
            driver.Close();
        }

        [TestMethod]
        public void NegativeMinPriceShouldBlockUserFromFiltering()
        {
            //arrange
            var priceValueToSet = -1;

            //act
            driver.FindElement(By.CssSelector("[formcontrolname='min']")).SendKeys(priceValueToSet.ToString());

            //assert
            var isMinFieldRed = driver.FindElement(By.CssSelector("[formcontrolname='min']")).GetAttribute("class").Contains("form_state_error");
            var isMaxFieldRed = driver.FindElement(By.CssSelector("[formcontrolname='max']")).GetAttribute("class").Contains("form_state_error");
            var isSubmitButtonEnabled = driver.FindElement(By.CssSelector("[type='submit']")).Enabled;
            Assert.IsFalse(isSubmitButtonEnabled);
            Assert.IsTrue(isMinFieldRed && isMaxFieldRed);
        }
    }
}
