using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakeFriends.Common.Data
{
    public class DALFactory
    {
        public static Data.IDAL Create()
        {            
            return Activator.CreateInstance(Type.GetType(Configuration.Configuration.GetConfiguration().DALType)) as Data.IDAL;
        }
    }
}
