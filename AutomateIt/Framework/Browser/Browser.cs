using System;
using AutomateIt.Framework.Page;

namespace AutomateIt.Framework.Browser
{
    using Interfaces;
    using OpenQA.Selenium;
    using AutomateIt.Logging;
    using AutomateIt.Framework.Service;
    using AutomateIt.Framework.Browser;

    public class Browser
    {
        private readonly IDriverManager _driverManager;
        public IWebDriver Driver { get; private set; }
        public ITestLogger Log { get; private set; }
        public Web Web { get; private set; }
        public BrowserOptions Options { get; private set; }
        public BrowserSettings Settings => _driverManager.Settings;
        
        public bool TimeoutDisabled { get; set; }
        
        public readonly BrowserAction Action;
        public readonly BrowserAlert Alert;
        public readonly BrowserFind Find;
        public readonly BrowserGet Get;
        public readonly BrowserGo Go;
        public readonly BrowserIs Is;
        public readonly BrowserState State;
        public readonly BrowserWait Wait;
        public readonly BrowserJs Js;
        public BrowserWindow Window;
        public BrowserCookies Cookies;
        
        public Browser(Web web, ITestLogger log, IDriverManager driverManager) {
            Web = web;
            Log = log;
            _driverManager = driverManager;
            _driverManager.InitDriver();
            Driver = _driverManager.Driver;
            Find = new BrowserFind(this);
            Get = new BrowserGet(this);
            Is = new BrowserIs(this);
            Alert = new BrowserAlert(this);
            State = new BrowserState(this);
            Action = new BrowserAction(this);
            Window = new BrowserWindow(this);
            Go = new BrowserGo(this);
            Wait = new BrowserWait(this);
            Js = new BrowserJs(this);
            Cookies = new BrowserCookies(this);
            Options = new BrowserOptions();
        }
        
        // Уничтожить драйвер(закрывает все открытые окна браузер)
        public void Destroy() => _driverManager.Dispose();

        // Пересоздать драйвер
        public void Recreate() {
            Log.Action("Close browser and open again");
            _driverManager.Dispose();
            _driverManager.InitDriver();
            Driver = _driverManager.Driver;
        }

        public void DisableTimeout() {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(value: 0);
            TimeoutDisabled = true;
        }

        public void EnableTimeout() {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(BrowserTimeouts.FIND);
            TimeoutDisabled = false;
        }

        public void WithOptions(Action action, bool findSingle = BrowserOptions.FINDSINGLE_DEFAULT, bool useJsClick=BrowserOptions.USE_JS_CLICK_DEFAULT) {
            var memento = (BrowserOptions)Options.Clone();
            Options.FindSingle = findSingle;
            Options.UseJsClick = useJsClick;
            action.Invoke();
            Options = memento;
        }

        public void ApplyPageOptions(IPage page) => Options = page.BrowserOptions;
    }
}
