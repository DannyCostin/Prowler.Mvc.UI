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
            return PartialView(MockHelper.GetMockProducts());
        }

        public ActionResult Send(MockProduct product, FormCollection collection)
        {
           return RedirectToAction("Index");
        }

        public ActionResult GetDropDownSideMenu()
        {
            //Thread.Sleep(5000);

            var model = new SideMenuModel();
            model.DataSource = new List<SideMenuItemModel> { new SideMenuItemModel { Title = "Basic Usage", View = "/DropDownList/Index" }, new SideMenuItemModel { Title = "Grouping", View = "/Grid/Index" } };

            for(int index =0; index <=150; index++)
            {
                model.DataSource.Add(new SideMenuItemModel
                {
                    Title = $"Item{index}"
                });
            }

            model.MenuTitle = "Drop Down List";

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
    }

    public class Test1
    {
        public int Id { get; set; }
    }
}