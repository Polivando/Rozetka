using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace RozetkaTest
{
    public static class Cons
    {
        public enum Types { textInput, result};

        public enum Filter { manufacturer, size, resolution, CPU, ukrainian, color};

        public static IWebDriver driver;
        
        public static readonly string path = "http://rozetka.com.ua/notebooks/c80004/filter/";

        public static string output = Directory.GetCurrentDirectory() + "\\log.txt";

        public static void log(string text)
        {
            string[] arr = new String [] {text};
            File.AppendAllLines(output, arr);
        }

        public static void OpenInNewTab(IWebElement item)
        {
            var url = item.FindElement(By.TagName("a"));
            url.SendKeys(Keys.Control + Keys.Return);
            url.SendKeys(Keys.Control + Keys.Tab);
            Cons.driver.SwitchTo().Window(Cons.driver.WindowHandles[Cons.driver.WindowHandles.Count - 1]);
        }

        //public static object ExecuteJavaScript(string script, params object[] args)
        //{
        //    var executor = (IJavaScriptExecutor)driver;
        //    return executor.ExecuteScript(script, args);
        //}
    }
}
