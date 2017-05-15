using System;
using System.Collections.Generic;

namespace FileGenLoaderDomain
{
    [Serializable]
    public class Config
    {
        public string PlugPath { get; set; }

        public List<PlugConfg> PlugConfigs { get; set; }

        public class PlugConfg
        {
            public string Exction { get; set; }
            public string DllName { get; set; }
            public string Type { get; set; }
        }

    }
}