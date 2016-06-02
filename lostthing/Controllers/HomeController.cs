using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lostthing.Models;

namespace lostthing.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        DataclassesDataContext dc = new DataclassesDataContext();


        public ActionResult RegisterUser()
        {
            string name = Request["uname"];
            string passwd = Request["passwd"];
            string email = Request["email"];

            User u = new User();
            u.username = name;
            u.password = passwd;
            u.email = email;
            dc.Users.InsertOnSubmit(u);
            dc.SubmitChanges();

            return RedirectToAction("signup");
        }

        public ActionResult Loginfn()
        {
            //string name = Request["username"];
            //string passwd = Request["password"];
            string name = Request["uname"];
            string passwd = Request["passwd"];
            Session["uname"] = name;
            Session["passwd"] = passwd;
            User u = new User();
            u.username = name;
            u.password = passwd;

            Admin a = new Admin();
            a.adminName = name;
            a.password = passwd;

            var v = from i in dc.Users
                    where (i.username.Equals(name) && i.password.Equals(passwd))
                    select i;

            var admin = from i in dc.Admins
                    where (i.adminName.Equals(name) && i.password.Equals(passwd))
                    select i;

            if (v.Count() > 0)
                return RedirectToAction("../User/return_me","UserController",new{ name,passwd });
            else if (admin.Count() > 0)
                return RedirectToAction("../Admin/Index", "AdminController", new { name, passwd });
            else
                return RedirectToAction("login");
        }

        public ActionResult logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Session["passwd"] = null;
            Session["uname"] = null;
            dc.SubmitChanges();
            return RedirectToAction("login");
        }

        public ActionResult index()
        {
            return View();
        }

        public ActionResult buy_sell()
        {
            var d = from i in dc.ads
                    orderby i.Id descending
                    select i;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult return_me()
        {
            var d = from i in dc.returnMePosts
                    orderby i.Id descending
                    select i;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult signup()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        public ActionResult loginPanel()
        {
            return View();
        }

        public ActionResult _header()
        {
            return PartialView();
        }

        public ActionResult _sideBar()
        {
            return PartialView();
        }

        public ActionResult _footer()
        {
            return PartialView();
        }

        public ActionResult ask_question()
        {
            return View();
        }

        public ActionResult blog_1()
        {
            return View();
        }

        public ActionResult blog_1_full_width()
        {
            return View();
        }

        public ActionResult blog_1_l_sidebar()
        {
            return View();
        }

        public ActionResult blog_2()
        {
            return View();
        }

        public ActionResult blog_2_full_width()
        {
            return View();
        }

        public ActionResult blog_2_l_sidebar()
        {
            return View();
        }

        public ActionResult cat_question()
        {
            return View();
        }

        public ActionResult contact_us()
        {
            return View();
        }

        public ActionResult edit_profile()
        {
            return View();
        }

        public ActionResult full_width()
        {
            return View();
        }

        public ActionResult icons()
        {
            return View();
        }

        public ActionResult return_me_single_post()
        {
            return View();
        }

        public ActionResult index_boxed_1()
        {
            return View();
        }

        public ActionResult index_boxed_2()
        {
            return View();
        }

        public ActionResult index_no_box()
        {
            return View();
        }

        public ActionResult left_sidebar()
        {
            return View();
        }

        public ActionResult p404()
        {
            return View();
        }

        public ActionResult right_sidebar()
        {
            return View();
        }

        public ActionResult shortcodes()
        {
            return View();
        }

        public ActionResult single_post()
        {
            return View();
        }

        public ActionResult single_post_full_width()
        {
            return View();
        }

        public ActionResult single_post_l_sidebar()
        {
            return View();
        }

        public ActionResult single_question()
        {
            return View();
        }

        public ActionResult single_question_poll()
        {
            return View();
        }

        public ActionResult user_answers()
        {
            return View();
        }

        public ActionResult user_favorite_answers()
        {
            return View();
        }

        public ActionResult user_points()
        {
            return View();
        }

        public ActionResult user_profile()
        {
            return View();
        }

        public ActionResult user_questions()
        {
            return View();
        }
    }

}
