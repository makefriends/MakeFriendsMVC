using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MakeFriendsMVC.Models
{
    public class RegisterModel : LoginModel
    {
        [Display(Name = "Firstname")]
        [Required(ErrorMessageResourceName = "PleaseEnterFirstname", ErrorMessageResourceType = typeof(Resources.Strings))]
        [StringLength(20)]
        public string FirstName
        {
            get;
            set;
        }

        [Display(Name = "Lastname")]
        [Required(ErrorMessageResourceName = "PleaseEnterLastname", ErrorMessageResourceType = typeof(Resources.Strings))]
        [StringLength(20)]
        public string LastName
        {
            get;
            set;
        }

        [Display(Name = "Email")]
        [Required(ErrorMessageResourceName = "PleaseEnterEmail", ErrorMessageResourceType = typeof(Resources.Strings))]
        [EmailAddressAttribute(ErrorMessageResourceName = "PleaseEnterEmail", ErrorMessageResourceType = typeof(Resources.Strings))]
        [StringLength(20)]
        public string Email
        {
            get;
            set;
        }
    }
}