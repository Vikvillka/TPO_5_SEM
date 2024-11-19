using System;
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
using NUnit.Framework;

namespace lab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
/*    [TestFixture]
    public class MathTests
    {
        // Метод, который будет тестировать сложение
        [Test]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            int a = 5;
            int b = 7;
            int expectedSum = 12;

            // Act
            int actualSum = Add(a, b);

            // Assert
            Assert.AreEqual(expectedSum, actualSum, "Сумма двух чисел должна быть правильной.");
        }

        // Простой метод сложения
        private int Add(int x, int y)
        {
            return x + y;
        }
    }*/

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

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

                // Проверка наличия элементов ввода
                if (elementsInput.Count > 0)
                {
                    IWebElement firstInput = elementsInput[0];
                    Thread.Sleep(1000); // Задержка в 1 секунду (необязательно)

                    // Клик по первому полю ввода и ввод значения
                    //firstInput.Click();
                    firstInput.Clear(); // Очистка поля ввода перед вводом
                    firstInput.SendKeys("30");
                    Thread.Sleep(1000);


                    IWebElement secondInput = elementsInput[1];
                   
                    secondInput.Clear(); // Очистка поля ввода перед вводом
                    secondInput.SendKeys("70");
                    Thread.Sleep(3000);

                    // Клик по первому полю ввода и ввод значения
                    //firstInput.Click();

                    //IWebElement button = wait.Until(driver => driver.FindElement(By.XPath("//button[text()='Готово']")));
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

                //< ins class="price__lower-price">119,96&nbsp;р.</ins>
                // Проверка наличия карточек
                bool isTestPassed = true;

                if (allSpan.Count > 0)
                {
                    // Берем первые 5 карточек
                    var firstFive = allSpan.Take(5).ToList();

                    // Проверяем цену каждой карточки
                    foreach (var price in firstFive)
                    {
                        string priceCount = price.Text;
                        // Убираем " р." и заменяем запятую на точку
                        string newPrice = System.Text.RegularExpressions.Regex.Replace(priceCount, @"[^\d.,]", "").Replace(",", ".");

                        newPrice = newPrice.Substring(0, newPrice.Length - 1);

                        // Парсим значение с использованием InvariantCulture для обработки десятичной точки
                        if (double.TryParse(newPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedPrice))
                        {
                            if (parsedPrice > 70 || parsedPrice < 30)
                            {
                                MessageBox.Show($"Тест не пройден: цена не соответствует диапазону 30-70 р. Значение: {parsedPrice}");
                                isTestPassed = false;
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
            finally
            {
               /* if (driver != null)
                {
                    driver.Quit();
                }*/
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new ChromeDriver();
            
            try
            {
                driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
                IWebElement element = driver.FindElement(By.Id("searchInput"));
                Thread.Sleep(3000);
                element.SendKeys("Книга");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(3000);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                element = wait.Until(driver => driver.FindElement(By.CssSelector("button.cookies__btn-link")));
                //Thread.Sleep(3000);
                element.Click();
                element = wait.Until(driver => driver.FindElement(By.CssSelector(".product-card__wrapper")));
                element.Click();
                Thread.Sleep(3000);

                element = wait.Until(driver => driver.FindElement(By.CssSelector("div.product-page__order-container.j-order-container > div > div > div > button")));
                element.Click();
                Thread.Sleep(3000);

                element = wait.Until(driver => driver.FindElement(By.CssSelector(".j-item-basket")));
                element.Click();
                Thread.Sleep(3000);

                var elements = wait.Until(driver => driver.FindElements(By.ClassName("list-item__wrap")));
                MessageBox.Show($"Элементов в корзине {elements.Count}");
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
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            try
            {
                //Console.WriteLine("привет");
                driver.Navigate().GoToUrl(@"https://www.wildberries.by/");
                IWebElement element = driver.FindElement(By.Id("searchInput"));
                Thread.Sleep(3000);
                element.SendKeys("Лампа");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(3000);

                element = wait.Until(driver => driver.FindElement(By.CssSelector("button.cookies__btn-link")));
                //Thread.Sleep(3000);
                element.Click();
                var elements = wait.Until(driver => driver.FindElements(By.ClassName("dropdown-filter__btn-name")));

                Actions actions = new Actions(driver);
                // Проверка, есть ли элементы
                if (elements.Count > 0)
                {
                    // Получение первого элемента
                    IWebElement firstElement = elements[1];

                    // Задержка в 1 секунду (необязательно)
                    Thread.Sleep(1000);

                   

                    actions.MoveToElement(firstElement).Perform();

                    //< div class="checkbox-with-text j-list-item selected" data-link="class{merge: isSelected toggle='selected'}">                <span class="checkbox-with-text__decor"></span>                <span class="checkbox-with-text__text">Лампочка</span>            </div>
                    var elementsInput = wait.Until(driver => driver.FindElement(By.CssSelector("div.filter > ul > li:nth-child(1) > div")));
                    elementsInput.Click();

                }
                else
                {
                    Console.WriteLine("Элементы не найдены.");
                }
                //<span class="product-card__name">                <span class=""> / </span>Светодиодные лампочки Е14 7.5 ВТ теплый свет 3 шт            </span>
                IWebElement body = driver.FindElement(By.TagName("body"));
                actions.MoveToElement(body).Perform();
                Thread.Sleep(3000);


                var allSpan = wait.Until(driver => driver.FindElement(By.CssSelector(".product-card__name")));
                string text = allSpan.Text;
                if (text.Contains("лампочки"))
                {
                    MessageBox.Show("Тест успешно пройден");
                }
                else
                {
                    MessageBox.Show("Тест успешно не пройден");
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
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            try
            {
                driver.Navigate().GoToUrl(@"https://www.pinterest.com/");
                //<div class="RedHeaderProfileItem_RedHeaderProfileItem__root__1ruq2" ae_button_type="click" ae_object_type="ui" ae_page_area="profil" ae_page_type="home" data-exp-s="" data-clk="" data-spm="profil" exp_page="home" exp_page_area="profil" exp_type="ui" data-aplus-ae="x17_383daf0d" data-spm-anchor-id="a2g2w.home.header.profil.5a50274c6iMHxG" data-aplus-clk="x13_6e651520"><a class="RedHeaderProfileItem_RedHeaderProfileItem__link__1ruq2" href="https://aliexpress.ru/account"><svg fill="none" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" class="RedHeaderProfileItem_RedHeaderProfileItem__icon__1ruq2"><path d="M4.74 7.232c-2.287 3.991-4.426 8.702-.777 12.67 1.639 1.782 4.287 2.969 7.029 3.071 6.843.256 11.434-4.79 11.89-9.985.221-2.528.224-5.286-1.555-7.477-1.248-1.537-3.137-2.074-5.059-2.819-2.345-.91-8.14-1.195-9.902.86-1.648 1.924-4.832 4.844-3.577 7.419m5.68-2.754.14.273m0 0c.11.217.216.415.317.59m-.318-.59c.54-1.241 1.892-2.583 2.213-1.575.056.176.088.387.098.61m-2.31.965c-.275.631-.338 1.237.053 1.493m.264-.904c.58 1.001 1.032 1.24 1.744-.152.184-.36.27-.91.249-1.401M8.926 9.079c.26-.762.723-1.621 1.913-1.572a.208.208 0 0 1 .08.019M8.926 9.079c-.078.23-.138.452-.193.636a5.118 5.118 0 0 0-.07.268m2.256-2.457c.541.25.04 2.879-1.941 2.57a.789.789 0 0 1-.316-.113m0 0c-.138.583-.31 1.67.485.837m6.835-4.251-.03.14m0 0c-.074.356-.129.626-.168.825m.168-.826c.031-.028.064-.055.097-.08.768-.594.518 2.989.472 3.163-.41 1.532-2.096 1.992-1.685.03.182-.872.183-1.772.674-2.564.05-.08.092-.138.127-.176m.315-.373c-.12.11-.225.235-.315.373m.147.453c-.163.837-.05.425 0 0Zm0 0c.038-.322.038-.653-.147-.453m0 0c-.542.833-.565 2.116-.565 2.915m-9.423 2.951c.774 2.516 3.792 7.465 7.308 6.175 1.002-.367 5.542-5.249 4.355-5.848" stroke="#FFB800" stroke-linecap="round" stroke-width="1.825"></path></svg><span class="RedHeaderProfileItem_RedHeaderProfileItem__title__1ruq2">Войти</span></a></div>
                IWebElement element = driver.FindElement(By.CssSelector("div.wc1.zI7.iyn.Hsu > button > div"));
                //#__aer_root__ > div > div:nth-child(2) > div > header > div.RedHeader_RedHeader__rowAligner__bos8u.RedHeader_RedHeader__searchRowAligner__bos8u > nav:nth-child(3) > ul > li:nth-child(3) > button > div
                element.Click();
                element = wait.Until(driver => driver.FindElement(By.CssSelector("#email")));
                //<input class="snow-ali-kit_Input__inputField__1aiyxh snow-ali-kit_Input-L__upPlaceholder__1hqh9w" name="login" id="login" type="text" inputmode="email" autocomplete="on" value="" data-spm-anchor-id="a2g2w.home.0.i1.41f3274cBpqIB6">
                element.SendKeys("vikviknim@gmail.com");
                element = wait.Until(driver => driver.FindElement(By.CssSelector("#password")));
                //<input class="snow-ali-kit_Input__inputField__1aiyxh snow-ali-kit_Input-L__upPlaceholder__1hqh9w" name="login" id="login" type="text" inputmode="email" autocomplete="on" value="" data-spm-anchor-id="a2g2w.home.0.i1.41f3274cBpqIB6">
                element.SendKeys("55188296164039");
               /* Thread.Sleep(1000);
                element.SendKeys(OpenQA.Selenium.Keys.Enter);*/


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

    }
}