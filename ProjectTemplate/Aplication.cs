using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    internal class Aplication
    {
        public static int Contador { get; set; }
        //contador limite de tempo 
        public static async Task ContadorLimiteTempo()
        {
            Task cont = Task.Run(async () =>
            {
                while (Contador < 900)
                {
                    Contador++;
                    await Task.Delay(1000);
                }
            });

            await cont;

            Environment.Exit(0);
        }
        public static void OnApplicationExit(object sender, EventArgs e)
        {

            KillChromeDriver();

        }

        public static int KillChromeDriver()
        {
            var i = Process.GetProcessesByName("ChromeDriver");
            int exitCode = 400;
            if (i.Length > 0)
            {
                string command = "taskkill /F /IM chromedriver.exe";
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + command;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                exitCode = process.ExitCode;
                process.Close();
            }


            var p = Process.GetProcesses();
            foreach (Process item in p)
            {
                if (item.ProcessName.ToUpper().Contains("CHROME") || item.ProcessName.Contains("MSEDGE") || item.ProcessName.Contains("IEXPLORE"))
                {
                    item.Kill();
                }
            }


            string tempfolder = @"C:\Selenium\Scope";
            if (!Directory.Exists(tempfolder))
                Directory.CreateDirectory(tempfolder);

            try
            {
                string[] tempfiles = Directory.GetDirectories(tempfolder, "scoped *", SearchOption.AllDirectories);
                foreach (var item in tempfiles)
                {
                    if (Directory.Exists(item))
                        Directory.Delete(item, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return exitCode;
        }

        public static void FinalizarProcesso()
        {
            KillChromeDriver();

            Environment.Exit(0);
        }


        public static void WaitForTitle(ChromeDriver driver)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(".//span[@data-testid='main-title']")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Title not found");
            }
        }
    }
}
