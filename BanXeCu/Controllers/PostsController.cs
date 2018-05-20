using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using BanXeCu.Models;

namespace BanXeCu.Controllers
{
    public class PostsController : Controller
    {
        private MyContext db = new MyContext();
        int numPostPerPage = 3;
        private List<Post> GetPostForUser()
        {
            List<Post> posts = null;
            if (Session["UserRole"] != null)
                posts = db.Posts.ToList();
            else if (Session["UserID"] != null)
            {
                int id = Convert.ToInt32(Session["UserID"]);
                posts = db.Posts.Where(model => model.MemID == id).ToList();
            }
            else
                posts = db.Posts.Where(model => model.Sold == false).Where(model => model.ExpiredDate.CompareTo(DateTime.Now) >= 0).OrderByDescending(model => model.PostType).ThenByDescending(model => model.TimeStart).ToList();
            return posts.OrderByDescending(model => model.TimeStart).ToList();
        }
       
        // GET: Posts
        public ActionResult Index(int id = 1)
        {
            int pageIndex = id;
            List<Post> posts = GetPostForUser();
            ViewBag.PostCount = posts.Count;
            ViewBag.PageIndex = pageIndex;
            List<Post> pagePost = posts.GetRange((pageIndex - 1) * numPostPerPage, (posts.Count >= pageIndex * numPostPerPage) ? numPostPerPage : posts.Count - (pageIndex - 1) * numPostPerPage);
            return View(pagePost);
        }

        public JsonResult PagedPost(int pageIndex = 1)
        {
            List<Post> posts = GetPostForUser();
            List<Post> pagePost = posts.GetRange((pageIndex - 1) * numPostPerPage, (posts.Count >= pageIndex * numPostPerPage) ? numPostPerPage : posts.Count - (pageIndex - 1) * numPostPerPage);
            return Json(pagePost);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            Member member = db.Members.Find(post.MemID);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.User = member;
            return View(post);
        }
        //Member create post
        // GET: Posts/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login","Members");
            if (Session["UserRole"] != null)
                return View();
            int id = Convert.ToInt32(Session["UserID"]);
            ViewBag.User = db.Members.Find(id);
            return View();
        }
        

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Title,Content,MemID,ExpiredDate,VideoLink,PostType,TimeStart,Sold,Name,CarModel,Family,Price,ManufactureYear,ComeFrom,Used,Distance,Size,Length,Weight,CylinderCapacity,FuelCapacity,TireInfo,WheelBase,MaxSeatingCapacity,DriveType,Transmission,NumDoor")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                if (Request.Files.Count > 3)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        var file = Request.Files[i];

                        if (file != null && file.ContentLength > 0)
                        {
                            
                            var fileName = Path.GetFileName(post.PostID + "_" + i + ".jpg");
                            var path = Path.Combine(Server.MapPath("~/Image/"), fileName);
                            file.SaveAs(path);
                        }
                    }

                }
                return RedirectToAction("Index");
            }

            return View(post);
        }

        public ActionResult AdminEditPost(int? id)
        {
            if (Session["UserRole"] == null)
                return RedirectToAction("Login", "Admins");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Members");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            Member member = db.Members.Find(post.MemID);
            ViewBag.User = member;
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Title,Content,MemID,ExpiredDate,VideoLink,PostType,TimeStart,Sold,Name,CarModel,Family,Price,ManufactureYear,ComeFrom,Used,Distance,Size,Length,Weight,CylinderCapacity,FuelCapacity,TireInfo,WheelBase,MaxSeatingCapacity,DriveType,Transmission,NumDoor")] Post post)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var file = Request.Files[i];

                        if (file != null && file.ContentLength > 0)
                        {

                            var fileName = Path.GetFileName(post.PostID + "_" + i + ".jpg");
                            var path = Path.Combine(Server.MapPath("~/Image/"), fileName);
                            file.SaveAs(path);
                        }
                    }

                }
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Sold (int? id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Members");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Sold = true;
            post.ExpiredDate = DateTime.Now;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserDashBoard","Members");
        }
        public ActionResult Renew(int ?id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Members");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renew([Bind(Include = "PostID,Title,Content,MemID,ExpiredDate,VideoLink,PostType,TimeStart,Sold,Name,CarModel,Family,Price,ManufactureYear,ComeFrom,Used,Distance,Size,Length,Weight,CylinderCapacity,FuelCapacity,TireInfo,WheelBase,MaxSeatingCapacity,DriveType,Transmission,NumDoor")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            return RedirectToAction("UserDashBoard","Members");
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
                return RedirectToAction("Login", "Admins");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            int memID = post.MemID;
            db.Posts.Remove(post);
            db.SaveChanges();
            for(int i = 0;i < 4; i++)
            {
                var fileName = Path.GetFileName(id + "_" + i + ".jpg");
                var path = Path.Combine(Server.MapPath("~/Image/"), fileName);
                System.IO.File.Delete(path);
            }
            Notification notification = new Notification();
            notification.Content = "Bài viết của bạn đã bị admin xóa vì vi phạm điều khoản sử dụng";
            notification.DateCreate = DateTime.Now;
            notification.ForAdmin = false;
            notification.IsRead = false;
            notification.MemID = memID;
            notification.PostID = id.ToString();
            notification.Type = "Delete Post Notification";
            db.Notifications.Add(notification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public List<CountListItem> GetTopCars()
        {
            var topCars = from post in db.Posts
                          where post.Sold == true
                          group post by post.Family into postGroup
                          select
                          new CountListItem
                          {
                              Name = postGroup.FirstOrDefault().Name + " " + postGroup.Key,
                              CountNumber = postGroup.Count()
                          };
            return topCars.OrderByDescending(model => model.CountNumber).ToList();
        }
        public ActionResult ViewTopCars()
        {
            var topCars = GetTopCars();
            return View(topCars);
        }
        
        public ActionResult Analyst()
        {
            var topFiveCars = GetTopCars().Take(5);
            return View(topFiveCars);
        }
        [HttpPost]
        public ActionResult Search(string carName, string carFamily, string price)
        {
            List<Post> posts = GetPostForUser();
            if (String.IsNullOrEmpty(carFamily))
            {
                if (!String.IsNullOrEmpty(carName))
                    posts = posts.Where(model => model.Name.Equals(carName)).ToList();
            }
            else
            {
                posts = posts.Where(model => model.Family.Equals(carFamily)).ToList();
            }
            if (!String.IsNullOrEmpty(price))
            {
                if (price.Equals("Dưới 500 triệu"))
                {
                    posts = posts.Where(model => model.Price < 500).ToList();
                }
                if (price.Equals("500 triệu - 2 tỷ"))
                {
                    posts = posts.Where(model => model.Price < 2000).Where(model => model.Price >= 500).ToList();
                }
                if (price.Equals("2 tỷ - 7 tỷ"))
                {
                    posts = posts.Where(model => model.Price < 7000).Where(model => model.Price >= 2000).ToList();
                }
                if (price.Equals("Trên 7 tỷ"))
                {
                    posts = posts.Where(model => model.Price >= 7000).ToList();
                }
            }


            return View(posts);
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
