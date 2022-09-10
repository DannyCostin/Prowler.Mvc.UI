using Prowler.Mvc.UI;
using Prowler.Presentation.Helpers;
using Prowler.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Prowler.Presentation.Controllers
{
    public class GridController : Controller
    {
        // GET: Grid
        public ActionResult Index()
        {
            return View(MockHelper.GetMockProducts(false));
        }
        
        public ActionResult Sort(string DescriptionFilter, string Name, FormCollection collection)
        {
            return RedirectToAction("Index");
        }

        public JsonResult Page(Pagination pageInfo, GridFilters filters, GridMock gridMock, GridMockList gridMockList, FormCollection collection)
        {
            Thread.Sleep(5000);

            return Json(MockHelper.GetMockProducts(false).ProductDataSource.Skip(pageInfo.PageIndex * pageInfo.PageItems).Take(pageInfo.PageItems));
        }
    }
}