
var toAddHash = true;

function AddHash(url) {

    var docNum = getParameterByName(url, "docnum");

    $.ajax({
        async: false,
        url: "api/IManageUtility/GetHash?fileNum=" + docNum,
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data) {
            url = url + "&amp;hash=" + data.Hash;
        }
    });

    return url;
}

function P_CreateDialog_Up(width, height, title, url) {

    if (toAddHash)
        url = AddHash(url);

    if (uploadInProgress) {

        AmAlert("Multiple upload is not supported.");

    }
    else {

        var iF = "<iframe class='iFrame' src='" + url + "'></iframe>"

        var div_id = 'divUp_' + $('.dmUp').length;

        $('body').append('<div class="dmUp" id="' + div_id + '"></div>');

        $('#' + div_id).dialog({
            autoOpen: true,
            resizable: false,
            modal: false,
            width: width,
            height: height,
            title: title,
            open: function () {

                uploadInProgress = true;

                $('#' + div_id).html(iF);

                $('#' + div_id).parents('.ui-dialog').children('.ui-dialog-titlebar').append('<a href="#" id="' + div_id +
                '-minMaxbutton" class="ui-dialog-titlebar-minimize ui-corner-all minMaxbutton">' +
                '<span class="ui-icon ui-icon-minusthick"></span></a>');
            },
            close: function () {

                selectedFolderId_CheckBoxId = viewModel.selectedFldr().ObjectID();

                newSession = true;
                ReloadWs("From_Upload");
            }
        });
    }
}

function P_CreateDialog_Down(width, height, title, url) {

    if (toAddHash)
        url = AddHash(url);

    if (IsUserIP_Outer()) {

        window.location = url;

    }
    else {
        var iF = "<iframe class='iFrame' src='" + url + "'></iframe>"

        var div_id = 'divDown_' + $('.dmDown').length;

        $('body').append('<div class="dmDown" id="' + div_id + '"></div>');

        $('#' + div_id).dialog({
            autoOpen: true,
            resizable: false,
            modal: false,
            width: width,
            height: height,
            title: title,
            open: function () {

                $('#' + div_id).html(iF);

                $('#' + div_id).parents('.ui-dialog').children('.ui-dialog-titlebar').append('<a href="#" id="' + div_id +
                '-minMaxbutton" class="ui-dialog-titlebar-minimize ui-corner-all minMaxbutton">' +
                '<span class="ui-icon ui-icon-minusthick"></span></a>');
            },
            close: function () {


            }
        });
    }
}

function CreateDialog_Download(fileName, fileNum, fileObjId) {

    toAddHash = false;

    //var encodeFileName = UriComponentEncode(fileName);
    //var singleQuotesEncodeFileName = encodeFileName.replace(/'/g, "%27");

    //var encodeFileObjId = UriComponentEncode(fileObjId);
    //var singleQuotesEncodeFileObjId = encodeFileObjId.replace(/'/g, "%27");

    var queryStr = [];
    //queryStr.push(singleQuotesEncodeFileName);
    queryStr.push(fileNum);
    //queryStr.push(singleQuotesEncodeFileObjId);

    if (IsUserIP_Outer()) {
        queryStr.push(true);
    }
    else {
        queryStr.push(false);
    }

    var url = downloadUrl.format(queryStr);

    CreateDialog(600, 280, "Download", url);

    //GetDialog(dialogType.DIALOG, dialogSize.SMALL, "", "Download")
    //            .load(url)
    //            .dialog("open");
}

function CreateDialog_Upload(ws, fldr) {

    toAddHash = false;

    var encodeWs = UriComponentEncode(ws);
    var singleQuotesEncodeWs = encodeWs.replace(/'/g, "%27");

    var encodeFldr = UriComponentEncode(fldr);
    var singleQuotesEncodeFldr = encodeFldr.replace(/'/g, "%27");

    var queryStr = [];
    queryStr.push(singleQuotesEncodeWs);
    queryStr.push(singleQuotesEncodeFldr);
    queryStr.push(false);

    var url = uploadUrl.format(queryStr);

    CreateDialog(600, 280, "Upload", url);

    //GetDialog(dialogType.DIALOG, dialogSize.SMALL, "", "Upload")
    //            .load(url)
    //            .dialog("open");
}

function CreateDialog(width, height, title, url) {

    if (title.IsMatch("Upload")) {
        P_CreateDialog_Up(width, height, title, url);
    }
    else if (title.IsMatch("Download")) {
        P_CreateDialog_Down(width, height, title, url);
    }
}

function IsUserIP_Outer() {
    if (usrIp.indexOf("10.1.3.10") > -1 || usrIp.indexOf("10.1.3.11") > -1 || usrIp.indexOf("10.1.3.12") > -1) {
        return true;
    }
    else {
        return false;
    }
}

function GetDownLoadUrl(downfileQs) {

    if (IsUserIP_Outer()) {
        return downloadUrl_Ip.format(downfileQs);
    }
    else {
        return downloadUrl.format(downfileQs);
    }
}