var dialogSize = {
    XSMALL: 1,
    SMALL: 2,
    MEDIUM: 3,
    LARGE: 4,
    XLARGE: 5,
    DEFAULT: 6,
    AUTO_ALL: 7,
    AUTO_HEIGHT_SMALL_WIDTH: 8,
    AUTO_HEIGHT_LARGE_WIDTH: 9,
    AUTO_HEIGHT_XLARGE_WIDTH: 10,
    AUTO_WIDTH: 11,
    properties: {
        1: { name: "XSMALL", width: 0.2, height: 0.2 },
        //2: { name: "SMALL", width: 0.3, height: 0.4 },
        2: { name: "SMALL", width: "auto", height: "auto" },
        3: { name: "MEDIUM", width: 0.4, height: 0.4 },
        4: { name: "LARGE", width: 0.6, height: 0.6 },
        5: { name: "XLARGE", width: 0.9, height: 0.8 },
        6: { name: "DEFAULT", width: 0.5, height: 0.5 },
        7: { name: "AUTO_ALL", width: "auto", height: "auto" },
        8: { name: "AUTO_HEIGHT_SMALL_WIDTH", width: 0.3, height: "auto" },
        9: { name: "AUTO_HEIGHT_LARGE_WIDTH", width: 0.6, height: "auto" },
        10: { name: "AUTO_HEIGHT_XLARGE_WIDTH", width: 0.9, height: "auto" },
        11: { name: "AUTO_WIDTH", width: "auto", height: 0.5 },
    }
};

var dialog_Option = {

    autoOpen: false,
    resizable: false,
    modal: true,
    draggable: false,

    width: dialogSize.properties[dialogSize.DEFAULT].width * windowWidth,
    height: dialogSize.properties[dialogSize.DEFAULT].height * windowHeight,

    open: function () {

        if (!IsNothing($(this).data('openAction'))) {
            $(this).data('openAction')();
        }

        myApp.hidePleaseWait();

    },
    close: function () {

        if (!IsNothing($(this).data('closeAction'))) {
            if (!IsNothing($(this).data('closeParam')))
                $(this).data('closeAction')($(this).data('closeParam'));
            else
                $(this).data('closeAction')();
        }

        //$(this).remove();
        //$(this).find("form").remove();
        //$(this).dialog('destroy');

        $(this).html('');

        DialogClose(this);

    }
};

var dialog_only_close_Option = {

    autoOpen: false,
    resizable: false,
    modal: true,
    draggable: false,

    width: dialogSize.properties[dialogSize.DEFAULT].width * windowWidth,
    height: dialogSize.properties[dialogSize.DEFAULT].height * windowHeight,

    buttons: {
        Close: function () {

            $(this).dialog("close");

        }
    },

    open: function () {
        if (!IsNothing($(this).data('openAction'))) {
            $(this).data('openAction')();
        }

        myApp.hidePleaseWait();
    },
    close: function () {

        if (!IsNothing($(this).data('closeAction'))) {
            if (!IsNothing($(this).data('closeParam')))
                $(this).data('closeAction')($(this).data('closeParam'));
            else
                $(this).data('closeAction')();
        }

        $(this).html('');

        DialogClose(this);
    }
};

var interaction_Option = {

    autoOpen: false,
    resizable: false,
    modal: true,
    draggable: false,

    width: dialogSize.properties[dialogSize.DEFAULT].width * windowWidth,
    height: dialogSize.properties[dialogSize.DEFAULT].height * windowHeight,

    buttons: {
        OK: function () {

            //alert($("#txtActivityComment").val());

            if (!IsNothing($(this).data('okAction'))) {
                if (!IsNothing($(this).data('param')))
                    $(this).data('okAction')(this, $(this).data('param'));
                else
                    $(this).data('okAction')(this);
            }

            if (!IsNothing($(this).data('doneAction')))
                $(this).data('doneAction')(this);

        },
        Cancel: function () {

            $(this).dialog("close");
        }
    },
    open: function () {
        if (!IsNothing($(this).data('openAction'))) {
            $(this).data('openAction')();
        }

        myApp.hidePleaseWait();
    },
    close: function () {

        if (!IsNothing($(this).data('closeAction'))) {
            if (!IsNothing($(this).data('closeParam')))
                $(this).data('closeAction')($(this).data('closeParam'));
            else
                $(this).data('closeAction')();
        }

        $(this).html('');

        DialogClose(this);
    }
};

