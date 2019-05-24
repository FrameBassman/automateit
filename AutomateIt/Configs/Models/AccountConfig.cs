using System;

namespace AutomateIt.Configs.Models
{
    public class AccountConfig
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public string NetworkLogin {
            get {
                if (string.IsNullOrEmpty(Domain))
                    throw new InvalidOperationException($"Account {Id} can not be used as IWA account. Property 'Domain' is not initialized.");
                return $"{Domain}\\{Login}";
            }
        }

        public AccountConfig(string id, string login, string password, string domain) {
            Id = id;
            Login = login;
            Password = password;
            Domain = domain;
        }
    }
}
