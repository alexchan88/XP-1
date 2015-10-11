
var isValid = (function () {
  var rg1 =/^[^\\/:\*\?"<>\|]+$/; // forbidden characters \ / : * ? " < > |
  var rg2=/^\./; // cannot start with dot (.)
  var rg3=/^(nul|prn|con|lpt[0-9]|com[0-9])(\.|$)/i; // forbidden file names
  return function isValid(fname) {
    return rg1.test(fname) &&!rg2.test(fname) &&!rg3.test(fname);
    }
    }) ();

$(function () {
    var ctrlDown = false;

    //var bootstrapTooltip = $.fn.tooltip.noConflict();
    //$.fn.bstooltip = bootstrapTooltip;

    //$(".toolTipImg").bstooltip();

    //var bsTooltip = $.fn.tooltip.noConflict();

    //$(document).tooltip();

    //$(document).tooltip();

    $("body").on("input", ".onlyNumber", (function (e) {
        //alert($(this).val());
        //alert(e.originalEvent.clipboardData.getData('Text'));
    }));

    $("body").on("paste", ".onlyNumber", (function (e) {
        //alert(e.originalEvent.clipboardData.getData('Text'));
    }));

    $("body").on("keydown", ".onlyNumber", (function (e) {
        if (e.keyCode == 17) {
            ctrlDown = true;
        }
        else if ((e.keyCode >= 48 && e.keyCode <= 57) || e.keyCode == 8 || e.keyCode == 46) {

        }
        else if (ctrlDown && e.keyCode == 86) {
            //ctrlDown = false;
        }
        else {
            return false;
        }
    })).on("keyup", ".onlyNumber", (function (e) {
        //alert(ctrlDown);
        if (ctrlDown && (e.keyCode == 86 || e.keyCode == 17)) {
        }

        if (e.keyCode == 17) {
            ctrlDown = false;
        }
    }));

    //----------------------------------------------------------------------------------------------------------
    $("body").on("keydown", ".onlyFileName", (function (e) {
        //alert("keydown => " + e.keyCode);
    })).on("keyup", ".onlyFileName", (function (e) {
        //alert("keyup => " + e.keyCode);
    }));
    //----------------------------------------------------------------------------------------------------------
});

function querySt(Key) {
    var url = window.location.href;
    KeysValues = url.split(/[\?&]+/);
    for (i = 0; i < KeysValues.length; i++) {
        KeyValue = KeysValues[i].split("=");
        if (KeyValue[0].toUpperCase() == Key.toUpperCase()) {
            return KeyValue[1];
        }
    }
}

var newSession = false;
var isWsCreate = false;
var internalReload = false;
var toLoadWs = true;

var usrAccessRights = [
    { Action: "View files", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "Yes" },
    { Action: "View workspace profile", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "Yes" },
    { Action: "Edit workspace profile", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "No" },
    { Action: "Add and remove users", Administrators: "Yes", ReadWrite: "No", ReadOnly: "No" },
    { Action: "View groups", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "Yes" },
    { Action: "Initiate workspace closure", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "No" },
    { Action: "Request RET", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "No" },
    { Action: "Request rollforward file", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "No" },
    { Action: "Remove file", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "No" },
    { Action: "Create workspace", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "Yes" },
    { Action: "Upload files", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "No" },
    { Action: "Download files", Administrators: "Yes", ReadWrite: "Yes", ReadOnly: "Yes" },
    { Action: "Approve/Reprocess/Remove RET", Administrators: "Yes", ReadWrite: "No", ReadOnly: "No" },
    { Action: "Reprocess rollforward file", Administrators: "Yes", ReadWrite: "No", ReadOnly: "No" },
];

function GetRemoteSize(size, percentage) {
    if (!remote)
        return size;

    var returnSize = Math.ceil(size * (IsNothing(percentage) ? .6 : percentage));
    return returnSize;
}

function UsrGrpNameToDisplay(grpName) {
    if (grpName.match(/E_ADMIN/i)) {
        return "Administrators Group";
    }
    else if (grpName.match(/E_MEMBERS/i)) {
        return "Read/Write Group";
    }
    else if (grpName.match(/E_READ_ONLY/i)) {
        return "Read Only Group";
    }
    else {
        return "";
    }
}

function UsrGrpToDisplay(grpName) {

    if ((grpName.match(/E_ADMIN/i)) || (grpName.match(/E_MEMBERS/i)) || (grpName.match(/E_READ_ONLY/i))) {
        return true;
    }

    return false;
}

var myApp;
myApp = myApp || (function () {
    var pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h3 id="waitText"></h3></div><div class="modal-body"><div class="progress progress-striped active"><div class="progress-bar" style="width: 100%;"></div></div></div></div></div></div>');
    return {
        showPleaseWait: function (waitText) {
            pleaseWaitDiv.find("#waitText").text(waitText + ".....");
            pleaseWaitDiv.modal('show');
            //$("#dmProcessing").dialog('open');
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.modal('hide');
            //$("#dmProcessing").dialog('close');
        },

    };
})();

var rollFwd = "/4 - Roll Forward ENG Files";
var wsLog = "/Workspace Log";
var bkUps = "/6 - Backups";

function ToIncludeInClosure(path) {
    //alert(path);
    if (path.indexOf(rollFwd) > -1 || path.indexOf(wsLog) > -1 || path.indexOf(bkUps) > -1) {
        return false;
    }

    return true;
}

function ToIncludeInClosure2(path) {
    //alert(path);
    if (path.indexOf(rollFwd) > -1 || path.indexOf(wsLog) > -1 || path.indexOf(bkUps) > -1) {
        return "disabled";
    }

    return "enabled";
}

function ToBindFileInGrid(file) {

    if (file.IsDeleted())
        return false;

    if (file.FilePath().indexOf(wsLog) > -1)
        return false;

    return true;

}

//Closure_RollFwdNBkUpMsg

function ClsrChkTitle(path) {
    //, attr: { title: #=ClsrChkTitle(FilePath)# }

    if (path.indexOf(rollFwd) > -1 || path.indexOf(wsLog) > -1 || path.indexOf(bkUps) > -1) {
        return AmMsg.Closure_RollFwdNBkUpMsg;
    }

    return "";
}

var daysToOffSet = 3;

//Msg
var ActivityStatusMsg = {
    RequestReceived: "The request has been received and is awaiting processing. Notification will be sent via e-mail to the Requestor and the Partner and Manager indicated in the request form when the request is completed and requested file is available.",
    InProgress: "The request is currently being processed. Notification will be sent via e-mail to the Requestor and the Partner and Manager indicated in the request form when the request is completed and requested file is available.",
    Completed: "The roll forward (RF) file has been created and uploaded to the workspace; no further action is required unless the RF file requires re-processing. Refer to the notification e-mail the Requestor and the Partner and Manager indicated in the request form  received for instructions on how to request reprocessing of the roll forward file.",
    Completed_S2: "The Year End ENG file created on Server 2 and uploaded to the workspace; no further action is required unless the ENG file requires removal.",
    PendingApproval: "The retention file has been created, uploaded to the workspace, and is awaiting review. Refer to the notification e-mail sent to the Requestor and the Partner and Manager indicated in the request form for instructions on how to acknowledge the receipt of the retention file.",
    ReprocessRequested: "The requested file did not meet the receipt criteria, and has been submitted for reprocessing by",
    Acknowledged: "The retention file was reviewed and acknowledged by",
    Removed: "The requested file did not meet the receipt criteria or was not needed, and its removal was requested by",
    retEngRemovalInProgress: "The file is currently being removed by the system; refresh your page in a few minutes, and the file will no longer be listed in the workspace once its removal is completed successfully.",
}

var delMsg = "The file you are trying to remove ";
var DeleteMsg = {
    IsUnderPreservationMsg: delMsg + "cannot be removed because it is stored in a workspace under preservation.  No files can be removed from a workspace currently under preservation.",
    IsDeletedMsg: delMsg + "has already been removed.",
    IsRecordMsg: delMsg + "is a record and cannot be removed.",
    IsCheckedOutMsg: delMsg + "is checked out to another user.",
    IsLockedMsg: delMsg + "is locked and cannot be removed.  Please contact NSC for guidance on how to proceed.",
    VersionCountMsg: delMsg + "has multiple versions and cannot be removed; please contact NSC for guidance on how to proceed.",
};

var AmMsg = {

    WhyNotRET: "RETs for interim review periods are created by the team. If this request is for an RET associated with an interim review period, please create the RET file yourself, and upload it to the Interim Reviews - RET Files folder of the workspace.",

    NoFile: "No files in the folder.",

    NoWsFldrSelected: "Please navigate to a Workspace folder.",

    CantTakeAction_UsrMgmt_Msg: "You cannot {0} - you must be a member of the Admin Group for this workspace in order to {0} . </br></br> The following workspace members have rights to perform this action: </br> {1}",
    CantTakeAction_File_Msg: "You cannot {0} this file - you must be a member of the Admin Group for this workspace in order to {0} this file. </br></br> The following workspace members have rights to perform this action: </br> {1}",
    CantTakeAction_Ws_Msg: "You cannot {0} this Workspace - you must be a member of the Read/Write Group for this workspace in order to {0} this Workspace. </br></br> The following workspace members have rights to perform this action: </br> {1}",

    CantTakeAction_Ws_Upload_Msg: "You cannot {0} a file to this Workspace - you must be a member of the Read/Write Group for this workspace in order to {0} the file. </br></br> The following workspace members have rights to perform this action: </br> {1}",

    //Closure_NoRETMsg: "You are attempting to close out a DRMS workspace without at least one .RET file in the workspace. If your engagement was completed using eAudIT, and your eAdudIT MAF is on Server 2, initiate closeout to ensure the .RET is moved into this workspace before initiating closure of your DRMS workspace. </br></br> If you have already initiated closure of the .ENG on Server 2, you may check the status of your .ENG and .RET files by viewing the RET and RF Status tab. </br></br> Do you wish to continue with the workspace closure?",

    Closure_NoRETMsg: "You are attempting to close out a DRMS workspace without at least one RET file in the workspace. " +
            "</br><b>If your engagement MAF is on Server 2</b>, wait until the interim and/or year-end RET file(s) have been generated,  " +
            "posted to the workspace, and acknowledged before proceeding with closure. " +
            "</br><b>If your engagement MAF is NOT on Server 2</b>, before proceeding with workspace closure, confirm you have: " +
                "</br>(1)	uploaded the appropriate files which may include the interim review RET or year-end Phase 1 Complete ENG and  " +
                "</br>(2)	Requested generation of the RET file from within the My Engagements tab.   " +

    "</br></br>Do you wish to continue with the workspace closure?",

    //Closure_ConfirmMsg: '<div id="divSubmitConfirm"> ' +
    //            '<p style="width: 600px\;"> ' +
    //            'By clicking the \'yes\' button below, you are confirming that the files in all folders of this workspace which have been associated with this workspace closure have been sufficiently reviewed; determined to be complete and final\; and are ready to be appropriately marked as Records of the firm. If you believe that any of the files included in the folders listed below should not be marked as firm Records, select the \'cancel\' button, perform the appropriate actions necessary (i.e. perform sufficient review of files, delete files from workspace, etc.) and initiate closure of the workspace when these actions have been completed. ' +

    //                'If any of the files are subject to a Preservation Notice, please follow the guidance issued by the ' +
    //                '<a target="_blank" href="http://usportal.us.kworld.kpmg.com/us/ProfessionalismAndIntegrity/OGC/Pages/PreservationGuidelines.aspx?usersegment=9C066CEE8647BEF3F702B13BD8E7AE47"> ' +
    //                    'Office of General Counsel (OGC). ' +
    //                '</a> ' +
    //                    'Consult with an OGC attorney if you have any question as to whether the files are subject to preservation or how they should be preserved. ' +
    //                '<br /><br />Do you want to continue? ' +
    //            '</p> ' +
    //        '</div> ',

    Closure_ConfirmMsg: 'By clicking the \'yes\' button below, you are confirming that the files in all folders of this workspace which have been associated with this workspace closure have been sufficiently reviewed; determined to be complete and final\; and are ready to be appropriately marked as Records of the firm. If you believe that any of the files included in the folders listed below should not be marked as firm Records, select the \'cancel\' button, perform the appropriate actions necessary (i.e. perform sufficient review of files, delete files from workspace, etc.) and initiate closure of the workspace when these actions have been completed. ' +

                    'If any of the files are subject to a Preservation Notice, please follow the guidance issued by the ' +
                    '<a target="_blank" href="http://usportal.us.kworld.kpmg.com/us/ProfessionalismAndIntegrity/OGC/Pages/PreservationGuidelines.aspx?usersegment=9C066CEE8647BEF3F702B13BD8E7AE47"> ' +
                        'Office of General Counsel (OGC). ' +
                    '</a> ' +
                        'Consult with an OGC attorney if you have any question as to whether the files are subject to preservation or how they should be preserved. ' +
                    '<br /><br />Do you want to continue? ',


    Closure_PendingAcknowledgeMsg: "You are attempting to associate with this workspace closure an RET file that has not been acknowledged. Either acknowledge the RET file or remove its association with this workspace closure.",
    Closure_EventTrgrDateMsg: "To proceed with the Initiate Closure, please enter the event trigger date.",
    Closure_NoFileSelectedMsg: "Please select a file to continue.",
    Activity_CommentEmptyMsg: "Comment is required.",

    Closure_RollFwdNBkUpMsg: 'Files in the back-ups and rollforward folders are not to be marked as records and associated with workspace closures. If you believe this file should be marked as a record and associated with the workspace closure, please download the file and upload it to a different folder within the workspace.',

    Reprocess_S2_InRev_RETMsg: "You cannot initiate reprocessing of an Interim Review RET from Audit Manager, please contact the National Support Center at 1-800-KPMG-HELP.",

    Reprocess_S2_NoEng_4_RETMsg: "No ENG file found for the RET.",

    WS_Dont_Exists_4_WSNum: "No Workspace found/Invalid Workspace number.",

    isInEdit_Msg: "You have to either save or cancel the workspace profile data.",

    createWs_NoAccessMsg: "Workspace created successfully.<br /><br />You currently do not have access to this workspace.<br />Contact one of the following workspace administrators to request access:<br />{0}",

};

var AmConfirmMsg = {
    ToAddRetentionFile: "Do you want to add file/s from retention server with this closure.",

    PeriodEndAuditRet: "<p><input type='radio' name='radPeriodEndAuditRet' value='Audit' /> This workbook closure is associated with an audit engagement</p>" +
                            "<p><input type='radio' name='radPeriodEndAuditRet' value='NonAudit' /> This workbook closure is associated with a non-audit engagement</p>",

    s2NonClosedWbGrid_Html_1: "<p>The following workbooks hosted on Server 2 have not been closed out and transferred to the workspace. " +
    "<b></br>If your closure is associated with an interim review</b> and you have acknowledged the RET(s) associated with your interim review, choose to proceed with the close out. " +
    "<b></br>If your closure is associated with a year-end audit</b> contained in the workbook(s) listed below, " +
    "consider whether additional workbook closeouts need to occur before proceeding with closure of this workspace.</p>",

    s2NonClosedWbGrid_Html: "<p>The following workbooks hosted on Server 2 have not been closed out and transferred to the workspace. " +
    "<b></br>If your closure is associated with an interim review</b> and you have acknowledged the RET(s) associated with your interim review, choose to proceed with the close out. " +
    "<b></br>If your closure is associated with a year-end audit</b> contained in the workbook(s) listed below, " +
    "consider whether additional workbook closeouts need to occur before proceeding with closure of this workspace.</p>" +
                                        '<div id=\'kGrid_S2GuidMap_NonClosedWorkbook\' class=\'kGrid\'></div>' +
                                        '<p>Do you wish to continue with the workspace closure at this time?</p>',

    s2GuidDelinkMsg: 'Do you want to delete the association?',

    request_RET_Ques: "Is this RET request for an interim review?",

    AnotherRETCheck_Ques: "Please ensure that each workbook in your file have been closed out through Phase 1.  RET files cannot be generated for workbooks that have not been closed out through Phase 1.  If you request a RET file for a workbook that has not been closed out through Phase 1, your request will be returned and receipt of the RET file will be delayed. <br/><br/> Do you wish to continue?",

};

var AmMsgType = {
    Warning: ["warning", "Warning!"],
    Error: ["error", "Error!"],
    Success: ["success", "Success!"],
    Info: ["info", "<i class='glyphicon glyphicon-info-sign'></i>"],
    Query: ["info", "<i class='glyphicon glyphicon-question-sign'></i>"],
};

var AmUsrType = {
    NONE: "NONE",
    ADMIN: "ADMIN",
    MEMBERS: "MEMBERS",
    READ: "READ"
};

var WhoCanAct = {
    ONLY_ADMIN: ["ADMIN"],
    ADMIN_N_MEMBERS: ["ADMIN", "MEMBERS"]
}

function ShowMsg(amMsgType, amMsg) {
    return BuildMsg(amMsgType[0], amMsgType[1], amMsg, false);
}

//M-585
//var msgBuilder = "<div class='alert alert-font no-margin alert-{0}'><p style=\'width: 600px\;'><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>{1}</strong> {2}</p></div>";
//var msgBuilderNoClose = "<div class='alert alert-font no-margin alert-{0}'><p style=\'width: 600px\;'><strong>{1}</strong> {2}</p></div>";

var msgBuilder = "<div class='alert alert-font no-margin alert-{0}'><p><a href='#' class='close' data-dismiss='alert'>&times;</a><strong>{1}</strong> {2}</p></div>";
var msgBuilderNoClose = "<div class='alert alert-font no-margin alert-{0}'><p class='text-justify'><strong>{1}</strong> {2}</p></div>";

function BuildMsg(msgType, msgPreFix, msg, hasClose) {
    //return '<div>' + msg + '</div>';
    var msgArr = [msgType, msgPreFix, msg];
    if (hasClose)
        return msgBuilder.format(msgArr);
    else
        return msgBuilderNoClose.format(msgArr);
}

function AmAlert_WithCallBack(amMsg, callBack) {
    //M-585
    GetDialog(dialogType.DIALOG, dialogSize.AUTO_HEIGHT_SMALL_WIDTH, dialogClass.ALERT)
    //$("#dmAlert")
        .html(BuildMsg(AmMsgType.Info[0], AmMsgType.Info[1], amMsg, true))
        .data('closeAction', callBack)
        .dialog('open');
    //M-585
}

function AmAlert(amMsg) {
    //M-585
    GetDialog(dialogType.DIALOG, dialogSize.AUTO_HEIGHT_SMALL_WIDTH, dialogClass.ALERT)
    //$("#dmAlert")
        .html(BuildMsg(AmMsgType.Info[0], AmMsgType.Info[1], amMsg, true)).dialog('open');
    //M-585
}

function AmAlert_2(amMsg) {
    //M-585
    GetDialog(dialogType.DIALOG, dialogSize.AUTO_HEIGHT_SMALL_WIDTH, dialogClass.ALERT)
    //$("#dmAlert")
        .html(amMsg).dialog('open');
    //M-585
}

function AmConfirm(amMsg, yesAction, noAction, yesParam, noParam) {
    //M-585
    GetDialog(dialogType.CONFIRM, dialogSize.AUTO_HEIGHT_SMALL_WIDTH, dialogClass.CONFIRM)
    //$("#dmConfirm")
        .html(BuildMsg(AmMsgType.Query[0], AmMsgType.Query[1], amMsg, false))
        .data('yesAction', yesAction)
        .data('yesParam', yesParam)
        .data('noAction', noAction)
        .data('noParam', noParam)
        .dialog('open');
}

function AmInteraction(amMsg, doneAction) {
    //M-585
    GetDialog(dialogType.INTERACTION, dialogSize.AUTO_HEIGHT_SMALL_WIDTH, dialogClass.INTERACTION)
    //$("#dmInteraction")
        .html(BuildMsg(AmMsgType.Query[0], AmMsgType.Query[1], amMsg, false)).data('doneAction', doneAction).dialog('open');
}

function CanTakeAction(action, whoCanAct, cantTakeActionMsg) {

    if (IsSupperUser())
        return true;

    var usrS = [];

    $.each(whoCanAct, function (idx, item) {
        GetUsers(item, usrS);
    });

    var match = ko.utils.arrayFirst(usrS, function (item) {
        return item.Name.IsMatch(usrId);
    });

    if (match)
        return true;
    else {
        if (!IsNothing(cantTakeActionMsg)) {
            var usrWhoCanAct = "";

            $.each(usrS, function (idx, item) {
                usrWhoCanAct = usrWhoCanAct + " </br>" + item.FullName;
            });

            AmAlert(cantTakeActionMsg.format([action, usrWhoCanAct]));
            return false;
        }
    }
}

function GetUsers(amUsrType, usrS) {

    var match = ko.utils.arrayFirst(viewModel.selectedWs().WsGroups()(), function (item) {
        return item.WsUserType().IsMatch(amUsrType);
    });

    if (match) {
        $.each(match.GrpUsers(), function (idx, item) {
            if (usrS.map(function (e) { return e.Name; }).indexOf(item.Name()) == -1) {
                usrS.push({ Name: item.Name(), FullName: item.FullName() });
            }
        });
    }

    return usrS;
}

$(function () {
    $("body").on("click", ".close", function (e) {
        //M-585
        //$("#dmAlert").dialog('close');
        //$(this).closest(".ui-dialog-content").dialog('close');
        CloseMyDialog(this);
    });
});

function UriComponentEncode(item) {
    return encodeURIComponent(item);
}

function UriComponentDecode(item) {
    return decodeURIComponent(item);
}

String.prototype.format = function (obj) {
    return this.replace(/\{\s*([^}\s]+)\s*\}/g, function (m, p1, offset, string) {
        return obj[p1]
    })
}

