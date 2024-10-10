using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using PipeliningLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipes.Navegador
{
    public class NavegarMercadoLivre : IPipe
    {
        public object Run(dynamic input)
        {
            ChromeDriver driver = input.driver;
            driver.Navigate().GoToUrl("https://www.mercadolivre.com.br/");

            IWebElement campoBusca = driver.FindElement(By.XPath(".//input[@id='cb1-edit']"));
            campoBusca.Click();
            campoBusca.SendKeys("PlayStation®5 Slim");
            campoBusca.SendKeys(Keys.Enter);

            string precoMercadoLivre = driver.FindElement(By.XPath(".//span[@class='andes-money-amount__fraction']")).Text;
            input.precoMercadoLivre = precoMercadoLivre;
            Console.WriteLine("preco Mercado Livre: " + precoMercadoLivre);
            Thread.Sleep(1500);

            return input;
        }
    }
}
