using System;
using System.IO;

namespace AutomateIt.Configs 
{
    public class ProvidePath {
        public static string ForConfig(string configFileName) => InOutputFolder(Path.Combine("configuration", configFileName));

        public static string InOutputFolder(string relativePath) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
    }
}
