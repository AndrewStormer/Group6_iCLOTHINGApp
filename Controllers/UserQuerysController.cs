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
    public class UserQuerysController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: UserQuerys
        public ActionResult Index()
        {
            var userQuerys = db.UserQuerys.Include(u => u.Customers);
            return View(userQuerys.ToList());
        }

        // GET: UserQuerys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuerys userQuerys = db.UserQuerys.Find(id);
            if (userQuerys == null)
            {
                return HttpNotFound();
            }
            return View(userQuerys);
        }

        // GET: UserQuerys/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName");
            return View();
        }

        // POST: UserQuerys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "queryDescription")] UserQuerys userQuerys)
        {
            if (ModelState.IsValid)
            {
                userQuerys.queryDate = DateTime.Now;
                if (Session["UserID"] != null)
                {
                    Customers myUser = db.Customers.Find(Session["UserID"]);
                    userQuerys.customerID = myUser.customerID;
                }
                else
                {
                    return RedirectToAction("NeedLogin", "Home");
                }
                db.UserQuerys.Add(userQuerys);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View("Create");
        }

        // GET: UserQuerys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuerys userQuerys = db.UserQuerys.Find(id);
            if (userQuerys == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", userQuerys.customerID);
            return View(userQuerys);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                int queryNo = Int32.Parse(form["QueryNo"]);
                UserQuerys queryToChange = db.UserQuerys.Find(queryNo);
                queryToChange.queryResponse = form["response"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult Reply(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuerys userQuerys = db.UserQuerys.Find(id);
            if (userQuerys == null)
            {
                return HttpNotFound();
            }
            return View(userQuerys);
        }

            // POST: UserQuerys/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "queryNo,queryDate,queryDescription,customerID")] UserQuerys userQuerys)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userQuerys).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", userQuerys.customerID);
            return View(userQuerys);
        }

        // GET: UserQuerys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuerys userQuerys = db.UserQuerys.Find(id);
            if (userQuerys == null)
            {
                return HttpNotFound();
            }
            return View(userQuerys);
        }

        // POST: UserQuerys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuerys userQuerys = db.UserQuerys.Find(id);
            db.UserQuerys.Remove(userQuerys);
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