String.prototype.SplitNGet = function (seperator, idx) {
    return this.split(seperator)[idx];
}



function ReloadWs(from) {

    if (remote) {
        //alert(viewModel.selectedWs().ObjectID());
        GetEngByWsId(viewModel.selectedWs().ObjectID(), "ALL", from);
    }
    else {
        viewModel.selectedWs().IsLoaded(false);
        var chk = $($("ul.ulEngFarm > li > .chkEngFldr:checked")[0]);
        chk.prop("checked", false);
        chk.trigger("click");
    }

}

//$(window).scrollTop

var uploadInProgress = false;
var selectedFolderId_CheckBoxId = "";

function getParameterByName(url, name) {

    var s_Url = url.SplitNGet('?', 1);

    if (IsNothing(s_Url))
        return null;

    var s_S_Url = s_Url.split('&amp;');

    var ret_V = null;

    $.each(s_S_Url, function (idx, item) {
        if (item.SplitNGet('=', 0).IsMatch(name)) {
            ret_V = item.SplitNGet('=', 1);
            return false;
        }
    })

    return ret_V;
}



$(function () {

    $('body').on({
        mouseenter: function () {
            $(this).addClass('ui-state-hover');
        },
        mouseleave: function () {
            $(this).removeClass('ui-state-hover');
        }
    }, '.minMaxbutton');

    $("body").on("click", ".lblEngFldrName-After", function () {
        newSession = true;
        ReloadWs();
    });

    $("body").on("click", ".minMaxbutton", function (e) {

        e.preventDefault();

        var me = $(this);
        var div_id = (me.attr("id")).split("-")[0];

        if (me.children('span').hasClass("ui-icon-minusthick")) {

            me.empty();
            me.append('<span class="ui-icon ui-icon-newwin"></span>');

            $("#" + div_id).parents('.ui-dialog').animate({
                height: '40px',
                width: '140px',
                top: 50,
                left: $(window).width() - 140
            }, 200);

        }
        else if (me.children('span').hasClass("ui-icon-newwin")) {

            me.empty();
            me.append('<span class="ui-icon ui-icon-minusthick"></span>');

            $("#" + div_id).parents('.ui-dialog').animate({
                height: 280,
                width: 600,
                top: ($(window).height() - 280) / 2,
                left: ($(window).width() - 600) / 2
            }, 200);
        }
    });
});
//Msg

