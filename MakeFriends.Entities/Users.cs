using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MakeFriends.Common;
using MakeFriends.Common.Data;

namespace MakeFriends.Entities
{
    [DBAction("users")]
    public class Users : Entity
    {
        [DBField("user_id", isKey = true, IsIdentity = true, IsSearchField = true)]
        public int user_id { get; set; }

        [DBField("username")]
        public string username { get; set; }

        [DBField("password")]
        public string password { get; set; }

        [DBField("first_name")]
        public string first_name { get; set; }

        [DBField("last_name")]
        public string last_name { get; set; }

        [DBField("email")]
        public string email { get; set; }

    }
}
