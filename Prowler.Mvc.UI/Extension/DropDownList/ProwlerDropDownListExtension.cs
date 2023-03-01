using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Prowler.Mvc.UI.Struct;
using Prowler.Mvc.UI.Global;
using Prowler.Mvc.UI.Global.Struct;

namespace Prowler.Mvc.UI
{
    public static class ProwlerDropDownListExtension
    {
        public static DropDownList<TModel> Name<TModel>(this DropDownList<TModel> entity, string name)
        {
            entity.Name = name;

            return entity;
        }

        public static DropDownList<TModel> DataTextField<TModel>(this DropDownList<TModel> entity, string dataTextField)
        {
            entity.DataTextField = dataTextField;

            return entity;
        }

        public static DropDownList<TModel> DataValueField<TModel>(this DropDownList<TModel> entity, string dataValueField)
        {
            entity.DataValueField = dataValueField;

            return entity;
        }

        public static DropDownList<TModel> Template<TModel>(this DropDownList<TModel> entity, string template, params string[] templateBindings)
        {
            entity.Template = template;

            if (templateBindings != null)
            {
                entity.TemplateField = templateBindings.ToList();
            }

            return entity;
        }

        public static DropDownList<TModel> Template<TModel>(this DropDownList<TModel> entity, MvcHtmlString template, params string[] templateBindings)
        {
            entity.Template = template.ToString();

            if (templateBindings != null)
            {
                entity.TemplateField = templateBindings.ToList();
            }

            return entity;
        }

        public static DropDownList<TModel> SelectedIndex<TModel>(this DropDownList<TModel> entity, int index)
        {
            entity.SelectedIndex = index;

            return entity;
        }

        public static DropDownList<TModel> SelectedValue<TModel>(this DropDownList<TModel> entity, string value)
        {
            entity.SelectedValue = value;

            return entity;
        }

        public static DropDownList<TModel> Height<TModel>(this DropDownList<TModel> entity, int height)
        {
            entity.Height = height;

            return entity;
        }

        public static DropDownList<TModel> Multiselect<TModel>(this DropDownList<TModel> entity, string bindingProperty, bool renderCheckBox = false)
        {
            entity.Multiselect = true;
            entity.MultiselectBindingProperty = bindingProperty;
            entity.MultiselectRenderCheckBox = renderCheckBox;

            return entity;
        }

        public static DropDownList<TModel> GroupBy<TModel>(this DropDownList<TModel> entity, string bindingKey, string bindingLabel)
        {
            entity.GroupByValueProperty = bindingKey;
            entity.GroupByLabelProperty = bindingLabel;

            return entity;
        }

        public static DropDownList<TModel> HtmlAttributes<TModel>(this DropDownList<TModel> entity, string key, string value)
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

        public static DropDownList<TModel> ClientFiltering<TModel>(this DropDownList<TModel> entity, string binding)
        {
            if (binding == null)
            {
                return entity;
            }

            entity.FilterField = binding;
            entity.ClientFilteringEnable = true;
            entity.ServerFilteringEnable = false;

            return entity;
        }

        public static DropDownList<TModel> ServerFiltering<TModel>(this DropDownList<TModel> entity, string url, string parameterName, int delay = 500, int minCharToSearch = 0, bool allowEmptySearch = true)
        {
            if(url == null)
            {
                return entity;
            }

            entity.ClientFilteringEnable = false;
            entity.ServerFilteringEnable = true;
            entity.ServerFilteringUrl = url;
            entity.ServerFilteringDelay = delay;
            entity.ServerFilteringSerializationName = parameterName;
            entity.ServerFilteringMinimumCharToSearch = minCharToSearch;
            entity.ServerFilterningAllowEmptySearch = allowEmptySearch;

            return entity;
        }

        public static DropDownList<TModel> BindTo<TModel>(this DropDownList<TModel> entity, IEnumerable<dynamic> dataSource, string optionLabel = null)
        {
            entity.DataSource = dataSource;
            entity.OptionLabel = optionLabel;

            return entity;
        }

        public static DropDownList<TModel> OptionLabelTemplate<TModel>(this DropDownList<TModel> entity, string optionLabelTemplate)
        {
            entity.OptionLabelTemplate = optionLabelTemplate;

            return entity;
        }

