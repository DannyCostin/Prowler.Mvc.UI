using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prowler.Mvc.UI
{
    public class Pagination
    {
        public int PageIndex { get; set; }
        public int Total { get; set; }
        public int PageItems { get; set; }
        public string Url { get; set; }
        public bool ServerSide { get; set; } = true;
        public int PaginationNumbersMax { get; set; } = 5;
    }
}
