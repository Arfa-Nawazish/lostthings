using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lostthing.Models;
using System.IO;

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

       public ActionResult addPostToDB(HttpPostedFileBase image)
        {
            string title = Request["title"];
            string phoneNumber = Request["phoneNumber"];
            string category = Request["category"];
            string detail = Request["detail"];

            if (category == "1")
            {
                NonApproveAd a = new NonApproveAd();
                a.title = title;
                a.phoneNo = phoneNumber;
                a.description = detail;
                a.status = "Available";
                a.date = DateTime.Now.ToString("d");
                a.time = DateTime.Now.ToString("t");
                a.username=Session["uname"].ToString();
                var v = from i in dc.Users
                        where (i.username.Equals(username) && i.password.Equals(password))
                        select i.Id;

                foreach (var i in v)
                {    
                    a.userID = i;
                }
                if (image != null)
                {
                    var reader = new BinaryReader(image.InputStream);
                    byte[] imgData = reader.ReadBytes(image.ContentLength);
                    a.image = imgData;
                }
                else
                {
                    a.image = null;
                }
                dc.NonApproveAds.InsertOnSubmit(a);
                dc.SubmitChanges();
            }

            else if (category == "2")
            {
                NonApproveLostThing r = new NonApproveLostThing();
                r.title = title;
                r.phoneNo = phoneNumber;
                r.description = detail;
                r.status = "lost";
                r.date = DateTime.Now.ToString("d");
                r.time = DateTime.Now.ToString("t");
                r.username = Session["uname"].ToString();
                var v= from i in dc.Users
                        where (i.username.Equals(username) && i.password.Equals(password))
                        select i.Id;
                foreach (var i in v)
                {
                    r.userID = i;
                }
                if (image != null)
                {
                    var reader = new BinaryReader(image.InputStream);
                    byte[] imgData = reader.ReadBytes(image.ContentLength);
                    r.image = imgData;
                }
                else
                {
                    r.image = null;
                }
                dc.NonApproveLostThings.InsertOnSubmit(r);
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
            var d = dc.ads.First(s => s.Id == id);
            dc.ads.DeleteOnSubmit(d);
            dc.SubmitChanges();

            return RedirectToAction("your_ads");
        }

        public ActionResult _header()
        {
            return PartialView();
        }

        public ActionResult _sideBar()
        {
            var lst= from i in dc.returnMePosts
                     orderby i.Id descending
                     select i;
            if (lst.Count() > 0)
            {
                ViewBag.lost = lst.First();
            }
            else
            {
                ViewBag.lost = null;
            }

            var ads = from i in dc.ads
                      orderby i.Id descending
                      select i;
            if (ads.Count() > 0)
            {
                ViewBag.ad = ads.First();
            }
            else
            {
                ViewBag.ad = null;
            };


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
                    orderby i.Id descending
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
                    orderby i.Id descending
                    select i;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            return View();
        }

        public ActionResult return_me_single_post(int id)
        {
            Session["Id"] = id;
            var d = from i in dc.returnMePosts
                    where (i.Id.Equals(id))
                    select i;
            var comments = from j in dc.Comments
                           where (j.postID == id)
                           select j;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            if (comments.Count() > 0)
            {
                ViewBag.comments = comments;
            }
            else
            {
                ViewBag.comments = null;
            }
            return View();
        }

        public ActionResult single_ad(int id)
        {
            Session["Id"] = id;
            var d = from i in dc.ads
                    where (i.Id.Equals(id))
                    select i;
            var comments = from j in dc.AdComments
                           where (j.adID == id)
                           select j;
            if (d.Count() > 0)
            {
                ViewBag.data = d;
            }
            
            if (comments.Count() > 0)
            {
                ViewBag.comments = comments;
            }
            else
            {
                ViewBag.comments = null;
            }

            return View();
        }

        public ActionResult saveAdComment()
        {
            int id = (int)Session["Id"];
            AdComment c = new AdComment();

            var userId = from i in dc.ads
                         where (i.Id == id)
                         select i.userID;
            var username = from u in dc.Users
                           where (u.Id == userId.First())
                           select u.username;

            c.adID = id;
            c.description = Request["comment"];
            c.username = username.First();
            c.date = DateTime.Now.ToString("d");
            c.time = DateTime.Now.ToString("t");
            dc.AdComments.InsertOnSubmit(c);
            dc.SubmitChanges();
            Session["Id"] = null;
            return RedirectToAction("buy_sell");
        }

        public ActionResult saveComment()
        {
            int id = (int)Session["Id"];
            Comment c = new Comment();

            var userId = from i in dc.returnMePosts
                         where (i.Id == id)
                         select i.userID;
            var username = from u in dc.Users
                           where (u.Id == userId.First())
                           select u.username;

            c.postID = id;
            c.description = Request["comment"];
            c.username = username.First();
            c.date = DateTime.Now.ToString("d");
            c.time = DateTime.Now.ToString("t");
            dc.Comments.InsertOnSubmit(c);
            dc.SubmitChanges();
            Session["Id"] = null;
            return RedirectToAction("return_me");
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