        public static DropDownList<TModel> OptionLabelTemplate<TModel>(this DropDownList<TModel> entity, MvcHtmlString template)
        {
            entity.OptionLabelTemplate = template.ToString();

            return entity;
        }

        public static DropDownList<TModel> Event<TModel>(this DropDownList<TModel> entity, EventDropDown eventDropDown, string funcName)
        {
            if(entity.Events == null)
            {
                entity.Events = new Dictionary<string, string>();
            }

            if (!entity.Events.ContainsKey(eventDropDown.GetEventName()))
            {
                entity.Events.Add(eventDropDown.GetEventName(), funcName);
            }

            return entity;
        }

        public static DropDownList<TModel> Disabled<TModel>(this DropDownList<TModel> entity, bool disable = true)
        {
            entity.Disabled = disable;

            return entity;
        }

        public static IProwlerControl RenderTemplate<TModel>(this DropDownList<TModel> entity)
        {
            return entity;
        }

        public static IHtmlString Render<TModel>(this DropDownList<TModel> entity)
        {
            var parentList = new TagBuilder(TagElement.Div);
            var dropDownArea = new TagBuilder(TagElement.Div);
            var scrollArea = new TagBuilder(TagElement.Div);

            string selectedItem = string.Empty;
            
            parentList.AddCssClass(entity.Disabled ? CssDropDownList.ParentDisable : CssDropDownList.ParentContainer);
            MergeHtmlAttributes(parentList, entity.HtmlAttributes);
            ApplyEvents(entity, parentList);          

            dropDownArea.MergeAttribute("style", "height:inherit;position:relative");
            dropDownArea.AddCssClass(entity.Disabled ? CssDropDownList.AreaDisable : CssDropDownList.Area);

            scrollArea.MergeAttribute("style", $"max-height:{entity.Height}px;display:none");
            scrollArea.AddCssClass(CssDropDownList.ScrollArea);

            string template = BuildDropDownListTemplate(entity, scrollArea, out selectedItem);

            var selectedItemContainer = new TagBuilder(TagElement.Div);
            selectedItemContainer.AddCssClass(CssDropDownList.SelectedContainer);
            selectedItemContainer.InnerHtml = selectedItem;

            dropDownArea.InnerHtml = string.Concat(selectedItemContainer.ToString(), SvgArrowDown());
            scrollArea.InnerHtml = string.Concat(template, scrollArea.InnerHtml);

            var dropDownListContainer = new TagBuilder(TagElement.Div);
            dropDownListContainer.AddCssClass(CssDropDownList.ListBoxContainer);
            dropDownListContainer.InnerHtml = string.Concat(CreateFilterArea(entity), scrollArea.ToString());

            parentList.InnerHtml = string.Concat(dropDownArea.ToString(), dropDownListContainer.ToString());
            ApplyServerFilter(entity, parentList);
            var html = parentList.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(html);
        }

        private static string SvgArrowDown()
        {
            var svgArrow = new TagBuilder(TagElement.Svg);
            var svgArrowPath = new TagBuilder(TagElement.Path);

            svgArrow.MergeAttribute("version", "1.1");
            svgArrow.MergeAttribute("xmlns", "http://www.w3.org/2000/svg");
            svgArrow.MergeAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");
            svgArrow.MergeAttribute("x", "0px");
            svgArrow.MergeAttribute("y", "0px");
            svgArrow.MergeAttribute("width", "20px");
            svgArrow.MergeAttribute("height", "20px");
            svgArrow.MergeAttribute("viewBox", "0 0 512 512");
            svgArrow.MergeAttribute("enable-background", "new 0 0 512 512");
            svgArrow.MergeAttribute("xml:space", "preserve");
            svgArrow.MergeAttribute("style", "position:absolute;top:2px;right:5px;height:100%");
            svgArrow.AddCssClass(CssDropDownList.DownArowIcon);

            svgArrowPath.MergeAttribute("d", "M256,352L128,160h256L256,352z");

            svgArrow.InnerHtml = svgArrowPath.ToString();

            return svgArrow.ToString();
        }

        private static string SvgSearch()
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
            svgSearch.AddCssClass(CssDropDownList.SvgSearchIcon);

            svgSearchPath0.MergeAttribute("d", "M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z");
            svgSearchPath1.MergeAttribute("d", "M0 0h24v24H0z");
            svgSearchPath1.MergeAttribute("fill", "none");

