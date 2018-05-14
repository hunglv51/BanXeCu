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
    public class NotificationsController : Controller
    {
        private MyContext db = new MyContext();
        private int numNoticePerPage = 5;
        // GET: Notifications
        public ActionResult Index()
        {

            var notifications = getNotification();
            ViewBag.NoticeCount = notifications.Count;
            if (notifications.Count > numNoticePerPage)
                notifications = notifications.Take(numNoticePerPage).ToList();
            return View(notifications);
        }
        [HttpPost]
        public JsonResult GetNotifyPaged(int pageIndex = 1)
        {
            var notifications = getNotification();
            List<Notification> pagedNotifications = notifications.GetRange((pageIndex - 1) * numNoticePerPage, (notifications.Count >= pageIndex * numNoticePerPage) ? numNoticePerPage : notifications.Count - (pageIndex - 1) * numNoticePerPage);
            return Json(pagedNotifications);
        }

        public string countNotification()
        {
            var notifications = getNotification().Where(notification => notification.IsRead == false).ToList();
            if(notifications != null)
                return notifications.Count.ToString();
            return "0";
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            if(!notification.IsRead)
            {
                notification.IsRead = true;
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(notification);
        }

        //// GET: Notifications/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Notifications/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Type,MemID,PostID,Content,DateCreate,ForAdmin,IsRead")] Notification notification)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Notifications.Add(notification);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(notification);
        //}

        //// GET: Notifications/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Notification notification = db.Notifications.Find(id);
        //    if (notification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(notification);
        //}

        //// POST: Notifications/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Type,MemID,PostID,Content,DateCreate,ForAdmin,IsRead")] Notification notification)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(notification).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(notification);
        //}

        //// GET: Notifications/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Notification notification = db.Notifications.Find(id);
        //    if (notification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(notification);
        //}

        //// POST: Notifications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Notification notification = db.Notifications.Find(id);
        //    db.Notifications.Remove(notification);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private List<Notification> getNotification()
        {
            List<Notification> notifications = null;
            if (Session["UserID"] != null)
                if (Session["UserRole"] != null)
                {
                    notifications = (from notification in db.Notifications
                                     where (notification.ForAdmin)
                                     select notification).OrderByDescending(model => model.DateCreate).ToList();
                }
                else
                {
                    int memID = Convert.ToInt32(Session["UserID"].ToString());
                    notifications = (from notification in db.Notifications
                                     where (!notification.ForAdmin && notification.MemID == memID)
                                     select notification).OrderByDescending(model => model.DateCreate).ToList();
                }
            return notifications;
        }

    }
}
