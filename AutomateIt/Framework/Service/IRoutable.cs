using System;
using System.Collections.Generic;
using AutomateIt.Framework.Page;
using AutomateIt.Logging;

namespace AutomateIt.Framework.Service {
    public interface IRoutable 
    {
        RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);
        IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);
        bool HasPage(IPage page);
        IEnumerable<Type> GetAllPageTypes();
    }
}