            svgSearch.InnerHtml = String.Concat(svgSearchPath0.ToString(), svgSearchPath1.ToString());

            return svgSearch.ToString();
        }

        private static string CreateFilterArea<TModel>(DropDownList<TModel> entity)
        {
            if (entity.ClientFilteringEnable || entity.ServerFilteringEnable)
            {
                TagBuilder container = new TagBuilder(TagElement.Div);
                TagBuilder inputSearch = new TagBuilder(TagElement.Input);
                TagBuilder layer = new TagBuilder(TagElement.Div);

                layer.AddCssClass(CssDropDownList.FilterLayer);
                container.AddCssClass(CssDropDownList.FilterContainer);
                container.MergeAttribute("style", "display:none");

                inputSearch.MergeAttribute("type", "text");
                inputSearch.AddCssClass(CssDropDownList.FilterInput);
                layer.InnerHtml = String.Concat(inputSearch.ToString(), SvgSearch());
                container.InnerHtml = layer.ToString();
                return container.ToString();
            }

            return string.Empty;
        }

        private static void MergeHtmlAttributes(TagBuilder tag, Dictionary<string, string> htmlAttributes)
        {
            if (htmlAttributes == null || !htmlAttributes.Any())
            {
                return;
            }

            foreach (var item in htmlAttributes)
            {
                tag.MergeAttribute(item.Key, item.Value);
            }
        }

        private static void ApplyGroupBy<TModel>(DropDownList<TModel> entity, dynamic item, TagBuilder container, StringBuilder htmlBuilder)
        {
            if (entity.GroupByValueProperty == null)
            {
                htmlBuilder.Append(container.ToString());
                return;
            }

            if (entity.GroupByList == null)
            {
                entity.GroupByList = new Dictionary<string, StringBuilder>();
            }

            var groupKey = ProwlerHelper.GetPropValue<string>(item, entity.GroupByValueProperty);

            StringBuilder groupStringBuilder = null;

            if (entity.GroupByList.ContainsKey(groupKey))
            {
                entity.GroupByList.TryGetValue(groupKey, out groupStringBuilder);

                if (groupStringBuilder == null)
                {
                    groupStringBuilder = new StringBuilder();
                }
            }
            else
            {
                groupStringBuilder = new StringBuilder();
                entity.GroupByList.Add(groupKey, groupStringBuilder);

                var groupContainer = new TagBuilder(TagElement.Div);
                var label = ProwlerHelper.GetPropValue<string>(item, entity.GroupByLabelProperty);

                if (!string.IsNullOrEmpty(label))
                {

                    var groupLabel = new TagBuilder(TagElement.Span);
                    groupLabel.SetInnerText(label);
                    groupLabel.AddCssClass(CssDropDownList.GroupLabel);
                    groupContainer.InnerHtml = groupLabel.ToString();
                    groupContainer.AddCssClass(CssDropDownList.GroupContainer);
                }

                groupStringBuilder.Append(groupContainer.ToString());
            }

            groupStringBuilder.Append(container.ToString());
        }

        private static void MergeGroupBy<TModel>(DropDownList<TModel> entity, StringBuilder htmlBuilder)
        {
            if (entity.GroupByList == null || !entity.GroupByList.Any())
            {
                return;
            }

            foreach (var item in entity.GroupByList)
            {
                htmlBuilder.Append(item.Value.ToString());
            }
        }

        private static void ApplyMultiSelectSelector<TModel>(DropDownList<TModel> entity, ref string selectedItemTemplate)
        {
            if (entity.Multiselect && entity.MultiselectSelectorHtml != null)
            {
                var multiSelectionContainer = new TagBuilder(TagElement.Div);
                multiSelectionContainer.AddCssClass(CssDropDownList.MultiselectSelectionContainer);

                multiSelectionContainer.InnerHtml = entity.MultiselectSelectorHtml.ToString();
                selectedItemTemplate = multiSelectionContainer.ToString();
            }
        }

