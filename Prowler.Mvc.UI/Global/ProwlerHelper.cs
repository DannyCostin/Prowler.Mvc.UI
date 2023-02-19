using Prowler.Mvc.UI.Global.Struct;
using Prowler.Mvc.UI.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Prowler.Mvc.UI.Global
{
    internal static class ProwlerHelper
    {
        internal static string GetSvgSearchIcon()
        {
            var svgSearch = new TagBuilder(TagElement.Svg);
            var svgSearchPath0 = new TagBuilder(TagElement.Path);
            var svgSearchPath1 = new TagBuilder(TagElement.Path);

            svgSearch.MergeAttribute("version", "1.1");
            svgSearch.MergeAttribute("xmlns", "http://www.w3.org/2000/svg");
            svgSearch.MergeAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");
            svgSearch.MergeAttribute("x", "0px");
            svgSearch.MergeAttribute("y", "0px");
            svgSearch.MergeAttribute("width", "20px");
            svgSearch.MergeAttribute("height", "20px");
            svgSearch.MergeAttribute("viewBox", "0 0 24 24");
            svgSearch.MergeAttribute("enable-background", "new 0 0 512 512");
            svgSearch.MergeAttribute("xml:space", "preserve");
            svgSearch.AddCssClass(CssGlobal.SvgSearchIcon);

            svgSearchPath0.MergeAttribute("d", "M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z");
            svgSearchPath1.MergeAttribute("d", "M0 0h24v24H0z");
            svgSearchPath1.MergeAttribute("fill", "none");

            svgSearch.InnerHtml = String.Concat(svgSearchPath0.ToString(), svgSearchPath1.ToString());

            return svgSearch.ToString();
        }

        internal static string ApplyTemplateValues(List<string> templateField, string template, dynamic item)
        {
            if (templateField == null)
            {
                return template;
            }

            foreach (var bindingField in templateField)
            {
                var bindingValue = ProwlerHelper.GetPropValue<string>(item, bindingField);

                template = template.Replace(String.Concat("{#", bindingField, "#}"), bindingValue);
            }

            return template;
        }

        internal static string GetBindingString(string property)
        {
            return String.Concat("{#", property, "#}");
        }

        internal static TagBuilder TagSetInnerHtml(this TagBuilder tag, TagBuilder source, bool asAppend = true)
        {
            if (asAppend)
            {
                tag.InnerHtml = string.Concat(tag.InnerHtml, source.ToString());

                return tag;
            }

            tag.InnerHtml = source.ToString();

            return tag;
        }

        internal static TagBuilder TagSetInnerHtmlParent(this TagBuilder tag, TagBuilder source, bool asAppend = true)
        {
            if (asAppend)
            {
                tag.InnerHtml = string.Concat(tag.InnerHtml, source.ToString());

                return tag;
            }

            tag.InnerHtml = source.ToString();

            return tag;
        }

        internal static TagBuilder TagSetName(this TagBuilder tag, string controlName, string columnName, string index = null)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                if (string.IsNullOrEmpty(index))
                {
                    tag.MergeAttribute("name", columnName);
                }
                else
                {
                    tag.MergeAttribute("name", $"[{index}].{columnName}");
                }

                return tag;
            }
            else
            {
                if (string.IsNullOrEmpty(index))
                {
                    tag.MergeAttribute("name", $"{controlName}.{columnName}");
                }
                else
                {
                    tag.MergeAttribute("name", $"{controlName}[{index}].{columnName}");
                }
            }

            return tag;
        }

        internal static TagBuilder TagSetName(this TagBuilder tag, string name)
        {
            if (name != null)
            {
                tag.MergeAttribute("name", name);
            }

            return tag;
        }

        internal static TagBuilder TagSetValue(this TagBuilder tag, string value)
        {
            tag.MergeAttribute("value", value);

            return tag;
        }

        internal static TagBuilder TagAsTextBox(this TagBuilder tag)
        {
            tag.MergeAttribute("type", "text");

            return tag;
        }

        internal static TagBuilder TagAsTextHidden(this TagBuilder tag)
        {
            tag.MergeAttribute("type", "hidden");

            return tag;
        }

        internal static TagBuilder TagAsCheckBox(this TagBuilder tag)
        {
            tag.MergeAttribute("type", "checkbox");

            return tag;
        }

        internal static TagBuilder TagSetChecked(this TagBuilder tag)
        {
            tag.MergeAttribute("checked", "checked");

            return tag;
        }

        internal static TagBuilder TagDisable(this TagBuilder tag)
        {
            tag.MergeAttribute("disable", "disable");

            return tag;
        }

        internal static TagBuilder TagDisplayNone(this TagBuilder tag)
        {
            tag.MergeAttribute("style", "display:none");

            return tag;
        }

        internal static T GetPropValue<T>(object src, string propName)
        {
            if (src == null)
            {
                return default;
            }

            try
            {
                object result;

                if (IsSubProperty(propName))
                {
                    result = GetSubPropValue<T>(src, propName);
                }
                else
                {
                    result = GetPropValueSingle<T>(src, propName);
                }

                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        private static T GetPropValueSingle<T>(object src, string propName)
        {
            if (src == null)
            {
                return default;
            }

            try
            {
                var result = src.GetType().GetProperty(propName ?? string.Empty)
                                       ?.GetValue(src, null);

                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        private static object GetPropValueObject(object src, string propName)
        {
            if (src == null)
            {
                return null;
            }

            try
            {
                return src.GetType().GetProperty(propName ?? string.Empty)
                                   ?.GetValue(src, null);
            }
            catch
            {
                return null;
            }
        }

        private static T GetSubPropValue<T>(object src, string propName)
        {
            if (src == null)
            {
                return default;
            }

            try
            {
                var propertyList = GetSubProperties(propName);
                dynamic prop = src;

                foreach (var item in propertyList)
                {
                    prop = GetPropValueObject(prop, item);
                }

                return prop;
            }
            catch
            {
                return default;
            }
        }

        private static List<string> GetSubProperties(string property)
        {
            if (property == null)
            {
                return new List<string>();
            }

            return property.Split('.').ToList();
        }

        private static bool IsSubProperty(string propertyName)
        {
            var propertyList = GetSubProperties(propertyName);

            if (propertyList.Any() && propertyList.Count > 1)
            {
                return true;
            }

            return false;
        }
    }
}
