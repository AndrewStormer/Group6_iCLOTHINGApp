using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Group6_iCLOTHINGApp.Models;

namespace Group6_iCLOTHINGApp.Controllers
{
    public class UserPasswordsController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: UserPasswords
        public ActionResult Index()
        {
            var userPasswords = db.UserPasswords.Include(u => u.Customers);
            return View(userPasswords.ToList());
        }

        // GET: UserPasswords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPasswords userPasswords = db.UserPasswords.Find(id);
            if (userPasswords == null)
            {
                return HttpNotFound();
            }
            return View(userPasswords);
        }

        // GET: UserPasswords/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.Customers, "customerID", "customerName");
            return View();
        }
        String encrypt(String message)
        {
            byte[] myBytes = Encoding.ASCII.GetBytes(message);
            int total = 0;
            foreach (byte toAdd in myBytes)
            {
                total += toAdd;
                total += 1;
            }
            total *= 3;
            total += 16;
            return total.ToString();
        }
        // POST: UserPasswords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userEncryptedPassword")] UserPasswords userPasswords)
        {
            if (ModelState.IsValid)
            {
                userPasswords.userEncryptedPassword = encrypt(userPasswords.userEncryptedPassword);
                Customers myUser = db.Customers.Find(Session["UserID"]);
                userPasswords.userID = myUser.customerID;
                userPasswords.userAccountName = myUser.customerName;
                userPasswords.userAccountExpiryDate = DateTime.Today.AddMonths(24);
                userPasswords.passwordExpiryTime = 9999999;
                db.UserPasswords.Add(userPasswords);
                ShoppingCarts cart = new ShoppingCarts();
                cart.customerID = (int)Session["UserID"];
                cart.cartProductPrice = 0;
                cart.cartProductQty = 0;
                Session["cartID"] = db.ShoppingCarts.Add(cart).cartID;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.userID = new SelectList(db.Customers, "customerID", "customerName", userPasswords.userID);
            return View(userPasswords);
        }
        public ActionResult AuthSuccess()
        {
            return View("AuthSuccess");
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "userEncryptedPassword")] UserPasswords userPasswords)
        {
            String encryptedPass = encrypt(userPasswords.userEncryptedPassword);
            UserPasswords currentUserPass = db.UserPasswords.Find((int)Session["TempUser"]);
            if (currentUserPass.userEncryptedPassword == encryptedPass)
            {
                Session["UserID"] = Session["TempUser"];
                Session["TempUser"] = null;
                return RedirectToAction("AuthSuccess");
            }
            else
            {
                Session["TempUser"] = null;
                return RedirectToAction("FailedLogin", "Customers");
            }
        }
        // GET: UserPasswords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPasswords userPasswords = db.UserPasswords.Find(id);
            if (userPasswords == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.Customers, "customerID", "customerName", userPasswords.userID);
            return View(userPasswords);
        }

        // POST: UserPasswords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPasswords userPasswords)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPasswords).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.Customers, "customerID", "customerName", userPasswords.userID);
            return View(userPasswords);
        }

        // GET: UserPasswords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPasswords userPasswords = db.UserPasswords.Find(id);
            if (userPasswords == null)
            {
                return HttpNotFound();
            }
            return View(userPasswords);
        }

        // POST: UserPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPasswords userPasswords = db.UserPasswords.Find(id);
            db.UserPasswords.Remove(userPasswords);
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
