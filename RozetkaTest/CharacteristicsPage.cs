using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RozetkaTest
{
    class CharacteristicsPage
    {

        [FindsBy(How = How.ClassName, Using = "pp-characteristics-tab-l")]
        public IWebElement Table { get; set; }

        [FindsBy(How = How.Id, Using = "tabs")]
        public IWebElement Overview { get; set; }

        public CharacteristicsPage()
        {
            PageFactory.InitElements(Cons.driver, this);
        }

        public string getKeyboard(string key)
        {
            var rows = Table.FindElements(By.ClassName("pp-characteristics-tab-i"));
            foreach (var item in rows)
            {
                var title = item.FindElement(By.TagName("dt"));
                if (title.Text.Contains("раскладка"))
                    return item.FindElement(By.TagName("dd")).Text;
            }
            return "";
        }

        public string getColor(string key)
        {
            var rows = Table.FindElements(By.ClassName("pp-characteristics-tab-i"));
            foreach (var item in rows)
            {
                var title = item.FindElement(By.TagName("dt"));
                if (title.Text.Contains("Цвет"))
                    return item.FindElement(By.TagName("dd")).Text;
            }
            return "";
        }
    }
}