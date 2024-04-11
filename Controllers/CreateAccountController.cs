using Group6_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Group6_iCLOTHINGApp.Controllers
{
    public class CreateAccountController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();
        public ActionResult AcctFormSubmit(FormCollection userInput)
        {
            Customers newCustomer = new Customers();
            UserPasswords newPassword = new UserPasswords();
            ShoppingCarts newCart = new ShoppingCarts();
            String newName = userInput["userName"];
            var searchResults = db.Customers.Where(c => c.customerName == newName);
            if (searchResults != null)
            {
                return View("AccountExists");
            }
            String newPass = encrypt(userInput["userName"]);
            String newShipping = userInput["shippingAddress"];
            String newBilling = userInput["billingAddress"];
            String[] formats = new String[] { "yyyy/MM/dd" };
            IFormatProvider cultInfo = CultureInfo.InvariantCulture.DateTimeFormat;
            DateTime newDOB;
            bool var = DateTime.TryParseExact(userInput["DOB"].ToString(), formats, cultInfo, DateTimeStyles.AllowWhiteSpaces, out newDOB);
            userInput["DOB"].ToString();
            String newGender = userInput["gender"];
            String newEmail = userInput["email"];
            newCustomer.customerEmail = newEmail;
            newCustomer.customerGender = newGender;
            newCustomer.customerDOB = newDOB;
            newCustomer.customerBillingAddress = newBilling;
            newCustomer.customerShippingAddress = newShipping;
            newCustomer.customerName = newName;
            db.Customers.Add(newCustomer);
            db.SaveChanges();
            newPassword.userAccountName = newName;
            newPassword.userID = newCustomer.customerID;
            newPassword.userEncryptedPassword = newPass;
            newPassword.passwordExpiryTime = 999999;
            newPassword.userAccountExpiryDate = DateTime.Today.AddMonths(24);
            db.UserPasswords.Add(newPassword);
            db.SaveChanges();
            newCart.customerID = newCustomer.customerID;
            db.ShoppingCarts.Add(newCart);
            db.SaveChanges();
            Session["UserID"] = newCustomer.customerID;
            Session["cartID"] = newCart.cartID;
            if (newName == "admin")
            {
                Session["admin"] = true;
            }
            return View("AccountCreated");

        }
        private String encrypt(String message)
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
        public ActionResult NewAcctForm()
        {
            return View("NewAcctForm");
        }
        public ActionResult AccountCreated()
        {
            return View("AccountCreated");
        }
    }
}