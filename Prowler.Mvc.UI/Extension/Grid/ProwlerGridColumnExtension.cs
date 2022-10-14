using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Prowler.Mvc.UI
{
    public static class ProwlerGridColumnExtension
    {
        public static Column BindTo(this Column entity, string binding)
        {
            entity.RowBinding = binding;

            return entity;
        }

        public static Column Width(this Column entity, int width)
        {
            entity.Width = width;

            return entity;
        }

        public static Column ColumnTemplate(this Column entity, string template)
        {
            entity.ColumnTemplate = template;

            return entity;
        }

        public static Column ColumnTemplate(this Column entity, IHtmlString template)
        {
            entity.ColumnTemplate = template.ToString();

            return entity;
        }

        public static Column RowTemplate(this Column entity, string template, params string[] templateBindings)
        {
            entity.RowTemplate = template;
            entity.RowTemplateBindings = templateBindings?.ToList();

            return entity;
        }

        private static Column RowTemplate(this Column entity, IProwlerControl template)
        {
            entity.RowTemplate = template.Render().ToHtmlString();
            entity.RowTemplateName = template.GetName();

            return entity;
        }

        public static Column RowTemplate(this Column entity, IHtmlString template, params string[] templateBindings)
        {
            entity.RowTemplate = template.ToString();
            entity.RowTemplateBindings = templateBindings?.ToList();

            return entity;
        }

        public static Column Title(this Column entity, string columnTitle)
        {
            entity.Title = columnTitle;

            return entity;
        }

        public static Column Sorting(this Column entity, string name)
        {
            entity.SortName = name;

            return entity;
        }

        public static Column HtmlAttributes(this Column entity, string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return entity;
            }

            if (entity.HtmlAttributes == null)
            {
                entity.HtmlAttributes = new Dictionary<string, string>();
            }

            if (!entity.HtmlAttributes.ContainsKey(key))
            {
                entity.HtmlAttributes.Add(key, value);
            }            

            return entity;
        }

        public static Column AsEditable(this Column entity, GridInputType inputType)
        {
            entity.AsEditable = true;
            entity.EditableInputType = inputType;

            return entity;
        }

        public static Column HeaderAsCheckbox(this Column entity, string label = null, bool stateChecked = false)
        {
            entity.HeaderAsCheckbox = true;
            entity.HeaderCheckboxValue = stateChecked;
            entity.HeaderCheckboxLabel = label;

            return entity;
        }

        public static Column AsReadOnly(this Column entity, string readOnlyBinding = null)
        {
            entity.AsReadOnlyInput = true;
            entity.AsReadOnlyInputBinding = readOnlyBinding;

            return entity;
        }
    }
}
