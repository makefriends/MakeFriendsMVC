using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MakeFriends.Common.Configuration
{
    public class Configuration : System.Configuration.ConfigurationSection
    {
        private static string CONFIGURATION_NAME = "MakeFriends.Common";

        public static Configuration GetConfiguration()
        {

            return (Configuration)ConfigurationManager.GetSection(CONFIGURATION_NAME) ??
               new Configuration();
        }        

        [ConfigurationProperty("DALType", IsRequired = false)]
        public string DALType
        {
            get
            {
                return this["DALType"] as string;
            }
        }
    }
}
