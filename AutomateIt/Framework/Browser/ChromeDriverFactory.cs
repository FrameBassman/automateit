using System.IO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using AutomateIt.Framework.Interfaces;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace AutomateIt.Framework.Browser
{
    public class ChromeDriverFactory : IDriverManager
    {
        private IWebDriver _driver;

        public BrowserSettings Settings { get; }

        public ChromeDriverFactory(BrowserSettings browserSettings)
        {
            Settings = browserSettings;
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        public IWebDriver Driver
        {
            get { return _driver; }
        }

        public void InitDriver()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddArgument("--allow-running-insecure-content");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--test-type");
            options.AddArgument("no-sandbox");
            options.AddUserProfilePreference("download.default_directory", Settings.DownloadDirectory);

			//options.AddUserProfilePreference("download.prompt_for_download", true);
			//options.AddUserProfilePreference("safebrowsing.enabled", true);
			//options.AddUserProfilePreference("safebrowsing.extended_reporting_enabled", false);
			//options.AddUserProfilePreference("safebrowsing.download_feedback_enabled", false);
			//options.AddUserProfilePreference("safebrowsing.reporting_enabled", false);
			//options.AddUserProfilePreference("safebrowsing.proceed_anyway_disabled", false);

            new DriverManager().SetUpDriver(new ChromeConfig());
			_driver = new ChromeDriver(options);
        }
    }
}
