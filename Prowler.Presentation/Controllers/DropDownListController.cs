using Prowler.Presentation.Helpers;
using Prowler.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}