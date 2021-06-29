using Prowler.Presentation.Helpers;
using Prowler.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Prowler.Presentation.Controllers
{
    public class DropDownListController : Controller
    {
        // GET: DropDownList
        public ActionResult Index()
        {
            return View(MockHelper.GetMockProducts());
        }

        public ActionResult Send(MockProduct product)
        {
           return RedirectToAction("Index");
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