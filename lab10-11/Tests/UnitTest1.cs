using ClassLibraryPOM;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;


[TestFixture]
public class Tests
{
    private IWebDriver _driver;
    private WebDriverWait wait;
    private string reportFilePath = "TestReport.docx";

    private HomePage homePage;
    private ProductPage productPage;
    private OneProductPage oneProductPage;
    private CartPage cartPage;
    private CookieHomePage cookieHomePage;


    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        CreateReportDocument();
    }

    [SetUp]
    public void Setup()
    {

        var options = new ChromeOptions();
        //options.AddArgument("--headless");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--start-maximized");


        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

        homePage = new HomePage(_driver);
        productPage = new ProductPage(_driver);
        oneProductPage = new OneProductPage(_driver);
        cartPage = new CartPage(_driver);
        cookieHomePage = new CookieHomePage(_driver);

        InsertTextToWord("Начало тестирования: " + DateTime.Now.ToString());
    }

    private void CreateReportDocument()
    {
        using (var document = WordprocessingDocument.Create(reportFilePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
        {
            var mainPart = document.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = new Body();

            var runProperties = new RunProperties();
            runProperties.Append(new FontSize() { Val = "32" }); 

            var run = new Run(runProperties, new Text("Отчет о тестировании"));

            var paragraph = new Paragraph(run);
            body.Append(paragraph);

            mainPart.Document.Append(body);
            mainPart.Document.Save();
        }
    }

    private void InsertTextToWord(string text)
    {
        using (var document = WordprocessingDocument.Open(reportFilePath, true))
        {
            var mainPart = document.MainDocumentPart;
            var body = mainPart.Document.Body;

            var runProperties = new RunProperties();
            runProperties.Append(new FontSize() { Val = "24" }); 

            var run = new Run(runProperties, new Text(text));

            body.Append(new Paragraph(run));
            mainPart.Document.Save();
        }
    }

    private void SaveScreenshot(IWebDriver driver, string fileName)
    {
       
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

        string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

        screenshot.SaveAsFile(filePath); 
        Console.WriteLine($"Скриншот сохранен: {filePath}");
    }

    
    [TestCase("Джинсы", 30, 70)]
    [TestCase("Кроссовки", 50, 200)]
    public void SearchAndFilterTest(string searchItem, double minPrice, double maxPrice)
    {
        try
        {
            InsertTextToWord($"Тест SearchAndFilterTest: searchItem = {searchItem}, minPrice = {minPrice}, maxPrice = {maxPrice} " + DateTime.Now.ToString());

            /* driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
             IWebElement element = driver.FindElement(By.Id("searchInput"));*/

            homePage.NavigateTo("https://www.wildberries.by/");
            Thread.Sleep(3000);

            /* element.SendKeys(searchItem);
             element.SendKeys(OpenQA.Selenium.Keys.Enter);*/

            homePage.SearchForItem(searchItem);
            Thread.Sleep(3000);
            homePage.AcceptCookies();

            /* element = wait.Until(driver => driver.FindElement(By.CssSelector("button.cookies__btn-link")));
             element.Click();*/

            Actions actions = new Actions(_driver);

            productPage.GetFilter();

           /* var elements = wait.Until(driver => driver.FindElements(By.ClassName("dropdown-filter__btn-name")));
            if (elements.Count > 0)
            {
                IWebElement firstElement = elements[2];
                Thread.Sleep(1000);
                actions.MoveToElement(firstElement).Perform();
            }
            else
            {
                Assert.Fail("Элементы не найдены.");
                InsertTextToWord("Тест провален: Элементы не найдены. " + DateTime.Now.ToString());
            }*/

            productPage.SetPriceFilter(minPrice, maxPrice);

            /*var elementsInput = wait.Until(driver => driver.FindElements(By.ClassName("j-price")));

            if (elementsInput.Count > 0)
            {
                IWebElement firstInput = elementsInput[0];
                Thread.Sleep(1000);

                firstInput.Clear();
                firstInput.SendKeys(minPrice.ToString());
                Thread.Sleep(1000);

                IWebElement secondInput = elementsInput[1];

                secondInput.Clear();
                secondInput.SendKeys(maxPrice.ToString());
                Thread.Sleep(3000);

                var button = wait.Until(driver => driver.FindElement(By.XPath("//button[text()='Готово']")));
                button.Click();
            }
            else
            {
                InsertTextToWord("Тест провален: Элементы ввода не найдены. " + DateTime.Now.ToString());
                Assert.Fail("Элементы ввода не найдены.");
            }*/

            IWebElement body = _driver.FindElement(By.TagName("body"));
            actions.MoveToElement(body).Perform();
            Thread.Sleep(3000);

            var prices = productPage.GetPrices();

            ValidatePrices(prices, minPrice, maxPrice);
            /*var allSpan = wait.Until(driver => driver.FindElements(By.CssSelector(".price__lower-price")));*/

        }
        catch (WebDriverTimeoutException)
        {
            InsertTextToWord("Тест провален: Элемент не был найден. " + DateTime.Now.ToString());
            Assert.Fail("Превышено время ожидания. Элемент не был найден.");
        }
       /* catch (Exception ex)
        {
            InsertTextToWord($"Тест провален: {ex.Message} " + DateTime.Now.ToString());
            Assert.Fail(ex.Message);
        }*/
    }

    private void ValidatePrices(List<IWebElement> prices, double minPrice, double maxPrice)
    {
        if (prices.Count == 0)
        {
            InsertTextToWord("Тест провален: нет доступных карточек для проверки. " + DateTime.Now.ToString());
            Assert.Fail("Ошибка: нет доступных карточек для проверки.");
        }

        bool isTestPassed = true;

        foreach (var price in prices.Take(5))
        {
            string priceCount = price.Text;
            string newPrice = System.Text.RegularExpressions.Regex.Replace(priceCount, @"[^\d.,]", "").Replace(",", ".");
            newPrice = newPrice.Substring(0, newPrice.Length - 1);

            if (double.TryParse(newPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedPrice))
            {
                if (parsedPrice > maxPrice || parsedPrice < minPrice)
                {
                    InsertTextToWord($"Тест провален: цена {parsedPrice} не соответствует диапазону " + DateTime.Now.ToString());
                    Assert.Fail($"Тест не пройден: цена {parsedPrice} не соответствует диапазону");
                    isTestPassed = false;
                    break;
                }
            }
            else
            {
                InsertTextToWord("Тест провален: не удалось распарсить цену. " + DateTime.Now.ToString());
                Assert.Fail("Ошибка: не удалось распарсить цену.");
                isTestPassed = false;
                break;
            }
        }

        if (isTestPassed)
        {
            InsertTextToWord("Тест пройден -- " + DateTime.Now.ToString());
            Assert.Pass("Тест успешно пройден");
        }
    }

    [Test, Category("TestCategory")]
    public void ProductInCartTest()
    {
        string searchItem = "Карандаш";
        try
        {
            homePage.NavigateTo("https://www.wildberries.by/");
            Thread.Sleep(3000);

            /*IWebElement element = driver.FindElement(By.Id("searchInput"));
            Thread.Sleep(3000);
            element.SendKeys("Карандаш");
            element.SendKeys(OpenQA.Selenium.Keys.Enter);*/
            homePage.SearchForItem(searchItem);
            Thread.Sleep(3000);
            homePage.AcceptCookies();
            Thread.Sleep(3000);

            productPage.GetProduct();
            /*element = wait.Until(driver => driver.FindElement(By.CssSelector(".product-card__wrapper")));
            element.Click();*/
            Thread.Sleep(3000);

            /*   element = wait.Until(driver => driver.FindElement(By.CssSelector(".product-page__order-buttons > div > div > div > button")));
               element.Click();*/

            oneProductPage.AddToCart();
            Thread.Sleep(5000);
            SaveScreenshot(_driver, "ClickedCardWrapper.png");

            /*  element = wait.Until(driver => driver.FindElement(By.CssSelector(".j-item-basket")));
              element.Click();*/
            cartPage.OpenCart();
            Thread.Sleep(5000);
            SaveScreenshot(_driver, "CheckingCard.png");

            /* var elements = wait.Until(driver => driver.FindElements(By.ClassName("list-item__wrap")));*/

            int itemCount = cartPage.GetItemCount();
            if (itemCount == 1)
            {
                InsertTextToWord($"Тест пройден: Элементов в корзине {itemCount} " + DateTime.Now.ToString());
                Assert.Pass($"Тест пройден: Элементов в корзине {itemCount}");
               
            }
            else
            {
                InsertTextToWord("Тест провален: " + DateTime.Now.ToString());
                Assert.Fail("Тест не пройден");
            }

        }
        catch (WebDriverTimeoutException)
        {
            InsertTextToWord("Тест провален: Элемент не был найден. " + DateTime.Now.ToString());
            Assert.Fail("Превышено время ожидания. Элемент не был найден.");
        }
       /* catch (Exception ex)
        {
            InsertTextToWord($"Тест провален: {ex.Message} " + DateTime.Now.ToString());
            Assert.Fail(ex.Message);
        }*/
    }

    
    [Test, Category("TestCategory")]
    //[Ignore("Этот тест временно пропущен.")]
    public void CookieTests()
    {
        try
        {
            cookieHomePage.NavigateTo("https://vk.com/");

            var cookies = cookieHomePage.GetCookies();

            Console.WriteLine("----------Cookies---------");

            if (cookies.Count == 0)
            {
                InsertTextToWord("Тест провален: Куки не найдены на странице. " + DateTime.Now.ToString());
                Assert.Fail("Куки не найдены на странице.");
            }

            foreach (var cookie in cookies)
            {
                InsertTextToWord($"Name: {cookie.Name}, Value: {cookie.Value}, Domain: {cookie.Domain}, Path: {cookie.Path}, Expiry: {cookie.Expiry}");
                Console.WriteLine($"Name: {cookie.Name}, Value: {cookie.Value}, Domain: {cookie.Domain}, Path: {cookie.Path}, Expiry: {cookie.Expiry}");
            }

        }
        catch (WebDriverTimeoutException)
        {
            InsertTextToWord("Тест провален: Элемент не был найден. " + DateTime.Now.ToString());
            Assert.Fail("Превышено время ожидания. Элемент не был найден.");
        }
        /* catch (Exception ex)
         {
             Assert.Fail($"Произошла ошибка: {ex.Message}");
         }*/
    }

    public void Dispose()
    {
        _driver?.Quit();
    }

    [TearDown]
    public void Cleanup()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}