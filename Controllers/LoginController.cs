using Group6_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Group6_iCLOTHINGApp.Controllers
{
    public class LoginController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();
        public ActionResult SubmitLoginForm(FormCollection userInput)
        {
            String nameInput = userInput["userName"];
            Customers getCustByName = db.Customers.FirstOrDefault(c => c.customerName == nameInput);
            if (getCustByName == null)
            {
                return RedirectToAction("FailedLogin");
            }
            else
            {
                String encryptedPass = encrypt(userInput["passWord"]);
                int custID = getCustByName.customerID;
                if (db.UserPasswords.Find(custID).userEncryptedPassword == encryptedPass)
                {
                    ShoppingCarts newCart = new ShoppingCarts();
                    newCart.customerID = getCustByName.customerID;
                    db.ShoppingCarts.Add(newCart);
                    db.SaveChanges();
                    Session["UserID"] = getCustByName.customerID;
                    Session["cartID"] = newCart.cartID;
                    if(nameInput == "admin")
                    {
                        Session["admin"] = true;
                    }
                    return RedirectToAction("AuthSuccess");
                }
                else
                {
                    return RedirectToAction("FailedLogin");
                }


            }

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
        public ActionResult UserLoginForm()
        {
            return View("UserLoginForm");
        }
        public ActionResult FailedLogin()
        {
            return View("FailedLogin");
        }
        public ActionResult AuthSuccess()
        {
            return View("AuthSuccess");
        }

    }
}
