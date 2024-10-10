using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PipeliningLibrary;

namespace ProjectTemplate.Pipes.Navegador
{
    public class NavegarKabum : IPipe
    {
        public object Run(dynamic input) 
        {
            ChromeDriver driver = input.driver;
            driver.Navigate().GoToUrl("https://www.kabum.com.br/");

            IWebElement campoBusca = driver.FindElement(By.XPath(".//input[@id='input-busca']"));
            campoBusca.Click();
            campoBusca.SendKeys("PlayStation®5 Slim");
            campoBusca.SendKeys(Keys.Enter);

            string precoKabum = driver.FindElement(By.XPath(".//span[contains(@class, 'priceCard')]")).Text;
            input.precoKabum = precoKabum;
            Console.WriteLine("preco Kabum: " +  precoKabum);
            Thread.Sleep(1500);

            return input;
        }
    }
}