using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lostthing.Models;

namespace lostthing.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DataclassesDataContext dc = new DataclassesDataContext();

        public ActionResult RegisterAdmin()
        {
            string name = Request["uname"];
            string passwd = Request["passwd"];

            Admin a = new Admin();
            a.adminName = name;
            a.password = passwd;
            dc.Admins.InsertOnSubmit(a);
            dc.SubmitChanges();

            return RedirectToAction("signup");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult add_post()
        {
            return View();
        }

        public ActionResult buy_sell()
        {
            return View();
        }

        public ActionResult register_admin()
        {
            return View();
        }

        public ActionResult return_me()
        {
            return View();
        }

        public ActionResult return_me_single_post()
        {
            return View();
        }

        public ActionResult signup()
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

    }
}
