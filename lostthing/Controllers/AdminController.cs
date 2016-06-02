using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lostthing.Models;
using System.Net.Mail;


namespace lostthing.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DataclassesDataContext dc = new DataclassesDataContext();
        public static string username = "";
        public static string password = "";
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

        public ActionResult Index(string name, string passwd)
        {
            username = name;
            password = passwd;
            var d = from i in dc.NonApproveLostThings
                    select i;

            var a = from i in dc.NonApproveAds
                    select i;

            if (d.Count() > 0)
            {
                ViewBag.lostthing = d;
                ViewBag.ads = a;
            }

            if (a.Count() > 0)
            {
                ViewBag.ads = a;
            }
            return View();
        }

        public ActionResult addTolostThings(int id)
        {
            var d = dc.NonApproveLostThings.First(s => s.Id == id);
            returnMePost r = new returnMePost();
            r.title = d.title;
            r.phoneNo = d.phoneNo;
            r.description = d.description;
            r.userID = d.userID;
            r.status = d.status;
            r.time = d.time;
            r.date = d.date;
            r.image = d.image;
            r.username = d.username;
            string message = r.description;                                                       //adding message
            dc.returnMePosts.InsertOnSubmit(r);
            dc.SubmitChanges();

            dc.NonApproveLostThings.DeleteOnSubmit(d);
            dc.SubmitChanges();

            var emails = from i in dc.Users
                         select i;
            string email = "";
            
            foreach (var e in emails)
            {
                if(email == "")
                    email = e.email;
                else
                    email = email + "," + e.email;
            }

            string a = "arfanawazish31@gmail.com";
            string name = "Student(s)";
            sendMail(name,a,message);
            return RedirectToAction("Index");
        }

        public void sendMail(string name, string email,string message)
        {
            try
            {
                string from = "lostthing31@gmail.com";
                string username = "lostthing31@gmail.com";
                string password = "lostthing";
                string to = email;
                MailAddressCollection TO_addressList = new MailAddressCollection();

                foreach (var curr_address in to.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    MailAddress mytoAddress = new MailAddress(curr_address, "Custom display name");
                    TO_addressList.Add(mytoAddress);
                }

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.Body = "Dear " + name + "!!\n" + message + "\n" + "www.losthings.com";
                mail.From = new MailAddress(from);

                foreach (var id in TO_addressList)
                {
                    mail.To.Add(id);
                }

                mail.Subject = "lost";
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                smtpServer.EnableSsl = true;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
            }

        }

        public ActionResult addToAds(int id)
        {
            var d = dc.NonApproveAds.First(s => s.Id == id);
            ad r = new ad();
            r.title = d.title;
            r.phoneNo = d.phoneNo;
            r.description = d.description;
            r.userID = d.userID;
            r.status = d.status;
            r.time = d.time;
            r.date = d.date;
            r.image = d.image;
            r.username = d.username;

            dc.ads.InsertOnSubmit(r);
            dc.SubmitChanges();

            dc.NonApproveAds.DeleteOnSubmit(d);
            dc.SubmitChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ignorelostThings(int id)
        {
            var d = dc.NonApproveLostThings.First(s => s.Id == id);
            
            dc.NonApproveLostThings.DeleteOnSubmit(d);
            dc.SubmitChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ignoreAds(int id)
        {
            var d = dc.NonApproveAds.First(s => s.Id == id);
            
            dc.NonApproveAds.DeleteOnSubmit(d);
            dc.SubmitChanges();

            return RedirectToAction("Index");
        }

        public ActionResult add_post()
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
