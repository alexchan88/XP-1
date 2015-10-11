//IsNothing(viewModel.selectedFile().DocumentStatus.FileIn)
//viewModel.currentActivity().DocumentStatus

var fileMenu_Req_RF = "<i class='glyphicon glyphicon-pencil'></i> Request RF";
var fileMenu_Req_RET = "<i class='glyphicon glyphicon-pencil'></i> Request RET";
var fileMenu_Remove = "<i class='glyphicon glyphicon-remove'></i> Remove";
var fileMenu_Download = "<i class='glyphicon glyphicon-download'></i> Download";
var fileMenu_Reprocess = "<i class='glyphicon glyphicon-pencil'></i> Reprocess";
var fileMenu_Acknowledge = "<i class='glyphicon glyphicon-ok'></i> Acknowledge";

var fileMenuArrayRF = [["Action"], [fileMenu_Req_RF, fileMenu_Req_RET, fileMenu_Remove, fileMenu_Download]];
var fileMenuArrayNonRF = [["Action"], [fileMenu_Remove, fileMenu_Download]];

function GetDocumentStatus(DocumentStatus) {
    if (IsNothing(DocumentStatus))
        return "";
    else if (DocumentStatus.Status.IsMatch("Danger")) {
        return "Activity-In-Progress";
    }
    else {
        var ret = "<img class='toolTipImg' src='/Content/images/" + P_GetStatusImg(DocumentStatus.Status) + ".png' title='" + P_GetStatusImgTitle(DocumentStatus) + "' />";
        return ret + DocumentStatus.Status;
    }
}

function P_GetStatusImgTitle(DocumentStatus) {
    var CompletedMsg = ActivityStatusMsg.Completed;
    if (DocumentStatus.FileIn.IsMatch("S2")) {
        CompletedMsg = ActivityStatusMsg.Completed_S2;
    }
    //var action = DocumentStatus.ActionBy + ", " + DocumentStatus.ActionDate;
    //alert(IsNothing(DocumentStatus.ActionDate) ? "--" : DocumentStatus.ActionDate.ToDate());
    var action = DocumentStatus.ActionBy + ", " + (IsNothing(DocumentStatus.ActionDate) ? "" : DocumentStatus.ActionDate.ToDate());
    var statusToolTip = "";
    switch (DocumentStatus.Status) {
        case "In-Progress":
            statusToolTip = ActivityStatusMsg.InProgress;
            break;
        case "ENG - Removal InProgress":
            statusToolTip = ActivityStatusMsg.retEngRemovalInProgress;
            break;
        case "RET - Removal InProgress":
            statusToolTip = ActivityStatusMsg.retEngRemovalInProgress;
            break;
        case "Request Received":
            statusToolTip = ActivityStatusMsg.RequestReceived;
            break;
        case "Complete":
            statusToolTip = CompletedMsg;
            break;
        case "Pending Approval":
            statusToolTip = ActivityStatusMsg.PendingApproval;
            break;
        case "Reprocess Requested":
            statusToolTip = ActivityStatusMsg.ReprocessRequested + " [" + action + "].";
            break;
        case "Acknowledged":
            statusToolTip = ActivityStatusMsg.Acknowledged + " [" + action + "].";
            break;
        case "Removed":
            statusToolTip = ActivityStatusMsg.Removed + " [" + action + "].";
            break;
        default:
            statusToolTip = "N/A";
    }
    return statusToolTip;
}

function P_GetStatusImg(status) {

    if (status.IsMatch("Pending Approval"))
        return "exclamation";
    else
        return "info";
}

function DrawFileMenu() {

}

