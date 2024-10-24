using ProjectTemplate;
using ProjectTemplate.Pipelines;
using ProjectTemplate.Pipes.Excel;
using System;

namespace program
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ExcelGenerator generator = new ExcelGenerator();
            generator.CreateXlsx("Vasculhador_de_Precos.xlsx");
            generator.CreateXlsx("Telegram_Report.xlsx");

            StartPipelines startPipelines = new StartPipelines();


            Console.WriteLine("Finalizou");
            Console.ReadLine();
        }
    }
}