var updateObj_Reprocess = "";

function DoLoadRETnRF(rT) {
    myApp.showPleaseWait("Loading " + rT);

    GetDialog(dialogType.DIALOG, dialogSize.XLARGE, "", 'Request ' + rT)
                .load('/Workspace/RETnRF/?surveyRequestType=' + rT)
                .dialog("open");

    //$('#dialog_XL')
    //    .load('/Workspace/RETnRF/?surveyRequestType=' + rT, function () {
    //    $(this).dialog('option', 'title', 'Request ' + rT);
    //    $(this).dialog('open');
    //});
}

function ShowWhyNotRET() {
    AmAlert(AmMsg.WhyNotRET);
}

function AnotherRETCheck(rT) {
    AmConfirm(AmConfirmMsg.AnotherRETCheck_Ques, DoLoadRETnRF, null, rT, null);
}

function GetRequestType(requestType) {
    var rT = requestType.IsMatch("Request RF") ? "RF" : (requestType.IsMatch("Request RET") ? "RET" : "");
    return rT;
}
function LoadRETnRF2(p) {
    //alert(updateObj);
    var requestType = p[0];
    var updateObj = p[1];

    updateObj_Reprocess = updateObj;

    var rT = GetRequestType(requestType);


    if (!IsNothing(rT)) {

        if (rT.IsMatch("RET")) {
            AmConfirm(AmConfirmMsg.request_RET_Ques, ShowWhyNotRET, AnotherRETCheck, null, rT);
        }
        else {
            DoLoadRETnRF(rT);
        }
    }
    else {
        AmAlert(requestType + " - Not a match.");
    }
}
function ToDocumentStatus(ActivityType, ActivityName, ActivityDescription, ActivityCategory, FileActivityStatus) {

    var status =["In-Progress", "ENG - Removal InProgress", "RET - Removal InProgress", "Request Received", "Complete", "Pending Approval", "Reprocess Requested", "Acknowledged", "Removed"];
    //var activity_Type = ["CLIENTPROCESS", "DRMSAPPROVED", "DRMSDELETE", "DRMSDELETERF", "PDFREVIEW", "RFREVIEW", "SURVEYIMPORT"];
    var activity_Type =["DRMSAPPROVED", "DRMSDELETE", "DRMSDELETERF", "PDFREVIEW", "RFREVIEW", "SURVEYIMPORT"];
    var activity_Type_Request_Received =["KDRIVEMOVE", "SURVEYIMPORT", "SURVEYPDFEMAIL", "SURVEYRFEMAIL"];

    if (activity_Type.indexOf(ActivityType) == -1) {
        return "In-Progress";
}
else {
    switch (FileActivityStatus) {
        case "Approved":
            return "Acknowledged";
            break;
        case "Success":
            return "Pending Approval";
            break;
        default:
            return "In-Progress";
            }
    }
    
}
function LoadRETnRF(requestType, updateObj) {
    //alert(updateObj);
    //alert(viewModel.selectedWs().WsProfile().EngNum());
    //alert(viewModel.selectedFile().Number());
    //alert(viewModel.selectedFile().DocumentStatus().FileIn());
    if (viewModel.selectedFile().Size() > 1) {
        var p = [requestType, updateObj];
        $.ajax(
        {
            type: 'GET',
            async: false,
            url: '/api/Workspace/GetSurveyInfo?engagementNumber=' +
                viewModel.selectedWs().WsProfile().EngNum()
                //6666667970
                + '&fileNumber=' +
                viewModel.selectedFile().Number()
                //888091
            ,
            headers: {
                'RequestVerificationToken': GetTokenHeaderValue()
            },
            success: function (data, textStatus, xhr) {
                if (!IsNothing(data)) {
                    var rT = GetRequestType(requestType);

                    var rTItem = [];

                    $.each(data, function (idx, item) {
                        if (item.RequestType == rT) {
                            rTItem.push(item);
                        }
                    });

                    if (rTItem.length > 0) {

                        //AmAlert(JSON.stringify(rTItem));

                        //var tab = "<br /><br /><table class='table table-striped'><tr><th>Request</th><th>Status</th><th>Submitted by</th><th>Request Date</th></tr>"

                        var tab = "<br /><br /><table class='table table-striped'><tr><th>Request</th><th>Submitted by</th><th>Request Date</th></tr>"

                        $.each(rTItem, function (idx, item) {
                            var DocumentStatus = {
                                Status: ToDocumentStatus(item.ActivityType, item.ActivityName, item.ActivityDescription, item.ActivityCategory, item.FileActivityStatus),
                                FileIn: "",//viewModel.selectedFile().DocumentStatus().FileIn(),
                                ActionBy: item.ActivityUser,
                                ActionDate: item.ActivityUpdateDate,
                            };
                            
                            //tab = tab + "<tr><td>" + item.SurveyRowID + "</td><td>" + GetDocumentStatus(DocumentStatus)  + "</td><td>" + item.SurveyRequestor + "</td><td>" + item.SurveyInsertDate.ToDate() + "</td></tr>"
                            tab = tab + "<tr><td>" +item.SurveyRowID + "</td><td>" +item.SurveyRequestor + "</td><td>" +item.SurveyInsertDate.ToDate() + "</td></tr>"
                        });

                        tab = tab + "</table>";

                        AmConfirm("The following requests already exist for the selected ENG file. You can view all requests on the Workspace Activity tab. Are you sure you want to submit a new request? " + tab,
                            LoadRETnRF2, null, p, null);
                    }
                    else {
                        LoadRETnRF2(p);
                    }
                }
                else {
                    LoadRETnRF2(p);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                HandleError(jqXHR, textStatus, errorThrown);
            },
            beforeSend: function () {
                myApp.showPleaseWait("Wait.........");
            },
            complete: function () {
                myApp.hidePleaseWait();
            }
        });
    }
    else {
        AmAlert("The size of ENG file you selected = 0K. You cannot submit requests for ENG of that size because it is incomplete. Please upload a new ENG file and try again.");
    }
};

String.prototype.IsMatch = function (itemToMatchWith) {

    //alert(this);

    if (!IsNothing(this) && !IsNothing(itemToMatchWith)) {
        if (typeof itemToMatchWith == "string") {
            return (this.trim().toUpperCase() == itemToMatchWith.trim().toUpperCase());
        }
    }

    return false;
}

String.prototype.FileType = function () {

    if (this.IsMatch("RET") || this.IsMatch("PDF") || this.IsMatch("Server 2 RET"))
        return "RET";
    else if (this.IsMatch("ENG") || this.IsMatch("RF"))
        return "ENG";
    else
        return "Other";

}

String.prototype.FileType2 = function () {

    if (this.IsMatch("RET") || this.IsMatch("PDF"))
        return "RET";
    else if (this.IsMatch("Server 2 RET"))
        return "Server 2 RET";
    else if (this.IsMatch("ENG") || this.IsMatch("RF"))
        return "RF";
    else
        return "UnKnown";

}

//function IsMatch(item, itemToMatchWith) {
//    if (!IsNothing(item) || !IsNothing(itemToMatchWith)) {
//        if (typeof item == "string" && typeof itemToMatchWith == "string") {
//            return (item.toUpperCase() == itemToMatchWith.toUpperCase());
//        }
//    }

//    return false;
//}

function IsNothing(obj) {

    //alert(obj);
    //if (typeof obj === 'undefined')
    //    return true;

    //will evaluate to true if value is not:

    //null
    //undefined
    //NaN --- Not good
    //empty string ("")
    //0
    //false

    if (obj)
        return false;

    return true;
};

function FormatDate(d) {

    if (IsNothing(d))
        return "";

    //var date = new Date(Date.parse(d));

    return d;
    var date = new Date(d);
    return date.Format();

    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
}

function FormatDateNTime(d) {

    if (IsNothing(d))
        return "";

    return d;
    //var date = new Date(Date.parse(d));
    var date = new Date(d);

    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + ", " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds() +
    ":" + date.getMilliseconds();
}

Date.prototype.Format = function () {
    return (this.getMonth() + 1) + '/' + this.getDate() + '/' + this.getFullYear();
}

String.prototype.ToDate = function () {

    if (!IsNothing(this)) {
        var that = this.replace(/\-/ig, '/').replace(/T/, ' ').split('.')[0];
    }


    if (!IsNothing(that) && !isNaN(Date.parse(that))) {
        var date = new Date(that);
        //var returnDate = date.toLocaleDateString("en-US");
        return date.Format();
    }

    return "n/a";
}

Date.prototype.ToDate = function () {

    if (!IsNothing(this) && !isNaN(Date.parse(this))) {
        var date = new Date(this);
        //var returnDate = date.toLocaleDateString("en-US");
        return date.Format();
    }

    //return ((new Date()).toLocaleDateString("en-US")).Format();
    return (new Date()).Format();
}

String.prototype.FileWithExtn = function (extn) {

    if (!IsNothing(this)) {
        if (!IsNothing(extn)) {
            var lIdx = this.lastIndexOf(".");
            if (lIdx == -1)
                return this + "." + extn;
            else {
                if ((this.substr(lIdx + 1, this.length)).toUpperCase() == extn.toUpperCase())
                    return this;
                else
                    return this + "." + extn;
            }
        }
        return this;
    }

    return "n/a";
}

Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};

