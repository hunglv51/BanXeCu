using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.IO;
using System.Web.Mvc;
using BanXeCu.Models;

namespace BanXeCu.Controllers
{
    public class MembersController : Controller
    {
        private MyContext db = new MyContext();

        public ActionResult Logout()
        {
            Session["UserRole"] = null;
            Session["UserID"] = null;
            Session["Salon"] = null;
            return RedirectToAction("Index", "Posts");
        }

        public ActionResult Login()
        {
            if (Session["UserID"] != null)
                return RedirectToAction("UserDashBoard");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "username,password")] Member member)
        {
            if (ModelState.IsValid)
            {

                var obj = db.Members.Where(mem => mem.UserName.Equals(member.UserName) && mem.Password.Equals(member.Password)).FirstOrDefault();
                if (obj != null)
                {

                    Session["UserID"] = obj.ID.ToString();
                    Session["UserName"] = obj.UserName.ToString();
                    if (obj.IsSalon)
                    {
                        Session["Salon"] = "yes";
                        LazyNotice(obj.ID);
                    }
                    return RedirectToAction("UserDashBoard");

                }
                ViewBag.Message = "Sai mật khẩu hoặc tài khoản không tồn tại";
                return View();
            }

            return View();
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                string uid = Session["UserID"].ToString();
                IEnumerable<Post> posts = db.Posts.Where(post => post.MemID.ToString().Equals(uid)).OrderByDescending(post => post.TimeStart).ToList();
                ViewBag.User = db.Members.Find(Convert.ToInt32(uid));
                ExpireNotice(Convert.ToInt32(uid));
                return View(posts);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // GET: Members
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Posts");
            if (Session["UserRole"] == null)
                return RedirectToAction("UserDashBoard");
            return View(db.Members.ToList());

        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            if (member.IsSalon)
            {
                return RedirectToAction("Details", "Salons", new { id = member.ID });
            }
            List<Post> posts = db.Posts.Where(post => post.MemID == id).ToList();
            ViewBag.Posts = posts;
            return View(member);
        }

        // GET: Members/Create
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "ID,PhoneNumber,City,Name,Password,Email,UserName,IsSalon")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                Session["UserID"] = member.ID.ToString();
                Session["UserName"] = member.UserName;
                return RedirectToAction("UserDashBoard");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PhoneNumber,City,Name,UserName,Password,Email,IsSalon")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            var posts = db.Posts.Where(post => post.MemID == id).ToList();
            if(posts.Count > 0)
            {
                foreach (var p in posts)
                {
                    Post post = db.Posts.Find(p.PostID);
                    for (int i = 0; i < 4; i++)
                    {
                        var fileName = Path.GetFileName(id + "_" + i + ".jpg");
                        var path = Path.Combine(Server.MapPath("~/Image/"), fileName);
                        System.IO.File.Delete(path);
                    }
                }
            }
            db.Posts.RemoveRange(posts);
            db.SaveChanges();
            var salon = db.Salons.Where(model => model.MemID == id).FirstOrDefault();
            if(salon != null)
            {
                db.Salons.Remove(salon);
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult RequestSalon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int memID = Convert.ToInt32(id);
            if (Session["UserID"] == null || Session["Salon"] != null || Session["UserRole"] != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var notification = new Notification();
            notification.Content = "Yêu cầu đăng ký Salon";
            notification.DateCreate = DateTime.Now;
            notification.ForAdmin = true;
            notification.IsRead = false;
            notification.MemID = memID;
            notification.PostID = null;
            notification.Type = "RequestSalonNotification";
            db.Notifications.Add(notification);
            db.SaveChanges();
            return RedirectToAction("UserDashBoard");
        }

        public string CheckUserNameRegister(string UserNameRegistering)
        {
            var duplicateUserName = db.Members.Where(member => member.UserName.Equals(UserNameRegistering)).ToList();
           if(duplicateUserName.Count == 0)
            {
                return "ok";
            }
            return "duplicate";
        }

        private void ExpireNotice(int memID)
        {
            var expiredPost = db.Posts.Where(post => post.MemID == memID).Where(post => post.Sold == false).Where(post => post.ExpiredDate.CompareTo(DateTime.Now) < 0).ToList();
            if(expiredPost.Count > 0)
            {
                foreach (Post post in expiredPost)
                {
                    if (db.Notifications.Where(model => model.PostID.Equals(post.PostID.ToString())).Where(model => model.Type.Equals("ExpirePostNotification")).Where(model => model.IsRead == false).ToList().Count < 1)
                    {
                        var notification = new Notification();
                        notification.Content = "Bài viết của bạn đã hết hạn, hãy gia hạn";
                        notification.DateCreate = DateTime.Now;
                        notification.ForAdmin = false;
                        notification.IsRead = false;
                        notification.MemID = memID;
                        notification.PostID = post.PostID.ToString();
                        notification.Type = "ExpirePostNotification";
                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }
                }
            }
        }
        private void LazyNotice(int memID)
        {
            var lastPost = db.Posts.Where(model => model.MemID == memID).OrderByDescending(model => model.TimeStart).FirstOrDefault();
            if(lastPost != null)
            {
                var range = DateTime.Now.Subtract(lastPost.TimeStart);
                if(range.Days > 7)
                {
                    var notification = new Notification();
                    notification.Content = "Yêu cầu đăng bài viết mới";
                    notification.DateCreate = DateTime.Now;
                    notification.ForAdmin = false;
                    notification.IsRead = false;
                    notification.MemID = memID;
                    notification.PostID = null;
                    notification.Type = "LazySalonNotification";
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
