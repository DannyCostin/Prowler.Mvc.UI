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
        public static Grid<TModel> BindTo<TModel>(this Grid<TModel> entity, IEnumerable<dynamic> dataSource)
        {
            entity.DataSource = dataSource;

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

        public static Grid<TModel> ActionSort<TModel>(this Grid<TModel> entity, string url)
        {
            entity.ActionSort = url;

            return entity;
        }

        private static void CreateTable<TModel>(this Grid<TModel> entity)
        {
            entity.TableTemplate.Tag = new TagBuilder(TagElement.Div);
            entity.TableTemplate.Tag.AddCssClass(CssGrid.GridMainContainer);

            var gridContainer = new TagBuilder(TagElement.Div);
            gridContainer.AddCssClass(CssGrid.GridContainer);


            var table = new TagBuilder(TagElement.Table);
            ApplyTableSize(table, entity);

            table.AddCssClass(CssGrid.Grid);

            if (entity.Columns != null)
            {
                table.TagSetInnerHtml(CreateTableHeader(entity));
                table.InnerHtml = string.Concat(table.InnerHtml, CreateTableRows(entity));

                gridContainer.InnerHtml = table.ToString();

                CreateDefaultTable(entity, gridContainer);
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
                   
                    if (item == totalPages)
                    {
                        disableNext = true;
                    }
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
            }
            catch (Exception ex)
            {
                parentcontainer.InnerHtml = ex.Message;
            }

            parentcontainer.InnerHtml = String.Concat(parentcontainer.InnerHtml, container.ToString());
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

                labelContainer.TagSetInnerHtml(span);

                CreateColumnTemplate(item, labelContainer);
                container.TagSetInnerHtml(labelContainer);

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

            foreach (var item in entity.Columns)
            {
                var td = new TagBuilder(TagElement.Td);

                td.AddCssClass(CssGrid.GridRowIdentityClass);
                string bindingValue = ProwlerHelper.GetPropValue<string>(dataItem, item.RowBinding);

                CreateRowLabel(entity, bindingValue, td, item);
                CreateTableRowTemplate(entity, item, dataItem, td);

                tr.InnerHtml = String.Concat(tr.InnerHtml, td.ToString());
            }

            return tr;
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

            var templateBindingProperties = entity.Columns.Select(i => i.RowBinding).ToList();

            foreach (var item in entity.Columns)
            {
                if (item.RowTemplateBindings != null)
                {
                    templateBindingProperties.AddRange(item.RowTemplateBindings);
                }

                var td = new TagBuilder(TagElement.Td);

                td.AddCssClass(CssGrid.GridRowIdentityClass);
                string bindingValue = ProwlerHelper.GetPropValue<string>(dataItem, item.RowBinding);

                CreateRowLabel(entity, bindingValue, td, item, true);
                CreateTableRowTemplate(entity, item, dataItem, td, true);

                tr.InnerHtml = String.Concat(tr.InnerHtml, td.ToString());
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
