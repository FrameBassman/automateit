using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutomateIt.Framework.Page;

namespace AutomateIt.Framework.Service {
    public class SelfMatchingPagesRouter : RouterBase {
        private readonly Dictionary<Type, ISelfMatchingPage> _pages;
        private List<ISelfMatchingPage> _sortedPages;

        public SelfMatchingPagesRouter() {
            _pages = new Dictionary<Type, ISelfMatchingPage>();
        }

        public override RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo) {
            var selfMatchingPage = page as SelfMatchingPageBase;
            if (selfMatchingPage == null)
                return null;
            return selfMatchingPage.GetRequest(defaultBaseUrlInfo);
        }

        public override IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo) {
            foreach (var dummyPage in _pages.Values) {
                UriMatchResult match = dummyPage.Match(requestData, baseUrlInfo);
                if (match.Success) {
                    var instance = (SelfMatchingPageBase) Activator.CreateInstance(dummyPage.GetType());
                    instance.BaseUrlInfo = baseUrlInfo;
                    instance.Data = match.Data;
                    instance.Params = match.Params;
                    instance.Cookies = match.Cookies;
                    return instance;
                }
            }
            return null;
        }
        
        public List<ISelfMatchingPage> GetSortedPages()
        {
            if (_sortedPages == null)
            {
                _sortedPages = _pages.Values.ToList();
                _sortedPages.Sort(ComparePages);
                _sortedPages.Reverse();
            }
            return _sortedPages;
        }

        private int ComparePages(ISelfMatchingPage page1, ISelfMatchingPage page2) => page1.Compare(page2);
        
        public override bool HasPage(IPage page) {
            return _pages.ContainsKey(page.GetType());
        }

        public override IEnumerable<Type> GetAllPageTypes() => _pages.Keys;

//        public void RegisterDerivedPages<T>() where T : SelfMatchingPageBase {
//            Type superType = typeof (T);
//            Assembly assembly = superType.GetTypeInfo().Assembly;
//            IEnumerable<Type> derivedTypes =
//                assembly.DefinedTypes.AsEnumerable().Where(t => !t.GetTypeInfo().IsAbstract && superType.IsAssignableFrom(t));
//            foreach (Type derivedType in derivedTypes)
//                RegisterPage(derivedType);
//        }

        public void RegisterPage<T>() {
            RegisterPage(typeof (T));
        }

        private void RegisterPage(Type pageType) {
            var pageInstance = (ISelfMatchingPage) Activator.CreateInstance(pageType);
            _pages.Add(pageType, pageInstance);
        }
    }
}