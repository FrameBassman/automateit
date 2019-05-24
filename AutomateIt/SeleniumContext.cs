using System;

using AutomateIt.Framework.Browser;
using AutomateIt.Framework.Service;
using AutomateIt.Logging;

namespace AutomateIt
{
    public class SeleniumContext<T> : ISeleniumContext where T : ISeleniumContext
    {
        private readonly BrowsersCache _browsersCache;

        protected SeleniumContext()
        {
            this.Log = new TestLogger();
            this.Web = new Web();
            this._browsersCache = new BrowsersCache(this.Web, this.Log);
        }

        public static T Inst
        {
            get
            {
                return SingletonCreator<T>.CreatorInstance;
            }
        }

        protected virtual void InitWeb()
        {
        }

        public virtual void Init()
        {
            this.InitWeb();
        }

        public void Destroy()
        {
            Inst.Browser.Destroy();
        }

        private sealed class SingletonCreator<S> where S : ISeleniumContext
        {
            public static S CreatorInstance { get; } = (S)Activator.CreateInstance(typeof(S));
        }

        public Web Web { get; }

        public ITestLogger Log { get; }

        public Browser Browser => this._browsersCache.GetBrowser(BrowserType.CHROME);
    }
}
