using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MakeFriends.Entities;
using System.Configuration;

namespace MakeFriendsMVC.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {

            Users User = new Users();
            User.user_id = 1;
            User.PopulateEntity();
            return View(User);

        }

    }
}
