using PipeliningLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ProjectTemplate.Pipes.Sydle
{
    internal class IniciarNavegador : IPipe
    {
        public static IWebDriver Driver { get; set; }
        public object Run(dynamic input) 
        {
            string pathDownload = $@"C:\RPA\Finanças";

            if (!Directory.Exists(@"C:\ScopeDir\ScopeDir"))
                Directory.CreateDirectory(@"C:\Selenium\Scope");

            ChromeOptions opt = new ChromeOptions();
            opt.AcceptInsecureCertificates = true;
            string scopeDirPath = @"C:\Selenium\Scope\ScopeDir";
            opt.AddArgument($"--user-data-dir={scopeDirPath}");
            opt.AddArgument("--allow-running-insecure-content");
            opt.AddArgument("--start-maximized");
            opt.AddArgument("--aways-authorize-plugins");
            opt.AddArgument("--disable-notifications");
            opt.AddArgument("--no-sandbox");
            opt.AddArgument("--disable-dev-shm-usage");
            opt.AddArgument("--ignore-certificate-errors");
            opt.AddArgument("--ignore-ssl-errors");
            opt.AddUserProfilePreference("download.default_directory", pathDownload);

            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService("");
            chromeDriverService.HideCommandPromptWindow = true;
            Driver = new ChromeDriver(chromeDriverService, opt, TimeSpan.FromSeconds(500));


            input.driver = Driver;
            input.pathDownload = pathDownload;

            return input;
        }
    }
}
