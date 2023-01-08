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
    public class GridController : Controller
    {
        // GET: Grid
        public ActionResult Index()
        {
            var dataSource = MockHelper.GetMockProducts(false);

            dataSource.ProductDataSource.ForEach(i =>
            {
                i.Checked = new Random().Next(1, 10) % 2 == 0 ? true : false;
                i.Disable = new Random().Next(1, 10) % 2 == 0 ? true : false;
            });
            return View(dataSource);
        }

        public ActionResult Page(GridDataSourceRequest<Product> gridDataSourceRequest, string SortColumnName,
            string DescriptionFiltersSort, FormCollection form, List<FilterGroup> filterGroups,
            string ClientName, string ClientId, bool? productFilterList, string additionalClient, string additionalFilter)
        {
            try
            {
                Thread.Sleep(500);

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

                //if(filterGroup != null && !hasMultipleGroups)
                //{
                //    list = MockHelper.FilterByGroup(list, filterGroup);
                //}

                var totalItems = list.Count();

                var data = list.Skip(gridDataSourceRequest?.PageInfo?.Skip ?? 0)
                               .Take(gridDataSourceRequest?.PageInfo?.PageItems ?? list.Count())
                               .ToList();

                data.ForEach(i =>
                {
                    i.Checked = new Random().Next(1, 10) % 2 == 0 ? true : false;
                    i.Disable = new Random().Next(1, 10) % 2 == 0 ? true : false;
                });

                //data[3].Checked = true;

                var datasouce = new GridDatasourceResponse<Product>
                {
                    DataSource = data,
                    TotalItems = totalItems,
                    PageIndex = gridDataSourceRequest?.PageInfo?.PageIndex ?? 1
                };

                //throw new InvalidOperationException("Error custom exception");

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