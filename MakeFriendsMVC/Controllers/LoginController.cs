using MakeFriendsMVC.Models;
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
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost()]
        public ActionResult Login(LoginModel model)
        {
            
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

    }
}
