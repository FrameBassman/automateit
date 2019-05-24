namespace AutomateIt.Framework.Interfaces
{
    using System;
    using AutomateIt.Framework.Browser;
    using OpenQA.Selenium;

    public interface IDriverManager : IDisposable
    {
        BrowserSettings Settings { get; }
        void InitDriver();
        IWebDriver Driver { get; }
    }
}
