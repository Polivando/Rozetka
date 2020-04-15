using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TestFramework.Pages;

namespace TestsUseFramework
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
            //Arrange
            var booksResultsPage = new FictionBooksPage(driver);
            var priceValueToSet = -1;

            //Act
            booksResultsPage.SetMinimumPrice(priceValueToSet);

            //Assert
            Assert.IsTrue(
                ! booksResultsPage.CanSubmitPriceFilter
                && booksResultsPage.MinPriceHasError
                && booksResultsPage.MaxPriceHasError);
        }
    }
}
