using System;
using AutomateIt.Configs.Enums;

namespace AutomateIt.Configs.Models
{
    public class ServerConfig {
        public string Id;
        public string ConnectionString;
        public AuthType AuthType;
        public string Host;
        public AccountConfig FormsAccount;
        public AccountConfig IWAAccount;
        public string Version;

        public AccountConfig GetAuthAccount() {
            switch (AuthType) {
                case AuthType.IWA:
                    return IWAAccount;
                case AuthType.Forms:
                    return FormsAccount;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Version GetVersion() {
            return new Version(Version);
        }
    }
}
