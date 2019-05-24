using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomateIt.Configs.Models;
using Newtonsoft.Json;

namespace AutomateIt.Configs 
{
    public class LoadConfigProvider : IServiceConfigProvider {
        private readonly string _configPath;

        public LoadConfigProvider(string configPath) {
            _configPath = configPath;
        }

        private List<ServiceConfig> GetAllServiceConfigs() {
            var json = File.ReadAllText(_configPath);
            var serviceConfigs = JsonConvert.DeserializeObject<List<ServiceConfig>>(json);
            foreach (var serviceConfig in serviceConfigs) {
                serviceConfig.Activate();
            }
            return serviceConfigs;
        }

        public ServiceConfig GetServiceConfig(string serviceName) => GetAllServiceConfigs().Single(s => s.Id == serviceName);
    }

    public interface IServiceConfigProvider {
        ServiceConfig GetServiceConfig(string serviceName);
    }
}