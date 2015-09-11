using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace RozetkaTest
{
    class LaptopPage
    {
        //Initialize elements in constructor
        public LaptopPage()
        {
            PageFactory.InitElements(Cons.driver, this);
        }

        //Reset button
        [FindsBy(How = How.XPath, Using = "//*[@id='title_page']/div/div/div[3]/ul/li[2]/noindex/a")]
        public IWebElement BtnReset { get; set; }

        //Laptops list
        public IReadOnlyCollection<IWebElement> results;

        //Price filter controls
        [FindsBy(How = How.Id, Using = "price[min]")]
        public IWebElement TxtMinPrice { get; set; }

        [FindsBy(How = How.Id, Using = "price[max]")]
        public IWebElement TxtMaxPrice { get; set; }

        [FindsBy(How = How.Id, Using = "submitprice")]
        public IWebElement BtnPrice { get; set; }

        //All other filters, 10 total
        [FindsBy(How = How.XPath, Using = "//*[@id='sort_producer']/li[1]/label/a/span")]
        public IWebElement FilterAcer { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_20861']/li[1]/label/a/span")]
        public IWebElement Filter9Inch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_20861']/li[2]/label/a/span")]
        public IWebElement Filter13Inch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='filter_parameters_form']/div[5]/div[2]/span/img")]
        public IWebElement ResolutionAll { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_25800']/li[4]/label/a/span")]
        public IWebElement FilterHD { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_25800']/li[5]/label/a/span")]
        public IWebElement FilterMoarHD { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='filter_parameters_form']/div[9]/div[2]/span/img")]
        public IWebElement CPUAll { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_processor']/li[5]/label/a/span")]
        public IWebElement FilterPentium { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_processor']/li[9]/label/a/span")]
        public IWebElement FilterAmdE { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='filter_parameters_form']/div[18]/div[1]/span")]
        public IWebElement KeyboardUnfold { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_56017']/li[1]/label/a/span")]
        public IWebElement FilterUkrainisch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_56017']/li[2]/label/a/span")]
        public IWebElement FilterKeinUkrainisch { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='filter_parameters_form']/div[20]/div[1]/span[1]")]
        public IWebElement ColorUnfold { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='sort_21737']/li[2]/label/a/span")]
        public IWebElement FilterBlue { get; set; }

        //Set just 1 out of 2 price limits
        public LaptopPage SetPrice(int minValue, int maxValue)
        {
            if (minValue == 0) { }
            else { TxtMinPrice.SendKeys(minValue.ToString()); }
            
            if (maxValue == 0) { }
            else TxtMaxPrice.SendKeys(maxValue.ToString());
            this.BtnPrice.Click();
            return new LaptopPage();
        }

        //set both price limits
        public LaptopPage SetPrice(string minvalue, string maxvalue)
        {
            TxtMinPrice.SendKeys(minvalue);
            TxtMaxPrice.SendKeys(maxvalue);
            this.BtnPrice.Click();
            return new LaptopPage();
        }

        //expands page, forms results list
        public void getResults()
        {
            bool t = true;
            while (t)
            {
                t = false;
                try
                {
                    var btnMoar = Cons.driver.FindElement(By.ClassName("g-i-more-link-text"));
                    btnMoar.Click();
                    Thread.Sleep(500);
                    t = true;
                }
                catch {}
            }
            results = Cons.driver.FindElements(By.ClassName("g-i-tile-i-box-desc"));
            Thread.Sleep(500);
            return;
        }

        //gets int price value from txtBox or result item
        public int getPrice (IWebElement item, Cons.Types type)
        {
            string temp;
            if (type == Cons.Types.result)
            {
                var reg = Regex.Matches(item.FindElement(By.ClassName("g-price-uah")).Text, "\\d+");
                temp = reg[0].ToString() + reg[1].ToString();
            }
            else /*if (type == Cons.Types.textInput)*/
            {
                temp = item.GetAttribute("value");
            }
            return Convert.ToInt32(temp);
        }

        //gets manufacturer string from result item
        public string getManufacturer (IWebElement item)
        {
            return item.FindElement(By.XPath("//*[@class='g-i-tile-i-title clearfix']/a")).Text;
        }

        //gets text value of checkbox
        public string getKey(IWebElement checkbox)
        {
            return checkbox.FindElement(By.TagName("i")).Text;
        }

        //gets short description
        public string getShort(IWebElement item)
        {
            var temp = item.FindElement(By.XPath("//*[@class='g-i-tile-short-detail']/li"));
            return ((IJavaScriptExecutor)Cons.driver).ExecuteScript("return arguments[0].innerHTML",temp).ToString();
            
        }
        
        //forms int array of size limits from a string
        public double[] parseSize(string str)
        {
            var reg = Regex.Matches(str, "\\d+\\.?\\d*[\"|']{1,2}");
            List<string> temp = new List<string>();
            temp.Add(Regex.Match(reg[0].ToString(), "\\d+").Value);
            try
            {
                temp.Add(Regex.Match(reg[1].ToString(), "\\d+").Value);
            }
            catch
            {
                temp.Add(temp[0]);
            }
            double[] res = new double[2];
            res[0] = Convert.ToDouble(temp[0]);
            res[1] = Convert.ToDouble(temp[1]);
            return res;
        }

        //gets resolution height from a string
        public int getResolution(string str)
        {
            if (str.Contains("HD"))
                return 1080;
            else
            {
                var temp = Regex.Match(str, "\\d+[x|х]\\d+");
                return Convert.ToInt32(Regex.Replace(temp.ToString(), "\\d+[x|х]", ""));
            }
            
        }

        public bool CPUCheck(string input, string key)
        {
            var temp = Regex.Replace(key, "\\s", "\\s.*\\s?");
            return Regex.IsMatch(input,temp);
        }
    }
}