Date.prototype.addBusinessDays = function (days) {

    var day = this.getDay();
    var newDays = 0;

    if (day == 0 || day == 1)
        newDays = days;
    else if (day == 6)
        newDays = days + 1;
    else if (day > 1)
        newDays = days + 2;

    this.setDate(this.getDate() + parseInt(newDays));
    return this;
};

Math.ToKb = function (byte) {

    if (byte < 0) {
        byte = 2147483647 - (byte);
    }


    if (!IsNothing(byte) && !isNaN(byte)) {
        return Math.round(byte/1024);
    }

    return 0;
}

Boolean.prototype.ToChar = function() {
    if (this.valueOf() == true) {
        return "Yes";
    } else {
        return "No";
    }
}

$(function () {

    $(".inOnlyNum").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $.widget("ui.dtpWithBS", {
        _init: function () {
            var $el = this.element;
            $el.datepicker(this.options);

            if (this.options && this.options.trigger) {
                $(this.options.trigger).bind("click", function () {
                    $el.datepicker("show");
                });
            }
        }
    });

    $('.dtpCustom').dtpWithBS({
        format: 'mm/dd/yyyy',
        trigger: ".dtpCustomBtn"
    });

    $("body").on("click", ".dtp-Btn-Evnt", function () {
        var dtp = $(this).prevAll("input");
        dtp.focus();
    });

    $("body").on("focus", ".dtp-Txt-Evnt", function () {
        $(this).datepicker({
            format: 'mm/dd/yyyy',
            constrainInput: true,
            //onSelect: function () {
            //    $(this).nextAll(".dtp-Btn").focus();
            //}
        });
    });

    $("body").on("click", ".dtp-Btn", function () {
        var dtp = $(this).prevAll("input");
        dtp.focus();
    });

    $("body").on("focus", ".dtp-Txt", function () {
        $(this).datepicker({
            format: 'mm/dd/yyyy',
            constrainInput: true,
            onSelect: function () {
                $(this).nextAll(".dtp-Btn").focus();
            }
        });
    });

    $("body").on("click", ".dtp-Btn-NoFuture", function () {
        var dtp = $(this).prevAll("input");
        dtp.focus();
    });

    $("body").on("focus", ".dtp-Txt-NoFuture", function () {

        $(this).datepicker({
            format: 'mm/dd/yyyy',
            constrainInput: true,
            beforeShowDay: function (date) {
                var day = date.getDay();

                if (date > new Date())
                    return [0, 'dtpUnselectable'];

                else
                    return [1, ''];
            }
        });
    });

    $("body").on("click", ".dtp-NoWeakEndPastDateNDateOffSet-Btn", function () {

        var dtp = $(this).prevAll("input");
        dtp.focus();

    });

    $("body").on("focus", ".dtp-NoWeakEndPastDateNDateOffSet-Txt", function () {

        $(this).datepicker({
            minDate: -0,

            format: 'mm/dd/yyyy',
            constrainInput: true,
            beforeShowDay: function (date) {
                var day = date.getDay();

                if (date < (new Date()).addBusinessDays(daysToOffSet))
                    return [0, 'dtpUnselectable'];

                if (day == 6)
                    return [0, 'dtpUnselectable'];
                else if (day == 0)
                    return [0, 'dtpUnselectable'];
                else
                    return [1, ''];
            },
            onSelect: function () {
                $(this).nextAll(".dtp-NoWeakEndPastDateNDateOffSet-Btn").focus();
            }
        });
    });

    $('.dtpNoBtn').datepicker({
        format: 'mm/dd/yyyy',
        constrainInput: true
    });

    $('.dtp').datepicker({

        //showOn: "button",
        //buttonImage: "Content/images/calendar.gif",
        //buttonImageOnly: true,

        format: 'mm/dd/yyyy',
        constrainInput: true
    });

    $('.dtpWeakEndNPastDayUnselectable').datepicker({

        //showOn: "button",
        //buttonImage: "Content/images/calendar.gif",
        //buttonImageOnly: true,

        format: 'mm/dd/yyyy',
        beforeShowDay: function (date) {
            var day = date.getDay();

            if (date < new Date())
                return [0, 'dtpUnselectable'];

            if (day == 6)
                return [0, 'dtpUnselectable'];
            else if (day == 0)
                return [0, 'dtpUnselectable'];
            else
                return [1, ''];
        }
    });

    $('.dtpWeakEndNPastDayNDateOffSetUnselectable').datepicker({

        //showOn: "button",
        //buttonImage: "Content/images/calendar.gif",
        //buttonImageOnly: true,

        minDate: -0,

        format: 'mm/dd/yyyy',
        beforeShowDay: function (date) {
            var day = date.getDay();

            if (date < (new Date()).addBusinessDays(daysToOffSet))
                return [0, 'dtpUnselectable'];

            if (day == 6)
                return [0, 'dtpUnselectable'];
            else if (day == 0)
                return [0, 'dtpUnselectable'];
            else
                return [1, ''];
        }
    });

    $('.dtpWeakEndUnselectable').datepicker({

        //showOn: "button",
        //buttonImage: "Content/images/calendar.gif",
        //buttonImageOnly: true,

        format: 'mm/dd/yyyy',
        beforeShowDay: function (date) {
            var day = date.getDay();
            if (day == 6)
                return [0, 'dtpUnselectable'];
            else if (day == 0)
                return [0, 'dtpUnselectable'];
            else
                return [1, ''];
        }
    });
});

