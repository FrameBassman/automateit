using AutomateIt.Exceptions;
using AutomateIt.Framework.Page;

namespace AutomateIt.Framework.Browser
{
    using System;
    using System.IO;
    using System.Threading;
    using AutomateIt.SmartSelector;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public class BrowserAction : DriverFacade
    {
        private readonly BrowserFind _find;

        public BrowserAction(Browser browser)
            : base(browser)
        {
            _find = browser.Find;
        }

        /// <summary>
        ///     Выбрать опцию в html теге Select
        /// </summary>
        public void Select(string scssSelector, string value)
        {
            Select(ScssBuilder.CreateBy(scssSelector), value);
        }

        public void Select(By by, string value)
        {
            RepeatAfterStale(
                () =>
                {
                    var select = _find.Element(by);
                    var dropDown = new SelectElement(select);
                    dropDown.SelectByValue(value);
                });
        }

        /// <summary>
        ///     Ввести значение в поле ввода
        /// </summary>
        public void TypeIn(string scssSelector, By frameBy, object value, bool clear = true)
        {
            TypeIn(ScssBuilder.CreateBy(scssSelector), frameBy, value, clear);
        }

        public void ClickBackButton()
        {
            var oldUrl = Browser.Window.Url;
            Driver.Navigate().Back();
            for (var i = 0; i < 10; i++)
            {
                Browser.State.Actualize();
                if (Browser.State.SystemAlert != null
                    || oldUrl != Browser.Window.Url)
                {
                    Browser.Wait.WhileAjax();
                    return;
                }
                Thread.Sleep(200);
            }
        }

        public void ClickNextButton()
        {
            var oldUrl = Browser.Window.Url;
            Driver.Navigate().Forward();
            for (var i = 0; i < 10; i++)
            {
                Browser.State.Actualize();
                if (Browser.State.SystemAlert != null
                    || oldUrl != Browser.Window.Url)
                {
                    Browser.Wait.WhileAjax();
                    return;
                }
                Thread.Sleep(200);
            }
        }

        public void TypeIn(By by, By frameBy, object value, bool clear = true)
        {
            RepeatAfterStale(
                () =>
                {
                    var element = _find.Element(by, frameBy);
                    if (clear)
                        Clear(element);
                    var valueString = value.ToString();
                    switch (Browser.Options.TypeInStyle)
                    {
                        case TypeInStyle.FullValue:
                            element.SendKeys(valueString);
                            break;
                        case TypeInStyle.Chars:
                            foreach (var c in valueString)
                                element.SendKeys(c.ToString());
                            break;
                        case TypeInStyle.Js:
                            Browser.Js.SetValue(element, valueString);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Browser.Options.TypeInStyle");
                    }
                });
        }

        public void TypeInAndWaitWhileAjax(By by, By frameBy, object value, bool clear = true,
            bool ajaxInevitable = false)
        {
            TypeIn(by, frameBy, value, clear);
            Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
        }


        public void Click(Selector selector, int sleepTimeout = 0) =>
            Click(selector.By, selector.FrameBy, sleepTimeout);

        public void Click(string scssSelector, By frameBy = null, int sleepTimeout = 0) =>
            Click(ScssBuilder.CreateBy(scssSelector), frameBy, sleepTimeout);

        public void Click(By by, By frameBy = null, int sleepTimeout = 0) => Click(Driver, @by, frameBy, sleepTimeout);

        public void Click(ISearchContext context, By by, By frameBy, int sleepTimeout = 0)
        {
            RepeatAfterStale(() =>
            {
                try
                {
                    if (Browser.Options.WaitWhileAjaxBeforeClick)
                        Browser.Wait.WhileAjax();
                    Click(_find.Element(context, by, frameBy), sleepTimeout);
                }
                catch (InvalidOperationException e)
                {
                    // TODO: !!find out how to fix this problem in a proper way!!
                    if (e.Message.Contains("Other element would receive the click: <div id=\"home-toast-wrapper\""))
                    {
                        try
                        {
                            Browser.Js.SetCssValue(By.CssSelector("#home-toast-wrapper"), null, ECssProperty.zIndex,
                                -1);
                            Click(_find.Element(context, by, frameBy), sleepTimeout);
                        }
                        finally
                        {
                            Browser.Js.SetCssValue(By.CssSelector("#home-toast-wrapper"), null, ECssProperty.zIndex,
                                999999);
                        }
                    }
                    else
                    {
                        Log.Selector(by);
                        Log.Exception(e);
                        throw;
                    }
                }
            });
        }

        public void Click(IWebElement element, int sleepTimeout = 0)
        {
            //if (BrowserOptions.ScrollIntoView) { }
            //Browser.Js.ScrollIntoView(element); // Fix for "element not visible" exception
            ScrollIntoView(element);
            if (Browser.Options.UseJsClick)
            {
                try
                {
                    Browser.Js.Click(element);
                }
                catch (InvalidOperationException)
                {
                    element.Submit();
                }
            }
            else
            {
                element.Click();
            }
            if (sleepTimeout != 0)
                Thread.Sleep(sleepTimeout);
        }

        private void ScrollIntoView(IWebElement element)
        {
            var actions = new Actions(Driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        public T ClickAndWaitForAlert<T>(By by, By frameBy, int timeout = BrowserTimeouts.AJAX) where T : AlertBase
        {
            ClickAndWaitForAlert(by, frameBy, timeout);
            if (!Browser.State.HtmlAlertIs<T>())
            {
                Throw.TestException($"{typeof(T).Name} did not appear after click.");
            }
            return Browser.State.HtmlAlertAs<T>();
        }

        /// <summary>
        ///     Клик по элементу с ожиданием алерта
        /// </summary>
        public void ClickAndWaitForAlert(string scssSelector, int timeout = BrowserTimeouts.AJAX,
            bool ajaxInevitable = false) =>
            ClickAndWaitForAlert(ScssBuilder.CreateBy(scssSelector), null, timeout, ajaxInevitable);

        public void ClickAndWaitNewWindow(By by, By frameBy, int timeout = BrowserTimeouts.WINDOW) =>
            RepeatAfterStale(() => ClickAndWaitNewWindow(Browser.Find.Element(by, frameBy), timeout));

        public void ClickAndWaitNewWindow(IWebElement element, int timeout)
        {
            Click(element);
            Browser.Wait.ForWindow(timeout);
        }

        public IAlert ClickAndWaitForAlert(By by, By frameBy, int timeout = BrowserTimeouts.AJAX, bool repeat = false,
            bool ajaxInevitable = false) => ClickAndWaitForAlert<IAlert>(by, frameBy, timeout, repeat, ajaxInevitable);

        public T ClickAndWaitForAlert<T>(By by, By frameBy, int timeout = BrowserTimeouts.AJAX, bool repeat = false,
            bool ajaxInevitable = false) where T : class, IAlert
        {
            if (repeat)
            {
                Browser.Wait.Until(() =>
                {
                    Click(by, frameBy);
                    return Browser.Wait.ForHtmlAlert<IHtmlAlert>(3) != null;
                }, timeout);
            }
            else
            {
                Click(by, frameBy);
                Browser.Wait.ForHtmlAlert(timeout);
            }
            Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
            return Browser.State.HtmlAlertAs<T>();
        }

        /// <summary>
        ///     Клик и ожидание редиректа
        /// </summary>
        public void ClickAndWaitForRedirect(string scssSelector, By frameBy, bool waitForAjax = false,
            bool ajaxInevitable = false)
        {
            ClickAndWaitForRedirect(ScssBuilder.CreateBy(scssSelector), frameBy, waitForAjax, ajaxInevitable);
        }

        public void ClickAndWaitForRedirect(By by, By frameBy, bool waitForAjax = false, bool ajaxInevitable = false)
        {
            RepeatAfterStale(() =>
                ClickAndWaitForRedirect(Browser.Find.Element(by, frameBy), waitForAjax, ajaxInevitable));
        }

        public void ClickAndWaitForRedirect(IWebElement element, bool waitForAjax = false, bool ajaxInevitable = false)
        {
            var oldUrl = Browser.Window.Url;
            Click(element, 1000);
            Browser.Wait.ForRedirect(oldUrl, waitForAjax, ajaxInevitable);
        }

        public void ClickAndWaitForAlertOrRedirect(By by, By frameBy, bool waitForAjax = false,
            bool ajaxInevitable = false)
        {
            RepeatAfterStale(() =>
                ClickAndWaitForAlertOrRedirect(Browser.Find.Element(by, frameBy), waitForAjax, ajaxInevitable));
        }

        public void ClickAndWaitForAlertOrRedirect(IWebElement element, bool waitForAjax = false,
            bool ajaxInevitable = false)
        {
            var oldUrl = Browser.Window.Url;
            Click(element, 1000);
            Browser.Wait.ForAlertOrRedirect(oldUrl, waitForAjax, ajaxInevitable);
        }

        public void ClickAndWaitForState(string scssSelector, Func<bool> checkState)
        {
            ClickAndWaitForState(ScssBuilder.CreateBy(scssSelector), checkState);
        }

        public void ClickAndWaitForState(By by, Func<bool> checkState)
        {
            RepeatAfterStale(() => ClickAndWaitForState(Browser.Find.Element(by), checkState));
        }

        public void ClickAndWaitForState(IWebElement element, Func<bool> checkState)
        {
            Click(element);
            try
            {
                Browser.Wait.Until(checkState);
            }
            catch (WebDriverTimeoutException)
            {
                Log.Info("Waited state not appeared");
            }
        }

        public void ClickAndWaitWhileAjax(Selector selector, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            ClickAndWaitWhileAjax(selector.By, selector.FrameBy, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(string scssSelector, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            ClickAndWaitWhileAjax(ScssBuilder.CreateBy(scssSelector), null, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(string scss, By frameBy, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            ClickAndWaitWhileAjax(ScssBuilder.CreateBy(scss), frameBy, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(By by, By frameBy, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            ClickAndWaitWhileAjax(Driver, by, frameBy, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(ISearchContext context, By by, By frameBy, int sleepTimeout = 0,
            bool ajaxInevitable = false)
        {
            Click(context, by, frameBy, sleepTimeout);
            Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(IWebElement element, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            // TODO: geti rid of this method
            Click(element, sleepTimeout);
            Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
        }


        /// <summary>
        ///     Нажать Enter в поле найденному по селектору
        /// </summary>
        public void PressEnter(string scssSelector) => PressEnter(ScssBuilder.CreateBy(scssSelector));

        public void PressEnter(By by) => PressEnter(@by, null);

        public void PressEnter(Selector selector) => PressEnter(selector.By, selector.FrameBy);

        public void PressEnter(By by, By frameBy) => PressKey(@by, frameBy, Keys.Enter);

        /// <summary>
        ///     Нажатие клавиши в поле ввода
        /// </summary>
        public void PressKey(string scssSelector, string key) => PressKey(ScssBuilder.CreateBy(scssSelector), key);

        public void PressKey(By by, string key) => PressKey(@by, null, key);

        public void PressKey(By by, By frameBy, string key) =>
            RepeatAfterStale(() => PressKey(Browser.Find.Element(@by, frameBy), key));

        public void PressKey(IWebElement element, string key) => element.SendKeys(key);

        /// <summary>
        ///     Очистить текстовое поле
        /// </summary>
        public void Clear(string scssSelector)
        {
            Clear(ScssBuilder.CreateBy(scssSelector));
        }

        public void Clear(By by, By frameBy = null)
        {
            RepeatAfterStale(() => Clear(Browser.Find.Element(by, frameBy)));
        }

        public void Clear(IWebElement element)
        {
            element.Clear();
        }

        /// <summary>
        ///     Убрать фокус с текущего компонента
        /// </summary>
        public void ChangeFocus()
        {
            PressKey(Driver.SwitchTo().ActiveElement(), Keys.Tab);
        }

        /// <summary>
        ///     Switch to frame
        /// </summary>
        public void SwitchToFrame(By by, By frameBy)
        {
            var frame = _find.Element(by, frameBy);
            Driver.SwitchTo().Frame(frame);
        }

        /// <summary>
        ///     Switch to default content
        /// </summary>
        public void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        ///     Кликнуть и подождать пока на странице отображается прогресс
        /// </summary>
        /// <param name="sleepTimeout">принудительное ожидание после выполнения клика</param>
        /// <param name="progressInevitable">
        ///     true означает что после клика прогресс точно должен появиться
        ///     поэтому сначала ожидаем его появления, потом ожидаем пока он не исчезнет
        /// </param>
        public void ClickAndWaitWhileProgress(string scssSelector, int sleepTimeout = 0,
            bool progressInevitable = false)
        {
            ClickAndWaitWhileProgress(ScssBuilder.CreateBy(scssSelector), null, sleepTimeout, progressInevitable);
        }

        public void ClickAndWaitWhileProgress(By by, By frameBy, int sleepTimeout = 0, bool progressInevitable = false)
        {
            Click(by, frameBy, sleepTimeout);
            if (progressInevitable)
                Browser.Wait.ForPageInProgress();
            Browser.Wait.WhilePageInProgress();
        }

        /// <summary>
        ///     Клик по всем элементам найденным по указанному селектору
        /// </summary>
        public void ClickByAll(string scssSelector)
        {
            ClickByAll(ScssBuilder.CreateBy(scssSelector));
        }

        public void ClickByAll(By by)
        {
            var elements = Browser.Find.Elements(by);
            foreach (var element in elements)
                Browser.Action.Click(element);
        }

        /// <summary>
        ///     Прокрутить страницу до низа
        /// </summary>
        public void ScrollToBottom()
        {
            var start = DateTime.Now;
            do
            {
                Browser.Js.ScrollToBottom();
                Browser.Wait.WhileAjax(ajaxInevitable: true);
            } while (!Browser.Js.IsPageBottom()
                     && (DateTime.Now - start).TotalSeconds < 300);
        }

        /// <summary>
        ///     Сохранить
        /// </summary>
        /// <param name="marker">название файла скриншота</param>
        /// <param name="folder">папка для скриншотов</param>
        public void SaveScreenshot(string marker = null, string folder = "d:\\")
        {
            var screenshot = Browser.Get.Screenshot();
            var filename = string.IsNullOrEmpty(marker) ? new Random().Next(100000).ToString() : marker;
            var screenshotFilePath = Path.Combine(folder, filename + ".png");
            //screenshot.Save(screenshotFilePath, ImageFormat.Png);
            Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
        }

        public void MouseOver(Selector selector, int sleepTimeout = 0) =>
            MouseOver(selector.By, selector.FrameBy, sleepTimeout);

        /// <summary>
        ///     Навести курсор на элемент
        /// </summary>
        public void MouseOver(string scssSelector, By frameBy, int sleepTimeout = 0)
        {
            MouseOver(ScssBuilder.CreateBy(scssSelector), frameBy, sleepTimeout);
        }

        public void MouseOver(By by, By frameBy, int sleepTimeout = 0)
        {
            var action = new Actions(Driver);
            RepeatAfterStale(() =>
            {
                var element = Browser.Find.Element(by, frameBy);
                action.MoveToElement(element).Build().Perform();
                if (sleepTimeout != 0)
                    Thread.Sleep(sleepTimeout);
            });
        }

        public void SetFocus(IWebElement element)
        {
            if (element.TagName == "input")
                element.SendKeys("");
            else
                new Actions(Driver).MoveToElement(element).Build().Perform();
        }

        /// <summary>
        ///     Перетащить элемент на указанное количество пикселей по горизонтали
        /// </summary>
        public void DragByHorizontal(string scssSelector, int pixels)
        {
            DragByHorizontal(ScssBuilder.CreateBy(scssSelector), pixels);
        }

        public void DragByHorizontal(By by, int pixels)
        {
            var builder = new Actions(Driver);
            builder.DragAndDropToOffset(Browser.Find.Element(by), pixels, 0).Build().Perform();
        }

        public void CleanDownloadsFolder()
        {
            if (Directory.Exists(Browser.Settings.DownloadDirectory))
                Directory.Delete(Browser.Settings.DownloadDirectory, true);
        }
    }
}