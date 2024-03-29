﻿using Prowler.Mvc.UI;
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

            dataSource.TotalNrElements = dataSource.ProductDataSource.Count;

            return PartialView("_Overview", dataSource);
        }

        public ActionResult GetGridSideMenu()
        {
            var model = new SideMenuModel() { DataSource = new List<SideMenuItemModel>() };
            model.MenuTitle = "Grid";
            model.DataSource.Add("Overview", "/Grid/Index");
            model.DataSource.Add("Basic Usage", "/Grid/GetSection?view=_BasicUsage");
            model.DataSource.Add("Pagination", "/Grid/GetSection?view=_Pagination");
            model.DataSource.Add("Sorting", "/Grid/GetSection?view=_Sorting");
            model.DataSource.Add("Edit Rows", "/Grid/GetSection?view=_EditRows");
            model.DataSource.Add("Events", "/Grid/GetSection?view=_Events");
            model.DataSource.Add("API", "/Grid/GetSection?view=_Api");
            model.DataSource.Add("Customizing templates", "/Grid/GetSection?view=_Templates");

            return PartialView("_SideMenu", model);
        }

        public ActionResult GetSection(string view)
        {
            var dataSource = MockHelper.GetMockProducts(false);
            dataSource.TotalNrElements = dataSource.ProductDataSource.Count;

            if (MockHelper.GetGridPaginationViews.Contains(view))
            {
                dataSource.ProductDataSource = dataSource.ProductDataSource.Take(5).ToList();
            }

            return PartialView(view, dataSource);
        }

        public ActionResult Page(GridDataSourceRequest<Product> gridDataSourceRequest, string SortColumnName,
            string DescriptionFiltersSort, FormCollection form, List<FilterGroup> filterGroups,
            string ClientName, string ClientId, bool? productFilterList, string additionalClient, string additionalFilter)
        {
            try
            {
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



                //data[3].Checked = true;

                var datasouce = new GridDataSourceResponse<Product>
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

        public ActionResult GetPage(GridDataSourceRequest<Product> request)
        {
            try
            {
                var list = MockHelper.GetMockProducts(false).ProductDataSource.AsEnumerable();

                var totalItems = list.Count();

                var data = list.Skip(request.PageInfo.Skip)
                               .Take(request.PageInfo.PageItems)
                               .ToList();

                var datasouce = new GridDataSourceResponse<Product>
                {
                    DataSource = data,
                    TotalItems = totalItems,
                    PageIndex = request.PageInfo.PageIndex
                };

                return Json(datasouce);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(ex.Message);
            }
        }

        public ActionResult GetPageSortedBy(GridDataSourceRequest<Product> request,
            string idOrderBy, string nameOrderBy, string descriptionOrderBy, string typeOrderBy)
        {

            var list = MockHelper.GetMockProducts(idOrderBy, nameOrderBy, descriptionOrderBy, typeOrderBy).ToList();

            var datasouce = new GridDataSourceResponse<Product>
            {
                DataSource = list
            };

            return Json(datasouce);
        }

        public ActionResult GridRefresh(GridDataSourceRequest<Product> request,
            string searchValue, string productId)
        {

            var list = MockHelper.GetMockProducts(false).ProductDataSource.AsEnumerable();

            var totalItems = list.Count();

            var data = list.Skip(request.PageInfo.Skip)
                               .Take(request.PageInfo.PageItems)
                               .ToList();

            var datasouce = new GridDataSourceResponse<Product>
            {
                DataSource = data,
                TotalItems = totalItems,
                PageIndex = request.PageInfo.PageIndex
            };

            return Json(datasouce);
        }

        public ActionResult GridNavigateToPage(GridDataSourceRequest<Product> request, List<FilterGroup> filterGroups, bool? setFirstPage, FormCollection form)
        {
            try
            {
                var list = MockHelper.GetMockProducts(false).ProductDataSource.AsEnumerable();

                if (setFirstPage.HasValue && setFirstPage.Value)
                {
                    request.PageInfo.PageIndex = 1;                    
                }

                if (filterGroups != null)
                {
                    list = MockHelper.FilterByGroup(list, filterGroups);
                }

                var totalItems = list.Count();

  
                var data = list.Skip(request.PageInfo.Skip)
                          .Take(request.PageInfo.PageItems)
                          .ToList();               

                var datasouce = new GridDataSourceResponse<Product>
                {
                    DataSource = data,
                    TotalItems = totalItems,
                    PageIndex = request.PageInfo.PageIndex
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