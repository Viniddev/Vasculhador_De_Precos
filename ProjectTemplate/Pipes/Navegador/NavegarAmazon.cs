using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PipeliningLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipes.Sydle
{
    internal class NavegarAmazon : IPipe
    {
        public object Run(dynamic input) 
        {
            ChromeDriver driver = input.driver;
            driver.Navigate().GoToUrl("https://www.amazon.com.br/");

            IWebElement campoBusca =  driver.FindElement(By.XPath(".//input[@id='twotabsearchtextbox']"));
            campoBusca.Click();
            campoBusca.SendKeys("PlayStation®5 Slim");
            campoBusca.SendKeys(Keys.Enter);

            string precoInteiro = driver.FindElement(By.XPath(".//span[@class='a-price-whole']")).Text;
            string precoDecimal = driver.FindElement(By.XPath(".//span[@class='a-price-fraction']")).Text;

            string precoFinal = precoInteiro + "," + precoDecimal;
            input.amazon = precoFinal;
            Console.WriteLine("preco Amazon  : " + precoFinal);
            Thread.Sleep(1500);

            return input;
        }
    }
}
