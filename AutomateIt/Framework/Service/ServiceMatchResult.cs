namespace AutomateIt.Framework.Service
{
    public class ServiceMatchResult
    {
        public BaseUrlInfo BaseUrlInfo { get;  }
        public IService Service { get; }

        public ServiceMatchResult(IService service, BaseUrlInfo baseUrlInfo)
        {
            Service = service;
            BaseUrlInfo = baseUrlInfo;
        }
    }
}