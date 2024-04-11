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
    public class EmailsController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: Emails
        public ActionResult Index()
        {
            var emails = db.Emails.Include(e => e.Administrators).Include(e => e.Customers);
            return View(emails.ToList());
        }

        // GET: Emails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emails emails = db.Emails.Find(id);
            if (emails == null)
            {
                return HttpNotFound();
            }
            return View(emails);
        }
        /* sendEmail is to be used when customer places an order
         * int recipient is the customerID of the customer that places the order
         * int sender is the ID of the admin who sends the email.
         * Not yet tested
         */
        public void sendEmail(String subject, String body, int recipient, int sender)
        {
            Emails newEmail = new Emails();
            newEmail.emailSubject = subject;
            newEmail.emailBody = body;
            newEmail.customerID = recipient;
            newEmail.adminID = sender;
            db.Emails.Add(newEmail);
            db.SaveChanges();
        }
        // GET: Emails/Create
        public ActionResult Create()
        {
            ViewBag.adminID = new SelectList(db.Administrators, "adminID", "adminName");
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName");
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emailNo,emailDate,emailSubject,emailBody,customerID,adminID")] Emails emails)
        {
            if (ModelState.IsValid)
            {
                db.Emails.Add(emails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adminID = new SelectList(db.Administrators, "adminID", "adminName", emails.adminID);
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", emails.customerID);
            return View(emails);
        }

        // GET: Emails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emails emails = db.Emails.Find(id);
            if (emails == null)
            {
                return HttpNotFound();
            }
            ViewBag.adminID = new SelectList(db.Administrators, "adminID", "adminName", emails.adminID);
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", emails.customerID);
            return View(emails);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emailNo,emailDate,emailSubject,emailBody,customerID,adminID")] Emails emails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.adminID = new SelectList(db.Administrators, "adminID", "adminName", emails.adminID);
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", emails.customerID);
            return View(emails);
        }

        // GET: Emails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emails emails = db.Emails.Find(id);
            if (emails == null)
            {
                return HttpNotFound();
            }
            return View(emails);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emails emails = db.Emails.Find(id);
            db.Emails.Remove(emails);
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
