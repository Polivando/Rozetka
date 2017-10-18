using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestsWithoutFramework
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver _driver;
        private string _url = "http://rozetka.com.ua/notebooks/c80004/filter/";

        [TestInitialize]
        public void TestInitialize()
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(_url);
        }

        [TestMethod]
        public void NegativeMinPriceShouldUpdatePriceToMinimalAvailable()
        {
            //arrange
            var minPriceField = _driver.FindElement(By.Id("price[min]"));
            var okButton = _driver.FindElement(By.Id("submitprice"));
            var priceValueToSet = -1;

            //act
            minPriceField.SendKeys(priceValueToSet.ToString());
            okButton.Click();

            //assert
            var actualValue = int.Parse(minPriceField.Text);
            Assert.IsTrue(actualValue - priceValueToSet > 0);
        }
    }
}
