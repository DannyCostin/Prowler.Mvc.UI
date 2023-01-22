using Prowler.Presentation.Helpers;
using Prowler.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Prowler.Presentation.Controllers
{
    public class DropDownListController : Controller
    {
        // GET: DropDownList
        public ActionResult Index()
        {
            //Thread.Sleep(5000);
            return PartialView("_Overview", MockHelper.GetMockProducts());
        }

        public ActionResult Send(MockProduct product, FormCollection collection)
        {
           return RedirectToAction("Index");
        }

        public ActionResult GetDropDownSideMenu()
        {
            //Thread.Sleep(5000);

            var model = new SideMenuModel() { DataSource = new List<SideMenuItemModel>() };
            model.MenuTitle = "DropDownList";
            model.DataSource.Add("Overview", "/DropDownList/Index");
            model.DataSource.Add("Basic Usage", "/DropDownList/GetSection?view=_BasicUsage");
            model.DataSource.Add("Grouping", "/DropDownList/GetSection?view=_GroupBy");
            model.DataSource.Add("Client Filtering", "/DropDownList/GetSection?view=_ClientFiltering");
            model.DataSource.Add("Server Filtering", "/DropDownList/GetSection?view=_ServerFiltering");
            model.DataSource.Add("Multiselect", "/DropDownList/GetSection?view=_Multiselect");
            model.DataSource.Add("Customizing Templates", "/DropDownList/GetSection?view=_Templates");
            model.DataSource.Add("API", "/DropDownList/GetSection?view=_Api");
            model.DataSource.Add("Events", "/DropDownList/GetSection?view=_Events");

            return PartialView("_SideMenu", model);
        }

        public JsonResult Search(string value, string customFilter)
        {
            Thread.Sleep(3000); // simulate server response delay :)

            value = value?.ToLower() ?? string.Empty;
            var model = MockHelper.GetMockProducts();
            model.ProductDataSource = model.ProductDataSource.Where(i => i.Name.ToLower().Contains(value)).ToList();

            return Json(model.ProductDataSource);
        }

        public ActionResult GetSection(string view)
        {
            return PartialView(view, MockHelper.GetMockProducts());
        }
    }
}