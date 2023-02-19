function renderMenu(action, controller) {
    let actionSend = "/" + controller + "/" + action;

    attachLoadingAnimation("#SideMenu");

    $.ajax({
        url: actionSend,
        type: "GET",
        dataType: "HTML",        
        success: function (result) {
            $("#SideMenu").html(result);
        },
        error: function (request, status, error) {
            alert(error);
        }
    });
}

function renderDropDownMenu() {
    renderMenu("GetDropDownSideMenu", "DropDownList")
}

function renderGridMenu() {
    renderMenu("GetGridSideMenu", "Grid")
}

$(document).on('click', '.itemSideMenu', function (event) {
    $(this).parent().find("li").removeClass("menuSiteSelected");
    $(this).addClass("menuSiteSelected");
    menuItemClick($(this).attr("pageSrc"));
});

function menuItemClick(url) {  
    if (url == null) {
        return;
    }

    attachLoadingAnimation("#mainContainer");

    $.ajax({
        url: url,
        type: "GET",
        dataType: "HTML",
        success: function (result) {
            $("#mainContainer").html(result);
        },
        error: function (request, status, error) {
            alert(error);
        }
    });
}

function attachLoadingMainContainer() {
    attachLoadingAnimation($("#mainContainer"));
}

function attachLoadingAnimation(container) {
    let loader = $('<center><div class="loader-prowler-container" style="margin:5px;height:40px;width:40px;display:block;"></center>');

    $('<div>').attr({
        class: "loader-prowler"
    }).appendTo(loader);

    $(container).html(loader);
}