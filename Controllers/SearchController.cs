using Group6_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group6_iCLOTHINGApp.Controllers
{
    public class SearchController : Controller
    {
        private Group6_iCLOTHINGDBEntities1 db = new Group6_iCLOTHINGDBEntities1();

        // GET: Search
        public ActionResult ItemSearch()
        {
            return View("ItemSearch");
        }
        public ActionResult SubmitSearchForm(FormCollection form)
        {
            String userQuery = form["query"];
            var searchResults = db.Products.Where(c => c.productName.ToLower().Contains(userQuery.ToLower()) == true);
            return View("Results", searchResults.ToList());
        }
    }
}