        private static void BuildMultiselectTemplate<TModel>(DropDownList<TModel> entity, TagBuilder container)
        {
            if (!entity.Multiselect)
            {
                return;
            }

            var checkState = $"{{#{entity.MultiselectBindingProperty}#}}";
            var label = $"{{#{entity.DataTextField}#}}";
            var value = $"{{#{entity.DataValueField}#}}";

            var groupByValue = $"{{#{entity.GroupByValueProperty}#}}";
            
            var checkBoxContainer = new TagBuilder(TagElement.Label);
            var checkBoxIcon = new TagBuilder(TagElement.Span);
            var checkbox = new TagBuilder(TagElement.Input);

            checkBoxContainer.AddCssClass(CssDropDownList.MultiselectContainer);
            checkBoxIcon.AddCssClass(CssDropDownList.MultiselectCheckMark);

            checkbox.MergeAttribute("type", "checkbox");
            checkbox.MergeAttribute(AttributeDropDownList.SerializationName, entity.Name);
            checkbox.MergeAttribute(AttributeDropDownList.SerializationValue, entity.DataValueField);
            checkbox.MergeAttribute(AttributeDropDownList.SerializationCheckValue, entity.MultiselectBindingProperty);
            checkbox.MergeAttribute(AttributeDropDownList.SerializationGroupValue, entity.GroupByValueProperty);
            checkbox.MergeAttribute(AttributeDropDownList.PropertyValueName, label);
            checkbox.MergeAttribute(AttributeDropDownList.PropertyValueValue, value);
            checkbox.MergeAttribute(AttributeDropDownList.PropertyCheckValue, checkState.ToString());
            checkbox.MergeAttribute(AttributeDropDownList.PropertyGroupValue, groupByValue);
            checkbox.MergeAttribute(AttributeDropDownList.PropertyIndex, $"{{#{AttributeDropDownList.PropertyIndex}#}}");

            checkbox.AddCssClass(CssDropDownList.ListElementMultiselect);

            checkBoxContainer.InnerHtml = string.Concat(checkbox.ToString(), checkBoxIcon.ToString());

            if (!entity.MultiselectRenderCheckBox)
            {
                checkBoxContainer.MergeAttribute("style", "display:none");
            }

            container.InnerHtml = string.Concat(container.InnerHtml, checkBoxContainer.ToString());
        }

