using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prowler.Presentation.Models
{
    public class GridMock
    {
        public bool Checked { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Product SingleProduct { get; set; }
        public List<Product> MultiProducts { get; set; }
    }

    public class GridMockList
    {
        public List<GridMock> GridData { get; set; }
    }
}