namespace AutomateIt.Extensions.Extensions
{
    using System;

    public static class UrlExtentions
    {
        public static string CutBaseUrl(this string s)
        {
            var uri = new Uri(s);
            return uri.AbsolutePath + uri.Query + uri.Fragment;
        }
    }
}