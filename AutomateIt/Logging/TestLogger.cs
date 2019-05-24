namespace AutomateIt.Logging
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using OpenQA.Selenium;
    using Exceptions;

    public class TestLogger : ITestLogger
    {
        private const string PREFIX = "-----> ";
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        private static readonly NLog.ILogger _log;

        private int _actionIndex;

        private bool _enabled;

        static TestLogger()
        {
            // const string CONFIG_FILE_NAME = "nlog.config";
            // //            string buildCheckoutDir = Environment.GetEnvironmentVariable("BuildCheckoutDir");
            // //            string baseDirectory = string.IsNullOrEmpty(buildCheckoutDir)
            // //                ? AppDomain.CurrentDomain.BaseDirectory
            // //                : buildCheckoutDir;
            // string baseDirectory = "";  // TODO:  AppDomain.CurrentDomain.BaseDirectory=>System.AppContext.BaseDirectory
            // string configFilePath = Path.Combine(baseDirectory, CONFIG_FILE_NAME);
            // LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);
            // Log = LogManager.GetLogger("TestLogger");
            
            //Set up DI
            IContainer _container = BuildContainer();
            ConfigureServices(_container);
            
        }

        /// <summary>
        /// Constructs an application container 
        /// </summary>
        /// <returns>An application container populated with desired types and services</returns>
        static IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //Register logging services for resolution
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            builder.Populate(services);

            return builder.Build();
        }

        /// <summary>
        /// Perform any desired configuration on container services
        /// </summary>
        /// <param name="container">The application container to be configured</param>
        static void ConfigureServices(IContainer container)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                //Add NLog as a log consumer
                ILoggerFactory loggerFactory = scope.Resolve<ILoggerFactory>();
                loggerFactory.AddNLog(); //notice: the project's only line of code referencing NLog (aside from .config)
            }
        }
        
        #region TestLogger Members

        public void Action(string msg) {
            if (_enabled) {
                var formatedMsg = $"{PREFIX}{_actionIndex}. {msg}";
                _log.Info(formatedMsg);
                Console.WriteLine(formatedMsg);
                _actionIndex++;
            }
        }

        public void Info(string msg)
        {
            if (_enabled) {
                _log.Info(msg);
                Console.WriteLine(msg);
            }
        }

        public void Debug(string msg) {
            if (_enabled) {
                _log.Debug(msg);
                Console.WriteLine(msg);
            }
        }

        public void FatalError(string msg, Exception e) {
            if (_enabled) {
                _log.Error(e, msg);
                Console.WriteLine("FATAL ERROR: {0}, {1}", msg, e.Message);
            }
        }

        public void WriteValue(string key, object value)
        {
            if (!_values.ContainsKey(key))
                _values.Add(key, value);
            else
                _values[key] = value;
        }

        public T GetValue<T>(object key) => GetValue<T>(key.ToString());

        public T GetValue<T>(string key)
        {
            if (!_values.ContainsKey(key))
                Throw.TestException($"Value with key '{key}' was not logged");
            return (T)_values[key];
        }

        public void Selector(By by)
        {
            if (_enabled) {
                _log.Info("By: {0}", by);
                Console.WriteLine("By: {0}", by);
            }
        }

        public void Exception(Exception exception, string message=null) {
            if (_enabled) {
                if (message == null) {
                    _log.Error(exception);
                }
                else {
                    _log.Error(exception, message);
                    Console.WriteLine("ERROR: {0}", message);
                }
                Console.WriteLine("EXCEPTION: {0}", exception.Message);
            }
        }

        public void Warning(Exception exception, string message = null) {
            if (_enabled) {
                if (message == null) {
                    _log.Warn(exception);
                    Console.WriteLine($"WARN EXCEPTION: {exception.GetType().FullName} - {exception.Message}");
                }
                else {
                    _log.Warn(exception, message);
                    Console.WriteLine($"WARN EXCEPTION: {exception.GetType().FullName} - {exception.Message}");
                    Console.WriteLine($"WARN: {message}");
                }
            }
        }

        public void Error(string error) {
            if (_enabled) {
                _log.Error(error);
                Console.WriteLine("ERROR: {0}", error);
            }
        }

        /// <summary>
        /// Ignore all the log data passed to logger
        /// </summary>
        public void Disable() => _enabled = false;

        /// <summary>
        /// Save the log data passed to logger
        /// </summary>
        public void Enable() => _enabled = true;

        #endregion

        public void Reset() => _actionIndex = 0;

        public T Milliseconds<T>(string actionName, Func<T> func) {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = func.Invoke();
            watch.Stop();
            _log.Info($"{PREFIX}[ Action '{actionName}' has taken {watch.ElapsedMilliseconds} milliseconds. ]");
            Console.WriteLine($"{PREFIX}[ Action '{actionName}' has taken {watch.ElapsedMilliseconds} milliseconds. ]");
            return result;
        }

        public void Milliseconds(string actionName, Action action) {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action.Invoke();
            watch.Stop();
            _log.Info($"{PREFIX}[ Action '{actionName}' has taken {watch.ElapsedMilliseconds} milliseconds. ]");
            Console.WriteLine($"{PREFIX}[ Action '{actionName}' has taken {watch.ElapsedMilliseconds} milliseconds. ]");
        }
    }
}