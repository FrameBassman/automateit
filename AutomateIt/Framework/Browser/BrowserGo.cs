using System;
using System.IO;
using System.Web;
using AutomateIt.Framework.Page;

namespace AutomateIt.Framework.Browser
{
    using Microsoft.AspNetCore.WebUtilities;

    using Service;
    using OpenQA.Selenium;


    public class BrowserGo : DriverFacade
    {
        public BrowserGo(Browser browser)
            : base(browser)
        {
        }

        // Определение Url, соответствующее классу страницы и переход на него
        public T ToPage<T>() where T :class, IPage
        {
            var pageInstance = (T)Activator.CreateInstance(typeof(T));
            ToPage(pageInstance);
            return Browser.State.PageAs<T>();
        }

        // Определение Url, соответствующее классу страницы и переход на него
        public void ToPage(IPage page)
        {
            var requestData = Web.GetRequestData(page);
            ToUrl(requestData);
            var logEntries = Browser.Get.JsErrors();
            foreach (var logEntry in logEntries) {
                Log.Error($"{logEntry.Timestamp}: {logEntry.Level} - {logEntry.Message}");
            }
        }

        public void ToUrl(string url)
        {
            ToUrl(new RequestData(url));
        }

        // Переход на указанный Url в текущем окне браузера
        public void ToUrl(RequestData requestData) {
            if (requestData.HasBasicAuth()) {
                Log.Action($"Navigating to url: {requestData.Url} with Basic Authentication");
                string basicAuthDomain =
                    $"{HttpUtility.UrlEncode(requestData.BasicAuthLogin)}:{HttpUtility.UrlEncode(requestData.BasicAuthPassword)}@{requestData.Url.Host}";
                string url = $"{requestData.Url.Scheme}://{basicAuthDomain}{requestData.Url.PathAndQuery}";
                Driver.Navigate().GoToUrl(url);
            }
            else {
                Log.Action($"Navigating to url: {requestData.Url}");
                Driver.Navigate().GoToUrl(requestData.Url);
            }
            Browser.State.Actualize();
            Browser.Wait.WhileAjax(ajaxInevitable: true);
        }

        /// <summary>
        ///     Сохранить исходный код страницы на диске и открыть страницу в браузере
        /// </summary>
        /// <typeparam name="T">Класс страницы</typeparam>
        /// <param name="html">Исходный код страницы</param>
        public T ToHtml<T>(string html) where T : IPage {
            // Сохраниить на диск
            var type = typeof(T);
            var fileName = type.Name + ".html";
            var pagesFolder = Path.Combine(Environment.CurrentDirectory, "SavedPages");
            if (!Directory.Exists(pagesFolder))
                Directory.CreateDirectory(pagesFolder);
            var filePath = Path.Combine(pagesFolder, fileName);
            File.WriteAllText(filePath, html);

            // Открыть в браузере
            ToUrl("file://" + filePath);

            // Создать соответствующий класс страницы
            var page = (T)Activator.CreateInstance(type);
            page.Activate(Browser, Log);
            Browser.ApplyPageOptions(page);
            return page;
        }

        /// <summary>
        ///     Найти письмо с указанным заголовком на указанном почтовом ящике.
        ///     Открыть текст письма в браузере
        /// </summary>
        /// <summary>
        ///     Вернуться на предыдущую страницу
        /// </summary>
        public void Back()
        {
            Driver.Navigate().Back();
            Log.Action($"Go.Back(). Result Url: {Driver.Url}");
            Browser.State.Actualize();
        }

        public void Refresh()
        {
            Log.Action($"Refresh page {Driver.Url}");
            Driver.Navigate().Refresh();
            Browser.State.Actualize();
        }
    }
}
