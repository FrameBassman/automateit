using AutomateIt.Framework.Page;

namespace AutomateIt.Framework.Service 
{
    public interface RequestAction 
    {
        IPage getPage(RequestData requestData, BaseUrlInfo baseUrlInfo);
        RequestData getRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);
    }
}