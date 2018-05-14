using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanXeCu.Models;

namespace BanXeCu.Controllers
{
    public class AdminsController : Controller
    {
        private MyContext db = new MyContext();

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "username,password")] Admin admin)
        {
            if (ModelState.IsValid)
            {

                var obj = db.Admins.Where(ad => ad.UserName.Equals(admin.UserName) && ad.Password.Equals(admin.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.ID.ToString();
                    Session["UserRole"] = "admin";
                    Session["UserName"] = obj.UserName.ToString();
                    return RedirectToAction("Index", "Posts");
                }
                ViewBag.Message = "Sai mật khẩu hoặc tài khoản không tồn tại";
                return View();
            }

            return View();
        }



        // GET: Admins
        public ActionResult Index()
        {
            if (Session["UserRole"] != null)
                return View(db.Admins.ToList());
            return RedirectToAction("Login");
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
                return RedirectToAction("Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
                return RedirectToAction("Login");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,Name,Password,Email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
                return RedirectToAction("Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,Name,Password,Email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
                return RedirectToAction("Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
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
