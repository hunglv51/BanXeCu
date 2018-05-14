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
    public class SalonsController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Salons
        public ActionResult Index()
        {
            if(Session["UserRole"] != null)
            {
                ViewBag.TopSalons = GetTopSalons().Take(5).ToList();
                ViewBag.LazySalons = GetLazySalons();
            }
            return View(db.Salons.ToList());
        }

        // GET: Salons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int memID = Convert.ToInt32(id);
            Salon salon = db.Salons.Where(model => model.MemID == memID).FirstOrDefault();
            Member member = db.Members.Find(memID);
            var salonPosts = db.Posts.Where(model => model.MemID == memID);
            if (salon == null)
            {
                return HttpNotFound();
            }
            ViewBag.Member = member;
            ViewBag.Posts = salonPosts;
            return View(salon);
        }

        // GET: Salons/Create
        public ActionResult Create(int id)
        {
            int memID = id;
            var member = db.Members.Find(memID);
            ViewBag.Member = member;
            return View();
        }

        // POST: Salons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MemID,SalonAddress,Introduction,SalonName")] Salon salon)
        {
            if (ModelState.IsValid)
            {
                db.Salons.Add(salon);
                db.SaveChanges();
                Notification notification = new Notification();
                notification.Content = "Yêu cầu đăng ký Salon của bạn đã được chấp nhận";
                notification.DateCreate = DateTime.Now;
                notification.ForAdmin = false;
                notification.IsRead = false;
                notification.MemID = salon.MemID;
                notification.PostID = null;
                notification.Type = "AcceptSalonNotification";
                db.Notifications.Add(notification);
                db.SaveChanges();
                var member = db.Members.Find(salon.MemID);
                member.IsSalon = true;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salon);
        }

        // GET: Salons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int memID = Convert.ToInt32(id);
            Salon salon = db.Salons.Where(model => model.MemID == memID).FirstOrDefault();
            if (salon == null)
            {
                return HttpNotFound();
            }
            return View(salon);
        }

        // POST: Salons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MemID,SalonAddress,Introduction,SalonName")] Salon salon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salon);
        }

        // GET: Salons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int memID = Convert.ToInt32(id);
            Salon salon = db.Salons.Where(model => model.MemID == memID).FirstOrDefault();
            if (salon == null)
            {
                return HttpNotFound();
            }
            return View(salon);
        }

        // POST: Salons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Salon salon = db.Salons.Find(id);
            db.Salons.Remove(salon);
            db.SaveChanges();
            Member member = db.Members.Find(salon.MemID);
            member.IsSalon = false;
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        public List<SalonInfo> GetTopSalons()
        {
            var topSalons = from post in db.Posts
                            join salon in db.Salons
                                on post.MemID equals salon.MemID
                            where post.Sold == true
                            group salon by salon.SalonName into salonGroup
                            select
                            new SalonInfo
                            {
                                MemID = salonGroup.FirstOrDefault().MemID,
                                SalonName = salonGroup.Key,
                                SalonAddress = salonGroup.FirstOrDefault().SalonAddress,
                                SalonIntro = salonGroup.FirstOrDefault().Introduction,
                                CountNumber = salonGroup.Count()
                          };
            return topSalons.OrderByDescending(model => model.CountNumber).ToList();
        }

        private DateTime getLastPostDate(int memID)
        {
            var lastPost = db.Posts.Where(model => model.MemID == memID).OrderByDescending(model => model.TimeStart).FirstOrDefault();
            return (lastPost == null) ? DateTime.Now.Subtract(TimeSpan.FromDays(8)) : lastPost.TimeStart;
        }
        private List<SalonInfo> GetLazySalons()
        {
            var lazySalons = db.Salons.AsEnumerable().Where(model => DateTime.Now.Subtract(getLastPostDate(model.MemID)).Days > 7);
            var listResult = from salon in lazySalons
                             select new SalonInfo
                             {
                                 MemID = salon.MemID,
                                 CountNumber = DateTime.Now.Subtract(getLastPostDate(salon.MemID)).Days,
                                 SalonName = salon.SalonName
                             };
            return listResult.ToList();
        }

        public ActionResult ViewTopSalons()
        {
            var topSalons = GetTopSalons();
            return View(topSalons);
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
