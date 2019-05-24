using System;
using System.Linq;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Browser
{
    public class BrowserWindow : DriverFacade {
        public BrowserWindow(Browser browser)
            : base(browser) {
        }

        public string Url => Driver.Url;

        public void CloseAllButFirst() {
            while (Driver.WindowHandles.Count > 1) {
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                Driver.Close();
            }
            try {
                var currenthandle = Driver.CurrentWindowHandle;
                GC.KeepAlive(currenthandle);
            }
            catch (NoSuchWindowException e) {
                Driver.SwitchTo().Window(Driver.WindowHandles.First());
                Browser.State.Actualize();
            }
        }
    }
}