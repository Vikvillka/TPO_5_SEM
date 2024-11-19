using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SeleniumExtras.PageObjects;

namespace ClassLibraryPOM
{
    public class OneProductPage
    {
        private IWebDriver _driver;
        private WebDriverWait wait;

        [FindsBy(How = How.CssSelector, Using = ".product-page__order-buttons > div > div > div > button")]
        private IWebElement _addToCart;

        [FindsBy(How = How.CssSelector, Using = ".j-item-basket")]
        private IWebElement _btnCart;

        public OneProductPage(IWebDriver driver)
        {
            _driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            PageFactory.InitElements(driver, this);
        }

        public void AddToCart()
        {
            _addToCart.Click();
        }

        public void GetButtonCart()
        {
            _btnCart.Click();
        }
    }
}
