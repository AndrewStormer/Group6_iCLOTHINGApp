using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group6_iCLOTHINGApp.Models;

namespace Group6_iCLOTHINGApp.Controllers
{
    public class UserCommentsController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: UserComments
        public ActionResult Index()
        {
            var userComments = db.UserComments.Include(u => u.Customers);
            return View(userComments.ToList());
        }
        public ActionResult Details()
        {
            return View("Details");
        }
        // GET: UserComments/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComments userComments = db.UserComments.Find(id);
            if (userComments == null)
            {
                return HttpNotFound();
            }
            return View(userComments);
        }

        // GET: UserComments/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName");
            return View();
        }

        // POST: UserComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentDescription")] UserComments userComments)
        {
            if (ModelState.IsValid)
            {
                userComments.commentDate = DateTime.Today;
                if (Session["UserID"] != null)
                {
                    Customers myUser = db.Customers.Find(Session["UserID"]);
                    userComments.customerID = myUser.customerID;
                }
                else
                {
                    return RedirectToAction("NeedLogin", "Home");
                }
                db.UserComments.Add(userComments);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", userComments.customerID);
            return View(userComments);
        }

        // GET: UserComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComments userComments = db.UserComments.Find(id);
            if (userComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", userComments.customerID);
            return View(userComments);
        }

        // POST: UserComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentNo,commentDate,commentDescription,customerID")] UserComments userComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", userComments.customerID);
            return View(userComments);
        }

        // GET: UserComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComments userComments = db.UserComments.Find(id);
            if (userComments == null)
            {
                return HttpNotFound();
            }
            return View(userComments);
        }

        // POST: UserComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserComments userComments = db.UserComments.Find(id);
            db.UserComments.Remove(userComments);
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
