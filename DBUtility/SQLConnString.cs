using System;
using System.Configuration;

namespace DBUtility
{
    public class SQLConnString
    {
        public static string GetConnSting()
        {
            return ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        }
    }
}
