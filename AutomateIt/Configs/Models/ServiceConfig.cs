using System;
using System.Collections.Generic;
using AutomateIt.Configs.Enums;
using AutomateIt.Exceptions;

namespace AutomateIt.Configs.Models {
    public class ServiceConfig {
        public string Id;
        public string DefaultServer;
        public AccountConfig DefaultIWAAccount;
        public AccountConfig DefaultFormsAccount;
        public Dictionary<string, ServerConfig> Servers;

        public void Activate() {
            if (string.IsNullOrEmpty(DefaultServer)) {
                Throw.FrameworkException("DefaultServer is not defined in config file");
            }
            if (!Servers.ContainsKey(DefaultServer)) {
                Throw.FrameworkException($"Invalid DefaultServer property. Server '{DefaultServer}' is not defined in config file.");
            }
            foreach (var serverId in Servers.Keys) {
                Servers[serverId].Id = serverId;
                if (Servers[serverId].IWAAccount == null)
                    Servers[serverId].IWAAccount = DefaultIWAAccount;
                if (Servers[serverId].FormsAccount == null)
                    Servers[serverId].FormsAccount = DefaultFormsAccount;
            }
        }

        public ServerConfig GetServer(string serverId) {
            if (!Servers.ContainsKey(serverId))
                Throw.FrameworkException($"Server '{serverId}' is not defined in config file.");
            return Servers[serverId];
        }

        public AccountConfig GetDefaultAccount(AuthType authType) {
            switch (authType) {
                case AuthType.IWA:
                    return DefaultIWAAccount;
                case AuthType.Forms:
                    return DefaultFormsAccount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authType), authType, null);
            }
        }

        public ServerConfig GetDefaultServer() => Servers[DefaultServer];
    }
}