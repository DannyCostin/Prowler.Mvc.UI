$(document).ready(function () {

    let globalProwlerDropdownOpen = false;

    $(document).bind('click', function () {

        if (globalProwlerDropdownOpen) {
            $('.p-dropdown-container').find(".p-dropdown-list").hide();
            $('.p-dropdown-container').find(".p-dropdown").removeClass("p-dropdown-focus");
            $('.p-dropdown-container').find(".p-dropdown-filter-container").hide();
        }

        globalProwlerDropdownOpen = false;
    });

    document.addEventListener('scroll', function (event) {
        if (globalProwlerDropdownOpen && event.target.className != 'p-dropdown-list') {
            $('.p-dropdown-container').find(".p-dropdown-list").hide();
            $('.p-dropdown-container').find(".p-dropdown").removeClass("p-dropdown-focus");
            $('.p-dropdown-container').find(".p-dropdown-filter-container").hide();
            globalProwlerDropdownOpen = false;
        }
    }, true /*Capture event*/);

    $(document).on('click', '.p-dropdown-list', function (event) {
        event.stopImmediatePropagation();
    });

    $(document).on('keydown', '.p-dropdown-filter-input', function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            return false;
        }
        event.stopImmediatePropagation();
    });

    $(document).on('keyup', '.p-dropdown-filter-input', function () {
        event.stopImmediatePropagation();

        var parentContainer = $(this).parent().parent().parent().parent();

        if ($(parentContainer).attr("p-dropdown-srv-filter-enable")) {
            prowlerDropDownHelper.serverFiltering(this);
        }
        else {
            prowlerDropDownHelper.clientFiltering(this);
        }
    });

    $(document).on('click', '.p-dropdown-filter-input', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
    });


    $(document).on('click', '.p-dropdown-container', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();

        if ($(this)[0].className == "p-dropdown-container-disabled") {
            return;
        }

        $('.p-dropdown-container').find(".p-dropdown").removeClass("p-dropdown-focus");

        $(this).find(".p-dropdown").addClass("p-dropdown-focus");

        if ($(this).find(".p-dropdown-list").is(":visible")) {

            var elmentVisible = true;
        }

        $('.p-dropdown-container').find(".p-dropdown-list").hide();
        $('.p-dropdown-container').find(".p-dropdown-filter-container").hide();

        if (elmentVisible) {
            return;
        }

        prowlerDropDownHelper.setDropDownCollision(this);

        $(this).find(".p-dropdown-list").slideToggle(250);
        $(this).find(".p-dropdown-filter-container").toggle();
        $(this).find(".p-dropdown-filter-container").find(".p-dropdown-filter-input").focus();
        prowlerDropDownHelper.eventOpen(this);
        globalProwlerDropdownOpen = true;
    });

    $(document).bind(".p-dropdown-list").on('click', ".p-dropdown-container-element", function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        event.preventDefault();

        if ($(this).attr("p-dropdown-ms-enable") == "1") {
            prowlerDropDownHelper.multiSelectClick(this, true);

            prowlerDropDownHelper.eventSelected($(this).parent().parent().parent(),
                $(this).attr("ps-p-value"), $(this).attr("ps-p-name"));

            prowlerDropDownHelper.eventSelectedChanged($(this).parent().parent().parent(),
                $(this).attr("ps-p-value"), $(this).attr("ps-p-name"), selectedCurrentValue);

            return;
        }

        var parentElement = $(this).parent();

        $(parentElement).find(".p-dropdown-list-element-selected")
            .removeClass("p-dropdown-list-element-selected");

        $(this).addClass("p-dropdown-list-element-selected");

        var selectedElement = parentElement.find("#ps-s-value-selected");
        var selectedGroupElement = parentElement.find("#ps-s-value-group-selected");


        if (selectedElement.length > 0) {
            var selectedCurrentValue = selectedElement.attr("value");
            selectedElement.attr("value", $(this).attr("ps-p-value"));
            var containerElement = parentElement.parent().parent();
            var selectedLabel = containerElement.find(".p-dropdown").find(".p-dropdown-label-selected")

            if (selectedLabel.length > 0) {
                selectedLabel.html($(this).html());

                prowlerDropDownHelper.eventSelected($(this).parent().parent().parent(),
                    $(this).attr("ps-p-value"), $(this).attr("ps-p-name"));

                prowlerDropDownHelper.eventSelectedChanged($(this).parent().parent().parent(),
                    $(this).attr("ps-p-value"), $(this).attr("ps-p-name"), selectedCurrentValue);
            }
        }

        if (selectedGroupElement.length > 0) {
            selectedGroupElement.attr("value", $(this).attr("ps-p-group"));
        }

        parentElement.hide();
        parentElement.parent().find(".p-dropdown-filter-container").hide();
    });

    window.prowlerDropDown = function ProwlerDropDownFunctions(id) {

        var dropDownId = "#" + id;

        function DisableDropDown() {
            $(dropDownId).addClass("p-dropdown-container-disabled")
                .removeClass("p-dropdown-container");

            $(dropDownId).find(".p-dropdown").addClass("p-dropdown-disabled")
                .removeClass("p-dropdown");
        }

        function EnableDropDown() {
            $(dropDownId).addClass("p-dropdown-container")
                .removeClass("p-dropdown-container-disabled");

            $(dropDownId).find(".p-dropdown-disabled")
                .addClass("p-dropdown")
                .removeClass("p-dropdown-disabled");
        }

        function GetSelectedValue() {

            var value = $(dropDownId).find("#ps-s-value-selected").val();

            if (value == undefined) {
                var selectedElements = $(dropDownId).parent().find('.ps-multiselect-srz');

                if (selectedElements.length > 0) {
                    value = [];
                    selectedElements.each(function () {
                        value.push($(this).attr("ps-p-value"));
                    });
                }
            }

            return value;
        }

        function DataBind(serverUrl, type, properties) {

            var dropdownContainer = $(dropDownId).find('.p-dropdown-list');

            $(dropdownContainer).find(".p-dropdown-group-container").remove();
            $(dropdownContainer).find(".p-dropdown-container-element").remove();

            prowlerHelper.RemoveLoader(dropdownContainer);
            prowlerHelper.GetLoader().appendTo(dropdownContainer);

            return prowlerHelper.DataBind(serverUrl, type,
                properties, prowlerDropDownHelper.dataBindParser, $(dropDownId));
        }

        function OpenDropdown() {
            event.stopPropagation();
            event.stopImmediatePropagation();
            $(dropDownId).trigger("click");
        }

        function ServerFilterParameter(parameters) {
            $(dropDownId).attr("p-dropdown-srv-filter-params", JSON.stringify(parameters));
        }

        function SetSelectedValue(value) {
            var dropdownContainer = $(dropDownId).find('.p-dropdown-list');
            var element = $(dropdownContainer).find(".p-dropdown-container-element[ps-p-value='" + value + "']");

            if (element != undefined) {
                $(element).trigger("click");
            }
        }

        return {
            disable: DisableDropDown,
            enable: EnableDropDown,
            getSelectedValue: GetSelectedValue,
            databind: DataBind,
            open: OpenDropdown,
            serverFilterParameterAdd: ServerFilterParameter,
            setSelectedValue: SetSelectedValue
        }
    }

    let prowlerHelper = (function ProwlerHelperGlobal() {

        function executeFunctionByName(functionName, context) {
            var argsList = Array.prototype.slice.call(arguments, 2);
            var namespaces = functionName.split(".");
            var func = namespaces.pop();

            for (let i = 0; i < namespaces.length; i++) {
                context = context[namespaces[i]];
            }

            var functionContext = context[func];

            if (functionContext != undefined) {
                return context[func].apply(context, argsList);
            }
        }

        function createLoader() {
            var loader = $('<div class="loader-prowler-container" style="margin:5px;height:40px;width:40px;display:block;">');

            $('<div>').attr({
                class: "loader-prowler"
            }).appendTo(loader);

            return loader;
        }

        function removeLoader(container) {
            $(container).find(".loader-prowler-container").remove();
        }

        function replaceString(original, searchTxt, replaceTxt) {
            const regex = new RegExp(searchTxt, 'g');
            return original.replace(regex, replaceTxt);
        }

        function prowler_DataBind(serverUrl, type, properties, bindingFunc, container, bindingErrorHandlingFunc) {

            if (serverUrl == "" || serverUrl == undefined) {
                return;
            }

            $.ajax({
                url: serverUrl,
                type: type,
                dataType: "JSON",
                data: properties,
                success: function (results) {
                    if (bindingFunc != undefined) {
                        bindingFunc(results, container);
                    }
                },
                error: function (request, status, error) {
                    if (bindingErrorHandlingFunc != undefined) {
                        prowlerHelper.callFunction(bindingErrorHandlingFunc, window, request);
                    }
                }
            });
        }

        return {
            callFunction: executeFunctionByName,
            DataBind: prowler_DataBind,
            StringReplace: replaceString,
            GetLoader: createLoader,
            RemoveLoader: removeLoader,
            AjaxSend: prowler_DataBind
        }

    })();

    let prowlerDropDownHelper = (function ProwlerHelperFunctions() {

        var searchFilterTimeDelay;

        function prowler_SetDropDownCollision(obj) {

            if (obj.className != 'p-dropdown-container' && obj.length == undefined) {
                obj = $(obj).closest('.p-dropdown-container');
            }

            if (obj.length > 0 && obj[0].className != 'p-dropdown-container') {
                obj = $(obj).closest('.p-dropdown-container');
            }

            if (obj.length == 0) {
                return;
            }

            var offsetTop = $(obj).offset().top
            var height = $(obj).outerHeight();
            var width = $(obj).outerWidth();
            var bodyHeight = $(window).outerHeight() + $(window).scrollTop();
            var listHeight = $(obj).find('.p-dropdown-list').outerHeight();
            var filterHeight = $(obj).find('.p-dropdown-filter-container').outerHeight();

            if (filterHeight == undefined) {
                filterHeight = 0;
            }

            var menuPosition = offsetTop + height;

            if (offsetTop + height + listHeight + 40 > bodyHeight) {

                menuPosition = offsetTop - listHeight - filterHeight;
            }

            $(obj).find('.p-dropdown-listbox-container').offset({
                top: menuPosition
            }).width(width);
        }

        function prowler_dropDownMultiSelectClick(obj, syncCheckBoxControl) {
            var parentElement = $(obj).parent().parent(); //changed
            var checkBoxControl = $(obj).find(".p-dropdown-list-element-multiselect");

            if (checkBoxControl.length == 0) {
                return;
            }

            if (syncCheckBoxControl && checkBoxControl.length > 0) {
                if (!$(checkBoxControl)[0].checked) {
                    $(checkBoxControl)[0].checked = true;
                }
                else {
                    $(checkBoxControl)[0].checked = false;
                }
            }

            prowlerDropDownHelper.multiSelectSetFocus(obj, checkBoxControl);

            var inputName = "p-dr-index" + $(checkBoxControl).attr("ps-index");

            var existingInput = $(parentElement).find("#" + inputName);

            if (existingInput.length > 0) {

                if (!$(checkBoxControl)[0].checked) {
                    existingInput.parent().remove();
                    prowlerDropDownHelper.multiSelectAddRemove(obj, $(checkBoxControl)[0].checked);
                    return;
                }

                $(existingInput).attr("value", $(checkBoxControl)[0].checked);

                return;
            }

            if (checkBoxControl.length > 0) {
                prowlerDropDownHelper.multiSelectAddRemove(obj, $(checkBoxControl)[0].checked);

                var serializeValue = $(checkBoxControl).attr("ps-s-name") + "[" + $(checkBoxControl).attr("ps-p-value") + "]." + $(checkBoxControl).attr("ps-s-value");
                var serializeCheckState = $(checkBoxControl).attr("ps-s-name") + "[" + $(checkBoxControl).attr("ps-p-value") + "]." + $(checkBoxControl).attr("ps-s-ckvalue");
                var serializeGroupValue = $(checkBoxControl).attr("ps-s-name") + "[" + $(checkBoxControl).attr("ps-p-value") + "]." + $(checkBoxControl).attr("ps-s-group");
                var serializeIndex = $(checkBoxControl).attr("ps-s-name") + ".Index";

                var container = $('<div class="ps-multiselect-srz">')
                    .attr('ps-p-group', $(checkBoxControl).attr("ps-p-group"))
                    .attr('ps-p-value', $(checkBoxControl).attr("ps-p-value"));

                $('<input>').attr({
                    type: 'hidden',
                    name: serializeCheckState,
                    value: $(checkBoxControl)[0].checked,
                    class: "p-dropdown-m-value",
                    id: "p-dr-index" + $(checkBoxControl).attr("ps-index")
                }).appendTo(container);

                $('<input>').attr({
                    type: 'hidden',
                    name: serializeGroupValue,
                    value: $(checkBoxControl).attr("ps-p-group"),
                }).appendTo(container);

                $('<input>').attr({
                    type: 'hidden',
                    name: serializeValue,
                    value: $(checkBoxControl).attr("ps-p-value")
                }).appendTo(container);

                $('<input>').attr({
                    type: 'hidden',
                    name: serializeIndex,
                    value: $(checkBoxControl).attr("ps-p-value")
                }).appendTo(container);

                container.appendTo(parentElement);
            }
        }

        function prowler_SelectedContainerAddRemove(obj, checkState) {
            var parentElement = $(obj).parent().parent();

            if ($(parentElement)[0].className == "p-dropdown-container-element") {
                parentElement = $(parentElement).parent();
            }

            var containerElement = parentElement.parent();
            var selectedLabel = containerElement.find(".p-dropdown").find(".p-dropdown-label-selected");

            if (selectedLabel.length > 0) {

                var selectionContainer = $(selectedLabel).find(".p-multiselect-selection-list-container");

                if (selectionContainer.length == 0) {
                    selectedLabel.html('<div class = "p-multiselect-selection-list-container"></div>');
                    selectionContainer = $(selectedLabel).find(".p-multiselect-selection-list-container");
                }

                if (!checkState) {
                    var containerValue = $(obj).attr("ps-p-name").toUpperCase();

                    $(selectionContainer).children('span').each(function () {

                        if (containerValue.indexOf($(this).html().toUpperCase()) >= 0) {
                            $(this).remove();
                        }
                    });
                }
                else {
                    $(selectionContainer).append('<span class = "p-multiselect-selection-list-container-element">' + $(obj).attr('ps-p-name') + '</span>');
                }
            }

            prowlerDropDownHelper.setDropDownCollision(obj);
        }

        function prowler_dropDownMultiSelectSetFocus(obj, checkBoxControl) {

            var checkBoxRenderHasRender = $(obj).attr("p-dropdown-ms-chk-render");
            var checkBoxControlExists = checkBoxControl.length > 0;

            if (checkBoxControlExists && checkBoxRenderHasRender == "0") {

                if ($(checkBoxControl)[0].checked) {
                    $(obj).addClass("p-dropdown-list-element-selected");
                }
                else {
                    $(obj).removeClass("p-dropdown-list-element-selected");
                }
            }
        }

        function prowler_dropDownOpenEvent(obj) {

            var eventFunc = $(obj).attr("p-dropdown-ev-open")

            if (eventFunc != null) {
                prowlerHelper.callFunction(eventFunc, window);
            }
        }

        function prowler_dropDownSelectedEvent(obj, value, label) {

            var eventFunc = $(obj).attr("p-dropdown-ev-selected")

            if (eventFunc != null) {
                prowlerHelper.callFunction(eventFunc, window, value, label);
            }
        }

        function prowler_dropDownSelectedChangedEvent(obj, value, label, currentValue) {

            var eventFunc = $(obj).attr("p-dropdown-ev-selected-changed")

            if (currentValue == value) {
                return;
            }

            if (eventFunc != null) {
                prowlerHelper.callFunction(eventFunc, window, value, label);
            }
        }

        function prowler_dropDownClientFiltering(obj) {

            var parentContainer = $(obj).parent().parent().parent();
            var searchValue = $(obj).val();

            $(parentContainer).find(".p-dropdown-list").find(".p-dropdown-container-element").each(function (index, e) {
                var filterValue = $(e).attr("p-dropdown-filter-value");

                if (filterValue != undefined && filterValue.toUpperCase().indexOf(searchValue.toUpperCase()) == -1) {
                    $(e).hide();
                }
                else {
                    $(e).show();
                }

                if (searchValue == null || searchValue == "") {
                    $(e).show();
                }
            });

            prowlerDropDownHelper.setDropDownCollision(parentContainer);
        }

        function prowler_dropDownDataBindingParser(results, container) {

            $(container).find('.p-dropdown-list').find(".p-dropdown-group-container").remove();
            $(container).find('.p-dropdown-list').find(".p-dropdown-container-element").remove();

            $(container).find("#ps-s-value-selected").attr("value", "-1");
            $(container).find("#ps-s-value-group-selected").attr("value", "-1");

            var multiselectContainer = $(container).find(".p-multiselect-selection-list-container");

            if (multiselectContainer.length > 0) {
                $(multiselectContainer).html('');
                $(container).find('.ps-multiselect-srz').remove();
            }

            var groupObject = {};

            $.each(results, function (i, result) {

                var templateElement = $(container).find('.p-dropdown-item-template-binding').clone();
                var attribute = $(templateElement).attr("p-dropdown-binding").split(',');
                var groupKeyIdentifier = $(templateElement).attr('p-dropdown-binding-groupkey-identifier');
                var groupLabelIdentifier = $(templateElement).attr('p-dropdown-binding-grouplabel-identifier');
                var multiSelectIdentifier = $(templateElement).attr('p-dropdown-binding-multiselect-identifier');
                var groupValue;
                var groupLabel;
                var itemCheckState;

                var html = $(templateElement).html();
                html = prowlerHelper.StringReplace(html, '{#ps-index#}', i)

                $.each(attribute, function (index, paramenter) {

                    var value = result[paramenter];

                    html = prowlerHelper.StringReplace(html, '{#' + paramenter + '#}', value)

                    if (paramenter == groupKeyIdentifier) {
                        groupValue = value;
                    }

                    if (paramenter == groupLabelIdentifier) {
                        groupLabel = value;
                    }

                    if (paramenter == multiSelectIdentifier) {
                        itemCheckState = value;
                    }
                });

                $(templateElement).html(html);

                prowler_dropDownDataBindApplyMultiselect(multiSelectIdentifier, itemCheckState, templateElement);

                if (!prowler_dropDownBuildGroupByList(groupObject, groupKeyIdentifier, groupValue, groupLabel, templateElement)) {

                    var templateHtml = $(templateElement.html());

                    $(container).find('.p-dropdown-list').append(templateHtml);

                    var multiselectState = $(templateHtml).find(".p-dropdown-list-element-multiselect");

                    if (multiselectState.length > 0 && multiselectState[0].checked) {
                        prowler_dropDownMultiSelectClick(templateHtml, false);
                    }
                }
            });

            prowler_dropDownApplyGroupBy($(container).find('.p-dropdown-list'), groupObject);
            $(container).find('.p-dropdown-list').scrollTop();
            prowlerHelper.RemoveLoader($(container).find('.p-dropdown-list'));
            prowlerDropDownHelper.setDropDownCollision(container);
        }

        function prowler_dropDownDataBindApplyMultiselect(multiSelectIdentifier, itemCheckState, templateElement) {

            if (multiSelectIdentifier != undefined && itemCheckState) {
                var checkBoxControl = $(templateElement).find(".p-dropdown-list-element-multiselect");
                $(checkBoxControl)[0].checked = true;
                $(checkBoxControl).attr('checked', 'checked');
            }
        }

        function prowler_dropDownBuildGroupByList(groupObject, groupKeyIdentifier, groupValue, groupLabel, templateElement) {

            if (groupKeyIdentifier != undefined && groupKeyIdentifier != "" && groupLabel != null) {

                if (groupObject[groupValue] == undefined) {
                    groupObject[groupValue] = [];

                    var containerGroup = $('<div>').attr({ class: "p-dropdown-group-container" });
                    var containerLabel = $('<span>').attr({ class: "p-dropdown-group-label" }).html(groupLabel);

                    containerGroup.append(containerLabel);
                    groupObject[groupValue].push(containerGroup);
                }

                groupObject[groupValue].push($(templateElement.html()));

                return true;
            }

            return false;
        }

        function prowler_dropDownApplyGroupBy(container, groupObject) {

            if (groupObject.length == 0) {
                return;
            }

            $.each(groupObject, function (i, result) {
                $.each(result, function (index, html) {
                    container.append(html);

                    var multiselectState = $(html).find(".p-dropdown-list-element-multiselect");

                    if (multiselectState.length > 0 && multiselectState[0].checked) {
                        prowler_dropDownMultiSelectClick(html, false);
                    }
                });
            });
        }

        function prowler_dropDownServerFiltering(obj) {
            var parentContainer = $(obj).parent().parent().parent().parent();

            var searchValue = $(obj).val();
            var timeDelayValue = $(parentContainer).attr("p-dropdown-srv-filter-delay");

            var properties = {}, propName = $(parentContainer).attr("p-dropdown-srv-filter-serialization-name");
            properties[propName] = searchValue;

            var dropdownContainer = $(parentContainer).find('.p-dropdown-list');
            var customParameters = $(parentContainer).attr("p-dropdown-srv-filter-params");

            if (customParameters != undefined) {
                jQuery.extend(properties, JSON.parse(customParameters))
            }

            $(dropdownContainer).find(".p-dropdown-group-container").remove();
            $(dropdownContainer).find(".p-dropdown-container-element").remove();
            prowlerDropDownHelper.setDropDownCollision(parentContainer);

            clearTimeout(searchFilterTimeDelay);

            searchFilterTimeDelay = setTimeout(function () {

                prowlerHelper.RemoveLoader(dropdownContainer);
                prowlerHelper.GetLoader().appendTo(dropdownContainer);
                prowlerDropDownHelper.setDropDownCollision(parentContainer);
                prowlerHelper.DataBind($(parentContainer).attr("p-dropdown-srv-filter-url"), "POST",
                    properties, prowler_dropDownDataBindingParser, parentContainer, prowler_dropDownDataBindingErrorParser);
            }, timeDelayValue);
        }

        function prowler_dropDownDataBindingErrorParser(results, container) {

            var dropdownContainer = $(container).find('.p-dropdown-list');
            $(dropdownContainer).html(results.statusText + ' ' + results.status);
        }

        return {
            multiSelectClick: prowler_dropDownMultiSelectClick,
            multiSelectAddRemove: prowler_SelectedContainerAddRemove,
            multiSelectSetFocus: prowler_dropDownMultiSelectSetFocus,
            eventOpen: prowler_dropDownOpenEvent,
            eventSelected: prowler_dropDownSelectedEvent,
            eventSelectedChanged: prowler_dropDownSelectedChangedEvent,
            clientFiltering: prowler_dropDownClientFiltering,
            serverFiltering: prowler_dropDownServerFiltering,
            dataBindParser: prowler_dropDownDataBindingParser,
            dataBingErrorParser: prowler_dropDownDataBindingErrorParser,
            setDropDownCollision: prowler_SetDropDownCollision
        }
    })();


    $(document).on('keyup', '.pw-grid-table-input-edit', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();

        var validationMethod = $(this).attr("pw-grid-table-vld-e");

        if (validationMethod != undefined) {
            var result = prowlerHelper.callFunction(validationMethod, window, $(this).val())

            if (result != '') {
                $(this).parent().find('.pw-validation-err-tip').remove()

                $(this).addClass("pw-grid-table-input-edit-validation-err");

                var tip = $('<div>').attr({
                    class: "pw-validation-err-tip"
                }).html(result);

                $(this).parent().append(tip);
            }
            else {
                $(this).removeClass("pw-grid-table-input-edit-validation-err");
                $(this).parent().find('.pw-validation-err-tip').remove()
            }
        }
    });

    $(document).on('click', '.pw-grid-command-cancel', function (event) {
        var parent = $(this).closest('tr');
        var cloneRow = $(parent).find(".pw-grid-row-upd-bkp").find('.pw-grid-row-iden-cls').clone();
        parent.html('');
        cloneRow.prependTo(parent);
    });

    $(document).on('click', '.pw-grid-command-edit', function (event) {
        var parent = $(this).closest('tr');

        var rowClone = $('<div style="display:none">').attr({
            class: "pw-grid-row-upd-bkp"
        }).html($(parent).find('td').clone());

        var row = $(parent).find('.pw-grid-row-iden-cls');

        $(row).removeClass('pw-grid-row-cell-disable');

        if (row.length > 0) {
            $(row).each(function () {

                var editContainer = $(this).find('.pw-grid-row-edit-cnt');

                if (editContainer.length > 0) {
                    editContainer.show();
                    $(this).find('.pw-grid-row-lbl').hide();
                }

            });
        }

        $(parent).find('input').removeAttr('disable');

        var deleteButton = $(parent).find(".pw-grid-command-del");
        var deleteLabel = $(deleteButton).attr("pw-btn-label-executed");

        if (deleteLabel == "") {
            deleteLabel = "Cancel";
        }

        $(deleteButton).toggleClass("pw-grid-command-cancel")
            .removeClass("pw-grid-command-del")
            .html(deleteLabel)

        var updateLabel = $(this).attr("pw-btn-label-executed")

        if (updateLabel == "") {
            updateLabel = "Update";
        }

        $(this).removeClass("pw-grid-command-edit");
        $(this).toggleClass("pw-grid-command-upd");
        $(this).html(updateLabel);

        parent.append(rowClone);
    });

    $(document).on('click', '.pw-grid-command-del', function (event) {
        var deleteContainer = $(this).closest('.pw-grid-table-container').find(".pw-grid-row-del-cnt");
        var rowItem = $(this).parent().parent().parent();
        $(rowItem).find('input').removeAttr('disable');
        $(rowItem).appendTo(deleteContainer);
    });

    $(document).on('click', '.pw-grid-command-upd', function (event) {

        var parent = $(this).closest('tr');

        $(this).find('.pw-grid-row-upd-bkp').remove();

        if ($(parent).find('.pw-validation-err-tip').length > 0) {
            return;
        }

        $(parent).find(".pw-grid-row-upd-bkp").remove();
        $(parent).find(".pw-grid-row-lbl").show();
        $(parent).find(".pw-grid-row-edit-cnt").hide();
        var deleteButton = $(parent).find(".pw-grid-command-cancel");
        $(deleteButton).removeClass('pw-grid-command-cancel')
            .toggleClass('pw-grid-command-del')
            .html($(deleteButton).attr("pw-btn-label"));
        $(parent).find('input').attr('disable');

        $(this).removeClass("pw-grid-command-upd");
        $(this).toggleClass("pw-grid-command-edit");
        $(this).html($(this).attr("pw-btn-label"));

        var tip = $('<form>').attr({
            class: "pw-validation-err-tip"
        }).html($(parent).clone());

        var parentTableContainer = $(this).closest(".pw-grid-table-container").find("table")
        var updateUrl = parentTableContainer.attr("pw-grd-ac-upd");

        prowlerHelper.AjaxSend(updateUrl, 'POST', $(tip).serialize(), null, null, null);

        $(parent).find('td').each(function () {
            var inputValue = $(this).find('.pw-grid-table-input-edit');
            var checkBox = $(this).find('.p-grid-table-checkBox-container').find('input');

            if (inputValue.length > 0) {
                $(this).find('.pw-grid-row-lbl').html($(inputValue).val());
            }

            if (checkBox.length > 0) {
                if ($(checkBox)[0].checked) {
                    $(this).find('.pw-grid-row-lbl').html('True');
                }
                else {
                    $(this).find('.pw-grid-row-lbl').html('False');
                }
            }

            if ($(this).attr("pw-grd-cel-enb-dis") == "1") {
                $(this).addClass("pw-grid-row-cell-disable");
            }
        });
    });

    $(document).on('keyup', '.pw-grid-filter-inps', function () {
        event.stopImmediatePropagation();

        var parentContainer = $(this).closest('.pw-grid-filter-cnt');

        var inputDelay = $(parentContainer).attr('pw-grd-filter-delay');
        var filterUrl = $(parentContainer).attr('pw-grd-filter');

        prowlerGridHelper.filter(this, inputDelay, filterUrl);
    });

    $('.pw-grid-table').on('click', '.p-grid-table-checkBox-container', function (event) {
        event.stopImmediatePropagation();
        event.preventDefault();

        var inputText = $(this).find('input');

        if (inputText.length == 1) {
            if (inputText[0].checked) {
                $(inputText).attr('value', "False");
                inputText[0].checked = false;
            }
            else {
                $(inputText).attr('value', "True");
                inputText[0].checked = true;
            }

            prowlerGridHelper.checkBoxHeaderClick(this);
            prowlerGridHelper.checkBoxHeaderStateUpdateByChild(this);
        }
    });

    let startX, startWidth;
    let resizeColumn;
    let resizeContent
    $(document).on('mousedown', '.pw-grid-block-resize', function (e) {
        event.stopImmediatePropagation();

        startX = e.clientX;
        resizeColumn = $(this).closest('th');
        resizeContent = $(this).parent();
        startWidth = parseInt($(resizeColumn).width(), 10);

        document.documentElement.addEventListener('mousemove', prowlerGridHelper.resizeColumnDoDrag, false);
        document.documentElement.addEventListener('mouseup', prowlerGridHelper.resizeColumnStopDrag, false);
    });

    $(document).on('click', '.pw-grid-pagination-cnt', function () {
        event.stopImmediatePropagation();

        var containerHost = $(this).closest(".pw-grid-table-container").find(".pw-grid-table").find("tbody");

        var inputContainer = $(this).parent().find('.pw-grid-pag-index-nam');
        $(inputContainer).attr('value', $(this).attr('pw-grid-pag-itm-index'));
        var paginationUrl = $(this).closest('.pw-grid-pagination-container').attr('pw-grid-pag-url');

        prowlerGridHelper.prowlerPostGrid(paginationUrl, this, prowlerGridHelper.dataBind, containerHost);
    });


    $(document).on('click', '.pw-grid-col-head-lbl-cnt', function (e) {
        event.stopImmediatePropagation();

        var sortParameter = $(this).attr("pw-grd-sort-prm");

        if (sortParameter != undefined) {

            var parentContainer = $(this).closest('tr');
            var resetSortIcons = $(parentContainer).find('.pw-glb-sort-by-ico');
            $(resetSortIcons).removeClass('pw-glb-sort-by-asc-ico');
            $(resetSortIcons).removeClass('pw-glb-sort-by-desc-ico');

            $(parentContainer).find('.pw-grd-sort-inp-ser').remove();

            var sortValue = $(this).attr('pw-glb-sort-val');
            var sortIcon = $(this).find('.pw-glb-sort-by-ico');
            var sortUrl = $(this).attr('pw-grd-sort');

            if (sortIcon.length == 0) {
                return;
            }

            if (sortValue == "desc") {
                sortValue = "asc";
                $(sortIcon).removeClass('pw-glb-sort-by-asc-ico')
                    .removeClass('pw-glb-sort-by-desc-ico')
                    .addClass('pw-glb-sort-by-asc-ico');
            }
            else {
                sortValue = "desc";
                $(sortIcon).removeClass('pw-glb-sort-by-asc-ico')
                    .removeClass('pw-glb-sort-by-desc-ico')
                    .addClass('pw-glb-sort-by-desc-ico');
            }

            $(this).attr('pw-glb-sort-val', sortValue);

            $('<input>').attr("type", "hidden")
                .attr("name", sortParameter)
                .attr("value", sortValue)
                .addClass("pw-grd-sort-inp-ser").appendTo(this);

            var containerHost = $(this).closest(".pw-grid-table-container").find(".pw-grid-table").find("tbody");

            prowlerGridHelper.prowlerPostGridHeader(sortUrl, this, prowlerGridHelper.dataBind, containerHost);
        }
    });

    let prowlerGridHelper = (function ProwlerHelperFunctions() {

        var searchFilterTimeDelay;

        function prowler_GirdGetLoader() {
            return $('<div class="pw-grid-loader-cnt">')
                .append(prowlerHelper.GetLoader())
        }

        function prowler_GridRemoveLoader(container) {
            $(container).find(".pw-grid-loader-cnt").remove();
        }

        function prowler_GetFilterContainer(containerId) {

            if (containerId == "") { return null; }

            return $('#' + containerId).clone(true);
        }

        function prowler_postGrid(url, sender, bindingFunc, container, errorHandlerFunc) {

            var dataSource = $(container).closest('.pw-grid-table-container');
            var errFunction = dataSource.attr('pw-grd-err-func');

            if ($(dataSource).length == 0) {
                return;
            }
            var includeFilterContainerId = $(dataSource).attr('pw-grd-filter-cnt-id');
            dataSource.append(prowlerGridHelper.GetGridLoader());
            $(container).closest('.pw-grid-table-main').find('.pw-grid-overlayer-cnt').show();

            var tip = $('<form>').html($(dataSource).clone(true));
            tip.append(prowler_GetFilterContainer(includeFilterContainerId));

            prowlerHelper.AjaxSend(url, 'POST', $(tip).serialize(), bindingFunc, container, errFunction);
        }

        function prowler_postGridHeaderAndPagination(url, sender, bindingFunc, container, errorHandlerFunc) {

            var dataSource = $(sender).closest('.pw-grid-table-container');
            var errFunction = dataSource.attr('pw-grd-err-func');

            var paginationContainer = $(dataSource).find('.pw-grid-pagination-container');
            var filterContainer = $(dataSource).find('.pw-grid-header-container');

            if ($(dataSource).length == 0) {
                return;
            }

            var includeFilterContainerId = $(dataSource).attr('pw-grd-filter-cnt-id');

            var tip = $('<form>').html($(filterContainer).clone(true)).append($(paginationContainer).html());
            tip.append(prowler_GetFilterContainer(includeFilterContainerId));
            tip.append($(dataSource).clone(true));

            dataSource.append(prowlerGridHelper.GetGridLoader());
            $(dataSource).closest('.pw-grid-table-main').find('.pw-grid-overlayer-cnt').show();

            prowlerHelper.AjaxSend(url, 'POST', $(tip).serialize(), bindingFunc, container, errFunction);
        }

        function prowler_gridServerFiltering(obj, inputDelay, filterUrl) {

            clearTimeout(searchFilterTimeDelay);

            searchFilterTimeDelay = setTimeout(function () {
                prowlerGridHelper.prowlerPostGridHeader(filterUrl, obj, null, null, null);
            }, inputDelay);
        }

        function doDrag(e) {

            var resizeValue = (startWidth + e.clientX - startX);

            $(resizeContent).width(resizeValue)
            $(resizeColumn).width(resizeValue)

        }

        function stopDrag(e) {
            document.documentElement.removeEventListener('mousemove', doDrag, false);
            document.documentElement.removeEventListener('mouseup', stopDrag, false);
        }

        function prowler_gridDataBindingParser(results, container) {

            var templateElementDefault = $(container).closest(".pw-grid-table-main").find(".pw-grid-table-defaultrow").find("tr");

            var headerRow = $(container).find('tr.pw-grid-header-container');

            $(container).html('');

            $(container).append(headerRow);

            var dataSource = results.DataSource;

            var paginationContainer = $(container).closest('.pw-grid-table-container').find('.pw-grid-pagination-container');
            var totalPages = Math.ceil(results.TotalItems / Number($(paginationContainer).attr('pw-grid-pag-size')));

            prowlerGridHelper.createPagination(paginationContainer, results.PageIndex, totalPages, results.TotalItems);

            $.each(dataSource, function (i, result) {

                var templateElement = templateElementDefault.clone();

                var attribute = $(templateElement).attr("pw-grd-dts-binding").split(',');

                var html = $(templateElement).html();

                $.each(attribute, function (index, paramenter) {

                    var value = result[paramenter];

                    html = prowlerHelper.StringReplace(html, '{#' + paramenter + '#}', value);
                });

                $(templateElement).html(html);
                prowlerGridHelper.checkBoxUpdateState(templateElement);
                $(container).append(templateElement);

            });

            prowlerGridHelper.RemoveGridLoader($(container).closest('.pw-grid-table-container'));
            $(container).closest('.pw-grid-table-main').find('.pw-grid-overlayer-cnt').hide();

            $(container).closest('.pw-grid-table').scrollTop(0);

            prowlerGridHelper.checkBoxHeaderStateRevaluate(container);
        }

        function prowler_gridUpdatePaginationPageItemLabel(container, pageIndex) {

            var labelElement = $(container).find(".pw-grd-pagination-lbl-txt");
            var totalItems = Number($(container).attr("pw-grd-pag-total-items"));
            var itemsPerPage = Number($(container).attr("pw-grid-pag-size"));


            if (labelElement == null) { return; }

            pageIndex = Number(pageIndex);
            totalItems = Number(totalItems);

            var itemPositionStart = (pageIndex - 1) * itemsPerPage + 1;
            var itemPositionEnd = pageIndex * itemsPerPage;
            var itemsText = "items";

            if (totalItems <= 1) { itemsText = "item"; }
            if (itemPositionEnd > totalItems) { itemPositionEnd = totalItems; }

            $(labelElement).html(itemPositionStart + " - " + itemPositionEnd + " of " + totalItems + " " + itemsText);
        }

        function prowler_gridCreatePaginationContainer(container, pageIndex, totalPagesForItems, totalItems) {

            pageIndex = Number(pageIndex);

            let totalPages, maxRange;
            let pageCounter = 1;

            let rangeList = [];

            let rangeGrow = Number($(container).attr('pw-grid-pag-range-grow'));

            totalPages = totalPagesForItems;
            maxRange = Number($(container).attr('pw-grid-pag-numbermax'));

            if (totalPages < maxRange) {
                maxRange = totalPages;
            }

            $(container).attr("pw-grd-pag-total-items", totalItems);
            $(container).find('.pw-grid-pagination-cnt').remove();
            $(container).find('.pw-grid-pagination-item-dis').remove();


            let lowValue = pageIndex - (maxRange - rangeGrow) + rangeGrow;
            let lowRangeUnalocated = 0;
            let hightRageGrow = pageIndex + rangeGrow;

            if (hightRageGrow > totalPages) {
                lowValue = lowValue - (hightRageGrow - totalPages);
                lowRangeUnalocated = totalPages - hightRageGrow;
            }

            for (index = lowValue - 1; index <= pageIndex; index++) {
                if (index > 0) {
                    rangeList.push(index);
                }
                else {
                    lowRangeUnalocated++;
                }
            }

            let highRange = pageIndex + lowRangeUnalocated + rangeGrow + 1;

            for (index = pageIndex + 1; index < highRange; index++) {
                rangeList.push(index);
            }

            let firstLastElementClass;

            if (pageIndex == '1') {
                firstLastElementClass = 'pw-grid-pagination-item-dis';
            }
            else {
                firstLastElementClass = 'pw-grid-pagination-cnt';
            }

            $('<a>').attr({
                'pw-grid-pag-itm-index': '1',
                class: firstLastElementClass
            }).html('❮❮').appendTo(container);


            for (index = 0; index < rangeList.length; index++) {

                let paginationSelector = 'pw-grid-pagination-cnt'

                if (rangeList[index] == pageIndex) {
                    paginationSelector = paginationSelector.concat(" ", 'pw-grid-pagination-sel');
                }

                $('<a>').attr({
                    'pw-grid-pag-itm-index': rangeList[index],
                    class: paginationSelector
                }).html(rangeList[index]).appendTo(container);

            }

            if (pageIndex == totalPages) {
                firstLastElementClass = 'pw-grid-pagination-item-dis';
            }
            else {
                firstLastElementClass = 'pw-grid-pagination-cnt';
            }

            $('<a>').attr({
                'pw-grid-pag-itm-index': totalPages,
                class: firstLastElementClass
            }).html('❯❯').appendTo(container);

            prowler_gridUpdatePaginationPageItemLabel(container, pageIndex);
        }

        function prowler_CheckBoxSetValue(sender, value) {

            if (sender == null) { return; }

            $(sender).attr('value', value);
            $(sender).prop("checked", (value.toLowerCase() === "true"))
        }

        function prowler_CheckBoxHeaderStateUpdateByChild(sender) {

            if (sender == null) { return; }

            var checkBoxIsChild = $(sender).hasClass("p-grid-table-checkBox-container");

            if (!checkBoxIsChild) { return; }

            var containerName = $(sender).find("input").attr('pw-grd-chk-name-s');
            if (containerName == null) { return; }

            var parentContainer = $(sender).closest('.pw-grid-table');

            var elements = $(parentContainer).find(".pw-grd-chk-name-inpt-s[pw-grd-chk-name-s='" + containerName + "']");

            var allCheckBoxSelected = "true";
            var allCheckBoxDisable = "true";

            if (elements != undefined) {
                $(elements).each(function (index) {
                    if ($(this).attr('value').toLowerCase() == "false"
                        && $(this).attr('pw-grd-ckk-dis-state').toLowerCase() == "false"
                    ) {
                        allCheckBoxSelected = "false";
                    }

                    if ($(this).attr('pw-grd-ckk-dis-state').toLowerCase() == "false") {
                        allCheckBoxDisable = "false";
                    }
                });

                if (allCheckBoxSelected == "true" && allCheckBoxDisable == "true") {
                    allCheckBoxSelected = "false";
                }

                var checkBoxHead = $(parentContainer).find(".p-grid-table-checkBox-container-head[pw-grd-chk-head-name-s='" + containerName + "']");
                prowler_CheckBoxSetValue($(checkBoxHead).find(".pw-grd-chk-name-id-s"), allCheckBoxSelected);
            }
        }

        function prowler_CheckBoxHeaderClick(sender) {

            var parentContainer = $(sender).closest('.pw-grid-table');
            var containerName = $(sender).attr('pw-grd-chk-head-name-s');

            if (containerName == null) { return; }

            var checkState = $(sender).find('input').attr('value');

            var elements = $(parentContainer).find(".pw-grd-chk-name-inpt-s[pw-grd-chk-name-s='" + containerName + "']");

            if (elements != undefined) {
                $(elements).each(function (index) {
                    if ($(this).attr('pw-grd-ckk-dis-state').toLowerCase() == "false") {
                        prowler_CheckBoxSetValue(this, checkState)
                    }
                });
            }
        }

        function prowler_CheckBoxHeaderStateRevaluate(sender) {

            var parentContainer = $(sender).closest('.pw-grid-table');

            var checkBoxHeaders = parentContainer.find(".p-grid-table-checkBox-container-head");


            if (checkBoxHeaders == null) { return; }

            $(checkBoxHeaders).each(function (index) {
                var containerName = $(this).attr('pw-grd-chk-head-name-s');
                var firstContainer = $(parentContainer).find(".pw-grd-chk-name-inpt-s[pw-grd-chk-name-s='" + containerName + "']").first().closest('.p-grid-table-checkBox-container');

                if (firstContainer != null) {
                    prowler_CheckBoxHeaderStateUpdateByChild(firstContainer);
                }
            });
        }

        function prowler_CheckBoxUpdateState(template) {

            var checkBox = $(template).find('.pw-grd-chk-name-inpt-s');

            if (checkBox == null) { return; }

            $(checkBox).each(function (index) {
                if ($(this).attr('value').toLowerCase() == "true") {
                    $(this).prop("checked", "checked");
                }

                if ($(this).attr('pw-grd-ckk-dis-state').toLowerCase() == "true") {
                    var parent = $(this).closest(".p-grid-table-checkBox-container");
                    parent.removeClass("p-grid-table-checkBox-container");
                    parent.addClass("p-grid-table-checkBox-container-dis");
                    parent.find(".p-grid-table-checkBox-checkmark").removeClass("p-grid-table-checkBox-checkmark").addClass("p-grid-table-checkBox-checkmark-dis");
                }
            });
        }

        return {
            filter: prowler_gridServerFiltering,
            resizeColumnDoDrag: doDrag,
            resizeColumnStopDrag: stopDrag,
            prowlerPostGrid: prowler_postGrid,
            prowlerPostGridHeader: prowler_postGridHeaderAndPagination,
            GetGridLoader: prowler_GirdGetLoader,
            RemoveGridLoader: prowler_GridRemoveLoader,
            dataBind: prowler_gridDataBindingParser,
            createPagination: prowler_gridCreatePaginationContainer,
            checkBoxHeaderClick: prowler_CheckBoxHeaderClick,
            checkBoxHeaderStateUpdateByChild: prowler_CheckBoxHeaderStateUpdateByChild,
            checkBoxHeaderStateRevaluate: prowler_CheckBoxHeaderStateRevaluate,
            checkBoxUpdateState: prowler_CheckBoxUpdateState,
            checkBoxSetValue: prowler_CheckBoxSetValue
        }
    })();

    window.prowlerGrid = function ProwlerGridFunctions(id) {

        var gridListId = "#" + id;

        function prowlerGridRefresh(url) {
            var tableContainer = $(gridListId);

            var containerHost = $(tableContainer).find(".pw-grid-table-container").find(".pw-grid-table").find("tbody");

            var paginationUrl = $(tableContainer).find('.pw-grid-pagination-container').attr('pw-grid-pag-url');

            if (url != null) {
                paginationUrl = url;
            }

            prowlerGridHelper.prowlerPostGrid(paginationUrl, this, prowlerGridHelper.dataBind, containerHost);
        }

        function prowler_CheckBoxUpdate(binding, state) {
            var tableContainer = $(gridListId);

            if (tableContainer == null) { return; }

            var elements = $(tableContainer).find(".pw-grd-chk-name-inpt-s[pw-grd-chk-name-s='" + binding + "']");

            if (elements == null) { return; }

            if (elements != undefined) {
                $(elements).each(function (index) {
                    if ($(this).attr('pw-grd-ckk-dis-state').toLowerCase() == "false") {
                        prowlerGridHelper.checkBoxSetValue(this, String(state))
                    }
                });
            }

            var checkBoxHead = $(tableContainer).find(".p-grid-table-checkBox-container-head[pw-grd-chk-head-name-s='" + binding + "']");
            prowlerGridHelper.checkBoxSetValue($(checkBoxHead).find(".pw-grd-chk-name-id-s"), String(state));
        }

        return {
            Refresh: prowlerGridRefresh,
            SetCheckBoxContainersState: prowler_CheckBoxUpdate
        }
    }
});