namespace AutomateIt.Framework.Service
{
    using System.Collections.Generic;
    using AutomateIt.Configs.Models;

    public interface IServiceFactoryable
    {
        // Создать маршрутизатор страниц(сопоставление Url-->Страницы) для сервиса
        IRoutable CreateRouter();
		// Создать сервис
		IService CreateService(ServiceConfig config);
        // Паттерн для Url, которым соответствует сервис
        BaseUrlPattern CreateBaseUrlPattern(List<string> serverHosts);
        // Дефолтные параметры базового Url
        BaseUrlInfo GetDefaultBaseUrlInfo(ServerConfig defaultServer);
    }
}