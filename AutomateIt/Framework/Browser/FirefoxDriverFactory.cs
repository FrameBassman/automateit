using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using AutomateIt.Framework.Interfaces;

namespace AutomateIt.Framework.Browser
{
    public class FirefoxDriverFactory : IDriverManager
    {
        private FirefoxDriver _driver;
        public BrowserSettings Settings { get; }
        public IWebDriver Driver => _driver;

        public FirefoxDriverFactory(BrowserSettings browserSettings)
        {
            Settings = browserSettings;
        }

        #region DriverManager Members

        public void InitDriver()
        {
            _driver = new FirefoxDriver();
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        #endregion
    }
}
