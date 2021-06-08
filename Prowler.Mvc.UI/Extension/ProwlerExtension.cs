using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prowler.Mvc.UI
{
    public static class ProwlerExtension 
    {
        public static Prowler<TModel> Prowler<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Prowler<TModel>
            {
                HtmlHelper = htmlHelper
            };
        }       

        public static DropDownList<TModel> DropDownList<TModel>(this Prowler<TModel> prowler)
        {
            return new DropDownList<TModel>()
            {
                Prowler = prowler
            };
        }
    }
}