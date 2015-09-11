using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Threading;
using System.Timers;

namespace RozetkaTest
{
    class Tests
    {
        static LaptopPage page;

        public static void Initialize()
        {
            Cons.driver = new FirefoxDriver();
            Cons.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Cons.driver.Navigate().GoToUrl(Cons.path);
            page = new LaptopPage();
        }

        //Price tests
        [Test]
        public void TestPriceMinNeg()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestPriceMinNeg");
            var newPage = page.SetPrice(-999, 0);
            Cons.log("Negative minprice input");
            //Thread.Sleep(500);
            Price(newPage);
            Clean();
        }

        [Test]
        public void TestPriceMaxNeg()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestPriceMaxNeg");
            var newPage = page.SetPrice(0, -999);
            Cons.log("Negative maxprice input");
            //Thread.Sleep(500);
            Price(newPage);
            Clean();
        }

        [Test]
        public void TestPriceSwap()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestPriceSwap");
            var newPage = page.SetPrice(20000, 10000);
            Cons.log("Swapped price input");
            //Thread.Sleep(500);
            Price(newPage);
            Clean();
        }

        [Test]
        public void TestPrice()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestPrice");
            var newPage = page.SetPrice(10000, 20000);
            Cons.log("Normal price input");
            //Thread.Sleep(500);
            Price(newPage);
            Clean();
        }

        [Test]
        public void TestPriceNonNumeric()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestPriceNonNumeric");
            var newPage = page.SetPrice("hi", "filter");
            Cons.log("Non-numeric price input");
            //Thread.Sleep(500);
            Price(newPage);
            Clean();
        }

        //Other tests
        [Test]
        public void TestManfacturer()
        {
            Initialize();
            Manufacturer(page.FilterAcer);
            //here go other instances of manufacturer filter
            Clean();
        }

        [Test]
        public void TestSize()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestSize");
            Size(page.Filter9Inch);
            Size(page.Filter13Inch);
            Clean();
        }

        [Test]
        public void TestResolution()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestResolution");
            Resolution(page.FilterHD);
            Resolution(page.FilterMoarHD, false);
            Clean();
        }
        
        [Test]
        public void TestCPU()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestCPU");
            CPU(page.FilterPentium);
            CPU(page.FilterAmdE);
            Clean();
        }

        [Test]
        public static void TestKeyboard()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestKeyboard");
            Keyboard(page.FilterUkrainisch);
            Keyboard(page.FilterKeinUkrainisch);
            Clean();
        }

        [Test]
        public void TestColor()
        {
            Initialize();
            Cons.log(DateTime.Now.ToString() + " Performing TestColor");
            Color(page.FilterBlue);
            Clean();
        }

        void Price(LaptopPage newPage)
        {
            string write;
            bool passed;
            int minprice = newPage.getPrice(newPage.TxtMinPrice, Cons.Types.textInput);
            int maxprice = newPage.getPrice(newPage.TxtMaxPrice, Cons.Types.textInput);
            if (minprice <= maxprice)
            {
                Cons.log("Price behavior OK");
                newPage.getResults();
                foreach (var item in newPage.results)
                {
                    int price = newPage.getPrice(item, Cons.Types.result);
                    if ((price < minprice) || (price > maxprice))
                    {
                        passed = false;
                        Cons.log("Result price out of range");
                        write = "Test passed: " + passed.ToString() + "\n";
                        Cons.log(write); Cons.log("");
                        throw new ArgumentOutOfRangeException();
                    }
                }
                passed = true;
                write = "Test passed: " + passed.ToString() + "\n";
                Cons.log(write); Cons.log("");
            }
            else
            {
                passed = false;
                write = "Test passed: " + passed.ToString() + "\n";
                Cons.log(write); Cons.log("");
                throw new ArgumentOutOfRangeException();
            }
        }

        void Manufacturer(IWebElement checkbox)
        {
            Cons.log(DateTime.Now.ToString() + " Performing TestManufacturer");
            checkbox.Click();
            bool passed = true;
            var newPage = new LaptopPage();
            newPage.getResults();
            var key = newPage.getKey(checkbox);
            Cons.log(key);
            foreach (var item in newPage.results)
            {
                var value = newPage.getManufacturer(item);
                if (!value.Contains(key))
                {
                    passed = false;
                    Cons.log("Results mismatch");
                    Cons.log("Test passed: " + passed.ToString() + "\n");
                    throw new Exception();
                }
            }
            Cons.log("Test passed: " + passed.ToString() + "\n");
            newPage.BtnReset.Click();
        }

        void Size(IWebElement checkbox)
        {
            checkbox.Click();
            bool passed = true;
            var newPage = new LaptopPage();
            newPage.getResults();
            var temp = newPage.getKey(checkbox);
            Cons.log(temp);
            var key = newPage.parseSize(temp);
            foreach (var item in newPage.results)
            {
                var value = newPage.parseSize(newPage.getShort(item));
                if ( value[0] < key[0] || value[0] > key[1])
                {
                    passed = false;
                    Cons.log("Results mismatch");
                    Cons.log("Test passed: " + passed.ToString() + "\n");
                    throw new Exception();
                }
            }
            Cons.log("Test passed: " + passed.ToString() + "\n");
            newPage.BtnReset.Click();
        }
        
        void Resolution(IWebElement checkbox, bool exact = true)
        {
            page.ResolutionAll.Click();
            checkbox.Click();
            bool passed = true;
            var newPage = new LaptopPage();
            newPage.getResults();
            var temp = newPage.getKey(checkbox);
            Cons.log(temp);
            int key = newPage.getResolution(temp);
            foreach (var item in newPage.results)
            {
                var value = newPage.getResolution(newPage.getShort(item));
                if (exact ? (value != key) : (value <= key))
                    {
                        passed = false;
                        Cons.log("Results mismatch");
                        Cons.log("Test passed: " + passed.ToString() + "\n");
                        throw new Exception();
                    }
            }
            Cons.log("Test passed: " + passed.ToString() + "\n");
            newPage.BtnReset.Click();
        }

        void CPU(IWebElement checkbox)
        {
            page.CPUAll.Click();
            checkbox.Click();
            bool passed = true;
            var newPage = new LaptopPage();
            newPage.getResults();
            var key = newPage.getKey(checkbox);
            Cons.log(key);
            foreach (var item in newPage.results)
            {
                if (!newPage.CPUCheck(newPage.getShort(item), key))
                {
                    passed = false;
                    Cons.log("Results mismatch");
                    Cons.log("Test passed: " + passed.ToString() + "\n");
                    throw new Exception();
                }
            }
            Cons.log("Test passed: " + passed.ToString() + "\n");
            newPage.BtnReset.Click();
        }

        static void Keyboard(IWebElement checkbox)
        {
            page.KeyboardUnfold.Click();
            var key = page.getKey(checkbox);
            checkbox.Click();
            bool passed = true;
            var newPage = new LaptopPage();
            newPage.getResults();
            Cons.log("Украинская раскладка "+key);
            foreach (var item in newPage.results)
                VerifyKeyboard(item, key);
            Cons.log("Test passed: " + passed.ToString() + "\n");
            newPage.BtnReset.Click();
        }

        void Color(IWebElement checkbox)
        {
            page.ColorUnfold.Click();
            var key = page.getKey(checkbox);
            checkbox.Click();
            bool passed = true;
            var newPage = new LaptopPage();
            newPage.getResults();
            Cons.log("Color " + key);
            foreach (var item in newPage.results)
                VerifyColor(item, key);
            Cons.log("Test passed: " + passed.ToString() + "\n");
            newPage.BtnReset.Click();
        }

        static void VerifyKeyboard(IWebElement item, string key)
        {
            var currHandle = Cons.driver.CurrentWindowHandle;
            Cons.OpenInNewTab(item);
            var about = new AboutPage();
            about.Characteristics.Click();
            var characteristics = new CharacteristicsPage();
            var value = characteristics.getKeyboard();
            bool passed = true;
            if (key != value)
            {
                passed = false;
                Cons.log("Results mismatch");
                Cons.log("Test passed: " + passed.ToString() + "\n");
                throw new Exception();
            }
            characteristics.Overview.SendKeys(Keys.Control + "w");
            Cons.driver.SwitchTo().Window(currHandle);
        }

        void VerifyColor(IWebElement item, string key)
        {
            var currHandle = Cons.driver.CurrentWindowHandle;
            Cons.OpenInNewTab(item);
            var about = new AboutPage();
            about.Characteristics.Click();
            var characteristics = new CharacteristicsPage();
            var value = characteristics.getColor(key);
            bool passed = true;
            if (!value.Contains(key))
            {
                passed = false;
                Cons.log("Results mismatch");
                Cons.log("Test passed: " + passed.ToString() + "\n");
                throw new Exception();
            }
            characteristics.Overview.SendKeys(Keys.Control + "w");
            Cons.driver.SwitchTo().Window(currHandle);
        }
        public static void Clean()
        {
            Cons.driver.Close();
        }
    }
}