        private static void ApplyMultiSelect<TModel>(DropDownList<TModel> entity, TagBuilder parent, TagBuilder container,
            int index, dynamic item)
        {

            if (entity.Multiselect)
            {
                var checkState = ProwlerHelper.GetPropValue<bool>(item, entity.MultiselectBindingProperty);
                var label = ProwlerHelper.GetPropValue<string>(item, entity.DataTextField);
                var value = ProwlerHelper.GetPropValue<string>(item, entity.DataValueField);
                dynamic groupByValue = string.Empty;

                if (entity.GroupByValueProperty != null)
                {
                    groupByValue = ProwlerHelper.GetPropValue<string>(item, entity.GroupByValueProperty);
                }

                var checkBoxContainer = new TagBuilder(TagElement.Label);
                var checkBoxIcon = new TagBuilder(TagElement.Span);
                var checkbox = new TagBuilder(TagElement.Input);

                checkBoxContainer.AddCssClass(CssDropDownList.MultiselectContainer);
                checkBoxIcon.AddCssClass(CssDropDownList.MultiselectCheckMark);

                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute(AttributeDropDownList.SerializationName, entity.Name);
                checkbox.MergeAttribute(AttributeDropDownList.SerializationValue, entity.DataValueField);
                checkbox.MergeAttribute(AttributeDropDownList.SerializationCheckValue, entity.MultiselectBindingProperty);
                checkbox.MergeAttribute(AttributeDropDownList.SerializationGroupValue, entity.GroupByValueProperty);
                checkbox.MergeAttribute(AttributeDropDownList.PropertyValueName, label);
                checkbox.MergeAttribute(AttributeDropDownList.PropertyValueValue, value);
                checkbox.MergeAttribute(AttributeDropDownList.PropertyCheckValue, checkState.ToString());
                checkbox.MergeAttribute(AttributeDropDownList.PropertyGroupValue, groupByValue);
                checkbox.MergeAttribute(AttributeDropDownList.PropertyIndex, $"{index}");

                checkbox.AddCssClass(CssDropDownList.ListElementMultiselect);

                if (checkState)
                {
                    if (!entity.MultiselectRenderCheckBox)
                    {
                        container.AddCssClass(CssDropDownList.ListElementSelected);
                    }

                    checkbox.MergeAttribute("checked", "checked");

                    var checkStateHiddenInput = new TagBuilder(TagElement.Input);
                    checkStateHiddenInput.MergeAttribute("type", "hidden");
                    checkStateHiddenInput.MergeAttribute("name", $"{entity.Name}[{value}].{entity.MultiselectBindingProperty}");
                    checkStateHiddenInput.MergeAttribute("value", $"{checkState}");
                    checkStateHiddenInput.MergeAttribute("class", "p-dropdown-m-value");
                    checkStateHiddenInput.MergeAttribute("id", $"p-dr-index{index}");

                    var valueHiddenInput = new TagBuilder(TagElement.Input);
                    valueHiddenInput.MergeAttribute("type", "hidden");
                    valueHiddenInput.MergeAttribute("name", $"{entity.Name}[{value}].{entity.DataValueField}");
                    valueHiddenInput.MergeAttribute("value", $"{value}");

                    var groupValueHiddenInput = new TagBuilder(TagElement.Input);
                    groupValueHiddenInput.MergeAttribute("type", "hidden");
                    groupValueHiddenInput.MergeAttribute("name", $"{entity.Name}[{value}].{entity.GroupByValueProperty}");
                    groupValueHiddenInput.MergeAttribute("value", $"{groupByValue}");

                    var indexHiddenInput = new TagBuilder(TagElement.Input);
                    indexHiddenInput.MergeAttribute("type", "hidden");
                    indexHiddenInput.MergeAttribute("name", $"{entity.Name}.Index");
                    indexHiddenInput.MergeAttribute("value", $"{value}");

                    var containerHiddenInput = new TagBuilder(TagElement.Div);

                    containerHiddenInput.InnerHtml = string.Concat(checkStateHiddenInput.ToString(),
                                                        valueHiddenInput.ToString(),
                                                        groupValueHiddenInput.ToString(),
                                                        indexHiddenInput.ToString());

                    parent.InnerHtml = string.Concat(parent.InnerHtml, containerHiddenInput.ToString());

                    if (entity.MultiselectSelectorHtml == null)
                    {
                        entity.MultiselectSelectorHtml = new StringBuilder();
                    }

                    var multiSelectContainerElement = new TagBuilder(TagElement.Span);
                    multiSelectContainerElement.AddCssClass(CssDropDownList.MultiselectListContainerElement);
                    multiSelectContainerElement.SetInnerText(ProwlerHelper.GetPropValue<string>(item, entity.DataTextField));

                    entity.MultiselectSelectorHtml.Append(multiSelectContainerElement.ToString());

                }

                checkBoxContainer.InnerHtml = string.Concat(checkbox.ToString(), checkBoxIcon.ToString());

                if (!entity.MultiselectRenderCheckBox)
                {
                    checkBoxContainer.MergeAttribute("style", "display:none");
                }

                container.InnerHtml = string.Concat(container.InnerHtml, checkBoxContainer.ToString());
            }
        }

        private static void ApplyDefaultLabel(TagBuilder element, TagBuilder container)
        {
            container.InnerHtml = string.Concat(container.InnerHtml, element.ToString());
        }

        private static void ApplyPostSerializationElement<TModel>(DropDownList<TModel> entity, StringBuilder htmlBuilder, dynamic selectedItem)
        {
            if (entity.Multiselect)
            {
                return;
            }

            var inputValue = entity.Name == null ? entity.DataValueField
                                              : string.Concat(entity.Name, ".", entity.DataValueField);

            var valueHiddenInput = new TagBuilder(TagElement.Input);
            valueHiddenInput.MergeAttribute("type", "hidden");
            valueHiddenInput.MergeAttribute("id", "ps-s-value-selected");
            valueHiddenInput.MergeAttribute("value", ProwlerHelper.GetPropValue<string>(selectedItem, entity.DataValueField));
            valueHiddenInput.MergeAttribute("name", inputValue);

            if (entity.GroupByValueProperty != null)
            {
                var inputGroupValue = entity.Name == null ? entity.GroupByValueProperty
                                             : string.Concat(entity.Name, ".", entity.GroupByValueProperty);

                var groupValueHiddenInput = new TagBuilder(TagElement.Input);
                groupValueHiddenInput.MergeAttribute("type", "hidden");
                groupValueHiddenInput.MergeAttribute("id", "ps-s-value-group-selected");
                groupValueHiddenInput.MergeAttribute("value", ProwlerHelper.GetPropValue<string>(selectedItem, entity.GroupByValueProperty));
                groupValueHiddenInput.MergeAttribute("name", inputGroupValue);
                htmlBuilder.Append(groupValueHiddenInput);
            }

            htmlBuilder.Append(valueHiddenInput.ToString());
        }

