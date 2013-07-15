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
            //MySqlConnection myconn = new MySqlConnection(ConfigurationManager.ConnectionStrings["makeFriends_conn"].ConnectionString);
            //MySqlCommand comm = new MySqlCommand("select * from Users where user_id=1", myconn);
            //myconn.Open();
            //MySqlDataReader dr = comm.ExecuteReader();

            Users User = new Users();
            //User.first_name = dr.GetString("first_name");
            //dr.Close();
            //myconn.Clone();
            User.user_id = 1;
            User.PopulateEntity();
            return View(User);
        }

    }
}
