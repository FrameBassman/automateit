namespace AutomateIt.Framework.Page {
    using AutomateIt.Framework.Service;

    public interface ISelfMatchingPage
    {
        UriMatchResult Match(RequestData requestData, BaseUrlInfo baseUrlInfo);

        int Compare(object page);
    }
}