        private static void ApplyClientFilter<TModel>(DropDownList<TModel> entity, dynamic item, TagBuilder container, bool returnTemplate = false)
        {
            if (entity.ClientFilteringEnable)
            {                
                string filter = ProwlerHelper.GetPropValue<string>(item, entity.FilterField);

                if (returnTemplate)
                {
                    filter = $"{{#{ entity.FilterField}#}}";
                }

                container.MergeAttribute(AttributeDropDownList.FilterValue, filter);
            }
        }

        private static void ApplyServerFilter<TModel>(DropDownList<TModel> entity, TagBuilder container)
        {
            if (entity.ServerFilteringEnable)
            {                
                container.MergeAttribute(AttributeDropDownList.ServerFilterUrl, entity.ServerFilteringUrl);
                container.MergeAttribute(AttributeDropDownList.ServerFilterDelay, entity.ServerFilteringDelay.ToString());
                container.MergeAttribute(AttributeDropDownList.ServerFilterSerializationName, entity.ServerFilteringSerializationName);
                container.MergeAttribute(AttributeDropDownList.ServerFilteringEnable, entity.ServerFilteringEnable.ToString());
                container.MergeAttribute(AttributeDropDownList.ServerFilterMinChar, entity.ServerFilteringMinimumCharToSearch.ToString());
                container.MergeAttribute(AttributeDropDownList.ServerFilterAllowEmptySearch, entity.ServerFilterningAllowEmptySearch.ToString());
            }

            container.InnerHtml = String.Concat(container.InnerHtml, GetTemplate(entity));
        }

        private static TagBuilder TemplateDefautDropDownItem<TModel>(DropDownList<TModel> entity, string label)
        {
            var templateElement = new TagBuilder(TagElement.Span);
            templateElement.SetInnerText(label);
            templateElement.AddCssClass(CssDropDownList.ListElement);

            if (entity.Multiselect)
            {
                if (entity.MultiselectRenderCheckBox)
                {
                    templateElement.MergeAttribute("style", "margin-left:25px;display:block;padding-top:5px;padding-bottom:5px;");
                }
                else
                {
                    templateElement.MergeAttribute("style", "margin-left:0px;display:block;padding-top:5px;padding-bottom:5px;");
                }
            }
            else
            {
                templateElement.MergeAttribute("style", "display:block;padding-top:5px;padding-bottom:5px;");
            }

            return templateElement;
        }

        private static TagBuilder TemplateDropDownItem<TModel>(DropDownList<TModel> entity, dynamic item, bool returnTemplate = false)
        {
            var templateElement = new TagBuilder(TagElement.Div);

            if (entity.Multiselect && entity.MultiselectRenderCheckBox)
            {
                templateElement.MergeAttribute("style", "margin-left:25px;display:block");
            }

            string template = entity.Template;

            foreach (var bindingField in entity.TemplateField)
            {
                var bindingValue = ProwlerHelper.GetPropValue<string>(item, bindingField);

                if (!returnTemplate)
                {
                    template = template.Replace(String.Concat("{#", bindingField, "#}"), bindingValue);
                }                
            }

            templateElement.InnerHtml = template;

            return templateElement;
        }

        private static void SetRenderCheckBox<TModel>(DropDownList<TModel> entity, TagBuilder container)
        {
            if (entity.MultiselectRenderCheckBox)
            {
                container.MergeAttribute(AttributeDropDownList.MultiselectCheckBoxRender, "1");
            }
            else
            {
                container.MergeAttribute(AttributeDropDownList.MultiselectCheckBoxRender, "0");
            }
        }

        private static void SetMultiselect<TModel>(DropDownList<TModel> entity, TagBuilder container)
        {
            if (entity.Multiselect)
            {
                container.MergeAttribute(AttributeDropDownList.MultiselectEnable, "1");
            }
        }

