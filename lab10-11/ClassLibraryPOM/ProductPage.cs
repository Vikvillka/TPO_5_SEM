using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.DevTools.V127.Page;
using System.Xml.Linq;
using SeleniumExtras.PageObjects;

namespace ClassLibraryPOM
{
    public class ProductPage
    {
        private IWebDriver _driver;
        private WebDriverWait wait;

        [FindsBy(How = How.ClassName, Using = "j-price")]
        private IList<IWebElement> _priceFilters;

        [FindsBy(How = How.XPath, Using = "//button[text()='Готово']")]
        private IWebElement _btnOk;

        [FindsBy(How = How.CssSelector, Using = ".price__lower-price")]
        private IList<IWebElement> _price_lowerPrice;

        [FindsBy(How = How.CssSelector, Using = ".product-card__wrapper")]
        private IWebElement _cart_wrapper;

        public ProductPage(IWebDriver driver)
        {
            _driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            PageFactory.InitElements(driver, this);
        }

        public void SetPriceFilter(double minPrice, double maxPrice)
        {
            if (_priceFilters.Count > 0)
            {
                _priceFilters[0].Clear();
                _priceFilters[0].SendKeys(minPrice.ToString());
                Thread.Sleep(2000);
                _priceFilters[1].Clear();
                _priceFilters[1].SendKeys(maxPrice.ToString());
                Thread.Sleep(4000);

                _btnOk.Click();
            }
            else
            {
                throw new NoSuchElementException("Элементы ввода не найдены.");
            }
        }

        public void GetFilter()
        {
            var elements = wait.Until(d => d.FindElements(By.ClassName("dropdown-filter__btn-name")));

            Actions actions = new Actions(_driver);

            if (elements.Count > 0)
            {
                IWebElement firstElement = elements[2];
                Thread.Sleep(1000);
                actions.MoveToElement(firstElement).Perform();
            }
            else
            {
                throw new NoSuchElementException("Элемент не найдены.");
            }
        }


        public List<IWebElement> GetPrices()
        {
            return _price_lowerPrice.ToList();
        }


        public void GetProduct()
        {
            if (_cart_wrapper != null)
            {
                _cart_wrapper.Click();
            }
            else
            {
                throw new NoSuchElementException("Элемент не найдены.");
            }
        }
    }
}
