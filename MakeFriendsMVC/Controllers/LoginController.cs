using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakeFriendsMVC.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost()]
        public ActionResult Login(string userName, string password)
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

    }
}
