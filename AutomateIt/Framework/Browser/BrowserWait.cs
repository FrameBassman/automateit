using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using AutomateIt.Framework.Page;
using AutomateIt.SmartSelector;
using OpenQA.Selenium.Support.UI;

namespace AutomateIt.Framework.Browser
{

    using AutomateIt.Framework.Browser;

    public class BrowserWait : DriverFacade
    {
        public BrowserWait(Browser browser)
            : base(browser)
        {
        }

        public void Until(Func<bool> condition, int timeout = 3, int sleepInterval = 100, string timeoutMessage=null)
        {
            var wait = new WebDriverWait(new SystemClock(), Driver, TimeSpan.FromSeconds(timeout), TimeSpan.FromMilliseconds(sleepInterval));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            if (timeoutMessage != null)
                wait.Message = timeoutMessage;
            wait.Until(driver => condition.Invoke());
        }

        /// <summary>
        /// Wait for condition. Do not throw TimeoutException if condition is not satisfied.
        /// </summary>
        public T UntilSoftly<T>(Func<T> condition, int timeout = 3, int sleepInterval = 100) {
            var wait = new WebDriverWait(new SystemClock(), Driver, TimeSpan.FromSeconds(timeout), TimeSpan.FromMilliseconds(sleepInterval));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            try {
                return wait.Until(driver => condition.Invoke());
            }
            catch (WebDriverTimeoutException) {
                return default(T);
            }
        }

        /// <summary>
        ///     Подождать пока элемент отображается на странице
        /// </summary>
        /// <param name="by">Селектор видимого элемента</param>
        /// <param name="timeout">Максимальный период ожидания</param>
        public void WhileElementVisible(string scssSelector, int timeout = BrowserTimeouts.AJAX)
        {
            WhileElementVisible(Scss.GetBy(scssSelector), timeout);
        }

        public void WhileElementVisible(By by, int timeout = BrowserTimeouts.AJAX, By frameBy = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver => !Browser.Is.Visible(by, frameBy));
        }

        /// <summary>
        ///     Wait until element is not visible
        /// </summary>
        public void ForElementVisible(string scssSelector, int timeout = BrowserTimeouts.FIND)
        {
            ForElementVisible(Scss.GetBy(scssSelector), null, timeout);
        }

        public void ForElementVisible(By by, By frameBy = null, int timeout = BrowserTimeouts.FIND) {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver => Browser.Is.Visible(by, frameBy));
        }

        /// <summary>
        ///     Подождать пока не скроются все зарегистрированные на страницы прогрессы
        /// </summary>
        public void WhilePageInProgress(int timeout = BrowserTimeouts.AJAX) {
            if (Browser.State.Page == null)
                return;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver => Browser.State.Page.ProgressBars.All(p => !p.IsVisible()));
        }

        public void ForPageInProgress(int milliseconds = 1000) {
            if (Browser.State.Page == null)
                return;
            const int POLLING_INTERVAL = 200;
            var count = (int)Math.Ceiling(milliseconds / (decimal)POLLING_INTERVAL);
            for (var i = 0; i < count; i++) {
                if (Browser.State.Page.ProgressBars.All(p => p.IsVisible()))
                    return;
                Thread.Sleep(POLLING_INTERVAL);
            }
        }

        /// <summary>
        ///     Подождать пока не завершатся Ajax запросы
        /// </summary>
        /// <param name="timeout">максимальное время ожидания пока отработают все ajax запросы</param>
        /// <param name="ajaxInevitable">
        ///     true - ajax запрос 100% должен выполниться
        ///     если этого не произошло, ожидаем 3 секунды и проверяем еще раз
        /// </param>
        public void WhileAjax(int timeout = BrowserTimeouts.AJAX, bool ajaxInevitable = false)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            var waited = false;
            wait.Until(
                driver =>
                    {
                        var ajaxActive = Browser.Is.AjaxActive();
                        if (ajaxActive)
                        {
                            waited = true;
                            return false;
                        }
                        return true;
                    });
            if (!waited && ajaxInevitable)
            {
                Browser.Wait.ForAjax(3000);
                wait.Until(driver => !Browser.Is.AjaxActive());
            }
        }

        /// <summary>
        ///     Подождать пока не начнется выполнения ajax запросов
        /// </summary>
        public void ForAjax(int miliseconds = 1000)
        {
            const int POLLING_INTERVAL = 200;
            var count = (int)Math.Ceiling(miliseconds / (decimal)POLLING_INTERVAL);
            for (var i = 0; i < count; i++)
            {
                if (Browser.Is.AjaxActive())
                    return;
                Thread.Sleep(POLLING_INTERVAL);
            }
        }

        public void ForWindow(int timeout = BrowserTimeouts.REDIRECT) => Browser.Wait.Until(
            () =>
                {
                    if (Browser.State.ActualizeWindow()) {
                        Browser.State.ActualizePage();
                        return true;
                    }
                    return false;
                }, timeout);

        public T ForRedirectTo<T>(int timeout = BrowserTimeouts.REDIRECT)
            where T : class {
            Browser.Wait.UntilSoftly(
                () =>
                    {
                        if (Browser.State.PageIs<T>())
                            return true;
                        Browser.State.ActualizePage();
                        return false;
                    }, timeout);
            return Browser.State.PageAs<T>();
        }

        public void ForRedirect(string oldUrl = null, bool waitForAjax = false, bool ajaxInevitable = false, int timeout = BrowserTimeouts.REDIRECT) {
            oldUrl = oldUrl ?? Browser.Window.Url;
            Browser.Wait.Until(
                () =>
                    {
                        Browser.State.Actualize();
                        if (oldUrl != Browser.Window.Url) {
                            if (waitForAjax || ajaxInevitable)
                                Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
                            Browser.State.Actualize();
                            return true;
                        }
                        return false;
                    }, timeout);
        }

        public T ForHtmlAlert<T>(int timeout = BrowserTimeouts.AJAX)
            where T : class, IHtmlAlert {
            Browser.Wait.Until(
                () =>
                    {
                        Browser.State.ActualizeHtmlAlert();
                        return Browser.State.HtmlAlert != null;
                    }, timeout);
			return Browser.State.HtmlAlertAs<T>();
        }

        public void ForHtmlAlert(int timeout = BrowserTimeouts.AJAX) => ForHtmlAlert<IHtmlAlert>(timeout);

        public void ForAlertOrRedirect(string oldUrl = null, bool waitForAjax = false, bool ajaxInevitable = false)
        {
            const int POLLING_INTERVAL = 200;
            oldUrl = oldUrl ?? Browser.Window.Url;
            for (var i = 0; i < 10; i++)
            {
                Browser.State.ActualizeHtmlAlert();
                // Browser.State.SystemAlert != null || 
                if (Browser.State.HtmlAlert != null)
                {
                    // . alert displayed
                    return;
                }
                if (oldUrl != Browser.Window.Url)
                {
                    // . redirect occured
                    if (waitForAjax || ajaxInevitable)
                        Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
                    Browser.State.Actualize();
                    return;
                }
                Thread.Sleep(POLLING_INTERVAL);
            }
        }
    }
}
