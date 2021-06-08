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

        var parentContainer = $(this).parent().parent().parent();
        var searchValue = $(this).val();

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

    $(".p-dropdown-container-element").on('click', function (event) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        event.preventDefault();

        if ($(this).attr("p-dropdown-ms-enable") == "1") {
            prowlerDropDownHelper.multiSelectClick(this, true);
            return;
        }
        
        var parentElement = $(this).parent(); //changed

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

        return {
            disable: DisableDropDown,
            enable: EnableDropDown,
            getSelectedValue: GetSelectedValue
        }
    }  

    let prowlerDropDownHelper = (function ProwlerHelperFunctions() {

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

                var container = $("<div>");

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

        return {
            callFunction: executeFunctionByName,
            multiSelectClick: prowler_dropDownMultiSelectClick,
            multiSelectAddRemove: prowler_SelectedContainerAddRemove,
            multiSelectSetFocus: prowler_dropDownMultiSelectSetFocus,
            eventOpen: prowler_dropDownOpenEvent,
            eventSelected: prowler_dropDownSelectedEvent,
            eventSelectedChanged: prowler_dropDownSelectedChangedEvent
        }
    })();
});