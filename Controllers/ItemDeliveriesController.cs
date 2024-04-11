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
    public class ItemDeliveriesController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: ItemDeliveries
        public ActionResult Index()
        {
            var itemDeliveries = db.ItemDeliveries.Include(i => i.Customers).Include(i => i.Products);
            return View(itemDeliveries.ToList());
        }

        // GET: ItemDeliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemDeliveries itemDeliveries = db.ItemDeliveries.Find(id);
            if (itemDeliveries == null)
            {
                return HttpNotFound();
            }
            return View(itemDeliveries);
        }

        // GET: ItemDeliveries/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName");
            ViewBag.productID = new SelectList(db.Products, "productID", "productName");
            return View();
        }

        // POST: ItemDeliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stickerID,stickerDate,customerID,productID")] ItemDeliveries itemDeliveries)
        {
            if (ModelState.IsValid)
            {
                db.ItemDeliveries.Add(itemDeliveries);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", itemDeliveries.customerID);
            ViewBag.productID = new SelectList(db.Products, "productID", "productName", itemDeliveries.productID);
            return View(itemDeliveries);
        }

        // GET: ItemDeliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemDeliveries itemDeliveries = db.ItemDeliveries.Find(id);
            if (itemDeliveries == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", itemDeliveries.customerID);
            ViewBag.productID = new SelectList(db.Products, "productID", "productName", itemDeliveries.productID);
            return View(itemDeliveries);
        }

        // POST: ItemDeliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stickerID,stickerDate,customerID,productID")] ItemDeliveries itemDeliveries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemDeliveries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "customerName", itemDeliveries.customerID);
            ViewBag.productID = new SelectList(db.Products, "productID", "productName", itemDeliveries.productID);
            return View(itemDeliveries);
        }

        // GET: ItemDeliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemDeliveries itemDeliveries = db.ItemDeliveries.Find(id);
            if (itemDeliveries == null)
            {
                return HttpNotFound();
            }
            return View(itemDeliveries);
        }

        // POST: ItemDeliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemDeliveries itemDeliveries = db.ItemDeliveries.Find(id);
            db.ItemDeliveries.Remove(itemDeliveries);
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
