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
            string report = "\U0001F6A9 Indicadores Favoráveis \U0001F6A9 \n\n";
            report = excelGenerator.ReadXlsx(report, "Telegram_Report.xlsx");

            TelegramApi.SendLogText(report, "Precos").Wait();
            TelegramApi.SendLogArchive("Telegram_Report.xlsx", "Telegram_Report.xlsx").Wait();

            return input;
        }
    }
}
