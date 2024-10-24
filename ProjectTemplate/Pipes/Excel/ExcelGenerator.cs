using AngleSharp.Html.Dom;
using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using PipeliningLibrary;
using ProjectTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectTemplate.Pipes.Excel
{
    public class ExcelGenerator
    {
        public void CreateXlsx(string arquivo)
        {
            bool arquivoExiste = File.Exists(arquivo);
            if(!arquivoExiste)
            {
                Console.WriteLine("Entrei no criador.");
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.AddWorksheet("Vasculhador_de_Precos");

                    worksheet.Cell(1, 1).Value = "Product";
                    worksheet.Cell(1, 2).Value = "Price";
                    worksheet.Cell(1, 3).Value = "Review";
                    worksheet.Cell(1, 4).Value = "Store";
                    worksheet.Cell(1, 5).Value = "Date";
                    worksheet.Cell(1, 6).Value = "SearchText";

                    workbook.SaveAs(arquivo);
                }

            }
        }

        public void EditXlsx(PriceIndicator priceIndicator, string arquivo)
        {
            using (var workbook = new XLWorkbook(arquivo))
            {
                var worksheet = workbook.Worksheet(1);

                int ultimaLinhaUsada = worksheet.LastRowUsed()?.RowNumber() ?? 0;
                int proximaLinha = ultimaLinhaUsada + 1;

                worksheet.Cell(proximaLinha, 1).Value = priceIndicator.Title;
                worksheet.Cell(proximaLinha, 2).Value = priceIndicator.Price;
                worksheet.Cell(proximaLinha, 3).Value = priceIndicator.Avaliation;
                worksheet.Cell(proximaLinha, 4).Value = priceIndicator.Store;
                worksheet.Cell(proximaLinha, 5).Value = priceIndicator.Date;
                worksheet.Cell(proximaLinha, 6).Value = priceIndicator.SearchText;

                workbook.SaveAs(arquivo);
            }        
        }

        public string ReadXlsx(string report, string arquivo)
        {
            using (var workbook = new XLWorkbook(arquivo))
            {
                var worksheet = workbook.Worksheet(1);

                foreach(var line in worksheet.RowsUsed().Skip(1))
                {
                    report += $"\u2705 Melhor indicativo {line.Cell(4).Value}: \n •Título: {line.Cell(1).Value} \n•Preço: R${line.Cell(2).Value} \n" +
                        $"•Avaliação: {line.Cell(3).Value} \n•Data: {line.Cell(5).Value} \n\n";
                }

                return report;
            }
        }
    }
}
