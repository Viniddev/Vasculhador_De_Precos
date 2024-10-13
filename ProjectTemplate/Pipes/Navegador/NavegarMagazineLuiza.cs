using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using PipeliningLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTemplate.Models;

namespace ProjectTemplate.Pipes.Navegador
{
    public class NavegarMagazineLuiza : IPipe
    {
        public object Run(dynamic input)
        {
            ChromeDriver driver = input.driver;
            string product = input.product;

            driver.Navigate().GoToUrl("https://www.magazineluiza.com.br/");

            IWebElement campoBusca = driver.FindElement(By.XPath(".//input[@id='input-search']"));
            campoBusca.Click();
            campoBusca.SendKeys(product);
            campoBusca.SendKeys(Keys.Enter);

            Thread.Sleep(1000);
            List<IWebElement> listaDivPrecos = driver.FindElements(By.XPath(".//div[@data-testid='product-card-content']")).ToList();

            PriceIndicator menorPreco = MontarObjeto(listaDivPrecos.ElementAt(4));
            foreach (IWebElement element in listaDivPrecos)
            {
                PriceIndicator indicator = MontarObjeto(element);

                if (indicator.Price < 1000 || !indicator.Title.Contains(product)) 
                {
                    continue;
                }
                else
                {
                    if (indicator.Price < menorPreco.Price)
                        menorPreco = indicator;
                }
            }

            menorPreco.ToString();
            input.menorPrecoMagazine = menorPreco;
            Thread.Sleep(1500);

            return input;
        }

        private PriceIndicator MontarObjeto(IWebElement indicadorDePreco)
        {
            string text = string.Empty;
            try
            {
                text = indicadorDePreco.FindElement(By.XPath(".//h2[@data-testid='product-title']")).Text;
            }
            catch (Exception ex) 
            {
                text = "n/a";
            }

            decimal preco = 0;
            try
            { 
                preco = Decimal.Parse(indicadorDePreco.FindElement(By.XPath(".//p[@data-testid='price-value']")).Text.ToString().Split(" ")[2]);
            }
            catch (Exception ex)
            {
                preco = 0;
            }

            string avaliacaoProduto = "";
            try
            {
                avaliacaoProduto = indicadorDePreco.FindElement(By.XPath(".//span[@format='score-count']")).Text.ToString().Split(" ")[0];
                if (avaliacaoProduto == string.Empty)
                    avaliacaoProduto = "n/a";
            }
            catch (Exception ex)
            {
                avaliacaoProduto = "n/a";
            }

            return new PriceIndicator()
            {
                Title = text,
                Price = preco,
                Avaliation = avaliacaoProduto,
            };
        }
    }
}
