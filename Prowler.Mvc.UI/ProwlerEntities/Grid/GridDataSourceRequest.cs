using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prowler.Mvc.UI
{
    public class GridDataSourceRequest<T> where T : class
    {
       public GridDataSourcePagination PageInfo { get; set; }
       public Filters Filter { get; set; }
       public List<T> DataSource { get; set; }
    }

    public class GridDataSourcePagination
    {
        public int PageIndex { get; set; }        
        public int PageItems { get; set; }
    }
}
