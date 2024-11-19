using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryPOM
{
    public class HomePage
    {
        private IWebDriver _driver;
        private WebDriverWait wait;

        [FindsBy(How = How.Id, Using = "searchInput")] 
        private IWebElement _searchInput;

        [FindsBy(How = How.CssSelector, Using = "button.cookies__btn-link")]
        private IWebElement _cookies_btn;

        public HomePage(IWebDriver driver)
        {
          
            _driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            PageFactory.InitElements(driver, this);
        }

        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void SearchForItem(string itemName)
        {
            _searchInput.SendKeys(itemName + Keys.Enter);
        }

        public void AcceptCookies()
        {
            _cookies_btn.Click();
        }
    }
}
