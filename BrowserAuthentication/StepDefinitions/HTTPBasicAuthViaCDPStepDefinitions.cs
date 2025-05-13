using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;

using System;
using System.Text;
using TechTalk.SpecFlow;
using OpenQA.Selenium.DevTools.V135.Network;
using NUnit.Framework;

namespace BrowserAuthentication.StepDefinitions
{
    [Binding]
    public class HTTPBasicAuthViaCDPStepDefinitions
    {
        private IWebDriver driver;
        private DevToolsSession devToolsSession;
        private OpenQA.Selenium.DevTools.V135.DevToolsSessionDomains devTools;



        [Given(@"I authenticate using CDP")]
        public async Task GivenIAuthenticateUsingCDP()
        {
            ChromeOptions options = new ChromeOptions();
            driver = new ChromeDriver(options);

            // Establish DevTools session
            var devToolsDriver = driver as IDevTools;
            devToolsSession = devToolsDriver.GetDevToolsSession();
            devTools = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V135.DevToolsSessionDomains>();

            // Enable network conditions and set the basic auth header
            await devTools.Network.Enable(new EnableCommandSettings());

            string auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:admin"));
            var headers = new Headers();
            headers.Add("Authorization", auth);
            await devTools.Network.SetExtraHTTPHeaders(new SetExtraHTTPHeadersCommandSettings
            {
                Headers = headers  // Pass the headers in the correct type
            });

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/basic_auth");
            ScenarioContext.Current["driver"] = driver;
        }

        [Then(@"I should see the success message")]
        public void ThenIShouldSeeTheSuccessMessage()
        {
            driver = (IWebDriver)ScenarioContext.Current["driver"];
            var message = driver.FindElement(By.CssSelector("div.example")).Text;
            Assert.IsTrue(message.Contains("Congratulations!"), "Success message not found");
            driver.Quit();
        }
    }
}
