using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prowler.Mvc.UI
{
    public static class ProwlerGridPaginationExtension
    {
        public static Pagination PageIndex(this Pagination entity, int pageIndex)
        {
            entity.PageIndex = pageIndex;

            return entity;
        }
        public static Pagination Total(this Pagination entity, int total)
        {
            entity.Total = total;

            return entity;
        }

        public static Pagination PageItems(this Pagination entity, int pageItems)
        {
            entity.PageItems = pageItems;

            return entity;
        }

        public static Pagination Url(this Pagination entity, string url)
        {
            entity.Url = url;

            return entity;
        }

        public static Pagination PaginationNumbersMax(this Pagination entity, int max)
        {
            entity.PaginationNumbersMax = max;

            return entity;
        }

    }
}
