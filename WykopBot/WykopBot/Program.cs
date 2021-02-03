using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WykopBot
{
    class Program
    {
        static void DeletePosts(string login, string pass)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("-headless");

            var driverService = FirefoxDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
          
            IWebDriver web = new FirefoxDriver(driverService, options);
            int postsDeletedCoutner = 0; 

            web.Navigate().GoToUrl("https://wykop.pl");

            web.SwitchTo().Frame("cmp-iframe");
            WebDriverWait wait = new WebDriverWait(web, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("MuiButton-containedPrimary")));
            IWebElement element = web.FindElement(By.ClassName("MuiButton-containedPrimary"));
            Actions actions = new Actions(web);
            actions.MoveToElement(element).Click().Perform();

            web.Url = "https://www.wykop.pl/zaloguj/?fEr[0]=YmVmYmh0dHBzOi8vd3d3Lnd5a29wLnBsLw%3D%3D";

            var loginCredentials = web.FindElement(By.CssSelector("div.form-marked:nth-child(2) > form:nth-child(1) > fieldset:nth-child(2) > div:nth-child(1) > input:nth-child(1)"));
            loginCredentials.Click();
            loginCredentials.SendKeys($"{login}");

            var passwordCredentials = web.FindElement(By.CssSelector("div.form-marked:nth-child(2) > form:nth-child(1) > fieldset:nth-child(2) > div:nth-child(2) > input:nth-child(1)"));
            passwordCredentials.Click();
            passwordCredentials.SendKeys($"{pass}");

            var loginAccountButton = web.FindElement(By.CssSelector(".unmarked-button"));
            loginAccountButton.Click();

            while (true)
            {
                try
                {
                    web.Navigate().GoToUrl("https://www.wykop.pl/ludzie/wpisy/" + login);
                    var posts = web.FindElement(By.CssSelector("li.entry:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(3) > ul:nth-child(2) > li:nth-child(3) > a:nth-child(1)"));
                    /*
                    Actions actionsDelete = new Actions(web);
                    actionsDelete.MoveToElement(posts);
                    actionsDelete.Perform();

                    var erase = web.FindElement(By.XPath("//*[@id='itemsStream']/li[1]/div/div/div[3]/ul/li[3]/a"));
                    erase.Click();
                    */
                    IJavaScriptExecutor scriptExecutor = (IJavaScriptExecutor)web;
                    scriptExecutor.ExecuteScript("arguments[0].click();", posts);
                    web.SwitchTo().Alert().Accept();
                    postsDeletedCoutner += 1;
                }

                catch (Exception ex)
                {
                    break;
                }
                Console.WriteLine($"{postsDeletedCoutner} posts have been deleted!");
            }
        }

        static void Main(string[] args)
        {
            string login = "";
            string password = "";
           
            Console.Write("Simple post eraser script for");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" wykop.pl ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("by ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Zeczero");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");

            Console.Write("Enter your login: ");
            login = Console.ReadLine();
            Console.Write("Enter your password: ");
            password = Console.ReadLine();
            DeletePosts(login, password);

            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("-headless");
            var driverService = FirefoxDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            IWebDriver webDriver = new FirefoxDriver(driverService, options);
        }
    }
}
