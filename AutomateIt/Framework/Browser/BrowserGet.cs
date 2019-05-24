using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using AutomateIt.Extensions.Extensions;
using AutomateIt.SmartSelector;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Browser
{
    public class BrowserGet : DriverFacade
    {
        public BrowserGet(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Получить исходный код страницы
        /// </summary>
        public string PageSource
        {
            get { return Browser.Driver.PageSource; }
        }

        // Получить содержимое элемента
        public string TextS(string scssSelector)
        {
            return TextS(ScssBuilder.CreateBy(scssSelector));
        }

        public string TextS(By by)
        {
            return RepeatAfterStale(
                () =>
                    {
                        var element = Browser.Find.ElementFastOrNull(by);
                        if (element == null)
                            return null;
                        return element.Text;
                    });
        }

        // Получить содержимое элемента
        public string Text(string scssSelector, By frameBy = null, bool displayed = false) => Text(ScssBuilder.CreateBy(scssSelector), frameBy, displayed);

        public string Text(By by, By frameBy, bool displayed = false)
        {
            return Text(Driver, by, frameBy, displayed);
        }

        /// <summary>
        ///     Получить содержимое элемента
        /// </summary>
        public string Text(ISearchContext context, By by, By frameBy, bool displayed = false)
        {
            if (displayed && !Browser.Is.Visible(context, by, frameBy))
                return null;
            return RepeatAfterStale(() => Browser.Find.Element(context, by, frameBy, displayed).Text);
        }

        /// <summary>
        ///     Найти элементы по указанному селектору и получить их тексты
        /// </summary>
        public List<string> Texts(Selector selector, bool displayed = false) => Texts(selector.By, selector.FrameBy);

        public List<string> Texts(string scssSelector, By frameBy = null, bool displayed = false) => Texts(ScssBuilder.CreateBy(scssSelector), frameBy, displayed);

        public List<string> Texts(By by, By frameBy = null, bool displayed = false) => Texts(Driver, @by, frameBy, displayed);

        public List<string> Texts(ISearchContext context, string scssSelector, By frameBy = null, bool displayed = false) => Texts(context, ScssBuilder.CreateBy(scssSelector), frameBy, displayed);

        public List<string> Texts(ISearchContext context, By by, By frameBy = null, bool displayed = false)
        {
            return RepeatAfterStale(
                () =>
                    {
						var elements = Browser.Find.Elements(context, by, frameBy);
                        if (displayed)
                            elements = elements.Where(e => e.Displayed).ToList();
                        return elements.Select(e => e.Text).ToList();
                    });
        }

        public List<string> TextsFast(Selector selector, bool displayed = false) => TextsFast(selector.By, selector.FrameBy, displayed);

        public List<string> TextsFast(string scss, By frameBy = null, bool displayed = false) => TextsFast(ScssBuilder.CreateBy(scss), frameBy, displayed);

        public List<string> TextsFast(By by, By frameBy = null, bool displayed = false) => TextsFast(Driver, @by, frameBy, displayed);

        public List<string> TextsFast(ISearchContext context, By by, By frameBy = null, bool displayed = false) {
            return RepeatAfterStale(
                () =>
                    {
                        var elements = Browser.Find.ElementsFast(context, by, frameBy);
                        if (displayed)
                            elements = elements.Where(e => e.Displayed).ToList();
                        return elements.Select(e => e.Text).ToList();
                    });
        }

        public List<string> Ids(string scssSelector, bool displayed = false)
        {
            return Ids(Driver, ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public List<string> Ids(By by, bool displayed = false)
        {
            return Ids(Driver, by, displayed);
        }

        public List<string> Ids(ISearchContext context, By by, bool displayed = false)
        {
            return RepeatAfterStale(
                () =>
                    {
                        var elements = Browser.Find.Elements(context, by);
                        if (displayed)
                            elements = elements.Where(e => e.Displayed).ToList();
                        return elements.Select(e => e.Text).ToList();
                    });
        }

        /// <summary>
        ///     Получить атрибут src тега img
        /// </summary>
        public string ImgFileName(string scssSelector)
        {
            return ImgFileName(ScssBuilder.CreateBy(scssSelector));
        }

        public string ImgFileName(By by)
        {
            return ImgSrc(by).Split('/').Last();
        }

        /// <summary>
        ///     Получить атрибут src тега img
        /// </summary>
        public string ImgSrc(string scssSelector)
        {
            return ImgSrc(ScssBuilder.CreateBy(scssSelector));
        }

        public string ImgSrc(By by)
        {
            return Attr(by, "src");
        }

        /// <summary>
        ///     Получить список атрибутов src тегов img
        /// </summary>
        public List<string> ImgSrcs(string scssSelector)
        {
            return ImgSrcs(ScssBuilder.CreateBy(scssSelector));
        }

        public List<string> ImgSrcs(By by)
        {
            return Attrs(by, "src");
        }

        /// <summary>
        ///     Получить аттрибут элемента
        /// </summary>
        /// <param name="scssSelector">селектор элемента</param>
        /// <param name="name">имя аттрибута</param>
        /// <param name="displayed">искать только видимые элементы</param>
        public string Attr(string scssSelector, string name, bool displayed = true) => Attr(ScssBuilder.CreateBy(scssSelector), null, name, displayed);

        public string Attr(By by, string name, bool displayed = true) => Attr(@by, null, name, displayed);

        public string Attr(By by, By frameBy, string name, bool displayed = true) => RepeatAfterStale(() => Attr(Browser.Find.Element(@by, frameBy, displayed), name));

        public T Attr<T>(Selector selector, string name, bool displayed = true) => Attr<T>(selector.By, selector.FrameBy, name, displayed);

        public T Attr<T>(string scssSelector, string name, bool displayed = true) => Attr<T>(ScssBuilder.CreateBy(scssSelector), name, displayed);

        public T Attr<T>(By by, string name, bool displayed = true) => Attr<T>(@by, null, name, displayed);

        public T Attr<T>(By by, By frameBy, string name, bool displayed = true) => Cast<T>(Attr(@by, frameBy, name, displayed));

        public T Attr<T>(IWebElement element, string name) => Cast<T>(Attr(element, name));

        public string Attr(IWebElement element, string name) => element.GetAttribute(name);

        /// <summary>
        ///     Получить аттрибуты элементов
        /// </summary>
        /// <param name="scssSelector">селектор элемента</param>
        /// <param name="name">имя аттрибута</param>
        public List<string> Attrs(string scssSelector, string name)
        {
            return Attrs(ScssBuilder.CreateBy(scssSelector), name);
        }

        public List<string> Attrs(By by, string name)
        {
            return RepeatAfterStale(
                () => Browser.Find.Elements(by).Select(e => Attr(e, name)).ToList());
        }

        /// <summary>
        ///     Получить атрибут href тега a
        /// </summary>
        public string Href(string scssSelector)
        {
            return Href(ScssBuilder.CreateBy(scssSelector));
        }

        public string Href(By by)
        {
            return Attr(by, "href");
        }

        /// <summary>
        ///     Получить список атрибутов href тегов a
        /// </summary>
        public List<string> Hrefs(string scssSelector)
        {
            return Hrefs(ScssBuilder.CreateBy(scssSelector));
        }

        public List<string> Hrefs(By by)
        {
            return Attrs(by, "href");
        }

        /// <summary>
        ///     Получить список атрибутов указанного типа
        /// </summary>
        public List<T> Attrs<T>(string scssSelector, string name)
        {
            return Attrs<T>(ScssBuilder.CreateBy(scssSelector), name);
        }

        public List<T> Attrs<T>(By by, string name)
        {
            return RepeatAfterStale(
                () => Enumerable.ToList(Browser.Find.Elements(@by).Select(e => Attr(e, name)).Select(Cast<T>)));
        }

        private T Cast<T>(string value)
        {
            var type = typeof(T);
            if (type == typeof(short)
                || type == typeof(int)
                || type == typeof(long))
                return (T)Convert.ChangeType(value.FindInt(), typeof(T));
            if (type == typeof(ushort)
                || type == typeof(uint)
                || type == typeof(ulong))
                return (T)Convert.ChangeType(value.FindUInt(), typeof(T));
            if (type == typeof(decimal)
                || type == typeof(float))
                return (T)Convert.ChangeType(value.FindNumber(), typeof(T));
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        ///     Получить содержимое поля ввода
        /// </summary>
        public string InputValue(By by, By frameBy, bool displayed = true) => InputValue<string>(by, frameBy, displayed);

        public T InputValue<T>(string scssSelector, bool displayed = true) => InputValue<T>(ScssBuilder.CreateBy(scssSelector), null, displayed);

        public T InputValue<T>(By by, By frameBy, bool displayed = true) {
            var value = Attr(by, frameBy, "value", displayed);
            return Cast<T>(value);
        }

        public T InputValue<T>(IWebElement element, bool displayed = true) {
            var value = Attr(element, "value");
            return Cast<T>(value);
        }

        /// <summary>
        ///     Получить целое значение из элемента найденного по селектору
        /// </summary>
        public int Int(string scssSelector, bool displayed = false) {
            return Int(ScssBuilder.CreateBy(scssSelector), null, displayed);
        }

        public int Int(By by, By frameBy, bool displayed = false)
        {
            return Int(Driver, by, frameBy, displayed);
        }

        public int Int(ISearchContext context, By by, By frameBy, bool displayed = false)
        {
            return Text(context, by, frameBy, displayed).AsInt();
        }

        /// <summary>
        ///     Получить целые значения из элементов, найденных по селектору
        /// </summary>
        public List<int> Ints(string scssSelector)
        {
            return Ints(ScssBuilder.CreateBy(scssSelector));
        }

        public List<int> Ints(By by)
        {
            return Texts(by).Select(s => s.AsInt()).ToList();
        }

        /// <summary>
        ///     Получить css параметр тега
        /// </summary>
        public T CssValue<T>(string scssSelector, ECssProperty property)
        {
            return CssValue<T>(ScssBuilder.CreateBy(scssSelector), property);
        }

        public T CssValue<T>(By by, ECssProperty property)
        {
            return RepeatAfterStale(() => CssValue<T>(Browser.Find.Element(by), property));
        }

        public T CssValue<T>(IWebElement element, ECssProperty property)
        {
            var value = element.GetCssValue(property.StringValue());
            return Cast<T>(value);
        }

        /// <summary>
        /// Get computed value(using window.getComputedStyle()) of specified CSS property
        /// </summary>
        public T ComputedCssValue<T>(By by,By frameBy, ECssProperty property) => RepeatAfterStale(() => ComputedCssValue<T>(Browser.Find.Element(by,frameBy), property));

        public T ComputedCssValue<T>(IWebElement element, ECssProperty property) {
            var value = Browser.Js.GetComputedStyle(element,property.StringValue());
            return Cast<T>(value);
        }

        /// <summary>
        ///     Получить количество элементов, найденных по указанному селектору
        /// </summary>
        public int Count(string scssSelector, By frameBy)
        {
            return Count(ScssBuilder.CreateBy(scssSelector), frameBy);
        }

        public int Count(By by, By frameBy)
        {
			return RepeatAfterStale(() => Browser.Find.Elements(by, frameBy).Count);
        }

        public object TakeScreenshot() {
            var screenshotDriver = Driver as ITakesScreenshot;
            var screenshot = screenshotDriver.GetScreenshot();
            return new Bitmap(new MemoryStream(screenshot.AsByteArray));
        }

        /// <summary>
        ///     Сделать скриншот указанной области экрана
        /// </summary>
        public object Screenshot()
        {            
            Browser.Js.ScrollToTop();
            // Get the Total Size of the Document
            var totalWidth = (int)Browser.Js.Excecute<long>("return document.documentElement.scrollWidth");
            var totalHeight = (int)Browser.Js.Excecute<long>("return document.documentElement.scrollHeight");

            // Get the Size of the Viewport
            var viewportWidth = (int)Browser.Js.Excecute<long>("return document.documentElement.clientWidth");
            var viewportHeight = (int)Browser.Js.Excecute<long>("return document.documentElement.clientHeight");

            // Split the Screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the Total Height is reached
            for (var i = 0; i < totalHeight; i += viewportHeight)
            {
                var newHeight = viewportHeight;
                // Fix if the Height of the Element is too big
                if (i + viewportHeight > totalHeight)
                {
                    newHeight = totalHeight - i;
                }
                // Loop until the Total Width is reached
                for (var ii = 0; ii < totalWidth; ii += viewportWidth)
                {
                    var newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (ii + viewportWidth > totalWidth)
                    {
                        newWidth = totalWidth - ii;
                    }

                    // Create and add the Rectangle
                    var currRect = new Rectangle(ii, i, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }

            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            var previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the Scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    var xDiff = rectangle.Right - previous.Right;
                    var yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    Browser.Js.Excecute($"window.scrollBy({xDiff}, {yDiff})");
                    Thread.Sleep(200);
                }

                // Take Screenshot
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();

                // Build an Image out of the Screenshot
                Image screenshotImage;
                using (var memStream = new MemoryStream(screenshot.AsByteArray))
                {
                    screenshotImage = Image.FromStream(memStream);
                }

                // Calculate the Source Rectangle
                var sourceRectangle = new Rectangle(viewportWidth - rectangle.Width,
                    viewportHeight - rectangle.Height, rectangle.Width,
                    rectangle.Height);

                // Copy the Image
                using (var g = Graphics.FromImage(stitchedImage))
                {
                    g.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }

                // Set the Previous Rectangle
                previous = rectangle;
            }
            // The full Screenshot is now in the Variable "stitchedImage"
            return stitchedImage;
        }

        /// <summary>
        ///     Получить прямоугольник, заданный Css свойствами элемента left, top, height, width
        /// </summary>
        public Rectangle Bounds(string scssSelector)
        {
            return Bounds(ScssBuilder.CreateBy(scssSelector));
        }

        public Rectangle Bounds(By by)
        {
            return RepeatAfterStale(() => Bounds(Browser.Find.Element(by)));
        }

        /// <summary>
        ///     Получить границы элемента
        /// </summary>
        public Rectangle Bounds(IWebElement element)
        {
            return new Rectangle(CssValue<int>(element, ECssProperty.left),
                CssValue<int>(element, ECssProperty.top),
                CssValue<int>(element, ECssProperty.width),
                CssValue<int>(element, ECssProperty.height));
        }

        /// <summary>
        ///     Получить координаты элемента
        /// </summary>
        public Point Point(IWebElement element)
        {
            return new Point(CssValue<int>(element, ECssProperty.left),
                CssValue<int>(element, ECssProperty.top));
        }

        /// <summary>
        ///     Получить значение указанного типа из найденного по селектору элементу
        /// </summary>
        public T Value<T>(string scssSelector)
        {
            return Value<T>(ScssBuilder.CreateBy(scssSelector));
        }

        public T Value<T>(By by, By frameBy=null)
        {
            return RepeatAfterStale(() => Value<T>(Browser.Find.Element(by, frameBy)));
        }

        public T Value<T>(IWebElement element)
        {
            return Cast<T>(element.Text);
        }

        /// <summary>
        ///     Получить список текстов по селектору и объединить их в строку
        /// </summary>
        public string TextsAsString(string scssSelector, string delimiter = " ")
        {
            return TextsAsString(ScssBuilder.CreateBy(scssSelector), delimiter);
        }

        public string TextsAsString(By by, string delimiter = " ")
        {
            return Texts(by).AsString(delimiter);
        }

        /// <summary>
        ///     Получить атрибут href и получить из него абсолютный Url(без домена)
        /// </summary>
        public string AbsoluteHref(By by)
        {
            return Href(by).CutBaseUrl();
        }

        public string LastDownloadedFileName(bool insecureFile = false) {
            var downloadedFile = Browser.Get.LastDownloadedFile(false, insecureFile);
            return downloadedFile.Name.EndsWith(".crdownload", StringComparison.Ordinal) ? Path.GetFileNameWithoutExtension(downloadedFile.FullName) : downloadedFile.Name;
        }

        /// <summary>
        ///     Returns FileSystemInfo of the last file downloaded in Browser
        /// </summary>
        public FileInfo LastDownloadedFile(bool waitUntilDownloaded = true, bool insecureFile=false) {
            FileInfo lastDownloadedFile = null;
            // Wait until file appear in the Downloads folder
            Browser.Wait.Until(() =>
                {
                    if (Directory.Exists(Browser.Settings.DownloadDirectory)) {
                        var files = new DirectoryInfo(Browser.Settings.DownloadDirectory).GetFiles();
                        lastDownloadedFile = files.OrderByDescending(f => f.CreationTime).FirstOrDefault();
                        if (lastDownloadedFile != null) {
                            return true;
                        }
                    }
                    return false;
                }, BrowserTimeouts.FILE_DOWNLOAD_START);
            if (waitUntilDownloaded) {
                const int NO_CHANGE_INTERVAL = 1000;
                long curLength = lastDownloadedFile.Length;
                Thread.Sleep(NO_CHANGE_INTERVAL);
                Browser.Wait.Until(() =>
                    {
                        if (lastDownloadedFile.Length == curLength) {
                            return true;
                        }
                        curLength = lastDownloadedFile.Length;
                        return false;
                    }, BrowserTimeouts.FILE_DOWNLOAD, NO_CHANGE_INTERVAL);
            }
            return lastDownloadedFile;
        }

        public string ChecksumOfLastDownloadedFile(bool waitUntilDownloaded = true)
        {
            var lastDownloadedFile = LastDownloadedFile(waitUntilDownloaded);
            string checksum;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(lastDownloadedFile.FullName))
                {
                    var computeHash = md5.ComputeHash(stream);
                    checksum = Convert.ToBase64String(computeHash);
                }
            }
            return checksum;
        }

        public List<LogEntry> JsErrors() {
            var errorStrings = new List<string>
            {
                "SyntaxError",
                "EvalError",
                "ReferenceError",
                "RangeError",
                "TypeError",
                "URIError"
            };
            return Driver.Manage().Logs.GetLog(LogType.Browser).Where(x => errorStrings.Any(e => x.Message.Contains(e))).ToList();
        }
    }
}