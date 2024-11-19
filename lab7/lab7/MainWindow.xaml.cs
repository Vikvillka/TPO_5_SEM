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
using OpenQA.Selenium.Support.UI;

namespace lab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(@"https://google.com/");
            IWebElement element = driver.FindElement(By.Id("APjFqb"));
            try
            {
                element.SendKeys("БГТУ");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                element = wait.Until(driver => driver.FindElement(By.CssSelector("#rso > div.hlcw0c > div > div > div > div > div > div > div > div.yuRUbf > div > span > a > h3")));
                element.Click();

                element = wait.Until(driver => driver.FindElement(By.CssSelector("body > header > div.header-main-container.desktop-only > div:nth-child(1) > a")));
                element.Click();

                element = wait.Until(driver => driver.FindElement(By.CssSelector("#conte > table > tbody > tr:nth-child(13) > td > a")));
                string text = element.Text;
                myLabel.Content = text;
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
                if (driver != null)
                {
                    driver.Quit();
                }
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(@"https://open.spotify.com/");
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement element = wait.Until(driver => driver.FindElement(By.TagName("input")));
                element.SendKeys("Сплин");
                element.SendKeys(OpenQA.Selenium.Keys.Enter);

                element = wait.Until(driver => driver.FindElement(By.PartialLinkText("Сплин")));
                element.Click();

                element = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"main\"]/div/div[2]/div[4]/div[1]/div[2]/div[2]/div/main/section/div/div[2]/div[3]/div[1]/div/div/div/div/div[2]/div[1]/div/div[2]/div/a/div")));
                element.Click();

                element = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"main\"]/div/div[2]/div[4]/div[1]/div[2]/div[2]/div/main/section/div[1]/div[3]/div[3]/div/span[2]/a")));
                element.Click();

                element = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"main\"]/div/div[2]/div[4]/div[1]/div[2]/div[2]/div/main/section/div[1]/div[3]/div[2]/div/img")));

                string imageUrl = element.GetAttribute("src"); 
                
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(imageUrl); 
                    using (MemoryStream stream = new MemoryStream(imageBytes))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                        bitmap.EndInit();

                        myImageControl.Source = bitmap; 
                    }
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
                if (driver != null)
                {
                    driver.Quit();
                }
            }

        }
    }
}