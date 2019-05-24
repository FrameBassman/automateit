using System;
using AutomateIt.Framework.Service;

namespace AutomateIt.Framework.Page {

public abstract class SelfMatchingPageBase : PageBase, ISelfMatchingPage {
        public abstract string AbsolutePath { get; }

        #region ISelfMatchingPage Members

        public virtual UriMatchResult Match(RequestData requestData, BaseUrlInfo baseUrlInfo) {
            return new UriMatcher(AbsolutePath, Data, Params).Match(requestData.Url, baseUrlInfo.AbsolutePath);
        }

    public int Compare(object page)
    {
        var selfMatchingPage = page as SelfMatchingPageBase;
        if (selfMatchingPage == null)
        {
            throw new ArgumentException("Invalid page type");
        }
        var compareResult =string.CompareOrdinal(AbsolutePath, selfMatchingPage.AbsolutePath);
        if (compareResult != 0)
        {
            return compareResult;
        }
        return Params.Count.CompareTo(selfMatchingPage.Params.Count);
    }

    #endregion

        public virtual RequestData GetRequest(BaseUrlInfo defaultBaseUrlInfo) {
            string url = new UriAssembler(BaseUrlInfo, AbsolutePath, Data, Params).Assemble(defaultBaseUrlInfo);
            return new RequestData(url);
        }
    }
}