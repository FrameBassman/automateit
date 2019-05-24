using AutomateIt.SmartSelector;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Browser
{
    public class BrowserJs : DriverFacade {
        public BrowserJs(Browser browser)
            : base(browser) {
        }

        /// <summary>
        ///     ��������� Java Script
        /// </summary>
        public T Excecute<T>(string js, params object[] args) => (T)Excecute(js, args);

        /// <summary>
        ///     ��������� Java Script
        /// </summary>
        public object Excecute(string js, params object[] args) {
            var excecutor = Driver as IJavaScriptExecutor;
            return excecutor.ExecuteScript(js, args);
        }

        /// <summary>
        ///     ���������� ��������� �� Y ����� ������� ���� �����
        /// </summary>
        public void ScrollIntoView(IWebElement element) => Excecute($"window.scrollTo(0, {element.Location.Y});");

        /// <summary>
        ///     �������� ����������� ������� ��� ���������� ��������
        /// </summary>
        /// <remarks>������ ��� ������� � JQuery</remarks>
        public string GetEventHandlers(string css, JsEventType eventType) {
            var js = string.Format($@"var handlers= $._data($('{css}').get(0),'events').{eventType};
                          var s='';
                          for(var i=0;i<handlers.length;i++)
                            s+=handlers[i].handler.toString();
                          return s;");
            return Excecute<string>(js);
        }

        /// <summary>
        ///     ��������� �� ����� ��������
        /// </summary>
        public bool IsPageBottom() => Excecute<bool>("return document.body.scrollHeight===" +
                                                     "document.body.scrollTop+document.documentElement.clientHeight");

        /// <summary>
        ///     ���������� ��������� �� ���� ��������
        /// </summary>
        public void ScrollToBottom() => Excecute(@"window.scrollTo(0,
                                       Math.max(document.documentElement.scrollHeight,
                                                document.body.scrollHeight,
                                                document.documentElement.clientHeight));");

        /// <summary>
        ///     ���������� ��������� �� ����� ��������
        /// </summary>
        public void ScrollToTop() => Excecute(@"window.scrollTo(0,0);");

        public void SetCssValue(By by, By frameBy, ECssProperty cssProperty, int value) => Excecute($"arguments[0].style.{cssProperty}={value};", Browser.Find.Element(by, frameBy));


        public void Click(By by, By frameBy) => Click(Browser.Find.Element(by, frameBy));

        public void Click(IWebElement element) {
            Excecute($"window.scrollTo(0,{element.Location.X})");
            Excecute("arguments[0].click();", element);
        }

        public void SetValue(IWebElement element, string value) => Excecute($"arguments[0].setAttribute('value', '{value}')", element);

        public string GetComputedStyle(IWebElement element, string cssPropertyName) => Excecute<string>($"return window.getComputedStyle(arguments[0])['{cssPropertyName}'];", element);

        public string GetFirstLevelText(Selector selector) => GetFirstLevelText(Browser.Find.Element(selector.By, selector.FrameBy));

        public string GetFirstLevelText(IWebElement element) => Excecute<string>(
            @"var iter = document.evaluate('text()', arguments[0], null, XPathResult.ORDERED_NODE_ITERATOR_TYPE);
                var want = '';
                var node;
                while (node = iter.iterateNext()){
                    want += node.data;
                }
                return want;", element);
    }

    /// <summary>
    ///     ���� ����������� js �������
    /// </summary>
    public enum JsEventType
    {
        click
    }
}
