var windowHeight = $(window).height();
var windowWidth = $(window).width();

var kGrid_InitiateClosure_Height = "";
var kGrid_S2GuidMap_NonClosedWorkbook_Height = "";
var kGrid_S2GuidMap_MappedGuid_Height = "";
var kGrid_S2GuidMap_Search_Height = "";
var kGrid_Access_SearchUser_Height = "";
var kGrid_Access_Help_Height = "";

var auditManagerDb_Grid_Height = "";

var gridColWidth = {
    PT05: 1,
    PT10: 2,
    PT15: 3,
    PT20: 4,
    PT25: 5,
    PT30: 6,
    PT35: 7,
    PT40: 8,
    PT45: 9,
    PT50: 10,
    PT60: 11,
    DEFAULT: 12,
    FIXED: 13,
    properties: {
        1: { name: "PT05", width: 0.05 },
        2: { name: "PT10", width: 0.1 },
        3: { name: "PT15", width: 0.15 },
        4: { name: "PT20", width: 0.20 },
        5: { name: "PT25", width: 0.25 },
        6: { name: "PT30", width: 0.30 },
        7: { name: "PT35", width: 0.35 },
        8: { name: "PT40", width: 0.40 },
        9: { name: "PT45", width: 0.45 },
        10: { name: "PT50", width: 0.50 },
        11: { name: "PT60", width: 0.60 },
        12: { name: "DEFAULT", width: 0.2 },
        13: { name: "FIXED", width: 100 },
    }
};

if (Object.freeze) {
    Object.freeze(gridColWidth);
};

function Init() {
    //SetGridSize();
}

function ResizeMainWindow() {
    if (IsNothing($("#div-footer").offset()) || IsNothing($("#left-eng").offset()))
        return;
    
    $("#left-eng").css('height', $("#div-footer").offset().top - $("#left-eng").offset().top);
    $(".content-right").css('height', $("#div-footer").offset().top - $(".content-right").offset().top);
    //alert($("#div-footer").offset().top - $(".div-left-sidebar-dimension").offset().top);
    $(".content-only").css('height', $("#div-footer").offset().top - $(".content-only").offset().top);
}

function ResizeGrid2() {
    $.each($(".k-grid-content"), function (idx, item) {
        var grid = $(item).closest(".kGrid");
        if (!IsNothing(grid.prop("id"))) {
            SetGridSize(grid.prop("id"));
        }
        else {
            var grid = $(item).closest("._kGrid");
            if (!IsNothing(grid.prop("id"))) {
                resizeGrid(grid.prop("id"));
            }
        }
    });
}

function SetGridSize(gridId) {
    var gridElement = $("#" + gridId);
    var dialog = gridElement.closest(".ui-dialog-content");
    var dataArea = gridElement.find(".k-grid-content");
    var gridHeight = gridElement.innerHeight();
    var otherElements = gridElement.children().not(".k-grid-content");
    var otherElementsHeight = 0;

    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });

    var availablePageHeight = ((dialog.height() + dialog.offset().top) - (gridElement.offset().top)) - 40;
    //alert(availablePageHeight);

    availablePageHeight = availablePageHeight < 100 ? 100 : availablePageHeight;
    //alert(availablePageHeight);

    dataArea.height(availablePageHeight - otherElementsHeight);
    gridElement.height(availablePageHeight);
}

//function SetGridSize() {

//    kGrid_InitiateClosure_Height = windowHeight * 0.50;
//    kGrid_S2GuidMap_NonClosedWorkbook_Height = windowHeight * 0.35;
//    kGrid_S2GuidMap_MappedGuid_Height = windowHeight * 0.50;
//    kGrid_S2GuidMap_Search_Height = windowHeight * 0.35;
//    kGrid_Access_SearchUser_Height = windowHeight * 0.25;
//    kGrid_Access_Help_Height = windowHeight * 0.30;

//    auditManagerDb_Grid_Height = windowHeight * 0.55;
//}


