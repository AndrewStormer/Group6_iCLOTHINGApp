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
    public class OrderStatusesController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: OrderStatuses
        public ActionResult Index()
        {
            var orderStatuses = db.OrderStatuses.Include(o => o.ShoppingCarts);
            return View(orderStatuses.ToList());
        }

        // GET: OrderStatuses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatuses orderStatuses = db.OrderStatuses.Find(id);
            if (orderStatuses == null)
            {
                return HttpNotFound();
            }
            return View(orderStatuses);
        }

        // GET: OrderStatuses/Create
        public ActionResult Create()
        {
            ViewBag.statusID = new SelectList(db.ShoppingCarts, "cartID", "cartID");
            return View();
        }

        // POST: OrderStatuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "statusID,status,statusDate,cartID")] OrderStatuses orderStatuses)
        {
            if (ModelState.IsValid)
            {
                db.OrderStatuses.Add(orderStatuses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.statusID = new SelectList(db.ShoppingCarts, "cartID", "cartID", orderStatuses.statusID);
            return View(orderStatuses);
        }

        // GET: OrderStatuses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatuses orderStatuses = db.OrderStatuses.Find(id);
            if (orderStatuses == null)
            {
                return HttpNotFound();
            }
            ViewBag.statusID = new SelectList(db.ShoppingCarts, "cartID", "cartID", orderStatuses.statusID);
            return View(orderStatuses);
        }

        // POST: OrderStatuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "statusID,status,statusDate,cartID")] OrderStatuses orderStatuses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderStatuses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.statusID = new SelectList(db.ShoppingCarts, "cartID", "cartID", orderStatuses.statusID);
            return View(orderStatuses);
        }

        // GET: OrderStatuses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatuses orderStatuses = db.OrderStatuses.Find(id);
            if (orderStatuses == null)
            {
                return HttpNotFound();
            }
            return View(orderStatuses);
        }

        // POST: OrderStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderStatuses orderStatuses = db.OrderStatuses.Find(id);
            db.OrderStatuses.Remove(orderStatuses);
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
