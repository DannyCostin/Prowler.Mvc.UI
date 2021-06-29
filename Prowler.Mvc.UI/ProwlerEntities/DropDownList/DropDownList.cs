using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Prowler.Mvc.UI
{
    public class DropDownList<TModel>
    {
        internal Prowler<TModel> Prowler { get; set; }
        internal string Name { get; set; }
        internal string DataTextField { get; set; }
        internal string DataValueField { get; set; }        
        internal IEnumerable<dynamic> DataSource { get; set; }
        internal string OptionLabel { get; set; }
        internal string OptionLabelTemplate { get; set; }
        internal string Template { get; set; }
        internal List<string> TemplateField { get; set; }
        internal string FilterField { get; set; }
        internal bool ClientFilteringEnable { get; set; }        
        internal int SelectedIndex { get; set; } = -1;
        internal string SelectedValue { get; set; }
        internal Dictionary<string, string> HtmlAttributes { get; set; }
        internal int Height { get; set; } = 310;
        internal bool Multiselect { get; set; }
        internal string MultiselectBindingProperty { get; set; }        
        internal bool MultiselectRenderCheckBox { get; set; }
        internal StringBuilder MultiselectSelectorHtml { get; set; }
        internal string GroupByValueProperty { get; set; }
        internal string GroupByLabelProperty { get; set; }
        internal Dictionary<string, StringBuilder> GroupByList { get; set; }
        internal bool Disabled { get; set; }
        internal Dictionary<string, string> Events { get; set; }
        internal bool ServerFilteringEnable { get; set; }
        internal string ServerFilteringUrl { get; set; }
        internal int ServerFilteringDelay { get; set; }
        internal string ServerFilteringSerializationName { get; set; }
    }
}