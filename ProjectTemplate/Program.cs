using ProjectTemplate;
using ProjectTemplate.Pipelines;
using System;

namespace program
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            StartPipelines startPipelines = new StartPipelines();


            Console.WriteLine("Finalizou");
            Console.ReadLine();
        }
    }
}