function LeftSelection() {
    
    var chk = $($("ul.ulEngFarm > li > .chkEngFldr:checked")[0]);
    if (chk.length == 0) {

        //alert("length - 0");

        if (!IsNothing(viewModel.selectedWs())) {
            var ch = $("[id='" + viewModel.selectedWs().ObjectID() + "']");
            ch.prop("checked", false);
            ch.trigger("click");
        }
    }
    else {
        chk.prop("checked", false);
        chk.trigger("click");
    }
}

function TabStyleNEvent() {
    $("#mainTab").tabs({ heightStyle: "auto" });
    $("#myEngTab").tabs({ heightStyle: "auto" });

    $("div.ui-tabs").css('padding', '2px');
    $("div.ui-tabs-panel").css('padding', '2px');

    $("div.ui-tabs").css('margin', '0');
    $("div.ui-tabs-panel").css('margin', '0');

    $("div.ui-tabs").css('border', '0');
    $("div.ui-tabs-panel").css('border', '0');

    $("#myEngTab").tabs("disable");

    $("#mainTab").on("tabsactivate", function (event, ui) {
        var selectedTab = $("#mainTab").tabs('option', 'active');
        
        if (isInEdit) {
            $("#mainTab").tabs("option", "active", 0);
            if (selectedTab != 0) {
                AmAlert(AmMsg.isInEdit_Msg);
            }
        }
        else {

            if (selectedTab == 1) {
                Draw_kGrid_Activity();
            }
            else if (selectedTab == 0) {

                var selectedTab_myEngTab = $("#myEngTab").tabs('option', 'active');

                if (selectedTab_myEngTab == 0) {
                    LeftSelection();
                }

                $("#myEngTab").tabs("option", "active", 0);
            }
            ResizeMainWindow();
        }
    });

    $("#myEngTab").on("tabsactivate", function (event, ui) {
        event.stopPropagation();
        var selectedTab = $("#myEngTab").tabs('option', 'active');
        if (selectedTab == 3) {
            Draw_kGrid_Eng_Activity();
        }
        else if (selectedTab == 0) {
            var chk = $($("ul.ulEngFarm > li > .chkEngFldr:checked")[0]);
            chk.trigger("click");
            //LeftSelection();
        }
        ResizeMainWindow();
    });
}

$(function () {
    TabStyleNEvent();
    ResizeMainWindow();
});

$(window).resize(function () {
    windowHeight = $(window).height();
    windowWidth = $(window).width();
    $(function () {
        ResizeMainWindow();
    });
    ResizeDialog();
    ResizeGrid2();
});

function SetGridSize_IFrame(gridId) {

    var gridElement = $("#" + gridId);
    var html = $("html");
    var dataArea = gridElement.find(".k-grid-content");
    var gridHeight = gridElement.innerHeight();
    var otherElements = gridElement.children().not(".k-grid-content");
    var otherElementsHeight = 0;

    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });

    var availablePageHeight = ((html.height() + html.offset().top) - (gridElement.offset().top)) - 40;

    dataArea.height(availablePageHeight - otherElementsHeight);
    gridElement.height(availablePageHeight);

}

function resizeGrid(gridId) {

    if (IsNothing($("#div-footer").offset())) {
        SetGridSize_IFrame(gridId);
        return;
    }

    var gridElement = $("#" + gridId);
    var dataArea = gridElement.find(".k-grid-content");
    var gridHeight = gridElement.innerHeight();
    var otherElements = gridElement.children().not(".k-grid-content");
    var otherElementsHeight = 0;

    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });

    var availablePageHeight = ($("#div-footer").offset().top - $("#" + gridId).offset().top) - 5;

    dataArea.height(availablePageHeight - otherElementsHeight);
    gridElement.height(availablePageHeight);
}

function GetGridColWidth(myGridColWidth, gridId) {
    if (gridColWidth.properties[myGridColWidth].name.IsMatch("FIXED"))
        return gridColWidth.properties[myGridColWidth].width;

    return $('#' + gridId).width() * gridColWidth.properties[myGridColWidth].width;
}

Init();


