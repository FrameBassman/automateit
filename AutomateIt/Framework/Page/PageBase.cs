using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using AutomateIt.Framework.Browser;
using AutomateIt.Framework.Service;
using AutomateIt.Logging;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Page
{
	public abstract class PageBase : IPage
    {
        protected PageBase()
        {
            Data = new Dictionary<string, string>();
            Params = new Dictionary<string, string>();
        }

        public Browser.Browser Browser { get; private set; }

        public ITestLogger Log { get; private set; }

        public BrowserAction Action
        {
            get { return Browser.Action; }
        }

        public BrowserAlert Alert
        {
            get { return Browser.Alert; }
        }

        public BrowserFind Find
        {
            get { return Browser.Find; }
        }

        public BrowserGet Get
        {
            get { return Browser.Get; }
        }

        public BrowserGo Go
        {
            get { return Browser.Go; }
        }

        public BrowserIs Is
        {
            get { return Browser.Is; }
        }

        public BrowserState State
        {
            get { return Browser.State; }
        }

        public BrowserWait Wait
        {
            get { return Browser.Wait; }
        }

        public BrowserJs Js
        {
            get { return Browser.Js; }
        }

        public BrowserWindow Window
        {
            get { return Browser.Window; }
        }

        BrowserCookies IPageObject.Cookies
        {
            get { return Browser.Cookies; }
        }

        #region IPage Members

        /// <summary>
        ///     Активизировать страницу
        /// </summary>
        /// <remarks>
        ///     Если страница активна, значит через нее можно работать с браузером
        /// </remarks>
        public virtual void Activate(Browser.Browser browser, ITestLogger log) {
            Browser = browser;
            Log = log;
        }

        public virtual void InitializeComponents() {
            Alerts = new List<IHtmlAlert>();
            Overlays = new List<IOverlay>();
            ProgressBars = new List<IProgressBar>();
            WebPageBuilder.InitPage(this);
        }

        public virtual void CleanUp() {
            if (State.PageInvalidated
                || Is.AjaxActive()
                || State.GetActiveAlert() != null) {
                // TODO: ideally, alerts should be closed as well as overlays, without page refresh
                Refresh();
            }
            try {
                foreach (var overlay in State.GetOverlays()) {
                    overlay.Close();
                }
            }
            catch (Exception e) {
                Log.Error("Error occured while closing overlay.");
                Log.Error(e.Message);
                Refresh();
            }
        }

        public virtual void Refresh() => Go.Refresh();

        public List<IProgressBar> ProgressBars { get; private set; }

        /// <summary>
        /// Browser Options to use on the page
        /// </summary>
        public virtual BrowserOptions BrowserOptions => new BrowserOptions();

	    public bool Invalidated { get; set; }

	    public List<IHtmlAlert> Alerts { get; private set; }

        public List<IOverlay> Overlays{ get; private set; }

        public BaseUrlInfo BaseUrlInfo { get; set; }

        public List<Cookie> Cookies { get; set; }

        public Dictionary<string, string> Params { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public void RegisterComponent(IComponent component)
        {
            if (component is IHtmlAlert)
                Alerts.Add(component as IHtmlAlert);
            else if(component is IOverlay) {
                Overlays.Add(component as IOverlay);
            }
            else if (component is IProgressBar)
                ProgressBars.Add(component as IProgressBar);
        }

        public T RegisterComponent<T>(string componentName, params object[] args) where T : IComponent
        {
            var component = CreateComponent<T>(args);
            RegisterComponent(component);
            component.ComponentName = componentName;
            return component;
        }

        public T CreateComponent<T>(params object[] args) where T : IComponent
        {
            return (T)WebPageBuilder.CreateComponent<T>(this, args);
        }

        #endregion
    }
}