        private static void MarkDropDownItemAsSelected<TModel>(DropDownList<TModel> entity, bool markAsSelected, TagBuilder container)
        {
            if (markAsSelected && !entity.Multiselect)
            {
                container.AddCssClass(CssDropDownList.ListElementSelected);
            }
        }

        private static TagBuilder GetTemplate<TModel>(DropDownList<TModel> entity)
        {

            var containerTemplate = new TagBuilder(TagElement.Div);

            containerTemplate.AddCssClass(CssDropDownList.ItemTemplateBindingContainer);
            containerTemplate.MergeAttribute("style", "display:none");

            var container = new TagBuilder(TagElement.Div);
            string bindingProperties = string.Concat(entity.DataValueField, ",", entity.DataTextField);

            if (entity.TemplateField!= null && entity.TemplateField.Any())
            {
                var templateProperties = entity.TemplateField.Aggregate((i, j) => i + "," + j);
                bindingProperties = string.Concat(bindingProperties, ",", templateProperties);
            }
           
            if(entity.GroupByValueProperty != null)
            {
                bindingProperties = string.Concat(bindingProperties, ",", entity.GroupByValueProperty, ",", entity.GroupByLabelProperty);
                containerTemplate.MergeAttribute(AttributeDropDownList.BindingGroupKey, entity.GroupByValueProperty);
                containerTemplate.MergeAttribute(AttributeDropDownList.BindingLabelKey, entity.GroupByLabelProperty);
            }

            if (entity.Multiselect)
            {
                bindingProperties = string.Concat(bindingProperties, ",", entity.MultiselectBindingProperty);
                containerTemplate.MergeAttribute(AttributeDropDownList.BindingMultiselectKey, entity.MultiselectBindingProperty);
            }

            containerTemplate.MergeAttribute(AttributeDropDownList.Bindings, bindingProperties);            

            SetRenderCheckBox(entity, container);
            SetMultiselect(entity, container);

            ApplyClientFilter(entity, null, container, true);
            BuildMultiselectTemplate(entity, container);
            ApplyDefaultLabel(BuildDropdownElementTemplate(entity, null, container, false, true), container);

            containerTemplate.InnerHtml = container.ToString();

            return containerTemplate;
        }

        private static TagBuilder BuildDropdownElementTemplate<TModel>(DropDownList<TModel> entity, dynamic item, TagBuilder container, bool markAsSelected, bool returnTemplate = false)
        {
            var label = ProwlerHelper.GetPropValue<string>(item, entity.DataTextField);
            var value = ProwlerHelper.GetPropValue<string>(item, entity.DataValueField);
            var valueGroup = ProwlerHelper.GetPropValue<string>(item, entity.GroupByValueProperty);

            if (returnTemplate)
            {
                label = $"{{#{entity.DataTextField}#}}";
                value = $"{{#{entity.DataValueField}#}}";
                valueGroup = $"{{#{entity.GroupByValueProperty}#}}";
            }

            if (container != null)
            {
                container.AddCssClass(CssDropDownList.ContainerElement);
                container.MergeAttribute(AttributeDropDownList.SerializationName, entity.Name);
                container.MergeAttribute(AttributeDropDownList.SerializationValue, entity.DataValueField);
                container.MergeAttribute(AttributeDropDownList.PropertyValueName, $"{label}");
                container.MergeAttribute(AttributeDropDownList.PropertyValueValue, $"{value}");
                container.MergeAttribute(AttributeDropDownList.PropertyGroupValue, $"{valueGroup}");

                MarkDropDownItemAsSelected(entity, markAsSelected, container);
                SetRenderCheckBox(entity, container);
                SetMultiselect(entity, container);
                ApplyClientFilter(entity, item, container);                
            }

            TagBuilder templateElement = string.IsNullOrEmpty(entity.Template) ? TemplateDefautDropDownItem(entity, label)
                                                                               : TemplateDropDownItem(entity, item, returnTemplate);

            return templateElement;
        }

        private static string BuildDropDownListTemplate<TModel>(DropDownList<TModel> entity, TagBuilder parent, out string selectedItemTemplate)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            selectedItemTemplate = string.Empty;
            dynamic selectedItem = null;
            int index = 0;
            bool markAsSelected = false;
                        
            if (entity.DataSource == null)
            {
                return string.Empty;
            }                       

            if (entity.DataSource.Count() <= entity.SelectedIndex)
            {
                entity.SelectedIndex = -1;
            }

