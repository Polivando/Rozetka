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
        private string _url = "https://rozetka.com.ua/hudojestvennaya-literatura/c4326593/";

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
        public void NegativeMinPriceShouldUpdatePriceToMinimalAvailable()
        {
            //arrange
            var getMinPriceField = driver.FindElement(By.Id("price[min]"));
            var okButton = driver.FindElement(By.Id("submitprice"));
            var priceValueToSet = -1;

            //act
            getMinPriceField.SendKeys(priceValueToSet.ToString());
            okButton.Click();

            //assert
            getMinPriceField = driver.FindElement(By.Id("price[min]"));
            var actualValue = int.Parse(getMinPriceField.GetAttribute("value"));
            Assert.IsTrue(actualValue - priceValueToSet > 0);
        }
    }
}
