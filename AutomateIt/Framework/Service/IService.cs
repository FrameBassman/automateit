using System;
using AutomateIt.Framework.Page;

namespace AutomateIt.Framework.Service
{
    public interface IService
    {
        // Роутер сервиса
        IRoutable Router { get; }
        // Получить паттерн, определяющий каким Url соответствует сервис
        BaseUrlPattern BaseUrlPattern { get; }
        // Получить параметры BaseUrl по умолчанию
        BaseUrlInfo DefaultBaseUrlInfo { get; }
        // Получить экземпляр класса страницы для указанного запроса
        IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);
        IPage GetEmailPage(Uri uri);
        // Получить запрос для указанного экземпляра класса страницы
        RequestData GetRequestData(IPage page);
    }
}