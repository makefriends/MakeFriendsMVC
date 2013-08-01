using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakeFriendsMVC.Models
{
    public class UserModel
    {
        MakeFriends.BLL.Entities.User user;

        public MakeFriends.BLL.Entities.User User
        {
            get
            {
                return user;
            }
        }

        public UserModel()
        {
            user = new MakeFriends.BLL.Entities.User();
        }
    }
}