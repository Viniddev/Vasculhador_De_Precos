using DocumentFormat.OpenXml.Office.CustomUI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PipeliningLibrary;
using ProjectTemplate.Models;
using ProjectTemplate.Pipes.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipes.Sydle
{
    internal class NavegadorMercadoLivre : IPipe
    {
        private ExcelGenerator excelGenerator = new ExcelGenerator();

        public object Run(dynamic input) 
        {
            ChromeDriver driver = input.driver;         
            driver.Navigate().GoToUrl("https://www.mercadolivre.com.br/");

            foreach (var product in input.Products)
            {
                IWebElement campoBusca = driver.FindElement(By.XPath("//input[@class='nav-search-input']"));
                campoBusca.Click();
                campoBusca.Clear();
                campoBusca.SendKeys(product);
                campoBusca.SendKeys(Keys.Enter);

                Thread.Sleep(1500);
                List<IWebElement> listaDivPrecos = driver.FindElements(By.XPath("//div[@class='ui-search-result__content']")).ToList();


                PriceIndicator menorPreco = MontarObjeto(listaDivPrecos.FirstOrDefault(), product.ToString());
                foreach (IWebElement element in listaDivPrecos)
                {
                    PriceIndicator indicator = MontarObjeto(element, product.ToString());
                    excelGenerator.EditXlsx(indicator, "Vasculhador_de_Precos.xlsx");

                    if (indicator.Price < 1000)
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
                Thread.Sleep(1500);
                input.menorPrecoAmazon = menorPreco;
                excelGenerator.EditXlsx(menorPreco, "Telegram_Report.xlsx");
            }
            return input;
        }

        private PriceIndicator MontarObjeto(IWebElement indicadorDePreco, string produto) 
        {
            string text = indicadorDePreco.FindElement(By.XPath(".//*[contains(@class, 'ui-search-link__title-card ui-search-link')]")).Text;
            decimal preco = 0;
            try 
            {
                preco = Decimal.Parse(indicadorDePreco.FindElement(By.XPath(".//*[contains(@class,'andes-money-amount__fraction')]")).Text);
            } catch(Exception ex) 
            {
                preco = 0;
            }

            string avaliacaoProduto = "";
            try 
            {
                avaliacaoProduto = indicadorDePreco.FindElement(By.XPath("//span[contains(@class, 'ui-search-reviews__rating-number')]")).Text;
                if (avaliacaoProduto == string.Empty)
                {
                    avaliacaoProduto = "n/a";
                }                   
                else
                {
                    string padrao = @"\d+([.,]\d+)?";
                    var avaliacao = Regex.Match(avaliacaoProduto, padrao);
                    avaliacaoProduto = avaliacao.ToString();
                }
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
                Store = "Mercado Livre",
                Date = DateTime.Today.ToString("dd/MM/yyyy"),
                SearchText = produto,
            };
        }
    }
}
