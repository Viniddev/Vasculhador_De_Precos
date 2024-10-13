using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PipeliningLibrary;
using ProjectTemplate.Models;

namespace ProjectTemplate.Pipes.Navegador
{
    public class NavegarKabum : IPipe
    {
        public object Run(dynamic input) 
        {
            string product = input.product;
            ChromeDriver driver = input.driver;
            driver.Navigate().GoToUrl("https://www.kabum.com.br/");

            IWebElement campoBusca = driver.FindElement(By.XPath(".//input[@id='input-busca']"));
            campoBusca.Click();
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
            input.menorPrecoKabum = menorPreco;
            Thread.Sleep(1500);
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