var confirm_Option = {

    autoOpen: false,
    resizable: false,
    modal: true,
    draggable: false,

    //hide: {
    //    effect: "scale",
    //    easing: "easeInBack",
    //},
    //show: {
    //    effect: "scale",
    //    easing: "easeOutBack",
    //},


    width: dialogSize.properties[dialogSize.DEFAULT].width * windowWidth,
    height: dialogSize.properties[dialogSize.DEFAULT].height * windowHeight,

    buttons: {
        Yes: function () {

            $(this).dialog("close");

            if (!IsNothing($(this).data('yesAction'))) {
                if (!IsNothing($(this).data('yesParam')))
                    $(this).data('yesAction')($(this).data('yesParam'));
                else
                    $(this).data('yesAction')();
            }

        },
        No: function () {

            $(this).dialog("close");

            if (!IsNothing($(this).data('noAction'))) {
                if (!IsNothing($(this).data('noParam')))
                    $(this).data('noAction')($(this).data('noParam'));
                else
                    $(this).data('noAction')();
            }
        }
    },
    open: function () {
        if (!IsNothing($(this).data('openAction'))) {
            $(this).data('openAction')();
        }

        myApp.hidePleaseWait();
    },
    close: function () {

        if (!IsNothing($(this).data('closeAction'))) {
            if (!IsNothing($(this).data('closeParam')))
                $(this).data('closeAction')($(this).data('closeParam'));
            else
                $(this).data('closeAction')();
        }

        $(this).html('');

        DialogClose(this);
    }
};

var dialogType = {
    DIALOG: 1,
    INTERACTION: 2,
    CONFIRM: 3,
    DIALOG_ONLY_CLOSE: 4,
    properties: {
        1: { name: "DIALOG", option: dialog_Option },
        2: { name: "INTERACTION", option: interaction_Option },
        3: { name: "CONFIRM", option: confirm_Option },
        4: { name: "DIALOG_ONLY_CLOSE", option: dialog_only_close_Option },
    }
};

var dialogClass = {
    ALERT: "no-title no-padding-no-min-height-dialog-5 no-padding-no-min-height-content-5",
    INTERACTION: "no-title no-padding-no-min-height-dialog-12 no-padding-no-min-height-content-12",
    CONFIRM: "no-title no-padding-no-min-height-dialog-12 no-padding-no-min-height-content-12",
    COMMENT: "no-close",
    DISPLAY_COMMENT: "no-title no-padding-no-min-height-dialog-12 no-padding-no-min-height-content-12",
};

if (Object.freeze) {
    Object.freeze(dialogSize);
    Object.freeze(dialogType);
    Object.freeze(dialogClass);
};

function GetDialog(myDialogType, myDialogSize, myDialogClass, myDialogTitle) {

    dialogType.properties[myDialogType].option.width = dialogSize.properties[myDialogSize].width.toString().IsMatch("auto") ?
        "auto" :
        dialogSize.properties[myDialogSize].width * windowWidth;

    dialogType.properties[myDialogType].option.height = dialogSize.properties[myDialogSize].height.toString().IsMatch("auto") ?
        "auto" :
        dialogSize.properties[myDialogSize].height * windowHeight;

    dialogType.properties[myDialogType].option.dialogClass = myDialogClass;
    dialogType.properties[myDialogType].option.title = myDialogTitle;

    return $("<div class='" + myDialogSize + "'></div>").dialog(dialogType.properties[myDialogType].option);
};

$(function () {
    html_comment_Template = $("#comment_Template").html();
    html_userSearch_Template = $("#userSearch_Template").html();
    html_s2GuidSearchResult_Template = $("#s2GuidSearchResult_Template").html();
    html_largeRetentionFile_Template = $("#largeRetentionFile_Template").html();
});

function ResizeDialog() {

    $.each($(".ui-dialog-content"), function (idx, item) {

        var myDialogSize = $(this).prop("class").SplitNGet(/\s+/, 0);

        $(".ui-dialog-content").dialog("option", "width",
                dialogSize.properties[myDialogSize].width.toString().IsMatch("auto") ?
                "auto" :
                dialogSize.properties[myDialogSize].width * windowWidth
            );

        $(".ui-dialog-content").dialog("option", "height",
            dialogSize.properties[myDialogSize].height.toString().IsMatch("auto") ?
            "auto" :
            dialogSize.properties[myDialogSize].height * windowHeight
            );
    })
}

var html_comment_Template = $("#comment_Template").html();
var html_userSearch_Template = $("#userSearch_Template").html();
var html_s2GuidSearchResult_Template = $("#s2GuidSearchResult_Template").html();
var html_largeRetentionFile_Template = $("#largeRetentionFile_Template").html();

function CloseMyDialog(that) {
    if (IsNothing(that))
        $(".ui-dialog-content").dialog('close');
    else
        $(that).closest(".ui-dialog-content").dialog('close');
}

function DialogClose(that) {
    $(that).dialog('destroy');
}
