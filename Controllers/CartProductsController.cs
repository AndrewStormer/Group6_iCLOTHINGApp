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
    public class CartProductsController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: CartProducts
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                id = (int)Session["CartID"];
                if (id == null)
                {
                    return View(db.CartProducts.Include(c => c.Products).Include(c => c.ShoppingCarts).ToList());
                }
                return View(db.CartProducts.Where(c => c.cartID == id).Include(c => c.Products).Include(c => c.ShoppingCarts).ToList());
            }
            var cartProducts = db.CartProducts.Where(c => c.cartID == id).Include(c => c.Products).Include(c => c.ShoppingCarts);
            return View(cartProducts.ToList());
        }

        public ActionResult CheckOut()
        {
            var shoppingCart = new ShoppingCarts();
            shoppingCart.cartProductQty = 0;
            shoppingCart.cartProductPrice = 0;
            shoppingCart.customerID = (int)Session["UserID"];
            db.ShoppingCarts.Add(shoppingCart);
            db.SaveChanges();
            Session["CartID"] = shoppingCart.cartID;

            return View();
        }

        // GET: CartProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartProducts cartProducts = db.CartProducts.Find(id);
            if (cartProducts == null)
            {
                return HttpNotFound();
            }
            return View(cartProducts);
        }

        // GET: CartProducts/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.Products, "productID", "productName");
            ViewBag.cartID = new SelectList(db.ShoppingCarts, "cartID", "cartID");
            return View();
        }

        // POST: CartProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cartID,productID,cartProductID,productQuantity")] CartProducts cartProducts)
        {
            if (ModelState.IsValid)
            {
                cartProducts.cartID = (int)Session["CartID"];
                db.CartProducts.Add(cartProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productID = new SelectList(db.Products, "productID", "productName", cartProducts.productID);
            ViewBag.cartID = new SelectList(db.ShoppingCarts, "cartID", "cartID", cartProducts.cartID);
            return View(cartProducts);
        }

        // GET: CartProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartProducts cartProducts = db.CartProducts.Find(id);
            if (cartProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "productName", cartProducts.productID);
            ViewBag.cartID = new SelectList(db.ShoppingCarts, "cartID", "cartID", cartProducts.cartID);
            return View(cartProducts);
        }

        // POST: CartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cartID,productID,cartProductID,productQuantity")] CartProducts cartProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "productName", cartProducts.productID);
            ViewBag.cartID = new SelectList(db.ShoppingCarts, "cartID", "cartID", cartProducts.cartID);
            return View(cartProducts);
        }

        // GET: CartProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartProducts cartProducts = db.CartProducts.Find(id);
            if (cartProducts == null)
            {
                return HttpNotFound();
            }
            db.CartProducts.Remove(cartProducts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: CartProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartProducts cartProducts = db.CartProducts.Find(id);
            db.CartProducts.Remove(cartProducts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int id)
        {
            if (Session["UserID"] != null)
            {
                CartProducts cartProducts = new CartProducts();
                cartProducts.productID = id;
                cartProducts.productQuantity = 1;

                ShoppingCarts cart = db.ShoppingCarts.Find(Session["cartID"]);
                cartProducts.cartID = (int)Session["cartID"];
                db.CartProducts.Add(cartProducts);
                db.SaveChanges();
                return RedirectToAction("Index", "CartProducts");
            }
            else
            {
                return RedirectToAction("NeedLogin", "Home");
            }
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
