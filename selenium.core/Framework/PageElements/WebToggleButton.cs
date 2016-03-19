using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using selenium.core.Framework.Page;
using selenium.core.SCSS;

namespace selenium.core.Framework.PageElements {
    public class WebToggleButton : WebButton {
        private readonly string _selectedClass;

        public WebToggleButton(IPage parent, string scssSelector, string selectedClass)
            : base(parent, ScssBuilder.CreateBy(scssSelector)) {
            _selectedClass = selectedClass;
        }

        public bool IsActive() {
            return Is.HasClass(By, _selectedClass);
        }

        public void SelectAndWaitWhileAjax(int sleepTimeout = 0, bool ajaxInevitable = false) {
            if (!IsActive())
                ClickAndWaitWhileAjax(sleepTimeout, ajaxInevitable);
        }

        public void Select(int sleepTimeout=0) {
            if (!IsActive())
                Click(sleepTimeout);
        }

        public void AssertIsSelected() {
            Assert.IsTrue(IsActive(), "'{0}' is not selected", ComponentName);
        }
    }
}