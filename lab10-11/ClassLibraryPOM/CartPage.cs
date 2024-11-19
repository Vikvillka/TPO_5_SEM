using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.PageObjects;

namespace ClassLibraryPOM
{
    public class CartPage
    {
        private IWebDriver _driver;
        private WebDriverWait wait;

        [FindsBy(How = How.CssSelector, Using = ".j-item-basket")]
        private IWebElement _item_basket;

        [FindsBy(How = How.ClassName, Using = "list-item__wrap")]
        private IList<IWebElement> _item_wrap_text;

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            PageFactory.InitElements(driver, this);
        }

        public void OpenCart()
        { 
            _item_basket.Click();
        }

        public int GetItemCount()
        {
            return _item_wrap_text.Count;
        }
    }
}
