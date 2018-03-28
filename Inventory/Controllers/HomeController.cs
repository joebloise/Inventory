using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search()
        {
            String location = Request["SLoc"];
            String component = Request["Component"];

            String sql = "select * from tblInventory";
            if (!String.IsNullOrEmpty(location) && !String.IsNullOrEmpty(component))
                sql += " where SLoc like '" + InventoryApplication.Data.Filter(location) + "%' and Component like '" + InventoryApplication.Data.Filter(component) + "%'";
            else if (!String.IsNullOrEmpty(location))
                sql += " where SLoc like '" + InventoryApplication.Data.Filter(location) + "%'";
            else if (!String.IsNullOrEmpty(location) && !String.IsNullOrEmpty(component))
                sql += " where Component like '" + InventoryApplication.Data.Filter(component) + "%'";

            sql += " order by SLoc, Component";
            ViewBag.SearchResult = InventoryApplication.Data.Select(sql);

            return View("Index");
        }
    }
}