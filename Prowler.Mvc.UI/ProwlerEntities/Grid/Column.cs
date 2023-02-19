using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prowler.Mvc.UI
{
    public class Column
    {       
        internal string RowBinding { get; set; }
        internal int Width { get; set; }
        internal string ColumnTemplate { get; set; }
        internal string Title { get; set; }
        internal string RowTemplate { get; set; }
        internal List<string> RowTemplateBindings { get; set; }
        internal Dictionary<string, string> HtmlAttributes { get; set; }
        internal string SortName { get; set; }
        internal bool AllowColumnResize { get; set; } = true;
        internal string RowTemplateName { get; set; }
        internal bool AsEditable { get; set; }
        internal string ValidationEvent { get; set; }
        internal GridInputType EditableInputType { get; set; }
        internal bool HeaderAsCheckbox { get; set; }
        internal bool HeaderCheckboxValue { get; set; }
        internal string HeaderCheckboxLabel { get; set; }
        internal bool AsReadOnlyInput { get; set; }
        internal string AsReadOnlyInputBinding { get; set; }

        internal bool HasRowTemplate 
        { 
            get 
            {
                return RowTemplate != null;
            } 
        }
    }
}
