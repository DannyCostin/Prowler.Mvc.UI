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
        
        public ActionResult Page(GridDataSourceRequest<Product> gridDataSourceRequest, string SortColumnName, string DescriptionFiltersSort)
        {
            Thread.Sleep(2000);

            var list = MockHelper.GetMockProducts(false).ProductDataSource.AsEnumerable();

            if (!string.IsNullOrEmpty(SortColumnName))
            {
               list = MockHelper.SortByName(list, SortColumnName);
            }

            if (!string.IsNullOrEmpty(DescriptionFiltersSort))
            {
               list = MockHelper.SortByDescription(list, DescriptionFiltersSort);
            }

            var data = list.Skip((gridDataSourceRequest.PageInfo.PageIndex - 1) * gridDataSourceRequest.PageInfo.PageItems)
                           .Take(gridDataSourceRequest.PageInfo.PageItems)
                           .ToList();

            return Json(data);
        }
    }
}