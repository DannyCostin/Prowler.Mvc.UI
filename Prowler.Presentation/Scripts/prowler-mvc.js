$(document).ready(function () {

    $(document).bind('click', function () {
        $('.p-dropdown-container').find(".p-dropdown-list").hide();
        $('.p-dropdown-container').find(".p-dropdown").removeClass("p-dropdown-focus");
        $('.p-dropdown-container').find(".p-dropdown-filter-container").hide();
    });

    $(".p-dropdown-list").on('click', function (event) {
        event.stopImmediatePropagation();
    });

    $(".p-dropdown-filter-input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            return false;
        }
        event.stopImmediatePropagation();
    });

    $(".p-dropdown-filter-input").keyup(function () {
        event.stopImmediatePropagation();

        var parentContainer = $(this).parent().parent().parent().parent();

        if ($(parentContainer).attr("p-dropdown-srv-filter-enable")) {
            prowlerDropDownHelper.serverFiltering(this);
        }
        else {
            prowlerDropDownHelper.clientFiltering(this);
        }
    });

    $(".p-dropdown-filter-input").on('click', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
    });

    
    $(".p-dropdown-container").on('click', function (event) {
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
            
        $(this).find(".p-dropdown-list").slideToggle(250);
        $(this).find(".p-dropdown-filter-container").toggle();
        $(this).find(".p-dropdown-filter-container").find(".p-dropdown-filter-input").focus();
        prowlerDropDownHelper.eventOpen(this);        
    });

    $(".p-dropdown-list").on('click', ".p-dropdown-container-element", function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        event.preventDefault();

        if ($(this).attr("p-dropdown-ms-enable") == "1") {
            prowlerDropDownHelper.multiSelectClick(this, true);
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
            return $(dropDownId).find("#ps-s-value-selected").val();
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
                    bindingFunc(results, container);                           
                },
                error: function (request, status, error) {
                    if (bindingErrorHandlingFunc != undefined) {
                        bindingErrorHandlingFunc(request, container)
                    }                  
                }
            });    
        }

        return {
            DataBind: prowler_DataBind,
            StringReplace: replaceString,
            GetLoader: createLoader,
            RemoveLoader: removeLoader
        }

    })();

    let prowlerDropDownHelper = (function ProwlerHelperFunctions() {

        var searchFilterTimeDelay;

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

                var container = $('<div class="ps-multiselect-srz">');

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
                executeFunctionByName(eventFunc, window);
            }
        }

        function prowler_dropDownSelectedEvent(obj, value, label) {

            var eventFunc = $(obj).attr("p-dropdown-ev-selected")

            if (eventFunc != null) {
                executeFunctionByName(eventFunc, window, value, label);
            }
        }

        function prowler_dropDownSelectedChangedEvent(obj, value, label, currentValue) {

            var eventFunc = $(obj).attr("p-dropdown-ev-selected-changed")

            if (currentValue == value) {
                return;
            }

            if (eventFunc != null) {
                executeFunctionByName(eventFunc, window, value, label);
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

                $.each(attribute, function(index, paramenter){

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
            
            clearTimeout(searchFilterTimeDelay);

            searchFilterTimeDelay = setTimeout(function () {

                prowlerHelper.RemoveLoader(dropdownContainer);
                prowlerHelper.GetLoader().appendTo(dropdownContainer);
                
                prowlerHelper.DataBind($(parentContainer).attr("p-dropdown-srv-filter-url"), "POST",
                    properties, prowler_dropDownDataBindingParser, parentContainer, prowler_dropDownDataBindingErrorParser);             
            }, timeDelayValue);                   
        }

        function prowler_dropDownDataBindingErrorParser(results, container) {

            var dropdownContainer = $(container).find('.p-dropdown-list');
            $(dropdownContainer).html(results.statusText + ' ' + results.status );
        }

        return {
            callFunction: executeFunctionByName,
            multiSelectClick: prowler_dropDownMultiSelectClick,
            multiSelectAddRemove: prowler_SelectedContainerAddRemove,
            multiSelectSetFocus: prowler_dropDownMultiSelectSetFocus,
            eventOpen: prowler_dropDownOpenEvent,
            eventSelected: prowler_dropDownSelectedEvent,
            eventSelectedChanged: prowler_dropDownSelectedChangedEvent,
            clientFiltering: prowler_dropDownClientFiltering,
            serverFiltering: prowler_dropDownServerFiltering,
            dataBindParser: prowler_dropDownDataBindingParser,
            dataBingErrorParser: prowler_dropDownDataBindingErrorParser
        }
    })();
});