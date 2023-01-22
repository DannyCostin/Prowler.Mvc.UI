using Prowler.Mvc.UI;
using Prowler.Presentation.Helpers;
using Prowler.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Prowler.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = MockHelper.GetMockProducts(false);
            dataSource.TotalNrElements = dataSource.ProductDataSource.Count;
            dataSource.ProductDataSource = dataSource.ProductDataSource.Take(10).ToList();
            return View(dataSource);
        }

        public ActionResult GridFilter(GridDataSourceRequest<Product> gridDataSourceRequest, string SortColumnName,
           string DescriptionFiltersSort, List<FilterGroup> filterGroups, bool resetPageIndex = false)
        {
            return GridNavigateToPage(gridDataSourceRequest, SortColumnName, DescriptionFiltersSort, filterGroups, true);
        }

        public ActionResult GridNavigateToPage(GridDataSourceRequest<Product> gridDataSourceRequest, string SortColumnName,
             string DescriptionFiltersSort, List<FilterGroup> filterGroups, bool resetPageIndex = false)
        {
            try
            {
                int pageIndex = 0;
                var list = MockHelper.GetMockProducts(false).ProductDataSource.AsEnumerable();

                if (!string.IsNullOrEmpty(SortColumnName))
                {
                    list = MockHelper.SortByName(list, SortColumnName);
                }

                if (!string.IsNullOrEmpty(DescriptionFiltersSort))
                {
                    list = MockHelper.SortByDescription(list, DescriptionFiltersSort);
                }

                if (filterGroups != null)
                {
                    list = MockHelper.FilterByGroup(list, filterGroups);
                }

                var totalItems = list.Count();

                List<Product> data = null;

                if (resetPageIndex)
                {
                    pageIndex = 1;
                    data = list.Skip(0)
                               .Take(gridDataSourceRequest?.PageInfo?.PageItems ?? list.Count())
                               .ToList();
                }
                else
                {
                    pageIndex = gridDataSourceRequest?.PageInfo?.PageIndex ?? 1;

                    data = list.Skip(gridDataSourceRequest?.PageInfo.Skip ?? 0)
                              .Take(gridDataSourceRequest?.PageInfo?.PageItems ?? list.Count())
                              .ToList();
                }

                var datasouce = new GridDataSourceResponse<Product>
                {
                    DataSource = data,
                    TotalItems = totalItems,
                    PageIndex = pageIndex
                };

                return Json(datasouce);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
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


        public ActionResult GridNavigateToPage(GridDataSourceRequest<Product> gridRequest)
        {
            try
            {
                var productList = MockHelper.GetMockProducts().ProductDataSource;

                var data = productList.Skip(gridRequest.PageInfo.Skip)
                                      .Take(gridRequest.PageInfo.PageItems)
                                      .ToList();

                var datasouce = new GridDataSourceResponse<Product>
                {
                    DataSource = data,
                    TotalItems = productList.Count,
                    PageIndex = gridRequest.PageInfo.PageIndex
                };

                return Json(datasouce);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}