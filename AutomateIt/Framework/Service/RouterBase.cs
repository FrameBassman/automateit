using System;
using System.Collections.Generic;
using AutomateIt.Framework.Page;
using AutomateIt.Logging;

namespace AutomateIt.Framework.Service {
    public abstract class RouterBase : IRoutable 
    {
        public abstract RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);
        public abstract IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);
        public abstract bool HasPage(IPage page);
        public abstract IEnumerable<Type> GetAllPageTypes();
    }
}