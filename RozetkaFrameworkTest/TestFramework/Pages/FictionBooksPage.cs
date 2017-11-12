using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;

namespace TestFramework.Pages
{
    public class FictionBooksPage
    {
        private IWebDriver _driver;

        public FictionBooksPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "price[min]")]
        public IWebElement MinimumPrice { get; set; }

        [FindsBy(How = How.Id, Using = "price[max]")]
        public IWebElement MaximumPrice { get; set; }

        [FindsBy(How = How.Id, Using = "submitprice")]
        public IWebElement FilterByPrice { get; set; }

        public void WaitForPageLoad(string url)
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(5)).Until(d => d.Url == url);
        }

        public FictionBooksPage SetMinimumPrice(int? price)
        {
            if (price == null) return this;
            MinimumPrice.SendKeys(price.ToString());
            return this;
        }

        public FictionBooksPage SetMaximumPrice(int? price)
        {
            if (price == null) return this;
            MaximumPrice.SendKeys(price.ToString());
            return this;
        }

        public FictionBooksPage SubmitPriceFilter()
        {
            FilterByPrice.SendKeys(Keys.Enter);
            return this;
        }

        public FictionBooksPage FilterByPriceRange(int? minPrice = null, int? maxPrice = null)
        {
            return SetMinimumPrice(minPrice).SetMaximumPrice(maxPrice).SubmitPriceFilter();
        }

        public int? GetMinPrice()
        {
            var stringValue = MinimumPrice.GetAttribute("value");
            if (stringValue == null | stringValue == "")
                return null;
            else
            {
                int.TryParse(stringValue, out int result);
                return result;
            }
        }

        public int? GetMaxPrice()
        {
            var stringValue = MaximumPrice.GetAttribute("value");
            if (stringValue == null | stringValue == "")
                return null;
            else
            {
                int.TryParse(stringValue, out int result);
                return result;
            }
        }
    }
}
