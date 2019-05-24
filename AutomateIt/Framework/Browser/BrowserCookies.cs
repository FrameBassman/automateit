using OpenQA.Selenium;

namespace AutomateIt.Framework.Browser
{
    public class BrowserCookies : DriverFacade
    {
        public BrowserCookies(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     �������� ��� Cookie
        /// </summary>
        public void Clear()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
        }

        public void Set(string name, string value)
        {
            Driver.Manage().Cookies.AddCookie(new Cookie(name, value));
        }
    }
}