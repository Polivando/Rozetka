using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestFramework.Pages;

namespace TestsUseFramework
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
            //options.AddArgument("--test-type");
            options.AddArgument("start-maximized");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(_url);
        }

        [TestCleanup]
        public void TestFinalize()
        {
            driver.Close();
        }

        [TestMethod]
        public void NegativeMinPriceShouldUpdatePriceToMinimalAvailable()
        {
            //Arrange
            var booksResultsPage = new FictionBooksPage(driver);
            booksResultsPage.WaitForPageLoad(_url);
            
            var priceValueToSet = -1;

            //Act
            booksResultsPage.FilterByPriceRange(priceValueToSet);

            //Assert
            var actualMinimumPrice = booksResultsPage.GetMinPrice();
            Assert.IsTrue(actualMinimumPrice - priceValueToSet > 0);
        }
    }
}
