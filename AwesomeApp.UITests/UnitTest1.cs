using System;
using AutomateIt.Framework.Browser;
using AutomateIt.Framework.Interfaces;
using AutomateIt.Framework.Service;
using AutomateIt.Logging;
using OpenQA.Selenium;
using Xunit;

namespace AwesomeApp.UITests
{
    public class UnitTest1 : IDisposable
    {
        private readonly IDriverManager _manager;
        private readonly Browser _browser;

        public UnitTest1()
        {
            _manager = new ChromeDriverFactory(new BrowserSettings());
            _browser = new Browser(new Web(),  new TestLogger(), _manager);
        }

        public void Dispose()
        {
            _manager.Dispose();
        }
        
        [Fact]
        public void Test1()
        {
            _browser.Go.ToUrl("http://ya.ru");
            _browser.Action.TypeIn(".search2__input", null, "Путин", false);
            _browser.Action.PressEnter(".search2__button");
        }
        
        
    }
}