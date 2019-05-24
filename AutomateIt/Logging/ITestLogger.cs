namespace AutomateIt.Logging
{
    using System;

    using OpenQA.Selenium;

    public interface ITestLogger {
        /// <summary>
        ///     Залогировать действие
        /// </summary>
        void Action(string msg);

        /// <summary>
        ///     Залогировать информационное сообщение
        /// </summary>
        void Info(string msg);

        void Debug(string message);

        /// <summary>
        ///     Залогировать критическуюж ошибку
        /// </summary>
        void FatalError(string s, Exception e);

        /// <summary>
        ///     Сохранить значение в лог
        /// </summary>
        void WriteValue(string key, object value);

        /// <summary>
        ///     Прочитать сохраненное ранее значение из логи
        /// </summary>
        T GetValue<T>(string key);

        T GetValue<T>(object key);

        /// <summary>
        ///     Залогировать селектор
        /// </summary>
        void Selector(By by);

        /// <summary>
        ///     Залогировать исключение
        /// </summary>
        void Exception(Exception exception, string message = null);

        void Warning(Exception exception, string message = null);

        /// <summary>
        /// Ignore all the log data passed to logger
        /// </summary>
        void Disable();

        /// <summary>
        /// Save the log data passed to logger
        /// </summary>
        void Enable();

        void Error(string error);

        void Reset();

        T Milliseconds<T>(string actionName, Func<T> func);

        void Milliseconds(string actionName, Action action);
    }
}
