using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeFriends.Log
{
    internal class Configuration : ConfigurationSection
    {
        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get
            {
                return this["Path"] as string;
            }

        }

        [ConfigurationProperty("Enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["Enabled"];
            }

        }


        public static Configuration GetConfiguration()
        {
            return ConfigurationManager.GetSection("MakeFriends.Log") as Configuration;
        }
    }
}
