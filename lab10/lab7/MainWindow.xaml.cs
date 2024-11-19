﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using System.Drawing.Imaging;



namespace lab7
{
    public partial class MainWindow : Window, IDisposable
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWebDriver();
        }

        private void InitializeWebDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-gpu");
            options.AddArgument("window-size=1920,1080");
            

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
                IWebElement element = driver.FindElement(By.Id("searchInput"));
                Thread.Sleep(3000);
                element.SendKeys("Джинсы");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(3000);

                element = wait.Until(driver => driver.FindElement(By.CssSelector("button.cookies__btn-link")));
                element.Click();

                var elements = wait.Until(driver => driver.FindElements(By.ClassName("dropdown-filter__btn-name")));

                Actions actions = new Actions(driver);

                if (elements.Count > 0)
                {
                    IWebElement firstElement = elements[2];
                    Thread.Sleep(1000);
                    actions.MoveToElement(firstElement).Perform();
                }
                else
                {
                    Console.WriteLine("Элементы не найдены.");
                }

                var elementsInput = wait.Until(driver => driver.FindElements(By.ClassName("j-price")));

                if (elementsInput.Count > 0)
                {
                    IWebElement firstInput = elementsInput[0];
                    Thread.Sleep(1000);

                    firstInput.Clear();
                    firstInput.SendKeys("30");
                    Thread.Sleep(1000);

                    IWebElement secondInput = elementsInput[1];

                    secondInput.Clear();
                    secondInput.SendKeys("70");
                    Thread.Sleep(3000);

                    var button = wait.Until(driver => driver.FindElement(By.XPath("//button[text()='Готово']")));
                    button.Click();
                }
                else
                {
                    Console.WriteLine("Элементы ввода не найдены.");
                }

                IWebElement body = driver.FindElement(By.TagName("body"));
                actions.MoveToElement(body).Perform();
                Thread.Sleep(3000);

                var allSpan = wait.Until(driver => driver.FindElements(By.CssSelector(".price__lower-price")));

                bool isTestPassed = true;

                if (allSpan.Count > 0)
                {
                    var firstFive = allSpan.Take(5).ToList();

                    foreach (var price in firstFive)
                    {
                        string priceCount = price.Text;
                        string newPrice = System.Text.RegularExpressions.Regex.Replace(priceCount, @"[^\d.,]", "").Replace(",", ".");
                        newPrice = newPrice.Substring(0, newPrice.Length - 1);

                        if (double.TryParse(newPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedPrice))
                        {
                            if (parsedPrice > 70 || parsedPrice < 30)
                            {
                                MessageBox.Show($"Тест не пройден: цена не соответствует диапазону 30-70 р. Значение: {parsedPrice}");
                                isTestPassed = false;
                                ResultTest1.Content = "Не пройден";
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка: не удалось распарсить цену.");
                            isTestPassed = false;
                            break;
                        }
                    }

                    if (isTestPassed)
                    {
                        MessageBox.Show("Тест успешно пройден");
                        ResultTest1.Content = "Пройден";
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка: нет доступных карточек для проверки.");
                }

            }
            catch (WebDriverTimeoutException)
            {
                MessageBox.Show("Превышено время ожидания. Элемент не был найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           /* finally
            {
                driver.Quit();
            }*/
        }

        private void SaveScreenshot(IWebDriver driver, string fileName)
        {
            // Получаем скриншот
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

            // Определяем путь для сохранения скриншота
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

            // Сохраняем скриншот в выбранном формате
            screenshot.SaveAsFile(filePath); // Используем ScreenshotImageFormat
            Console.WriteLine($"Скриншот сохранен: {filePath}");
        }


        private void Button2_Click(object sender, RoutedEventArgs e)
        {
           

            try
            {
                driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
                IWebElement element = driver.FindElement(By.Id("searchInput"));
                Thread.Sleep(3000);
                element.SendKeys("Литература");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(3000);

               /* element = wait.Until(driver => driver.FindElement(By.CssSelector("button.cookies__btn-link")));
                element.Click();*/
                element = wait.Until(driver => driver.FindElement(By.CssSelector(".product-card__wrapper")));
                element.Click();
               
                Thread.Sleep(3000);
                
                element = wait.Until(driver => driver.FindElement(By.CssSelector(".product-page__order-buttons > div > div > div > button")));
                element.Click();
                Thread.Sleep(5000);
                SaveScreenshot(driver, "ClickedCardWrapper.png");

                element = wait.Until(driver => driver.FindElement(By.CssSelector(".j-item-basket")));
                element.Click();
               
                Thread.Sleep(5000);
                SaveScreenshot(driver, "CheckingCard.png");
                var elements = wait.Until(driver => driver.FindElements(By.ClassName("list-item__wrap")));
                if (elements.Count == 1)
                {
                    MessageBox.Show($"Тест пройден: Элементов в корзине {elements.Count}");
                    ResultTest2.Content = "Пройден";
                }
                else
                {
                    ResultTest2.Content = "Не пройден";
                }
              
            }
            catch (WebDriverTimeoutException)
            {
                MessageBox.Show("Превышено время ожидания. Элемент не был найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                /* if (driver != null)
                 {
                     driver.Quit();
                 }*/
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
                IWebElement element = driver.FindElement(By.Id("searchInput"));
                Thread.Sleep(3000);
                element.SendKeys("Лампа");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(5000);

                element = wait.Until(driver => driver.FindElement(By.CssSelector("button.cookies__btn-link")));
                element.Click();
                Thread.Sleep(3000);
                var elements = wait.Until(driver => driver.FindElements(By.ClassName("dropdown-filter__btn-name")));

                Actions actions = new Actions(driver);
                if (elements.Count > 0)
                {
                    IWebElement firstElement = elements[1];
                    Thread.Sleep(1000);

                    actions.MoveToElement(firstElement).Perform();

                    var elementsInput = wait.Until(driver => driver.FindElement(By.CssSelector("div.filter > ul > li:nth-child(1) > div")));
                    elementsInput.Click();
                }
                else
                {
                    Console.WriteLine("Элементы не найдены.");
                }

                IWebElement body = driver.FindElement(By.TagName("body"));
                actions.MoveToElement(body).Perform();
                Thread.Sleep(3000);

                var allSpan = wait.Until(driver => driver.FindElement(By.CssSelector(".product-card__name")));
                string text = allSpan.Text;
                if (text.Contains("лампочки"))
                {
                    MessageBox.Show("Тест успешно пройден: подстрока \"лампочки\" найдена в карточке товара");
                    ResultTest3.Content = "Пройден";
                }
                else
                {
                    MessageBox.Show("Тест не пройден");
                    ResultTest3.Content = "Не пройден";
                }
            }
            catch (WebDriverTimeoutException)
            {
                MessageBox.Show("Превышено время ожидания. Элемент не был найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                /* if (driver != null)
                 {
                     driver.Quit();
                 }*/
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                // Переход на сайт Pinterest
                driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
                Thread.Sleep(1000);

                 // Получение всех куки
                 var cookies = driver.Manage().Cookies.AllCookies;

                Console.Clear();
                Console.WriteLine("----------Cookies---------");

                foreach (var cookie in cookies)
                {
                    Console.WriteLine($"Name: {cookie.Name}, Value: {cookie.Value}, Domain: {cookie.Domain}, Path: {cookie.Path}, Expiry: {cookie.Expiry}");
                }
            }
            catch (WebDriverTimeoutException)
            {
                MessageBox.Show("Превышено время ожидания. Элемент не был найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public void Dispose()
        {
            driver?.Quit();
        }
    }
}