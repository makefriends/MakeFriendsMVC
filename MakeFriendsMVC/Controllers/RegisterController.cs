using MakeFriendsMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakeFriendsMVC.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            RegisterModel model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {

            return View(model);
        }

    }
}