function GetFileMenu(DocumentStatus, Extn, gridId) {

    var fileMenuArray = [];
    fileMenuArray.push(fileMenu_Remove);
    fileMenuArray.push(fileMenu_Download);

    if (IsNothing(DocumentStatus)) {
    }
    else {
        //ENG
        if (Extn.IsMatch('ENG')) {
            if (DocumentStatus.FileIn.IsMatch('SSC')) {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
                else if (DocumentStatus.Status.IsMatch('Complete')) {
                    //Delete
                    fileMenuArray.push(fileMenu_Reprocess);
                }
                else {
                    fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                    fileMenuArray.splice($.inArray(fileMenu_Download, fileMenuArray), 1);
                }
            }
            else if (DocumentStatus.FileIn.IsMatch('S2')) {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
                else if (DocumentStatus.Status.IsMatch('Complete')) {
                    //Delete
                    //fileMenuArray.push(fileMenu_Reprocess);
                }
                else {
                    fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                }
            }
            else {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
            }
        }
            //RET
        else if (Extn.IsMatch('RET')) {
            if (DocumentStatus.FileIn.IsMatch('SSC')) {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
                else if (DocumentStatus.Status.IsMatch('Acknowledged')) {
                    //Delete
                    fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                }
                else if (DocumentStatus.Status.IsMatch('Pending Approval')) {
                    //Delete
                    fileMenuArray.push(fileMenu_Reprocess);
                    fileMenuArray.push(fileMenu_Acknowledge);
                }
                else {
                    fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                    fileMenuArray.splice($.inArray(fileMenu_Download, fileMenuArray), 1);
                }
            }
            else if (DocumentStatus.FileIn.IsMatch('S2')) {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
                else if (DocumentStatus.Status.IsMatch('Acknowledged')) {
                    //Delete
                    fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                }
                else if (DocumentStatus.Status.IsMatch('Pending Approval')) {
                    //Delete
                    fileMenuArray.push(fileMenu_Reprocess);
                    fileMenuArray.push(fileMenu_Acknowledge);
                }
                else {
                    fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                    fileMenuArray.splice($.inArray(fileMenu_Download, fileMenuArray), 1);
                }
            }
            else {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
            }
        }
        else {
            if (DocumentStatus.FileIn.IsMatch('SSC')) {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }

                if (Extn.IsMatch('PDF')) {
                    if (DocumentStatus.Status.IsMatch('Pending Approval')) {
                        //Delete
                        fileMenuArray.push(fileMenu_Reprocess);
                        fileMenuArray.push(fileMenu_Acknowledge);
                    }
                    else {
                        fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                        fileMenuArray.splice($.inArray(fileMenu_Download, fileMenuArray), 1);
                    }
                }
            }
            else if (DocumentStatus.FileIn.IsMatch('S2')) {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }

                if (Extn.IsMatch('PDF')) {
                    if (DocumentStatus.Status.IsMatch('Pending Approval')) {
                        //Delete
                        fileMenuArray.push(fileMenu_Reprocess);
                        fileMenuArray.push(fileMenu_Acknowledge);
                    }
                    else {
                        fileMenuArray.splice($.inArray(fileMenu_Remove, fileMenuArray), 1);
                        fileMenuArray.splice($.inArray(fileMenu_Download, fileMenuArray), 1);
                    }
                }
            }
            else {
                if (IsNothing(DocumentStatus.Status)) {
                    //Delete
                }
            }
        }
    }

    var actionMenuArray = [fileMenuArray];
    return CreateMenu_OneDim_BS(actionMenuArray, gridId);
}

function GetFileMenu_Act(DocumentStatus, Extn, gridId) {
    var result = GetFileMenu(DocumentStatus, Extn, gridId);
    result = result.replace(/menuAction/gi, 'menuAction-Act');
    return result;
}

function OnDataBoundDrawMenu(e) {

    //$('.menuParent').menu({ position: { my: "left bottom", at: "right bottom" } });
    //$('.menuParent').menu({ position: "auto" });
    //$('.menuParent').menu();
    $('.menuParent').menu({ position: { my: "left top", at: "right top" } }); //Default

    $(".menuParent li ul").mouseover(function () {
        $(this).show();
    }).mouseout(function () {
        //$(this).hide().attr({ "aria-expanded": "false", "aria-hidden": "true" });
        $(this).hide();
    });

}

function P_MenuChildItem(item, gridId) {
    return "<li><a href='#' class='menuAction gridId-" + gridId + "'>" + item + "</a></li>";
}

function CreateMenu_BS_OneDim(menuArray) {
    if (!IsNothing(menuArray)) {

        var menuParent = "";
        var menuChild = "";

        if (menuArray.length > 1) {
            menuParent = menuArray[0];
            menuChild = menuArray[1];
        }
        else {
            menuParent = "Action";
            menuChild = menuArray[0];
        }

        var ret = "<div class='collapse navbar-collapse'>";
        ret = ret + "<ul class='nav navbar-nav'><li class='dropdown'>"
        ret = ret + "<a href='#' data-toggle='dropdown' class='dropdown-toggle'>" + menuParent + "<b class='caret'></b></a>";
        ret = ret + "<ul class='dropdown-menu'>";

        $.each(menuChild, function (idx, item) {
            ret = ret + P_MenuChildItem(item);
        })

        ret = ret + "</ul></li></ul></div>"

        return ret;
    }

    return null;
}

function CreateMenu_OneDim(menuArray) {

    if (!IsNothing(menuArray)) {

        var menuParent = "";
        var menuChild = "";

        if (menuArray.length > 1) {
            menuParent = menuArray[0];
            menuChild = menuArray[1];
        }
        else {
            menuParent = "Action";
            menuChild = menuArray[0];
        }

        var ret = "";
        ret = ret + "<ul class='menuParent'><li>"
        ret = ret + "<a href='#'>" + menuParent + "</a>";
        ret = ret + "<ul class='menuChild'>";

        $.each(menuChild, function (idx, item) {
            ret = ret + P_MenuChildItem(item);
        })

        ret = ret + "</ul></li></ul>"

        return ret;
    }

    return null;
}

