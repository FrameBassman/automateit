namespace AutomateIt.Framework.Service
{
    using System;
    using System.Collections.Generic;

    using OpenQA.Selenium;

    public class RequestData
    {
        public RequestData(Uri url)
            : this(url, new List<Cookie>())
        {
        }

        public RequestData(string url)
            : this(url, new List<Cookie>())
        {
        }

        public RequestData(Uri url, List<Cookie> cookies)
        {
            Url = url;
            Cookies = cookies;
        }

        public RequestData(string url, List<Cookie> cookies)
            : this(new Uri(url), cookies)
        {
        }

        public Uri Url { get; private set; }
        public List<Cookie> Cookies { get; private set; }

        public string BasicAuthLogin { get; set; }
        public string BasicAuthPassword { get; set; }

        public bool HasBasicAuth()
        {
            return !string.IsNullOrEmpty(BasicAuthLogin);
        }
    }
}
