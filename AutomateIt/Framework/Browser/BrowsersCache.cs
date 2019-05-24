using System;
using System.Collections.Generic;
using System.IO;
using AutomateIt.Framework.Interfaces;
using AutomateIt.Framework.Service;
using AutomateIt.Logging;

namespace AutomateIt.Framework.Browser
{
    public class BrowsersCache
    {
        private readonly Dictionary<BrowserType, Browser> _browsers;
        private readonly ITestLogger _log;
        private readonly Web _web;

        public BrowsersCache(Web web, ITestLogger log)
        {
            _web = web;
            _log = log;
            _browsers = new Dictionary<BrowserType, Browser>();
        }

        public Browser GetBrowser(BrowserType browserType)
        {
            if (_browsers.ContainsKey(browserType))
                return _browsers[browserType];
            var browser = CreateBrowser(browserType);
            _browsers.Add(browserType, browser);
            return browser;
        }

        private Browser CreateBrowser(BrowserType browserType)
        {
            var browserSettings = new BrowserSettings();
            var driverManager = getDriverFactory(browserType, browserSettings);
            return new Browser(_web, _log, driverManager);
        }

        private IDriverManager getDriverFactory(BrowserType browserType, BrowserSettings browserSettings)
        {
            switch (browserType)
            {
                case BrowserType.FIREFOX:
                    return new FirefoxDriverFactory(browserSettings);
                case BrowserType.CHROME:
                    return new ChromeDriverFactory(browserSettings);
                default:
                    return null;
            }
        }

        public bool BrowserIsCreated(BrowserType browserType) => _browsers.ContainsKey(browserType);
    }
}
