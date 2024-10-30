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
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipes.Sydle
{
    internal class NavegadorAutomatico : IPipe
    {
        private ExcelGenerator excelGenerator = new ExcelGenerator();

        public object Run(dynamic input) 
        {
            ChromeDriver driver = input.driver;
            string filePath = Path.Combine(AppContext.BaseDirectory, "configuracoes.json");
            string jsonContent = File.ReadAllText(filePath);
            List<Loja> lojas = JsonSerializer.Deserialize<List<Loja>>(jsonContent);


            foreach (Loja loja in lojas) 
            {
                driver.Navigate().GoToUrl(loja.Site);

                foreach (var product in input.Products)
                {
                    IWebElement campoBusca = driver.FindElement(By.XPath(loja.Busca));
                    campoBusca.Click();
                    campoBusca.Clear();
                    campoBusca.SendKeys(product);
                    campoBusca.SendKeys(Keys.Enter);

                    Thread.Sleep(1500);
                    List<IWebElement> listaDivPrecos = driver.FindElements(By.XPath(loja.ListProdutos)).ToList();
                    Thread.Sleep(1500);

                    PriceIndicator menorPreco = MontarObjeto(listaDivPrecos.FirstOrDefault(), product.ToString(), loja);

                    foreach (IWebElement element in listaDivPrecos)
                    {
                        PriceIndicator indicator = MontarObjeto(element, product.ToString(), loja);
                        excelGenerator.EditXlsx(indicator, "Vasculhador_de_Precos.xlsx");

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
                    input.menorPrecoAmazon = menorPreco;
                    excelGenerator.EditXlsx(menorPreco, "Telegram_Report.xlsx");
                }
            }
            return input;
        }

        private PriceIndicator MontarObjeto(IWebElement indicadorDePreco, string produto, Loja loja) 
        {
            string text = string.Empty;
            try
            {
                text = indicadorDePreco.FindElement(By.XPath(loja.TituloProduto)).Text;
            }
            catch (Exception ex)
            {
                text = "n/a";
            }

            decimal preco = 0;
            try 
            {
                if (loja.Nome.Equals("Magazine Luiza"))
                {
                    preco = Decimal.Parse(indicadorDePreco.FindElement(By.XPath(loja.PrecoProduto)).Text.ToString().Split(" ")[2]);
                }
                else if (loja.Nome.Equals("Kabum"))
                {
                    preco = Decimal.Parse(indicadorDePreco.FindElement(By.XPath(loja.PrecoProduto)).Text.ToString().Split(" ")[1]);
                }
                else 
                {
                    preco = Decimal.Parse(indicadorDePreco.FindElement(By.XPath(loja.PrecoProduto)).Text);
                }


            } catch(Exception ex) 
            {
                preco = 0;
            }

            string avaliacaoProduto = "";
            try 
            {
                if (loja.Nome.Equals("Amazon") || loja.Nome.Equals("Kabum"))
                {
                    avaliacaoProduto = indicadorDePreco.FindElement(By.XPath(loja.AvaliacaoProduto)).GetAttribute("aria-label");
                } 
                else if (loja.Nome.Equals("Magazine Luiza"))
                {
                    avaliacaoProduto = indicadorDePreco.FindElement(By.XPath(loja.AvaliacaoProduto)).Text.ToString().Split(" ")[0];
                }
                else 
                {
                    avaliacaoProduto = indicadorDePreco.FindElement(By.XPath(loja.AvaliacaoProduto)).Text;
                }


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
                Store = loja.Nome,
                Date = DateTime.Today.ToString("dd/MM/yyyy"),
                SearchText = produto,
            };
        }
    }
}
