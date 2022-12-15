using BeautySalon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Controllers
{
    public class HomeController : Controller
    {
        public SalonDatabaseEntities db = new SalonDatabaseEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Services()
        {
            ViewBag.Message = "Your services page.";

            return View();
        }

        public ActionResult Work()
        {
            ViewBag.Message = "Your work page.";

            return View();
        }

        public ActionResult Signup()
        {
            return RedirectToAction("Create", "Users");
        }


        public ActionResult Login()
        {
            ViewBag.msg = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email,string password)
        {
            if(email=="admin@gmail.com"&&password=="admin123")
            {
                Session["admin"] = "admin@gmail.com";
                return RedirectToAction("Index");
            }
            else
            {
                User user = db.Users.Where(x => x.user_email == email && x.user_password == password).FirstOrDefault();
                if (user != null)
                {
                    Session["user_id"] = user.user_id;
                    Session["user"] = user.user_email;
                    return View("Index");
                }
                else
                {
                    ViewBag.msg = "Invalid Credentials";
                    return View(user);
                }
            }

            
            
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return View("Index");
        }
    }
}