            foreach (var item in entity.DataSource)
            {
                var container = new TagBuilder(TagElement.Div);

                if (index == entity.SelectedIndex && string.IsNullOrEmpty(entity.SelectedValue))
                {
                    selectedItem = item;
                    selectedItemTemplate = BuildDropdownElementTemplate(entity, item, null, false).ToString();
                    markAsSelected = true;
                }

                if (!string.IsNullOrEmpty(entity.SelectedValue) 
                    && ProwlerHelper.GetPropValue<string>(item, entity.DataValueField) == entity.SelectedValue)
                {
                    selectedItem = item;
                    selectedItemTemplate = BuildDropdownElementTemplate(entity, item, null, false).ToString();
                    markAsSelected = true;
                }

                ApplyMultiSelect(entity, parent, container, index, item);
                ApplyDefaultLabel(BuildDropdownElementTemplate(entity, item, container, markAsSelected), container);
                ApplyGroupBy(entity, item, container, htmlBuilder);

                markAsSelected = false;
                index++;
            }

            ApplyOptionLabel(entity, htmlBuilder, ref selectedItemTemplate);
            ApplyMultiSelectSelector(entity, ref selectedItemTemplate);
            MergeGroupBy(entity, htmlBuilder);
            ApplyPostSerializationElement(entity, htmlBuilder, selectedItem);

            return htmlBuilder.ToString();
        }

        private static void ApplyOptionLabel<TModel>(DropDownList<TModel> entity, StringBuilder htmlBuilder, ref string selectedItemTemplate)
        {
            if (entity.OptionLabel == null && entity.OptionLabelTemplate == null)
            {
                return;
            }
           
            var container = new TagBuilder(TagElement.Div);
            container.AddCssClass(CssDropDownList.ContainerElement);
            container.MergeAttribute(AttributeDropDownList.SerializationName, entity.Name);
            container.MergeAttribute(AttributeDropDownList.SerializationValue, entity.DataValueField);
            container.MergeAttribute(AttributeDropDownList.PropertyValueName, "");
            container.MergeAttribute(AttributeDropDownList.PropertyValueValue, $"{-1}");
            container.MergeAttribute(AttributeDropDownList.SerializationGroupValue, $"{-1}");

            SetMultiselect(entity, container);

            TagBuilder templateElement = null;

            if (entity.OptionLabel != null)
            {
                templateElement = new TagBuilder(TagElement.Span);
                templateElement.SetInnerText(entity.OptionLabel);
                templateElement.MergeAttribute("style", "display:block;padding-top:5px;padding-bottom:5px;");
                templateElement.AddCssClass(CssDropDownList.ListElement);

                container.InnerHtml = templateElement.ToString();                
            }

            if(entity.OptionLabelTemplate != null)
            {
                templateElement = new TagBuilder(TagElement.Span);
                templateElement.AddCssClass(CssDropDownList.ListElement);
                templateElement.InnerHtml = entity.OptionLabelTemplate;

                container.InnerHtml = templateElement.ToString();                
            }
           
            htmlBuilder.Insert(0, container.ToString());

            if (entity.SelectedIndex < 0)
            {
                selectedItemTemplate = templateElement?.ToString();
            }
        }

        private static string GetEventName(this EventDropDown eventDropDown)
        {
            string eventType = string.Empty;

            switch (eventDropDown)
            {
                case EventDropDown.Selected:
                    eventType = AttributeDropDownList.EventSelected;
                    break;
                case EventDropDown.SelectedChanged:
                    eventType = AttributeDropDownList.EventSelectedChanged;
                    break;
                case EventDropDown.Open:
                    eventType = AttributeDropDownList.EventOpen;
                    break;
                case EventDropDown.DataBindError:
                    eventType = AttributeDropDownList.EventDataBindError;
                    break;
                case EventDropDown.DataBindSuccess:
                    eventType = AttributeDropDownList.EventDataBindSuccess;
                    break; 
            }

            return eventType;
        }

        private static void ApplyEvents<TModel>(DropDownList<TModel> entity, TagBuilder parent)
        {
            if (entity.Events == null)
            {
                return;
            }

            foreach (var item in entity.Events)
            {
                parent.MergeAttribute(item.Key, item.Value);
            }
        }
    }
}