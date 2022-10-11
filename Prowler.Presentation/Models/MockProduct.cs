using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prowler.Presentation.Models
{
    public class MockProduct
    {
        public List<Product> ProductDataSource { get; set; }
        public Product SingleProduct { get; set; }
        public List<Product> MultiProducts { get; set; }
        public List<FilterGroup> FilterGroups { get; set; }

    }  

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Deleted { get; set; }
        public bool Disable { get; set; }
    }

    public class FilterGroup
    {
        public bool Checked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}