function IsSupperUser() {

    var retResult = false;
    $.each(superUser.split(","), function (idx, item) {
        //alert(item);
        if (usrId.IsMatch(item)) {
            retResult = true;
            return false;
        }
    });
    return retResult;
}

function GetDefaultDate() {
    var d = new Date();

    var dPostFix = wsDefaultDate.slice(-1);

    if (dPostFix.IsMatch("Y")) {
        d.setFullYear(d.getFullYear() - wsDefaultDate.slice(0, -1));
    }
    else if (dPostFix.IsMatch("M")) {
        d.setMonth(d.getMonth() - wsDefaultDate.slice(0, -1));
    }

    return d.ToDate();
}

function HandleError(jqXHR, textStatus, errorThrown) {

    //alert("jqXHR - " + jqXHR);
    //alert("jqXHR - " + jqXHR.responseText);
    //alert("textStatus - " + textStatus);
    //alert("errorThrown - " + errorThrown);

    //alert("HandleError");
    //var err = textStatus + ", " + errorThrown;
    //console.log("Request Failed: " + err);
    var err = jqXHR.responseText.replace(/"/g, "");

    myApp.hidePleaseWait();
    AmAlert(err);
}

function HandleSuccess_WithCallBack(action, msg, callBack) {
    myApp.hidePleaseWait();

    var alertMsg = ''

    if (action.IsMatch("CreateWs")) {
        if (IsNothing(msg))
            alertMsg = "Workspace created successfully";
        else
            alertMsg = msg;
    }

    AmAlert_WithCallBack(alertMsg, callBack);
}

function HandleSuccess(action, msg) {

    myApp.hidePleaseWait();

    //AmAlert(IsNothing(msg) ? "Success" : msg);

    var alertMsg = '';

    if (action.indexOf("PostUpdateFileActivity") > -1) {
        var activity = (action.SplitNGet("-", 1)).SplitNGet("_", 1);

        if (activity.IsMatch("Reprocess"))
            alertMsg = "Reprocess request submitted successfully";
        else if (activity.IsMatch("Acknowledge"))
            alertMsg = "File acknowledged successfully";
        else if (activity.IsMatch("Remove"))
            alertMsg = "File removed successfully";

    }
    else if (action.IsMatch("DeleteDoc")) {
        alertMsg = "File removed successfully";
    }
    else if (action.IsMatch("PostSurveyRequest-RET")) {
        alertMsg = "Request for RET file submitted successfully";
    }
    else if (action.IsMatch("PostSurveyRequest-RF")) {
        alertMsg = "Request for RF file submitted successfully";
    }
    else if (action.IsMatch("PostSurveyRequest-Reprocess")) {
        alertMsg = "Request to reprocess file submitted successfully";
    }
    else if (action.IsMatch("PostInitiateClosure")) {
        alertMsg = "Workspace closure submitted successfully";
    }
    else if (action.IsMatch("PostUpdateWs")) {
        alertMsg = "Workspace profile updated successfully";
    }
    else if (action.IsMatch("DeLinkMAF")) {
        alertMsg = "Unlinking request submitted successfully";
    }
    else if (action.IsMatch("LinkMAF")) {
        alertMsg = "Linking completed successfully";
    }
    else if (action.indexOf("RemoveUser") > -1) {

        var usrActivity = action.SplitNGet("-", 1);
        alertMsg = "User " + usrActivity.SplitNGet("_", 1) + " successfully removed from the " + usrActivity.SplitNGet("_", 0) + ".";

        //alert(usrActivity.SplitNGet("_", 2));

        if (usrActivity.SplitNGet("_", 2) == "true") {
            alertMsg = alertMsg + "<br>";
            alertMsg = alertMsg + "If this user is in additional security groups and you want to completely revoke his/her access to the entire workspace, you need to remove the user from all other workspace groups.";
        }
        else {

        }

    }
    else if (action.indexOf("AddUser") > -1) {

        var usrActivity = action.SplitNGet("-", 1);

        alertMsg = "User " + usrActivity.SplitNGet("_", 1) + " successfully added to the " + usrActivity.SplitNGet("_", 0) + "";
    }
    else if (action.IsMatch("CreateWs")) {
        if (IsNothing(msg))
            alertMsg = "Workspace created successfully";
        else
            alertMsg = msg;
    }
    else if (action.IsMatch("RequestAccess")) {
        if (IsNothing(msg))
            //alertMsg = "Your request has been submitted to the members of the workspace Administrators group.";
            alertMsg = "Your request has been sent to the selected  members of the workspace Administrators group.";
        else
            alertMsg = msg;
    }

    AmAlert(alertMsg);

    if (action.indexOf("PostSurveyRequest") > -1) {
        //if (action.IsMatch("PostSurveyRequest")) {
        //$('#dialog_XL').dialog('close');
        CloseMyDialog();

        AmAlert(alertMsg);
    }
}

function IsFunction(possibleFunction) {
    return (typeof (possibleFunction) == typeof (Function));
}

var UsrNameType = {
    "Last": "Last",
    "First": "First"
}

function GetUsrName(fullName, usrNameType) {

    return usrNameType == UsrNameType.Last ? fullName.SplitNGet(",", 0) : fullName.SplitNGet(",", 1);

}
