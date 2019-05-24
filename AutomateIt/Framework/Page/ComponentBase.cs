using System;
using System.Text.RegularExpressions;
using AutomateIt.Exceptions;
using AutomateIt.Framework.Browser;
using AutomateIt.Logging;
using AutomateIt.SmartSelector;
using Xunit;
using OpenQA.Selenium;

namespace AutomateIt.Framework.Page
{
	public abstract class ComponentBase : IComponent {
        private string _componentName;

        protected ComponentBase(IPage parent) {
            ParentPage = parent;
        }

        #region IComponent Members

        public virtual string ComponentName {
            get {
                _componentName = _componentName ?? (_componentName = GetType().Name);
                return _componentName;
            }
            set { _componentName = value; }
        }

        public string FrameScss { get; set; }

        public By FrameBy => FrameScss != null ? ScssBuilder.CreateBy(FrameScss) : null;

        public IPage ParentPage { get; }
        public abstract bool IsVisible();
        public abstract bool IsNotVisible();
        public abstract bool HasClass(string className);
        public abstract bool IsDisabled();

        public virtual string Text { get; }

        public Browser.Browser Browser => ParentPage.Browser;

        public ITestLogger Log => ParentPage.Log;

        public BrowserAction Action => Browser.Action;

        public BrowserAlert Alert => Browser.Alert;

        public BrowserFind Find => Browser.Find;

        public BrowserGet Get => Browser.Get;

        public BrowserGo Go => Browser.Go;

        public BrowserIs Is => Browser.Is;

        public BrowserState State => Browser.State;

        public BrowserWait Wait => Browser.Wait;

        public BrowserJs Js => Browser.Js;

        public BrowserWindow Window => Browser.Window;

        BrowserCookies IPageObject.Cookies => Browser.Cookies;

        #endregion

        //*****     ACTION     **********************************************************************************************************************
        public T Open<T>(Action action)
            where T : ComponentBase {
            return (T)Open(action);
        }

        public virtual ComponentBase Open(Action action) {
            if (!IsVisible()) {
                Log.Action($"Open component '{ComponentName}'");
                action.Invoke();
                try {
                    Wait.Until(IsVisible);
                }
                catch (WebDriverTimeoutException) {
                    Throw.FrameworkException("Component '{0}' is not opened", ComponentName);
                }
            }
            return this;
        }

        public void WaitForVisible(int timeout = 3) {
            Wait.Until(IsVisible, timeout);
        }

        public void WaitForNotVisible(int timeout = 3) {
            Wait.Until(IsNotVisible, timeout);
        }

        //*****     IS     **************************************************************************************************************************
        //*****     GET     *************************************************************************************************************************
        public virtual string GetValue() => Text;

        public abstract T GetValue<T>();

        //*****     ASSERT     **********************************************************************************************************************
        public virtual void AssertContains(string expected, bool ignoreRegister = false) {
            var text = ignoreRegister ? Text.ToLower().Trim() : Text;
            expected = ignoreRegister ? expected.ToLower().Trim() : expected;
            Assert.Contains(text, expected, StringComparison.Ordinal);
        }

        public void AssertMatch(string expectedPattern) {
            var regex = new Regex(expectedPattern);
            Log.Info(Text);
            Assert.True(regex.IsMatch(Text), $"Text in component {ComponentName} doesn't match pattern '{expectedPattern}'. " +
                                               $"Actual value is '{Text}'");
        }

        public void AssertDisabled() => Assert.True(IsDisabled(), $"'{ComponentName}' is enabled.");

        public void AssertEnabled() => Assert.True(IsDisabled(), $"'{ComponentName}' is disabled.");

        public void AssertVisible() {
            Assert.True(IsVisible(), $"'{ComponentName}' is not displayed");
        }

        public void AssertNotExist() {
            Assert.False(Browser.Is.Exists(FrameBy), $"'{ComponentName}' is existed");
        }

        public void AssertNotVisible() {
            Assert.False(IsVisible(), $"'{ComponentName}' is displayed");
        }

        public void AssertNoExist() {
            Assert.False(Browser.Is.Exists(FrameBy), $"'{ComponentName}' is existed");
        }

        public void AssertNotEqual(string expected, bool ignoreRegister = false) {
            var text = ignoreRegister ? Text.ToLower() : Text;
            expected = ignoreRegister ? expected.ToLower() : expected;
            Assert.NotEqual(expected, text.Replace("'", ""));
        }

        public virtual void AssertEqual(string expected, bool ignoreRegister = false) {
            var value = GetValue();
            if (ignoreRegister) {
                value = value.ToLower();
                expected = expected.ToLower();
            }
            Assert.Equal(expected.Trim(), value.Trim());
        }

        public abstract void Click(int sleepTimeout = 0);
    }
}