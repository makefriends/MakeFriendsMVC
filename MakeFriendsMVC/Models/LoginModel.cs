using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MakeFriendsMVC.Models
{
    public class LoginModel : UserModel
    {
        [Display(Name="Username")]
        [Required(ErrorMessageResourceName = "PleaseEnterUsername", ErrorMessageResourceType = typeof(Resources.Strings))]
        [StringLength(20)]
        public string UserName
        {
            get;
            set;
        }

        [Display(Name = "Password")]
        [Required(ErrorMessageResourceName = "PleaseEnterPassword", ErrorMessageResourceType=typeof(Resources.Strings))]
        [StringLength(20)]
        public string Password
        {
            get;
            set;
        }

    }
}