using Prowler.Mvc.UI.Global;
using Prowler.Mvc.UI.Global.Struct;
using Prowler.Mvc.UI.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Prowler.Mvc.UI
{
    public static class ProwlerGridExtension
    {
        internal const string DataSourceRequestProperty = "DataSource";

        public static Grid<TModel> BindTo<TModel>(this Grid<TModel> entity, IEnumerable<dynamic> dataSource)
        {
            entity.DataSource = dataSource;

            return entity;
        }

        public static Grid<TModel> UniqueId<TModel>(this Grid<TModel> entity, string bindingProperty)
        {
            entity.UniqueId = bindingProperty;

            return entity;
        }

        public static Grid<TModel> Name<TModel>(this Grid<TModel> entity, string name)
        {
            entity.Name = name;

            return entity;
        }

        public static Grid<TModel> AllowColumnResize<TModel>(this Grid<TModel> entity)
        {
            entity.AllowColumnResize = true;

            return entity;
        }

        public static IHtmlString Render<TModel>(this Grid<TModel> entity)
        {
            CreateTable(entity);

            return MvcHtmlString.Create(entity.TableTemplate.Tag.ToString());
        }

        public static Grid<TModel> Pagination<TModel>(this Grid<TModel> entity, Func<Pagination> pagination)
        {
            entity.Pagination = pagination?.Invoke();

            return entity;
        }

        public static Grid<TModel> Height<TModel>(this Grid<TModel> entity, int value)
        {
            entity.Height = value;

            return entity;
        }

        public static Grid<TModel> Width<TModel>(this Grid<TModel> entity, int value)
        {
            entity.Width = value;

            return entity;
        }

        public static Grid<TModel> HtmlAttributes<TModel>(this Grid<TModel> entity, string key, string value)
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


        public static Grid<TModel> Columns<TModel>(this Grid<TModel> entity, params Action<Column>[] column)
        {
            entity.Columns = new List<Column>();

            foreach (var item in column)
            {
                var columnElement = new Column();
                item.Invoke(columnElement);
                entity.Columns.Add(columnElement);
            }

            return entity;
        }

        public static Grid<TModel> ErrorFunction<TModel>(this Grid<TModel> entity, string function)
        {
            entity.ErrorFunction = function;

            return entity;
        }

        public static Grid<TModel> ActionSort<TModel>(this Grid<TModel> entity, string url)
        {
            entity.ActionSort = url;

            return entity;
        }

        public static Grid<TModel> IncludeFilterContainer<TModel>(this Grid<TModel> entity, string containerId)
        {
            entity.FilterContainerId = containerId;

            return entity;
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

        private static void ApplyTableSize<TModel>(TagBuilder tag, Grid<TModel> entity)
        {
            string tableSize = "";

            if (entity.Width > 0)
            {
                tableSize = $"width:{entity.Width}px";
            }

            if (entity.Height > 0)
            {
                if (string.IsNullOrEmpty(tableSize))
                {
                    tableSize = string.Concat(tableSize, $"height:{entity.Height}px;overflow:auto");
                }
                else
                {
                    tableSize = string.Concat(tableSize, $";height:{entity.Height}px;overflow:auto");
                }
            }

            tag.MergeAttribute("style", tableSize);
        }

        private static void CreateTable<TModel>(this Grid<TModel> entity)
        {
            entity.TableTemplate.Tag = new TagBuilder(TagElement.Div);
            entity.TableTemplate.Tag.AddCssClass(CssGrid.GridMainContainer);

            var gridContainer = new TagBuilder(TagElement.Div);
            gridContainer.AddCssClass(CssGrid.GridContainer);
            gridContainer.Attributes.Add(AttributeGrid.GridFilterContainerId, entity.FilterContainerId);

            if (!string.IsNullOrEmpty(entity.ErrorFunction))
            {
                gridContainer.Attributes.Add(AttributeGrid.GridErrorFunction, entity.ErrorFunction);
            }

            MergeHtmlAttributes(entity.TableTemplate.Tag, entity.HtmlAttributes);

            var table = new TagBuilder(TagElement.Table);
            ApplyTableSize(table, entity);

            table.AddCssClass(CssGrid.Grid);

            if (entity.Columns != null)
            {
                table.TagSetInnerHtml(CreateTableHeader(entity));
                table.InnerHtml = string.Concat(table.InnerHtml, CreateTableRows(entity));

                gridContainer.InnerHtml = table.ToString();

                CreateDefaultTable(entity, entity.TableTemplate.Tag);
            }

            CreatePaginationArea(entity, gridContainer);

            entity.TableTemplate.Tag.TagSetInnerHtml(gridContainer);
            CreateOverLayerContainer(entity.TableTemplate.Tag);
        }

        private static void CreatePaginationArea<TModel>(Grid<TModel> entity, TagBuilder parentcontainer)
        {
            if (entity.Pagination == null || entity.Pagination.Total == 0)
            {
                return;
            }
            
            if(entity.Pagination.PaginationRangeGow < 1)
            {
                entity.Pagination.PaginationRangeGow = 1;
            }

            var container = new TagBuilder(TagElement.Div);
            container.AddCssClass(CssGrid.PaginationContainer);
            container.MergeAttribute(AttributeGrid.PaginationUrl, entity.Pagination.Url);
            container.MergeAttribute(AttributeGrid.PaginationIndex, entity.Pagination.PageIndex.ToString());
            container.MergeAttribute(AttributeGrid.PaginationSize, entity.Pagination.PageItems.ToString());
            container.MergeAttribute(AttributeGrid.PaginationNumberMax, entity.Pagination.PaginationButtons.ToString());
            container.MergeAttribute(AttributeGrid.PaginationRangeGrow, entity.Pagination.PaginationRangeGow.ToString());
            container.MergeAttribute(AttributeGrid.PaginationTotalItems, entity.Pagination.Total.ToString());

            bool disableNext = false;
          
            try
            {
                var rangeList = new List<int>();
              
                
                int totalPages = (int)Math.Ceiling((double)entity.Pagination.Total / entity.Pagination.PageItems);

                if (totalPages < entity.Pagination.PaginationButtons)
                {
                    entity.Pagination.PaginationButtons = totalPages;
                }

                var lowRangeUnalocated = 0;
                var lowValue = entity.Pagination.PageIndex - (entity.Pagination.PaginationButtons - entity.Pagination.PaginationRangeGow) + entity.Pagination.PaginationRangeGow;
                var highRageGrow = entity.Pagination.PageIndex + entity.Pagination.PaginationRangeGow;

                if (highRageGrow > totalPages)
                {
                    lowValue = lowValue - (highRageGrow - totalPages);
                    lowRangeUnalocated = totalPages - highRageGrow;
                }

                for(int index = lowValue -1; index <= entity.Pagination.PageIndex; index++)
                {
                    if(index > 0)
                    {
                        rangeList.Add(index);
                    }
                    else
                    {
                        lowRangeUnalocated++;
                    }
                }

                var highRange = entity.Pagination.PageIndex + lowRangeUnalocated + entity.Pagination.PaginationRangeGow + 1;

                for(int index = entity.Pagination.PageIndex + 1; index < highRange; index++)
                {
                    rangeList.Add(index);
                }

                container.MergeAttribute(AttributeGrid.PaginationTotal, totalPages.ToString());

                var previous = new TagBuilder(TagElement.Ahref);
                previous.InnerHtml = "❮❮";
                previous.MergeAttribute(AttributeGrid.PaginationItemIndex, "1");
             
                if (entity.Pagination.PageIndex <= 1)
                {
                    previous.AddCssClass(CssGrid.PaginationItemDisable);
                }
                else
                {
                    previous.AddCssClass(CssGrid.PaginationItem);
                }

                container.InnerHtml = previous.ToString();

                var itemPerPageInput = new TagBuilder(TagElement.Input)
                            .TagAsTextHidden()
                            .TagSetValue(entity.Pagination.PageItems.ToString())
                            .TagSetName(AttributeGrid.PaginationItemsName);

                itemPerPageInput.AddCssClass(CssGrid.PaginationItemsPerPageInput);

                container.TagSetInnerHtml(itemPerPageInput);

                container.MergeAttribute(AttributeGrid.PaginationSize, entity.Pagination.PageItems.ToString());
              
                foreach(var item in rangeList)
                {
                    var pageItem = new TagBuilder(TagElement.Ahref);
                    pageItem.AddCssClass(CssGrid.PaginationItem);
                    pageItem.InnerHtml = (item).ToString();
                    pageItem.MergeAttribute(AttributeGrid.PaginationItemIndex, (item).ToString());

                    if (entity.Pagination.PageIndex == item)
                    {
                        pageItem.AddCssClass(CssGrid.PaginationSelected);
                        var pageInput = new TagBuilder(TagElement.Input)
                            .TagAsTextHidden()
                            .TagSetValue(item.ToString())
                            .TagSetName(AttributeGrid.PaginationIndexName);

                        pageInput.AddCssClass(CssGrid.PaginationIndexInput);

                        container.TagSetInnerHtml(pageInput);
                    }

                    container.InnerHtml = string.Concat(container.InnerHtml, pageItem.ToString());
                }

                if (entity.Pagination.PageIndex == totalPages)
                {
                    disableNext = true;
                }

                var next = new TagBuilder(TagElement.Ahref);
                next.InnerHtml = "❯❯";

                if (disableNext)
                {
                    next.AddCssClass(CssGrid.PaginationItemDisable);
                }
                else
                {
                    next.AddCssClass(CssGrid.PaginationItem);
                    next.MergeAttribute(AttributeGrid.PaginationItemIndex, totalPages.ToString());
                }

                container.TagSetInnerHtml(next);
                container.TagSetInnerHtml(CreatePaginationToolBarRight(entity));
            }
            catch (Exception ex)
            {
                parentcontainer.InnerHtml = ex.Message;
            }

            parentcontainer.InnerHtml = String.Concat(parentcontainer.InnerHtml, container.ToString());
        }

        private static TagBuilder CreatePaginationToolBarRight<TModel>(Grid<TModel> entity)
        {
            var container = new TagBuilder(TagElement.Div);
            container.AddCssClass(CssGrid.GridPaginationToolBarRight);

            container.TagSetInnerHtml(CreatePaginationCurentPageItemsLabel(entity));

            return container;
        }

        private static TagBuilder CreatePaginationCurentPageItemsLabel<TModel>(Grid<TModel> entity)
        {
            var label = new TagBuilder(TagElement.Span);
            label.AddCssClass(CssGrid.GridPaginationPageItemLabel);

            var itemPositionStart = (entity.Pagination.PageIndex - 1) * entity.Pagination.PageItems + 1;
            var itemPositionEnd = entity.Pagination.PageIndex * entity.Pagination.PageItems;
            var itemsText = "items";

            if (entity.Pagination.Total <= 1) { itemsText = "item"; }
            if (itemPositionEnd > entity.Pagination.Total) { itemPositionEnd = entity.Pagination.Total; }

            label.SetInnerText($"{itemPositionStart} - {itemPositionEnd} of {entity.Pagination.Total} {itemsText}");

            return label;
        }


        private static TagBuilder CreateTableHeader<TModel>(Grid<TModel> entity)
        {
            var tr = new TagBuilder(TagElement.Tr);
            tr.AddCssClass(CssGrid.GridHeaderContainer);

            if (entity.Columns == null)
            {
                return tr;
            }

            foreach (var item in entity.Columns)
            {
                var th = new TagBuilder(TagElement.Th);
                th.MergeAttribute("valign", "top");

                var container = new TagBuilder(TagElement.Div);
                container.AddCssClass(CssGrid.GridColumnHeaderContainer);

                var labelContainer = new TagBuilder(TagElement.Div);
                labelContainer.AddCssClass(CssGrid.GridColumnHeaderLabelContainer);

                if (item.SortName != null)
                {
                    labelContainer.MergeAttribute(AttributeGrid.ActionSort, entity.ActionSort);
                    labelContainer.MergeAttribute(AttributeGrid.ColumnSortParameterName, item.SortName);
                }

                CreateHeaderResizeBlock(entity, container, item);

                if (item.SortName != null)
                {
                    var sortContainer = new TagBuilder(TagElement.I);
                    sortContainer.AddCssClass(CssGlobal.SortIconContainer);

                    labelContainer.TagSetInnerHtml(sortContainer);
                }

                var span = new TagBuilder(TagElement.Span);
                span.AddCssClass(CssGrid.GridContentNoSelect);
                span.SetInnerText(item.Title);

                if(item.SortName != null)
                {
                    span.AddCssClass(CssGrid.GridColumnHeaderContainerSort);
                }

                labelContainer.TagSetInnerHtml(span);

                CreateColumnTemplate(item, labelContainer);
                container.TagSetInnerHtml(labelContainer);

                CreateHeaderCheckBox(entity, container, item);

                ApplyRowHtmlAttributes(entity, item, th);

                th.TagSetInnerHtml(container);
                tr.TagSetInnerHtml(th);
            }

            return tr;
        }

        private static void CreateColumnTemplate(Column column, TagBuilder header)
        {
            if (column.ColumnTemplate == null)
            {
                return;
            }

            header.InnerHtml = string.Concat(header.InnerHtml, column.ColumnTemplate);
        }

        private static string CreateTableRows<TModel>(Grid<TModel> entity)
        {
            StringBuilder dataRows = new StringBuilder();
            int itemIndex = 0;

            foreach (var dataItem in entity.DataSource)
            {

                itemIndex++;

                entity.CurrentRowItemIndex = $"prg{entity.Pagination?.PageIndex}{itemIndex}";

                dataRows.Append(CreateTableRow(entity, dataItem).ToString());
            }

            return dataRows.ToString();
        }

        private static void CreateTableRowTemplate<TModel>(Grid<TModel> entity, Column column, dynamic item, TagBuilder rowContainer, bool defaultItem = false)
        {
            if (column.RowTemplate != null)
            {
                rowContainer.InnerHtml = defaultItem ? column.RowTemplate : ProwlerHelper.ApplyTemplateValues(column.RowTemplateBindings, column.RowTemplate, item);
            }
        }

        private static TagBuilder CreateTableRow<TModel>(Grid<TModel> entity, dynamic dataItem)
        {
            var tr = new TagBuilder(TagElement.Tr);

            var uniqueIdAlocated = false;

            foreach (var column in entity.Columns)
            {
                var td = new TagBuilder(TagElement.Td);

                td.AddCssClass(CssGrid.GridRowIdentityClass);
                string bindingValue = ProwlerHelper.GetPropValue<string>(dataItem, column.RowBinding);

                CreateRowLabel(entity, bindingValue, td, column);
                CreateTableRowTemplate(entity, column, dataItem, td);
                CreateCheckBox(entity, td, bindingValue, column, dataItem);
                CreateTextBox(entity, td, bindingValue, column, dataItem);

                if (!uniqueIdAlocated)
                {
                    CreateUniqueIdIdentifier(entity, td, dataItem);
                    uniqueIdAlocated = true;
                }                

                tr.InnerHtml = String.Concat(tr.InnerHtml, td.ToString());
            }

            return tr;
        }

        private static void CreateTextBox<TModel>(Grid<TModel> entity, TagBuilder tdContainer, string value,
            Column column, dynamic dataItem, bool defaultItem = false)
        {
            if (column.AsEditable && !column.HasRowTemplate && column.EditableInputType == GridInputType.Text)
            {
                var textBox = new TagBuilder(TagElement.Input);
                textBox.TagAsTextBox();
                textBox.TagSetValue(defaultItem ? ProwlerHelper.GetBindingString(column.RowBinding) : value);
                textBox.AddCssClass(CssGrid.InputEdit);
                textBox.AddCssClass(CssGrid.GridRowEditContainer);

                string uniqueIdValue = ProwlerHelper.GetPropValue<string>(dataItem, entity.UniqueId);

                if (column.ValidationEvent != null)
                {
                    textBox.MergeAttribute(AttributeGrid.ValidationEvent, column.ValidationEvent);
                }

                if (defaultItem)
                {
                    textBox.TagSetName(DataSourceRequestProperty, column.RowBinding, ProwlerHelper.GetBindingString(entity.UniqueId));
                }
                else
                {
                    textBox.TagSetName(DataSourceRequestProperty, column.RowBinding, uniqueIdValue);
                }

                tdContainer.TagSetInnerHtml(textBox);
            }
        }

        private static void CreateUniqueIdIdentifier<TModel>(Grid<TModel> entity, TagBuilder tdContainer, dynamic dataItem, bool defaultItem = false)
        {
            if (string.IsNullOrEmpty(entity.UniqueId)) { return; }

            string bindingValue = ProwlerHelper.GetPropValue<string>(dataItem, entity.UniqueId);

            var textBoxUniqueId = new TagBuilder(TagElement.Input).TagAsTextHidden();
            textBoxUniqueId.TagSetValue(defaultItem ? ProwlerHelper.GetBindingString(entity.UniqueId) : bindingValue);

            if (defaultItem)
            {
                textBoxUniqueId.TagSetName(DataSourceRequestProperty, entity.UniqueId, ProwlerHelper.GetBindingString(entity.UniqueId));
            }
            else
            {
                textBoxUniqueId.TagSetName(DataSourceRequestProperty, entity.UniqueId, bindingValue);
            }

            var textBoxIndexMap = new TagBuilder(TagElement.Input).TagAsTextHidden();

            textBoxIndexMap.TagSetName($"{DataSourceRequestProperty}.Index");
            textBoxIndexMap.TagSetValue(defaultItem ? ProwlerHelper.GetBindingString(entity.UniqueId)
                                                    : bindingValue);

            tdContainer.TagSetInnerHtml(textBoxUniqueId);
            tdContainer.TagSetInnerHtml(textBoxIndexMap);
        }

        private static void CreateHeaderCheckBox<TModel>(Grid<TModel> entity, TagBuilder tdContainer, Column column, bool defaultItem = false)
        {
            if (column.AsEditable && !column.HasRowTemplate
                && column.EditableInputType == GridInputType.CheckBox
                && column.HeaderAsCheckbox)
            {
                var checkBoxContainer = new TagBuilder(TagElement.Div);
                var checkBoxIcon = new TagBuilder(TagElement.Span);
                var checkBox = new TagBuilder(TagElement.Input).TagAsCheckBox();                

                checkBoxContainer.AddCssClass(CssGrid.CheckBox);
                checkBoxContainer.AddCssClass(CssGrid.CheckBoxHeader);
                checkBoxContainer.AddCssClass(CssGrid.GridRowEditContainer);
                checkBoxIcon.AddCssClass(CssGrid.CheckBoxIcon);
                checkBoxContainer.MergeAttribute(AttributeGrid.CheckBoxHeaderName, column.RowBinding);
                checkBox.AddCssClass(CssGrid.CheckboxHeaderId);

                checkBox.TagSetValue(column.HeaderCheckboxValue.ToString());

                if (column.HeaderCheckboxValue)
                {
                    checkBox.TagSetChecked();
                }

                checkBoxContainer.TagSetInnerHtml(checkBox);
                checkBoxContainer.TagSetInnerHtml(checkBoxIcon);

                if(column.HeaderCheckboxLabel != null)
                {
                    var label = new TagBuilder(TagElement.Span);
                    label.SetInnerText(column.HeaderCheckboxLabel);
                    tdContainer.TagSetInnerHtml(label);
                }

                tdContainer.TagSetInnerHtml(checkBoxContainer);
            }
        }

        private static void CreateCheckBox<TModel>(Grid<TModel> entity, TagBuilder tdContainer, string value,
            Column column, dynamic dataItem, bool defaultItem = false)
        {
            if (column.AsEditable && !column.HasRowTemplate && column.EditableInputType == GridInputType.CheckBox)
            {
                var checkBoxContainer = new TagBuilder(TagElement.Div);
                var checkBoxIcon = new TagBuilder(TagElement.Span);
                var checkBox = new TagBuilder(TagElement.Input);

                string uniqueIdValue = ProwlerHelper.GetPropValue<string>(dataItem, entity.UniqueId);


                if (defaultItem)
                {
                    checkBox.TagSetName(DataSourceRequestProperty, column.RowBinding, ProwlerHelper.GetBindingString(entity.UniqueId));
                }
                else
                {
                    checkBox.TagSetName(DataSourceRequestProperty, column.RowBinding, uniqueIdValue);
                }                
          
                checkBox.TagAsCheckBox();
                checkBoxContainer.AddCssClass(CssGrid.CheckBox);
                checkBoxContainer.AddCssClass(CssGrid.GridRowEditContainer);
                checkBoxIcon.AddCssClass(CssGrid.CheckBoxIcon);
                checkBox.MergeAttribute(AttributeGrid.CheckBoxName, column.RowBinding);
                checkBox.AddCssClass(CssGrid.CheckBoxInputId);

                bool.TryParse(value, out bool bindingValue);

                checkBox.TagSetValue( defaultItem ? ProwlerHelper.GetBindingString(column.RowBinding) 
                                                  : bindingValue.ToString());

                if (bindingValue && !defaultItem)
                {
                    checkBox.TagSetChecked();
                }

                checkBoxContainer.TagSetInnerHtml(checkBox);
                checkBoxContainer.TagSetInnerHtml(checkBoxIcon);

                tdContainer.TagSetInnerHtml(checkBoxContainer);             
            }
        }

        private static void CreateDefaultTable<TModel>(Grid<TModel> entity, TagBuilder parentcontainer)
        {
            var defaultRowContainer = new TagBuilder(TagElement.Div);
            defaultRowContainer.AddCssClass(CssGrid.GridDefaultRowItem);

            var table = new TagBuilder(TagElement.Table);
            table.TagSetInnerHtml(CreateTableDefaultRow(entity));

            defaultRowContainer.TagSetInnerHtml(table);
            parentcontainer.TagSetInnerHtml(defaultRowContainer);
        }

        private static TagBuilder CreateTableDefaultRow<TModel>(Grid<TModel> entity)
        {
            dynamic dataItem = entity.DataSource.FirstOrDefault();
            var tr = new TagBuilder(TagElement.Tr);
            var uniqueIdAlocated = false;

            var templateBindingProperties = entity.Columns.Select(i => i.RowBinding).ToList();            

            foreach (var column in entity.Columns)
            {
                if (column.RowTemplateBindings != null)
                {
                    templateBindingProperties.AddRange(column.RowTemplateBindings);
                }

                var td = new TagBuilder(TagElement.Td);

                td.AddCssClass(CssGrid.GridRowIdentityClass);
                string bindingValue = ProwlerHelper.GetPropValue<string>(dataItem, column.RowBinding);

                CreateRowLabel(entity, bindingValue, td, column, true);
                CreateTableRowTemplate(entity, column, dataItem, td, true);
                CreateCheckBox(entity, td, bindingValue, column, dataItem, true);
                CreateTextBox(entity, td, bindingValue, column, dataItem, true);

                if (!uniqueIdAlocated)
                {
                    CreateUniqueIdIdentifier(entity, tr, dataItem, true);
                    uniqueIdAlocated = true;
                }                

                tr.InnerHtml = String.Concat(tr.InnerHtml, td.ToString());
            }

            if(!String.IsNullOrEmpty(entity.UniqueId) && !templateBindingProperties.Any(i => i == entity.UniqueId))
            {
                templateBindingProperties.Add(entity.UniqueId);
            }

            tr.MergeAttribute(AttributeGrid.GridBindings, string.Join(",", templateBindingProperties.Select(i => i)));

            return tr;
        }

        private static void CreateHeaderResizeBlock<TModel>(Grid<TModel> entity, TagBuilder container, Column column)
        {
            if (entity.AllowColumnResize && column.AllowColumnResize)
            {
                var resizeBlock = new TagBuilder(TagElement.Div);
                resizeBlock.AddCssClass(CssGrid.GridColumnResizeBlock);

                container.InnerHtml = string.Concat(container.InnerHtml, resizeBlock.ToString());
            }
        }

        private static void CreateRowLabel<TModel>(Grid<TModel> entity, string bindingValue, TagBuilder container, Column column, bool defaultItem = false)
        {
            if (column.AsEditable) { return; }

            var span = new TagBuilder(TagElement.Span);
            span.InnerHtml = defaultItem ? ProwlerHelper.GetBindingString(column.RowBinding) : bindingValue;
            span.AddCssClass(CssGrid.GridRowLabel);
            container.InnerHtml = string.Concat(container.InnerHtml, span.ToString());
        }

        private static void ApplyRowHtmlAttributes<TModel>(Grid<TModel> entity, Column item, TagBuilder tdContainer)
        {
            if (item.Width > 0)
            {
                tdContainer.MergeAttribute("style", $"width:{item.Width}px");
            }
        }

        private static void CreateOverLayerContainer(TagBuilder parent)
        {
            var container = new TagBuilder(TagElement.Div);
            container.AddCssClass(CssGrid.GridOverLayerContainer);
            parent.TagSetInnerHtml(container);
        }
    }
}
