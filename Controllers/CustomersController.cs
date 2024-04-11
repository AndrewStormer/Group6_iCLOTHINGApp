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
    public class CustomersController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.UserPasswords);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }



        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.UserPasswords, "userID", "userAccountName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "customerID,customerName,customerShippingAddress,customerBillingAddress,customerDOB,customerGender,customerEmail")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customers);
                db.SaveChanges();
                Session["UserID"] = customers.customerID;
                return RedirectToAction("create", "UserPasswords");
            }

            ViewBag.customerID = new SelectList(db.UserPasswords, "userID", "userAccountName", customers.customerID);
            return View(customers);
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult FailedLogin()
        {
            return View("FailedLogin");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "customerName")] Customers customers)
        {
            Customers getCustByName = db.Customers.FirstOrDefault(c => c.customerName == customers.customerName);
            if (getCustByName == null)
            {
                return RedirectToAction("FailedLogin");
            }
            else
            {
                Session["TempUser"] = getCustByName.customerID;
                return RedirectToAction("Login", "UserPasswords");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FailedLogin([Bind(Include = "customerName")] Customers customers)
        {
            Customers getCustByName = db.Customers.FirstOrDefault(c => c.customerName == customers.customerName);
            if (getCustByName == null)
            {
                return RedirectToAction("FailedLogin");
            }
            else
            {
                Session["TempUser"] = getCustByName.customerID;
                return RedirectToAction("Login", "UserPasswords");
            }
        }
        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.UserPasswords, "userID", "userAccountName", customers.customerID);
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "customerID,customerName,customerShippingAddress,customerBillingAddress,customerDOB,customerGender,customerEmail")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.UserPasswords, "userID", "userAccountName", customers.customerID);
            return View(customers);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["cartID"] = null;
            Session["admin"] = null;
            return RedirectToAction("Index", "Home");
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