//In-Use
function CreateMenu_OneDim_BS(menuArray, gridId) {

    if (!IsNothing(menuArray)) {

        var menuParent = "";
        var menuChild = "";

        if (menuArray.length > 1) {
            menuParent = menuArray[0];
            menuChild = menuArray[1];
        }
        else {
            menuParent = "Action";
            menuChild = menuArray[0];
        }

        var ret = "";

        ret = ret + '<div class="btn-group">'
        ret = ret + '<button class="btn btn-xs btn-primary" type="button">' + menuParent + '</button>'

        if (menuChild.length == 0)
            ret = ret + '<button class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown" disabled>'
        else
            ret = ret + '<button class="btn btn-xs btn-primary dropdown-toggle" data-toggle="dropdown">'

        ret = ret + '<span class="caret"></span>'
        ret = ret + '</button>'
        ret = ret + '<ul class="dropdown-menu pull-right" role="menu">'

        $.each(menuChild, function (idx, item) {
            ret = ret + P_MenuChildItem(item, gridId);
        })

        ret = ret + '</ul>'
        ret = ret + '</div>'

        return ret;
    }

    return null;
}

function P_GetRecordImgTitle(RecordUser, RecordDate) {

    var ret = "This file was declared a record during workspace closure initiated by " +
        (IsNothing(RecordUser) ? "n/a" : RecordUser) + ", " +
        (IsNothing(RecordDate) ? "n/a" : (RecordDate).ToDate());

    return ret;
}

function GetRecordControl(RecordUser, RecordDate) {

    var ret = "<img class='toolTipImg' src='/Content/images/Locked.png' title='" + P_GetRecordImgTitle(RecordUser, RecordDate) + "' />";
    return ret;
}

function GetFileIcon(extn) {
    var ret = "<img src='/Content/images/FileIcon/{0}' style='height:10px;' />";

    switch (extn.toUpperCase()) {
        case "TXT":
            ret = ret.replace(/\{0\}/gi, "Icon_TXT.gif");
            break;
        case "DOC":
            ret = ret.replace(/\{0\}/gi, "Icon_DOC.gif");
            break;
        case "DOCX":
            ret = ret.replace(/\{0\}/gi, "Icon_DOCX.gif");
            break;
        case "XLS":
            ret = ret.replace(/\{0\}/gi, "Icon_XLS.gif");
            break;
        case "XLSX":
            ret = ret.replace(/\{0\}/gi, "Icon_XLSX.gif");
            break;
        case "PPT":
            ret = ret.replace(/\{0\}/gi, "Icon_PPT.gif");
            break;
        case "PPTX":
            ret = ret.replace(/\{0\}/gi, "Icon_PPTX.gif");
            break;
        case "PDF":
            ret = ret.replace(/\{0\}/gi, "Icon_PDF.gif");
            break;
        case "ENG":
            ret = ret.replace(/\{0\}/gi, "Icon_ENG.png");
            break;
        case "RET":
            ret = ret.replace(/\{0\}/gi, "Icon_RET.png");
            break;
        case "MSG":
            ret = ret.replace(/\{0\}/gi, "Icon_MSG.png");
            break;
        case "IMG":
        case "BMP":
        case "GIF":
        case "JPG":
        case "JPEG":
        case "PNG":
        case "TIF":
            ret = ret.replace(/\{0\}/gi, "Icon_image.png");
            break;
        case "HTM":
        case "HTML":
            ret = ret.replace(/\{0\}/gi, "Icon_htm.png");
            break;
        default:
            ret = ret.replace(/\{0\}/gi, "Icon_Unknown.png");
            break;
    }

    return ret;
}

function GetLongTextToolTip(txt, lengthToShow, moreTxt) {

    if (IsNothing(txt))
        return "";

    if (txt.length <= lengthToShow)
        if (IsNothing(moreTxt))
            return txt;

    var ret = txt.substring(0, lengthToShow);

    if (IsNothing(moreTxt))
        return ret + "..... <img class='toolTipImg' title='" + txt + "' src='/Content/images/Expander.png' />";
    else
        return ret + "..... <img class='toolTipImg' title='" + moreTxt + txt + "' src='/Content/images/Expander.png' />";
}

