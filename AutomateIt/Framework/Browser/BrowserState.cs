using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using AutomateIt.Framework.Page;
using AutomateIt.Framework.Service;

namespace AutomateIt.Framework.Browser {
    public class BrowserState : DriverFacade {
        // Объект для работы со страницей в активном окне браузера
        public IPage Page;

        // Объект для работы с html имитацией алерта, отображаемой в активной странице браузера
        public IHtmlAlert HtmlAlert;

        // Объект для работы с системным алертом, отображаемым в активной странице браузера
        public IAlert SystemAlert;

        /// <summary>
        ///     Идентификатор текущего окна
        /// </summary>
        public string CurrentWindowHandle;

        private readonly PagesPool _pagesPool;

        public BrowserState(Browser browser)
            : base(browser) {
            var pageTypes = Web.Services.SelectMany(s => s.Router.GetAllPageTypes());
            _pagesPool = new PagesPool(pageTypes);
            _pagesPool.Run();
        }

        public bool PageInvalidated => Page?.Invalidated ?? false;

        public List<IOverlay> GetOverlays() => PageAs<IPage>()?.Overlays.Where(o => o.IsOpened()).ToList() ?? new List<IOverlay>();

        public IAlert GetActiveAlert() {
            return Browser.Alert.GetSystemAlert() ?? (HtmlAlert != null && HtmlAlert.IsVisible() ? HtmlAlert : null);
        }

        /// <summary>
        ///     Приведение текущего html алерта к указанному типу
        /// </summary>
        public T HtmlAlertAs<T>()
            where T : class
        {
	        return (T)HtmlAlert;
        }

	    /// <summary>
        ///     Приведение текущего html алерта к указанному типу
        /// </summary>
        public bool HtmlAlertIs<T>() {
            if (HtmlAlert == null)
                return false;
            return HtmlAlert is T;
        }

        // Приведение текущей страницы к нужному типу
        public T PageAs<T>()
            where T : class => Page as T;

        // Проверка соответствия класса текущей страницы указанному типу
        public bool PageIs<T>() {
            if (Page == null)
                return false;
            return Page is T;
        }

        // Определение текущего состояния браузера
        public void Actualize() {
            ActualizeSystemAlert();
            if (SystemAlert != null)
                return;
            ActualizeWindow();
            //Browser.Wait.WhileAjax(ajaxInevitable: true);
            ActualizePage();
            ActualizeHtmlAlert();
        }

        /// <summary>
        ///     Актуализация текущего окна
        /// </summary>
        public bool ActualizeWindow() {
            if (Driver.WindowHandles.Last() != CurrentWindowHandle) {
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                CurrentWindowHandle = Driver.CurrentWindowHandle;
                Driver.Manage().Window.Maximize();
                return true;
            }
            return false;
        }

        public int GetWindowsCount() {
            return Driver.WindowHandles.Count;
        }

        /// <summary>
        ///     Актуализация Html алерта
        /// </summary>
        public void ActualizeHtmlAlert() {
            HtmlAlert = null;
            if (Page == null)
                return;
            HtmlAlert = Page.Alerts.FirstOrDefault(a => a.IsVisible());
        }

        /// <summary>
        ///     Актуализация системного алерта
        /// </summary>
        private void ActualizeSystemAlert() {
            SystemAlert = Browser.Alert.GetSystemAlert();
        }

        public void ActualizePage() => ActualizePage(new RequestData(Driver.Url, new List<Cookie>(Driver.Manage().Cookies.AllCookies.AsEnumerable())));

        /// <summary>
        ///     Определение класса для работы с текущей активной страницей браузера
        /// </summary>
        public void ActualizePage(RequestData requestData) {
            // . empty
            Page = null;

            // . match
            var result = Web.MatchService(requestData);
            if (result != null) {
                Page = result.Service.GetPage(requestData, result.BaseUrlInfo);
            }

            // . initialize and activate
            if (Page == null) {
                //Log.Debug("We are on unknown page.");
            }
            else {
                var initializedPage = _pagesPool.GetInitializedPage(Page);
                if (initializedPage != null) {
                    Page = initializedPage;
                }
                else {
                    Page.InitializeComponents();
                }
                Page.Activate(Browser, Log);
                Browser.ApplyPageOptions(Page);
                //Log.Debug($"We are on the {Page.GetType().Name}.");
            }
        }

        public bool HasOverlay() => PageAs<IPage>()?.Overlays.Any(o => o.IsOpened()) ?? false;

        public void InvalidatePage() {
            if (Page != null) {
                Page.Invalidated = true;
            }
        }

        public T GetHtmlAlert<T>()
            where T : class {
            ActualizeHtmlAlert();
            return HtmlAlertAs<T>();
        }
    }

    internal class PagesPool {
        private readonly IEnumerable<Type> _pageTypes;
        private readonly ConcurrentDictionary<Type, IPage> _initializedPages;
        private Thread _thread;

        public PagesPool(IEnumerable<Type> pageTypes) {
            _pageTypes = pageTypes;
            _initializedPages = new ConcurrentDictionary<Type, IPage>();
        }

        public void Run() {
            _thread = new Thread(GeneratePages);
            _thread.Start();
        }

        public void GeneratePages() {
            while (true) {
                if (_initializedPages.Count < _pageTypes.Count()) {
                    foreach (var pageType in _pageTypes) {
                        if (!_initializedPages.ContainsKey(pageType)) {
                            _initializedPages.TryAdd(pageType, GeneratePage(pageType));
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private IPage GeneratePage(Type pageType) {
            var instance = (IPage)Activator.CreateInstance(pageType);
            instance.InitializeComponents();
            return instance;
        }

        public IPage GetInitializedPage(IPage page) {
            IPage initializedPage;
            if (_initializedPages.TryRemove(page.GetType(), out initializedPage)) {
                initializedPage.BaseUrlInfo = page.BaseUrlInfo;
                initializedPage.Data = page.Data;
                initializedPage.Params = page.Params;
                initializedPage.Cookies = page.Cookies;
            }
            return initializedPage;
        }
    }
}