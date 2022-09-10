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
        
        public ActionResult Sort(GridDataSourceRequest<Product> gridDataSourceRequest, FormCollection collection)
        {
            return Json(MockHelper.GetMockProducts(false).ProductDataSource.Skip((gridDataSourceRequest.PageInfo.PageIndex - 1) * gridDataSourceRequest.PageInfo.PageItems).Take(gridDataSourceRequest.PageInfo.PageItems));
        }

        public JsonResult Page(GridDataSourceRequest<Product> gridDataSourceRequest, FormCollection collection)
        {
            Thread.Sleep(2000);

            return Json(MockHelper.GetMockProducts(false).ProductDataSource.Skip(( gridDataSourceRequest.PageInfo.PageIndex -1) * gridDataSourceRequest.PageInfo.PageItems).Take(gridDataSourceRequest.PageInfo.PageItems));
        }
    }
}