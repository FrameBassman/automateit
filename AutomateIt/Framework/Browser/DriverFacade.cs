using System;
using OpenQA.Selenium;
using AutomateIt.Framework.Service;
using AutomateIt.Logging;

namespace AutomateIt.Framework.Browser
{
    public abstract class DriverFacade {
        protected DriverFacade(Browser browser) {
            Browser = browser;
        }

        protected Browser Browser { get; }

        protected Web Web => Browser.Web;

        protected ITestLogger Log => Browser.Log;

        protected IWebDriver Driver => Browser.Driver;

        /// <summary>
        ///     ��������� �������� ���� �������� StaleReferenceException
        /// </summary>
        /// <param name="func">��������</param>
        public T RepeatAfterStale<T>(Func<T> func) {
            const int TRY_COUNT = 3;
            var result = default(T);
            for (var i = 0; i < TRY_COUNT; i++) {
                try {
                    result = func.Invoke();
                    break;
                }
                catch (StaleElementReferenceException e) {
                    Log.Warning(e);
                    if (i == TRY_COUNT - 1)
                        throw;
                }
                catch (InvalidOperationException e) {
                    if (e.Message.Contains("element is not attached to the page document")) {
                        // Chrome sometimes throws InvalidOperationException instead of StaleElementReferenceException
                        Log.Warning(e, "Unable to perform action using stale element reference.");
                        if (i == TRY_COUNT - 1)
                            throw;
                    }
                    else {
                        // it is a regular InvalidOperationException
                        throw;
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///     ��������� �������� ���� �������� StaleReferenceException
        /// </summary>
        /// <param name="action">��������</param>
        public void RepeatAfterStale(Action action) =>
            RepeatAfterStale(() =>
                {
                    action.Invoke();
                    return true;
                });
    }
}
