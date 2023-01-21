using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prowler.Presentation.Models
{
    public class SideMenuModel
    {
        public string MenuTitle { get; set; }        
        public List<SideMenuItemModel> DataSource { get; set; }
    }
}