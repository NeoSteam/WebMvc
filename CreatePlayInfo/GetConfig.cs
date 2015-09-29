using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CreatePlayInfo
{
    public static class GetConfig
    {
        public static string GetValueByName(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
