using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PipeliningLibrary;
using ProjectTemplate.Models;
using ProjectTemplate.Pipes.Excel;
using System.Text.RegularExpressions;

namespace ProjectTemplate.Pipes.Navegador
{
    public class NavegarKabum : IPipe
    {
        private ExcelGenerator excelGenerator = new ExcelGenerator();

        public object Run(dynamic input) 
        {
            ChromeDriver driver = input.driver;
            driver.Navigate().GoToUrl("https://www.kabum.com.br/");

            foreach(var product in input.Products)
            {
                IWebElement campoBusca = driver.FindElement(By.XPath(".//input[@id='input-busca']"));
                campoBusca.Click();
                campoBusca.Clear();
                campoBusca.SendKeys(product);
                campoBusca.SendKeys(Keys.Enter);

                List<IWebElement> cardsProdutos = driver.FindElements(By.XPath(".//article[contains(@class, 'productCard')]")).ToList();

                Thread.Sleep(1500);
                PriceIndicator menorPreco = MontarObjeto(cardsProdutos.ElementAt(5));
                foreach (IWebElement element in cardsProdutos)
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
                Thread.Sleep(1500);
                input.menorPrecoKabum = menorPreco;

                excelGenerator.EditXlsx(menorPreco);
            }
        
            return input;
        }


        private PriceIndicator MontarObjeto(IWebElement indicadorDePreco)
        {
            string text = string.Empty;
            try
            {
                text = indicadorDePreco.FindElement(By.XPath(".//span[contains(@class, 'nameCard')]")).Text;
            }
            catch (Exception ex) 
            {
                text = "n/a";
            }
            decimal preco = 0;

            try
            {
                preco = Decimal.Parse(indicadorDePreco.FindElement(By.XPath(".//span[contains(@class,'priceCard')]")).Text.ToString().Split(" ")[1]);
            }
            catch (Exception ex)
            {
                preco = 0;
            }

            string avaliacaoProduto = "";
            try
            {
                avaliacaoProduto = indicadorDePreco.FindElement(By.XPath("//div[contains(@aria-label, 'Classificação')]")).GetAttribute("aria-label");
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
                Store = "Kabum",
                Date = DateTime.Today.ToString("dd/MM/yyyy"),
            };
        }
    }
}