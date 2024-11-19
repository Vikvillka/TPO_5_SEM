using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryPOM
{
    public class CookieHomePage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public CookieHomePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        public void NavigateTo(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public IReadOnlyCollection<Cookie> GetCookies()
        {
            return driver.Manage().Cookies.AllCookies;
        }
    }
}
