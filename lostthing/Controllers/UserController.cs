using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lostthing.Models;

namespace lostthing.Controllers
{
   
    public class UserController : Controller
    {
        //
        // GET: /User/
        DataclassesDataContext dc = new DataclassesDataContext();
        public static string username = "";
        public static string password = "";
        public static int id;
        public ActionResult addPostToDB()
        {
            string title = Request["title"];
            string phoneNumber = Request["phoneNumber"];
            string category = Request["category"];
            string detail = Request["detail"];

            if (category == "1")
            {
                ad a = new ad();
                a.title = title;
                a.phoneNo = phoneNumber;
                a.description = detail;
                a.status = "lost";
                a.date = DateTime.Now.ToString("d");
                a.time = DateTime.Now.ToString("t");
                var v = from i in dc.Users
                        where (i.username.Equals(username) && i.password.Equals(password))
                        select i.Id;
                foreach (var i in v)
                {
                    a.userID = i;
                }
                dc.ads.InsertOnSubmit(a);
                dc.SubmitChanges();
            }

            else if (category == "2")
            {
                returnMePost r = new returnMePost();
                r.title = title;
                r.phoneNo = phoneNumber;
                r.description = detail;
                r.status = "lost";
                r.date = DateTime.Now.ToString("d");
                r.time = DateTime.Now.ToString("t");
                var v= from i in dc.Users
                        where (i.username.Equals(username) && i.password.Equals(password))
                        select i.Id;
                foreach (var i in v)
                {
                    r.userID = i;
                    id = i;
                }
                dc.returnMePosts.InsertOnSubmit(r);
                dc.SubmitChanges(); 
            }           

            return RedirectToAction("your_posts");
        }
        
        public ActionResult deleteLostthing(int id)
        {
            var d = from i in dc.returnMePosts
                    where (i.Id.Equals(id))
                    select i;

            if (d.Count() > 0)
            {
                dc.returnMePosts.DeleteOnSubmit(d.First());
                dc.SubmitChanges();
            }

            return RedirectToAction("your_posts");
        }

        public ActionResult deleteAds(int id)
        {
            var d = from i in dc.ads
                    where (i.Id.Equals(id))
                    select i;

            if (d.Count() > 0)
            {
                dc.ads.DeleteOnSubmit(d.First());
                dc.SubmitChanges();
            }

            return RedirectToAction("your_ads");
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

        public ActionResult return_me(string name,string passwd)
        {
            username = name;
            password = passwd;
            var d = from i in dc.returnMePosts
                    select i;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult buy_sell()
        {
            var d = from i in dc.ads
                    select i;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult return_me_single_post()
        {
            return View();
        }

        public ActionResult your_ads()
        {
            ViewBag.username = username;
            ViewBag.password = password;
            var v = from i in dc.Users
                    where (i.username.Equals(username) && i.password.Equals(password))
                    select i.Id;
            foreach (var i in v)
            {
                id = i;
            }
            var d = from i in dc.ads
                    where (i.userID.Equals(id))
                    select i;

            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult your_posts()
        {
            ViewBag.username = username;
            ViewBag.password = password;
            var v = from i in dc.Users
                    where (i.username.Equals(username) && i.password.Equals(password))
                    select i.Id;
            foreach (var i in v)
            {
                id = i;
            }
            var d = from i in dc.returnMePosts
                        where (i.userID.Equals(id))
                        select i;

            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult editLostThing(int id)
        {
            var d = dc.returnMePosts.First(s => s.Id == id);
            d.title=Request["title"];
            d.phoneNo=Request["phoneNumber"];
            d.description=Request["detail"];
            dc.SubmitChanges();
            return RedirectToAction("your_posts");
        }

        public ActionResult editPost(int id)
        {
            return View(dc.returnMePosts.First(s => s.Id == id));
        }


        public ActionResult edit_ad(int id)
        {
            return View(dc.ads.First(s => s.Id == id));
        }

        public ActionResult editAd(int id)
        {
            var d = dc.ads.First(s => s.Id == id);
            d.title = Request["title"];
            d.phoneNo = Request["phoneNumber"];
            d.description = Request["detail"];
            dc.SubmitChanges();
            return RedirectToAction("your_ads");
        }

        public ActionResult add_post()
        {
            return View();
        }
    }
}
