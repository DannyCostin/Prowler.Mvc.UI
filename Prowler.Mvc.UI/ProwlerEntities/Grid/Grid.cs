using Prowler.Mvc.UI.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prowler.Mvc.UI
{
    public class Grid<TModel>
    {
        public Grid()
        {
            TableTemplate = new Template();
        }

        internal Prowler<TModel> Prowler { get; set; }
        internal string Name { get; set; }
        internal IEnumerable<dynamic> DataSource { get; set; }
        internal List<Column> Columns { get; set; }
        internal Template TableTemplate {get; set;}       
        internal bool AllowColumnResize { get; set; }
        internal Pagination Pagination { get; set; }
        internal string ActionSort { get; set; }
        internal int Height { get; set; }
        internal int Width { get; set; }
        internal string CurrentRowItemIndex { get; set; }
        internal Dictionary<string, string> HtmlAttributes { get; set; }
        internal string FilterContainerId { get; set; }
        internal string UniqueId { get; set; }
        internal string ErrorFunction { get; set; }
        internal string DataBindedFunction { get; set; }
        internal bool AutoSizeHeaders { get; set; }
        internal string ToolBarTemplate { get; set; }
    }
}
