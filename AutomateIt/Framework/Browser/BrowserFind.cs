using System;
using System.Collections.Generic;
using System.Linq;
using AutomateIt.Exceptions;
using AutomateIt.SmartSelector;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Browser
{
    public class BrowserFind : DriverFacade
    {
        public BrowserFind(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Поиск элемента. Если не найден - кинуть исключение
        /// </summary>
        public IWebElement Element(string scssSelector)
        {
            return Element(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement Element(Selector selector) => Element(selector.By, selector.FrameBy);

        public IWebElement Element(By by, By frameBy = null, bool displayed = true)
        {
            return Element(Driver, by, frameBy, displayed);
        }

        public IWebElement Element(ISearchContext context, By by, bool displayed = true)
        {
            return Element(context, by, null, displayed);
        }

        public IWebElement Element(ISearchContext context, By by, By frameBy, bool displayed = true) {
            Browser.Driver.SwitchTo().DefaultContent();
            if (frameBy != null) {
                SwitchToFrame(context, frameBy);
            }
            var start = DateTime.Now;
            var elements = context.FindElements(by).ToList();
            if (elements.Count == 0) {
                Log.Selector(by);
                throw new NoSuchElementException($"Search time: {(DateTime.Now - start).TotalMilliseconds}");
            }
            if (displayed) {
                elements = elements.Where(e => e.Displayed).ToList();
                if (elements.Count == 0) {
                    if (Browser.TimeoutDisabled) {
                        Log.Selector(by);
                        throw new NoVisibleElementsException();
                    }
                    try {
                        Browser.Wait.ForElementVisible(by, frameBy);
                        elements = context.FindElements(by).Where(e => e.Displayed).ToList();
                    }
                    catch (WebDriverTimeoutException) {
                        Log.Selector(by);
                        throw new NoVisibleElementsException();
                    }
                }
            }
            if (Browser.Options.FindSingle && elements.Count > 1) {
                Log.Selector(by);
                Throw.TestException("Found more then 1 element. Count: ", elements.Count);
            }
            return elements.First();
        }

        private void SwitchToFrame(ISearchContext context, By frameBy) {
            var start = DateTime.Now;
            var frameElements = context.FindElements(frameBy);
            if (frameElements.Count == 0) {
                Log.Selector(frameBy);
                throw new NoSuchElementException($"Search time: {(DateTime.Now - start).TotalMilliseconds}");
            }
            if (frameElements.Count > 1) {
                Log.Selector(frameBy);
                Throw.TestException("Found more then 1 IFRAME element. Count: ", frameElements.Count);
            }
            else {
                Browser.Driver.SwitchTo().Frame(frameElements[0]);
            }
        }

        /// <summary>
        ///     Попытка поиска элемента. Если не найден - не кидать исключение
        /// </summary>
        public IWebElement ElementFastOrNull(string scssSelector, bool displayed = true)
        {
            return ElementFastOrNull(ScssBuilder.CreateBy(scssSelector), null, displayed);
        }

        public IWebElement ElementFastOrNull(By by, By frameBy = null, bool displayed = true)
        {
            return ElementFastOrNull(Driver, by, frameBy, displayed);
        }

        public IWebElement ElementFastOrNull(ISearchContext context, By by, By frameBy, bool displayed = true)
        {
            try {
                Browser.DisableTimeout();
                Log.Disable();
                Browser.Options.FindSingle = false;
                return Element(context, by, frameBy, displayed);
            }
            catch (NoSuchElementException) {
                return null;
            }
            catch (NoVisibleElementsException) {
                return null;
            }
            finally {
                Browser.Options.FindSingle = true;
                Browser.EnableTimeout();
                Log.Enable();
            }
        }

        /// <summary>
        ///     Найти элемент без ожидания
        /// </summary>
        public IWebElement ElementFast(string scssSelector)
        {
            return ElementFast(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement ElementFast(By by)
        {
            try
            {
                Browser.DisableTimeout();
                Log.Selector(by);
                return Driver.FindElement(by);
            }
            finally
            {
                Browser.EnableTimeout();
            }
        }

        /// <summary>
        ///     Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        /// </summary>
        public List<IWebElement> Elements(string scssSelector)
        {
            return Elements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> Elements(By by, By frameBy = null)
        {
            return Elements(Driver, by, frameBy);
        }

        public List<IWebElement> ElementsFast(Selector selector) => ElementsFast(Driver, selector.By, selector.FrameBy);

        public List<IWebElement> ElementsFast(ISearchContext context, By by, By frameBy = null) {
            try {
                Browser.DisableTimeout();
                return Elements(context, by, frameBy);
            }
            finally {
                Browser.EnableTimeout();
            }
        }

        public List<IWebElement> Elements(ISearchContext context, By by, By frameBy = null) {
            try {
                Browser.Driver.SwitchTo().DefaultContent();
                if (frameBy != null) {
                    SwitchToFrame(context,frameBy);
                }
                return new List<IWebElement>(context.FindElements(by));
            }
            catch (NoSuchElementException) {
                return new List<IWebElement>();
            }
        }

        /// <summary>
        ///     Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        ///     Вернуть только видимые элементы
        /// </summary>
        public List<IWebElement> VisibleElements(string scssSelector)
        {
            return VisibleElements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> VisibleElements(By by)
        {
            return RepeatAfterStale(() => Elements(by).Where(e => e.Displayed).ToList());
        }
    }
}