function GetENGDocByRETDocNumber(retFileNum, updateObj) {
    $.ajax(
    {
        type: 'GET',
        async: false,
        url: '/api/Workspace/GetENGDocByRETDocNumber?retFileNum=' + retFileNum,
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data, textStatus, xhr) {
            if (!IsNothing(data)) {
                SetSelectedFile(data.ObjectID);
                //alert(updateObj);
                LoadRETnRF("Request RET", updateObj);
            }
            else
                AmAlert(AmMsg.Reprocess_S2_NoEng_4_RETMsg);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Retrieving Eng for RET.");
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

$(function () {

    $("body").on("click", ".menuAction-Act", function (e) {

        var selectedAction = $(e.target).text();

        var classList = $(e.target).prop('class').split(/\s+/);
        var gridId = "";
        $.each(classList, function (idx, item) {
            if (item.indexOf('gridId-') > -1) {
                gridId = item.slice('gridId-'.length, item.length);
                return false;
            }
        });

        var row = $(this).closest("tr");
        var grid = $("#" + gridId).data("kendoGrid");
        var dataItem = grid.dataItem(row);

        viewModel.currentDataItem(dataItem);
        viewModel.currentGrid(grid);
        viewModel.currentActivity(ko.mapping.fromJS(dataItem));

        if (selectedAction.IsMatch("Download")) {

            //alert(viewModel.currentActivity().DocNum());
            //alert(viewModel.currentActivity().EngagementNumber());
            //alert(viewModel.currentActivity().EngagementFileName());

            if (IsNothing(viewModel.currentActivity().DocNum())) {
                AmAlert("File don't have a document number.");
            }
            else {
                var downfileQs = [];
                downfileQs.push(viewModel.currentActivity().DocNum());
                downfileQs.push(usrId);
                downfileQs.push(env);
                downfileQs.push(viewModel.currentActivity().EngagementNumber());
                var fileName = viewModel.currentActivity().EngagementFileName();
                var encodeFileName = UriComponentEncode(fileName);
                var singleQuotesEncode = encodeFileName.replace(/'/g, "%27");
                downfileQs.push(singleQuotesEncode);
                //CreateDialog(600, 280, "Download", downloadUrl.format(downfileQs));

                //06-26-2015
                //CreateDialog(600, 280, "Download", GetDownLoadUrl(downfileQs));
                CreateDialog_Download(fileName, viewModel.currentActivity().DocNum(), null);
            }

        }
        else {


            //alert(JSON.stringify(ko.mapping.toJS(viewModel.currentActivity().DocumentStatus)));

            //return;

            //alert(viewModel.currentActivity().EngagementNumber());
            //myApp.showPleaseWait("Loading Workspace - " + viewModel.currentActivity().EngagementNumber());
            if (IsNothing(viewModel.selectedWs())) {
                //alert("1");
                //GetEngByEngNum(viewModel.currentActivity().EngagementNumber(), "ALL", true, false, false);
                GetEngByEngNum(viewModel.currentActivity().EngagementNumber(), "ALL", true, false);
            }
            else if (!(viewModel.selectedWs().WsProfile().EngNum().IsMatch(viewModel.currentActivity().EngagementNumber()))) {
                //alert(viewModel.selectedWs().ObjectID());
                viewModel.selectedWsId(viewModel.selectedWs().ObjectID());
                viewModel.toReloadSelectedWsId(true);

                //alert(viewModel.selectedWsId());
                //alert(viewModel.toReloadSelectedWsId());
                //alert("2");
                GetEngByEngNum(viewModel.currentActivity().EngagementNumber(), "ALL", true, false);
            }
            //myApp.hidePleaseWait();
            //alert(viewModel.selectedWs().WsProfile().EngNum());

            if (CanTakeAction(selectedAction, WhoCanAct.ONLY_ADMIN, AmMsg.CantTakeAction_File_Msg)) {

                if (selectedAction.IsMatch("Remove")) {

                    //alert(viewModel.selectedWs().WsProfile().IsUnderPreservation());
                    //alert(viewModel.selectedFile().IsDeleted());

                    if (viewModel.selectedWs().WsProfile().IsUnderPreservation()) {
                        AmAlert(DeleteMsg.IsUnderPreservationMsg);
                        return false;
                    }

                    //if (viewModel.selectedFile().IsDeleted()) {
                    //    AmAlert(DeleteMsg.IsDeletedMsg);
                    //    return false;
                    //}

                    //if (viewModel.selectedFile().IsRecord()) {
                    //    AmAlert(DeleteMsg.IsRecordMsg);
                    //    return false;
                    //}

                    //if (viewModel.selectedFile().IsCheckedOut()) {
                    //    AmAlert(DeleteMsg.IsCheckedOutMsg);
                    //    return false;
                    //}

                    //if (viewModel.selectedFile().IsLocked()) {
                    //    AmAlert(DeleteMsg.IsLockedMsg);
                    //    return false;
                    //}

                    //if (viewModel.selectedFile().VersionCount() > 1) {
                    //    AmAlert(DeleteMsg.VersionCountMsg);
                    //    return false;
                    //}

                    PerformActivity_Act("Remove");

                }
                else if (selectedAction.IsMatch("Reprocess")) {

                    if (viewModel.currentActivity().DocumentStatus.FileIn().IsMatch('S2')
                        && viewModel.currentActivity().DocumentStatus.DocumentType().FileType().IsMatch('RET')) {

                        //if (viewModel.currentActivity().DocumentStatus.FilePath.IsMatch('/2 - Period-end Audit/RET Files'))
                        if (viewModel.currentActivity().ReviewType().IsMatch("Year End Review")) {

                            GetENGDocByRETDocNumber(viewModel.currentActivity().DocumentStatus.DocumentNumber(), GetUpdateModel_Act("Remove", "Reprocess Requested"));

                            //LoadRETnRF("Request RF");
                        }
                        else {
                            AmAlert(AmMsg.Reprocess_S2_InRev_RETMsg);
                        }
                    }
                    else {
                        PerformActivity_Act("Reprocess");
                    }

                }

                else if (selectedAction.IsMatch("Acknowledge")) {

                    //07-29-2015[Monika asked to change the File movement]

                    //if (viewModel.currentActivity().DocumentStatus.FileIn().IsMatch('S2')
                    //    && viewModel.currentActivity().DocumentStatus.DocumentType().FileType().IsMatch('RET')
                    //    //&& viewModel.currentActivity().DocumentStatus.FilePath.IsMatch('/2 - Period-end Audit/RET Files'))
                    //    && viewModel.currentActivity().ReviewType().IsMatch("Year End Review")) {

                    //    AmInteraction(AmConfirmMsg.PeriodEndAuditRet, ValidatePeriodEndAuditRet_Act);
                    //}
                    //else {
                    //    PerformActivity_Act("Acknowledge");
                    //}

                    PerformActivity_Act("Acknowledge");

                    //07-29-2015[Monika asked to change the File movement]
                }
            }
        }
    });

    $("body").on("click", ".menuAction", function (e) {

        var selectedAction = $(e.target).text();

        //if (CanTakeAction(selectedAction, WhoCanAct.ONLY_ADMIN, AmMsg.CantTakeAction_File_Msg)) {
        //alert($(e.target).prop('class'));

        //alert($("*").filter(function () { return /gridId-.+/.test($(e.target).attr("class")) }));
        //alert(/gridId-.+/.test($(e.target).attr("class")));

        //var check = "gridId-";
        //$('[class^="gridId-"], [class*=" gridId-"]').each(function () {
        //    var className = this.className;

        //    var cls = $.map(this.className.split(' '), function (val, i) {
        //        if (val.indexOf(check) > -1) {
        //            return val.slice(check.length, val.length)
        //        }
        //    });

        //    console.log(cls.join(' '));
        //});

        var classList = $(e.target).prop('class').split(/\s+/);
        var gridId = "";
        $.each(classList, function (idx, item) {
            if (item.indexOf('gridId-') > -1) {
                gridId = item.slice('gridId-'.length, item.length);
                return false;
            }
        });

        var row = $(this).closest("tr");
        var grid = $("#" + gridId).data("kendoGrid");
        var dataItem = grid.dataItem(row);

        var selectedFileId = dataItem.ObjectID;

        SetSelectedFile(selectedFileId);

        viewModel.currentDataItem(dataItem);
        viewModel.currentGrid(grid);

        //alert(JSON.stringify(ko.mapping.toJS(viewModel.selectedFile().DocumentStatus)));

        //return;

        if (selectedAction.IsMatch("Download")) {

            var downfileQs = [];
            downfileQs.push(viewModel.selectedFile().Number());
            downfileQs.push(usrId);
            downfileQs.push(env);
            downfileQs.push(viewModel.selectedWs().WsProfile().EngNum());

            //var fileName = viewModel.selectedFile().Description() + "." + viewModel.selectedFile().Extn();
            //FileWithExtn
            var fileName = viewModel.selectedFile().Description().FileWithExtn(viewModel.selectedFile().Extn());
            //alert(viewModel.selectedFile().Description() + "." + viewModel.selectedFile().Extn());
            //alert(fileName);
            var encodeFileName = UriComponentEncode(fileName);
            var singleQuotesEncode = encodeFileName.replace(/'/g, "%27");
            downfileQs.push(singleQuotesEncode);

            //CreateDialog(600, 280, "Download", downloadUrl.format(downfileQs));

            //06-26-2015
            //CreateDialog(600, 280, "Download", GetDownLoadUrl(downfileQs));
            CreateDialog_Download(fileName, viewModel.selectedFile().Number(), null);
        }

        else if (selectedAction.IsMatch("Remove")) {


            if (CanTakeAction(selectedAction,
                ((!IsNothing(viewModel.selectedFile().DocumentStatus.FileIn)) &&
                (viewModel.selectedFile().DocumentStatus.FileIn().IsMatch('S2') || viewModel.selectedFile().DocumentStatus.FileIn().IsMatch('SSC')))
                ? WhoCanAct.ONLY_ADMIN : WhoCanAct.ADMIN_N_MEMBERS, AmMsg.CantTakeAction_File_Msg)) {

                if (viewModel.selectedWs().WsProfile().IsUnderPreservation()) {
                    AmAlert(DeleteMsg.IsUnderPreservationMsg);
                    return false;
                }

                if (viewModel.selectedFile().IsDeleted()) {
                    AmAlert(DeleteMsg.IsDeletedMsg);
                    return false;
                }

                if (viewModel.selectedFile().IsRecord()) {
                    AmAlert(DeleteMsg.IsRecordMsg);
                    return false;
                }

                //if (viewModel.selectedFile().IsCheckedOut()) {
                //    AmAlert(DeleteMsg.IsCheckedOutMsg);
                //    return false;
                //}

                //if (viewModel.selectedFile().IsLocked()) {
                //    AmAlert(DeleteMsg.IsLockedMsg);
                //    return false;
                //}

                if (viewModel.selectedFile().VersionCount() > 1) {
                    AmAlert(DeleteMsg.VersionCountMsg);
                    return false;
                }

                PerformActivity("Remove");
            }
        }

        else if (selectedAction.IsMatch("Request RF") || selectedAction.IsMatch("Request RET")) {

            if (CanTakeAction(selectedAction, WhoCanAct.ADMIN_N_MEMBERS, AmMsg.CantTakeAction_File_Msg)) {
                LoadRETnRF($(e.target).text());
            }

        }

        else if (selectedAction.IsMatch("Reprocess")) {

            if (CanTakeAction(selectedAction, WhoCanAct.ONLY_ADMIN, AmMsg.CantTakeAction_File_Msg)) {

                if (viewModel.selectedFile().DocumentStatus.FileIn().IsMatch('S2')
                    && viewModel.selectedFile().Extn().IsMatch('RET')) {

                    if (viewModel.selectedFile().FilePath().IsMatch('/2 - Period-end Audit/RET Files')) {
                        GetENGDocByRETDocNumber(viewModel.selectedFile().DocumentStatus.DocumentNumber(), GetUpdateModel("Remove", "Reprocess Requested"));

                        //LoadRETnRF("Request RF");
                    }
                    else {
                        AmAlert(AmMsg.Reprocess_S2_InRev_RETMsg);
                    }
                }
                else {
                    PerformActivity("Reprocess");
                }
            }
        }

        else if (selectedAction.IsMatch("Acknowledge")) {

            if (CanTakeAction(selectedAction, WhoCanAct.ONLY_ADMIN, AmMsg.CantTakeAction_File_Msg)) {

                //07-29-2015[Monika asked to change the File movement]

                //if (viewModel.selectedFile().DocumentStatus.FileIn().IsMatch('S2')
                //    && viewModel.selectedFile().Extn().IsMatch('RET')
                //    && viewModel.selectedFile().FilePath().IsMatch('/2 - Period-end Audit/RET Files')) {

                //    AmInteraction(AmConfirmMsg.PeriodEndAuditRet, ValidatePeriodEndAuditRet);
                //}
                //else {
                //    PerformActivity("Acknowledge");
                //}

                PerformActivity("Acknowledge");

                //07-29-2015[Monika asked to change the File movement]
            }
        }
        //}
    });

    //$("#btnUpload").click(function () {
    $("body").on("click", "#btnUpload", function (e) {

        if (IsNothing(viewModel.selectedFldr())) {
            AmAlert("Please select a Workspace folder.");
        }
        else {
            if (CanTakeAction("Upload", WhoCanAct.ADMIN_N_MEMBERS, AmMsg.CantTakeAction_Ws_Upload_Msg)) {

                var upfileQs = [];
                upfileQs.push(viewModel.selectedWs().WsProfile().EngNum());
                upfileQs.push(UriComponentEncode(viewModel.selectedFldr().FolderPath()));
                upfileQs.push(env);
                upfileQs.push(usrId);

                //06-25-2015
                //CreateDialog(600, 280, "Upload", uploadUrl.format(upfileQs));
                CreateDialog_Upload(viewModel.selectedWs().WsProfile().EngNum(), UriComponentEncode(viewModel.selectedFldr().FolderPath()));
            }
        }
    })
})

function ValidatePeriodEndAuditRet(that) {

    if (!$('input:radio[name=radPeriodEndAuditRet]').is(':checked')) {
        AmAlert("Please select one of the option.");
    }
    else {
        //alert($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val());
        $(that).dialog("close");
        //alert($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val());
        PerformActivity("Acknowledge");
    }
}

function PerformActivity(activity) {

    //$("#dmComment")
    //    .data('okAction', ValidateActivityComment)
    //    .data('param', activity)
    //    .dialog('open');

    GetDialog(dialogType.INTERACTION, dialogSize.SMALL, dialogClass.COMMENT, 'Comment')
                .html(html_comment_Template)
                .data('okAction', ValidateActivityComment)
                .data('param', activity)
                .dialog("open");

    //DoActivity(activity);
}

function DoActivity(activity, comment) {

    if (activity.IsMatch("Remove")) {

        PerformDelete(comment, true);
        return;

        //alert(viewModel.selectedFile().DocumentStatus.FileIn);

        if (IsNothing(viewModel.selectedFile().DocumentStatus)) {
            PerformDelete(comment, true);
            return;
        }
        else if (viewModel.selectedFile().DocumentStatus.FileIn().IsMatch("Orphan")) {
            PerformDelete(comment, true);
            return;
        }
    }

    PostUpdateFileActivity(GetUpdateModel(activity, comment), viewModel.selectedWs().Name(), viewModel.selectedFile().ObjectID());
}

function ValidateActivityComment(that, activity) {

    if (activity.IsMatch("Acknowledge")) {
        DoActivity(activity, $("#txtActivityComment").val());
        $(that).dialog("close");
    }
    else {
        if (IsNothing($("#txtActivityComment").val())) {
            AmAlert(AmMsg.Activity_CommentEmptyMsg);
        }
        else {
            DoActivity(activity, $("#txtActivityComment").val());
            $(that).dialog("close");
        }
    }
}

//
function ValidatePeriodEndAuditRet_Act(that) {

    if (!$('input:radio[name=radPeriodEndAuditRet]').is(':checked')) {
        AmAlert("Please select one of the option.");
    }
    else {
        //alert($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val());
        $(that).dialog("close");
        //alert($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val());
        PerformActivity_Act("Acknowledge");
    }
}

function PerformActivity_Act(activity) {

    //$("#dmComment")
    //    .data('okAction', ValidateActivityComment_Act)
    //    .data('param', activity)
    //    .dialog('open');

    GetDialog(dialogType.INTERACTION, dialogSize.SMALL, dialogClass.COMMENT, 'Comment')
              .html(html_comment_Template)
              .data('okAction', ValidateActivityComment_Act)
              .data('param', activity)
              .dialog("open");
}

function GetUpdateModel(activity, comment) {

    var naFlag = IsNothing($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val()) ? null : $('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val();

    //.IsMatch("NonAudit")

    var obj = {

        'IsPage_Act': false,

        'wsId': viewModel.selectedWs().ObjectID(),
        'docObjId': viewModel.selectedFile().ObjectID(),

        'EngNum': viewModel.selectedWs().WsProfile().EngNum(),
        'WsActivityType': "Activity_" + activity,
        'WsFileType': viewModel.selectedFile().WsFileType(),
        'FileIn': IsNothing(viewModel.selectedFile().DocumentStatus) ? 'Orphan' : viewModel.selectedFile().DocumentStatus.FileIn(),
        'FileUniqueId': IsNothing(viewModel.selectedFile().DocumentStatus) ? '0' : viewModel.selectedFile().DocumentStatus.UniqueId(),
        'FileNum': viewModel.selectedFile().Number(),
        //'NonAuditFlag': (IsNothing($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val()) ? null : $('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val()).IsMatch("NonAudit"),
        'NonAuditFlag': IsNothing(naFlag) ? null : naFlag.IsMatch("NonAudit"),
        'Comment': comment,
    };

    //alert(obj.NonAuditFlag);
    return obj;
}

function GetLogAs() {

    if (document.URL.indexOf("Workspace/WsActivity") > -1) {

        if ($("#chkLogAs").length > 0) {

            //alert("1");
            if ($('input[name="chkLogAs"]').is(':checked')) {

                //alert($("#hidLogAs").val());
                return $("#hidLogAs").val();
            }

        }
    }

    return "";
}

function GetUpdateModel_Act(activity, comment) {

    //alert(viewModel.currentActivity().DocumentStatus);

    var naFlag = IsNothing($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val()) ? null : $('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val();

    var obj = {

        'IsPage_Act': true,

        'EngNum': viewModel.currentActivity().EngagementNumber(),
        'WsActivityType': "Activity_" + activity,

        'WsFileType': IsNothing(viewModel.currentActivity().DocumentStatus) ? 'Other' : viewModel.currentActivity().DocumentStatus.DocumentType().FileType(),
        'FileIn': IsNothing(viewModel.currentActivity().DocumentStatus) ? 'Orphan' : viewModel.currentActivity().DocumentStatus.FileIn(),
        'FileUniqueId': IsNothing(viewModel.currentActivity().DocumentStatus) ? '0' : viewModel.currentActivity().DocumentStatus.UniqueId(),
        'FileNum': IsNothing(viewModel.currentActivity().DocumentStatus) ? viewModel.currentActivity().DocNum : viewModel.currentActivity().DocumentStatus.DocumentNumber(),

        //'NonAuditFlag': (IsNothing($('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val()) ? null : $('input:radio[name=radPeriodEndAuditRet]').filter(":checked").val()).IsMatch("NonAudit"),
        'NonAuditFlag': IsNothing(naFlag) ? null : naFlag.IsMatch("NonAudit"),
        'Comment': comment,
        'logAs': GetLogAs(),
    };

    return obj;
}

function DoActivity_Act(activity, comment) {

    //if (activity.IsMatch("Remove")) {

    //    if (IsNothing(viewModel.selectedFile().DocumentStatus)) {
    //        PerformDelete_Act(comment);
    //        return;
    //    }
    //    else if (viewModel.selectedFile().DocumentStatus.FileIn().IsMatch("Orphan")) {
    //        PerformDelete_Act(comment);
    //        return;
    //    }
    //}
    //viewModel.currentActivity()
    //alert(viewModel.currentActivity().DocumentStatus.DocumentType);
    //alert(viewModel.currentActivity().DocNum);
    //alert(viewModel.currentActivity().DocumentStatus.DocumentNumber);

    //alert(obj.NonAuditFlag);

    PostUpdateFileActivity_Act(GetUpdateModel_Act(activity, comment), viewModel.currentActivity().EngagementName(), viewModel.currentActivity);

}

function ValidateActivityComment_Act(that, activity) {

    if (activity.IsMatch("Acknowledge")) {
        DoActivity_Act(activity, $("#txtActivityComment").val());
        $(that).dialog("close");
    }
    else {
        if (IsNothing($("#txtActivityComment").val())) {
            AmAlert(AmMsg.Activity_CommentEmptyMsg);
        }
        else {
            DoActivity_Act(activity, $("#txtActivityComment").val());
            $(that).dialog("close");
        }
    }
}

function PostUpdateFileActivity_Act(obj, engName, currentActivity) {
    //alert("PostUpdateFileActivity_Act");
    $.ajax(
    {
        type: 'POST',
        url: '/api/Workspace/PostUpdateFileActivity',
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        dataType: 'json',
        data: ko.toJSON(obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data, textStatus, xhr) {

            //alert(viewModel.currentActivity().DocumentStatus.Status);
            //UpdateFileStatus(fileObjID, data);

            //alert(viewModel.currentActivity().DocumentStatus.Status());
            //viewModel.currentActivity().DocumentStatus.Status(data);
            //alert(viewModel.currentActivity().DocumentStatus.Status());

            //var dataItem = viewModel.currentDataItem();
            //dataItem.DocumentStatus.Status = data;
            //dataItem.set("Status", GetDocumentStatus(dataItem.DocumentStatus));

            UpdateFileStatusAfterActivity(data);

            //viewModel.selectedWsId(viewModel.selectedWs().ObjectID());
            //viewModel.toReloadSelectedWsId(true);

            ReloadSelectedWs();

            HandleSuccess("PostUpdateFileActivity_Act-" + obj.WsActivityType, "");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Processing File Activity - " + engName);
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

function ReloadSelectedWs() {
    if (viewModel.toReloadSelectedWsId()) {
        if (remote) {
            //alert(viewModel.selectedWs().ObjectID());
            GetEngByWsId(viewModel.selectedWsId(), "ALL", null);
        }
        else {

            //viewModel.selectedWs().IsLoaded(false);
            //var chk = $($("ul.ulEngFarm > li > .chkEngFldr:checked")[0]);
            //chk.prop("checked", false);
            //chk.trigger("click");
            //alert(viewModel.selectedWsId());
            var match = ko.utils.arrayFirst(viewModel.treeRoot(), function (item) {
                return viewModel.selectedWsId() === item.ObjectID();
            });

            var idx = viewModel.treeRoot().indexOf(match);

            //alert(idx);
        }

        viewModel.toReloadSelectedWsId(false);
    }
}
//

function PostUpdateFileActivity(obj, engName, fileObjID) {
    $.ajax(
    {
        type: 'POST',
        url: '/api/Workspace/PostUpdateFileActivity',
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        dataType: 'json',
        data: ko.toJSON(obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data, textStatus, xhr) {

            //UpdateFileStatus(fileObjID, data);

            //var dataItem = viewModel.currentDataItem();
            //dataItem.DocumentStatus.Status = data;
            //dataItem.set("Status", GetDocumentStatus(dataItem.DocumentStatus));

            UpdateFileStatusAfterActivity(data);

            HandleSuccess("PostUpdateFileActivity-" + obj.WsActivityType, "");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Processing File Activity - " + engName);
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}


//Delete
function PerformDelete(comment, toValidate) {
    //var wsLogFldrID = "";

    //$.each(viewModel.selectedWs().WsFldrs()(), function (idx, item) {
    //    if (item.Name().IsMatch("Workspace Log")) {
    //        wsLogFldrID = item.ObjectID();
    //        return false;
    //    }
    //});

    //alert("PerformDelete - Called");

    //$.ajax({
    //    type: "Delete",
    //    url: "/api/Workspace/DeleteDoc/?wsId=" + viewModel.selectedWs().ObjectID() +
    //        "&docObjId=" + viewModel.selectedFile().ObjectID() + "&comment=" + comment + "&toValidate=" + toValidate,

    //    error: function (jqXHR, textStatus, errorThrown) {
    //        HandleError(jqXHR, textStatus, errorThrown);
    //    },
    //    success: function (data, textStatus, jqXHR) {
    //        RemoveDeletedFile(viewModel.selectedFile().ObjectID(), data);
    //        HandleSuccess("DeleteDoc", "");
    //    },
    //    beforeSend: function () {
    //        myApp.showPleaseWait("Deleting File - " + viewModel.selectedFile().Number());
    //    },
    //    complete: function () {
    //        myApp.hidePleaseWait();
    //    }
    //});

    PerformDeleteFinal(viewModel.selectedWs().ObjectID(), viewModel.selectedFile().ObjectID(), comment, toValidate);
}

function PerformDeleteFinal(wsId, docObjId, comment, toValidate) {

    $.ajax({
        type: "Delete",

        url: "/api/Workspace/DeleteDoc/?wsId=" + wsId +
            "&docObjId=" + docObjId + "&comment=" + comment + "&toValidate=" + toValidate,
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        success: function (data, textStatus, jqXHR) {
            RemoveDeletedFile(viewModel.selectedFile().ObjectID(), data);
            HandleSuccess("DeleteDoc", "");
        },
        beforeSend: function () {
            myApp.showPleaseWait("Deleting File - " + viewModel.selectedFile().Number());
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

//function PerformDelete_Act(comment) {

//    $.ajax({
//        type: "Delete",

//        url: "/api/Workspace/DeleteDoc/?wsId=" + viewModel.selectedWs().ObjectID() +
//            "&docObjId=" + viewModel.selectedFile().ObjectID() + "&comment=" + comment,

//        error: function (jqXHR, textStatus, errorThrown) {
//            HandleError(jqXHR, textStatus, errorThrown);
//        },
//        success: function (data, textStatus, jqXHR) {
//            RemoveDeletedFile(viewModel.selectedFile().ObjectID(), data);
//            HandleSuccess("DeleteDoc", "");
//        },
//        beforeSend: function () {
//            myApp.showPleaseWait("Deleting File - " + viewModel.selectedFile().Number());
//        },
//        complete: function () {
//            myApp.hidePleaseWait();
//        }
//    });
//}
//