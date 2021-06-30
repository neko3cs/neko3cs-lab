using System;
using System.Collections.Generic;

namespace CsxWebAPI.Models
{
    public class AppSettings
    {
        private static AppSettings instance;
        public static AppSettings Instance
        {
            get
            {
                if (instance is null) { instance = new AppSettings(); }
                return instance;
            }
            set { instance = value; }
        }

        //public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Dictionary<string, string> ScriptPaths { get; set; }
    }

    public class Logging
    {
        public Dictionary<string, string> LogLevel { get; set; }
    }
}
