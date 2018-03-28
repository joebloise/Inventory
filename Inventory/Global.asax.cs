using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Inventory
{
    public class InventoryApplication : System.Web.HttpApplication
    {
        public static DataConnection Data;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Data = new Inventory.DataConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString + "Provider=SQLOLEDB;");

            try
            {
                int tables = Data.SelectScalarInt32("select count(*) from sysobjects where type = 'U'");
            }
            catch (Exception ex)
            {
                throw new Exception("Data connection error: " + ex.Message);
            }

            if (!Data.TableExists("tblInventory"))
            {
                Data.Execute("create table tblInventory(SLoc varchar(max), Component varchar(max))");
                Data.Execute("insert into tblInventory(SLoc, Component) values ('test location 1', 'test component 1')");
                Data.Execute("insert into tblInventory(SLoc, Component) values ('test location 2', 'test component 2')");
                Data.Execute("insert into tblInventory(SLoc, Component) values ('test location 3', 'test component 3')");
            }
        }
    }
}
