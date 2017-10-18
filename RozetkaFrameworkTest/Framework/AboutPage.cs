using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaTest
{
    class AboutPage
    {
        [FindsBy(How = How.ClassName, Using = "detail-title")]//<h1 class="detail-title" itemprop="name">
        public IWebElement Manufacturer { get; set; }

        [FindsByAll]
        [FindsBy(How = How.ClassName, Using = "m-tabs-i",Priority = 0)]
        [FindsBy(How = How.Name, Using = "characteristics", Priority = 1)]//Button to characteristics tab
        public IWebElement Characteristics { get; set; }

        [FindsBy(How = How.ClassName, Using = "detail-description")]
        public IWebElement Details { get; set; }//Short characteristics

        public AboutPage()
        {
            PageFactory.InitElements(Cons.driver, this);
        }
    }
}
