using PipeliningLibrary;
using ProjectTemplate.Models;
using ProjectTemplate.Pipes.Excel;
using ProjectTemplate.Pipes.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Pipes.Telegram
{
    public class BuildReport : IPipe
    {
        private ExcelGenerator excelGenerator = new ExcelGenerator();

        public object Run(dynamic input) 
        {

            PriceIndicator indicadorAmazon = input.menorPrecoAmazon;
            PriceIndicator indicadorMagazine = input.menorPrecoMagazine;
            PriceIndicator indicadorKabum = input.menorPrecoKabum;

            string report = "\U0001F6A9 Indicadores Favoráveis \U0001F6A9 \n\n";

            report = excelGenerator.ReadXlsx(report);

            //report += "\u2705 Melhor indicativo Amazon: \n" + indicadorAmazon.ToString() + "\n";
            //report += "\u2705 Melhor indicativo Kabum: \n" + indicadorKabum.ToString() + "\n";
            //report += "\u2705 Melhor indicativo Magazine Luiza: \n" + indicadorMagazine.ToString() + "\n";
            //report += "\n";

            TelegramApi.SendMessageAsync(report).Wait();

            return input;
        }
    }
}
