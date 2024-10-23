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
            generator.CreateXlsx();

            StartPipelines startPipelines = new StartPipelines();


            Console.WriteLine("Finalizou");
            Console.ReadLine();
        }
    }
}