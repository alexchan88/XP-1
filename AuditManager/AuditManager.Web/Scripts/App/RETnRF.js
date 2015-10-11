


var ins2ProcRFMsg = "Example <br />MAF #1: How were workbooks added Single.<br />MAF Name #2 ABC Co Sub<br />How were workbooks added: Multi Workbook Functionality<br />Workbook 1 Name: Division A<br />Workbook 2 Name: Division B<br />Workbook 3 Name: Division C<br /><br />Additional instructions: Please roll forward all the workbooks in MAF #1 and only the first two workbooks in MAF #2<br />(Division A and Division B)";

var busUnitArray =
    [
        "Bay Area",
        "Chicago Metro",
        "Coastal",
        "Dallas",
        "Denver",
        "Gateway West",
        "Houston",
        "Metro NY",
        "Mid-America",
        "Midsouth",
        "NEUNY",
        "Northern Heartland",
        "NYFS",
        "Pacific Northwest",
        "Pacific Southwest",
        "Pennsylvania",
        "Virginia",
        "Washington/Baltimore"
    ];

//var eAudITWFArray = ["Integrated Public", "Integrated Non-Public", "FSA Public", "FSA Non-Public", "US Private Entity"];
var eAudITWFArray = ["Integrated Public", "Integrated Non-Public", "FSA Public", "US Private Entity"];

$(".divMultiMAF21_Tog_1").hide();
$(".divMultiMAF21_Tog_2").hide();
$(".divDiffWf_Tog_1").hide();
$(".divPartialRF_Tog_1").hide();

$("input[name=rad1MAF2Multi]").change(function () {
    if (this.value.toLowerCase() == "no") {
        $(".divMultiMAF21_Tog_1").show();
    }
    if (this.value.toLowerCase() == "yes") {
        $(".divMultiMAF21_Tog_1").hide();
        $(".divMultiMAF21_Tog_2").hide();
        $("input[name=radMultiMAF21]").each(function (idx, item) {
            item.checked = false;
        });
    }
});

$("input[name=radMultiMAF21]").change(function () {
    if (this.value.toLowerCase() == "yes") {
        $(".divMultiMAF21_Tog_2").show();
    }
    if (this.value.toLowerCase() == "no") {
        $(".divMultiMAF21_Tog_2").hide();
    }
});

$("input[name=radDiffWf]").change(function () {
    if (this.value.toLowerCase() == "yes") {
        $(".divDiffWf_Tog_1").show();
    }
    if (this.value.toLowerCase() == "no") {
        $(".divDiffWf_Tog_1").hide();
    }
});

$("input[name=radPartialRF]").change(function () {
    if (this.value.toLowerCase() == "yes") {
        $(".divPartialRF_Tog_1").show();
    }
    if (this.value.toLowerCase() == "no") {
        $(".divPartialRF_Tog_1").hide();
    }
});

$(".txtBusUnit").append('<option value="">--Select Business Unit--</option>');
$.each(busUnitArray, function (idx, item) {
    $(".txtBusUnit").append('<option value="' + item + '">' + item + '</option>');
});

$(".txteAudITWF").append('<option value="">--Select eAudIT workflow--</option>');
$.each(eAudITWFArray, function (idx, item) {
    $(".txteAudITWF").append('<option value="' + item + '">' + item + '</option>');
});

$(".txteAudITYr").append('<option value="">--Select Year--</option>');
for (var i = -2; i < 1; i++) {
    //$(".txteAudITYr").append('<option value="' + "eAudIT" + ((new Date).getFullYear() - i) + '">' + "eAudIT" + ((new Date).getFullYear() - i) + '</option>');
    $(".txteAudITYr").append('<option value="' + ((new Date).getFullYear() - i) + '">' + "eAudIT" + ((new Date).getFullYear() - i) + '</option>');
}

$(".txteAudITYr_RET").append('<option value="">--Select eAudIT Year--</option>');
for (var i = -0; i < 6; i++) {
    //$(".txteAudITYr_RET").append('<option value="' + "eAudIT" + ((new Date).getFullYear() - i) + '">' + "eAudIT" + ((new Date).getFullYear() - i) + '</option>');
    $(".txteAudITYr_RET").append('<option value="' + ((new Date).getFullYear() - i) + '">' + "eAudIT" + ((new Date).getFullYear() - i) + '</option>');
}

$(".txteAudITYr_RF").append('<option value="">--Select Year--</option>');
for (var i = -0; i < 6; i++) {
    $(".txteAudITYr_RF").append('<option value="' + ((new Date).getFullYear() - i) + '">' + ((new Date).getFullYear() - i) + '</option>');
}

$(".lblIns2ProcRFMsg").hover(
   function () {
       $(this).popover({
           placement: 'auto',
           content: ins2ProcRFMsg,
           html: true,
           container: 'body'
       }).popover('show');
   },
    function () {
        $(this).popover('hide');
    });

$("#btnRETnRFConfirm").click(function () {
    if ($("#surveyRequestType").val().IsMatch("RET")) {
        if (wbArray().length == 0) {
            AmAlert("Please add Workbook.");
            return false;
        }
        else {
            var wBs = wbArray().join(',');
            $("#workBooks").val(wBs);
        }
    }

    //alert($("input[name=radKPMGContEnt]:radio:checked").val());
    //var radKPMGContEnt_whatSelected = $("input[name=radKPMGContEnt]:radio:checked").val();
    //var radKPMGContEnt_whatComment = $("#txtADesc").val().trim();

    //if (!IsNothing(radKPMGContEnt_whatSelected)) {
    //    if (radKPMGContEnt_whatSelected.IsMatch('Yes')) {
    //        if (IsNothing(radKPMGContEnt_whatComment)) {
    //            AmAlert("Please enter the comment.");
    //            $("#txtADesc").focus();
    //            return false;
    //        }
    //    }
    //}

});

$("#btnRETnRFCancle").click(function () {
    //$('#dialog_XL').dialog('close');
    CloseMyDialog(this);
});

var GetDCSServerNullExecuted = false;

function GetDCSServer(prjCode) {

    //alert("dcs");
    //if (prjCode == null) {
    //    alert("1");
    //}
    //else if (IsNothing(prjCode)) {
    //    alert("2");
    //    $(".cmbDCSServer").empty();
    //    $(".cmbDCSServer").prop("disabled", "disabled");

    //    return;
    //}

    prjCode = IsNothing(prjCode) ? '' : prjCode.IsMatch("None of the above") ? '' : prjCode;

    if (prjCode == null || IsNothing(prjCode)) {
        if (GetDCSServerNullExecuted) {
            return;
        }
        else {
            GetDCSServerNullExecuted = true;
        }
    }
    else {
        GetDCSServerNullExecuted = false;
    }

    $.ajax(
    {
        type: 'GET',
        async: true,
        url: '/api/PrjNDCS/GetDCSServer?prjCode=' + prjCode,
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data, textStatus, xhr) {

            if (IsNothing(data) || data.length == 0) {
                GetDCSServer(null);
            }
            else {

                $(".cmbDCSServer").prop("disabled", "");
                $(".cmbDCSServer").empty();

                $("#hid_cmbDCSServer_Options").val('');

                $(".cmbDCSServer").append('<option value="">--Select Server--</option>');
                //Monika06172015
                $(".cmbDCSServer").append('<option value="eAudIT Server 2">eAudIT Server 2</option>');
                $.each(data, function (idx, item) {
                    $("#hid_cmbDCSServer_Options").val($("#hid_cmbDCSServer_Options").val() + ',' + item.ServerName);
                    $(".cmbDCSServer").append('<option value="' + item.ServerName + '">' + item.ServerName + '</option>');
                });

                //$(".cmbDCSServer").append('<option value="Not on a server">Not on a server</option>');
                $(".cmbDCSServer").append('<option value="File was not on server">File was not on server</option>');
                
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Loading Server Code");
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

function GetPrjCode(engNum, clientCode) {

    var GetPrjCode_QS = "";
    if (fakePrjNDCS.IsMatch("true")) {
        GetPrjCode_QS = 'clientCode=' + '60004534' + '&engNum=' + '11418848';
    }
    else {
        GetPrjCode_QS = 'clientCode=' + clientCode + '&engNum=' + engNum;
    }

    $.ajax(
    {
        type: 'GET',
        async: true,
        //url: '/api/PrjNDCS/GetPrjCode?clientCode=' + '60004534' + '&engNum=' + '11418848',
        //url: '/api/PrjNDCS/GetPrjCode?clientCode=' + clientCode + '&engNum=' + engNum,
        url: '/api/PrjNDCS/GetPrjCode?' + GetPrjCode_QS,
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data, textStatus, xhr) {

            if (IsNothing(data) || data.length == 0) {

                //$(".cmbProjectCode").prop("required", "");
                //$(".cmbProjectCode").prop("disabled", "disabled");

                //$("#txtProjectCode").prop("disabled", "");
                //$("#txtProjectCode").prop("required", "required");

                //AmAlert("No Project code returned.");

                GetDCSServer(null);
            }
            else {

                //$(".cmbProjectCode").prop("disabled", "");
                //$(".cmbProjectCode").prop("required", "required");

                //$("#txtProjectCode").prop("required", "");
                //$("#txtProjectCode").prop("disabled", "disabled");


                //$(".cmbProjectCode").append('<option value="">--Select Project Code--</option>');
                //$.each(data, function (idx, item) {
                //    $(".cmbProjectCode").append('<option value="' + item.ProjectCode + '">' + item.ProjectCode + '</option>');
                //});
                //$(".cmbProjectCode").append('<option value="None of the above">None of the above</option>');
                //$(".cmbProjectCode").append('<option value="Don’t have one">Don’t have one</option>');
            }

            $(".cmbProjectCode").prop("disabled", "");
            $(".cmbProjectCode").prop("required", "required");

            $("#txtProjectCode").prop("required", "");
            $("#txtProjectCode").prop("disabled", "disabled");


            
            if (!IsNothing(data) && data.length > 0) {
                $(".cmbProjectCode").append('<option value="">--Select Project Code--</option>');
                $.each(data, function (idx, item) {
                    $(".cmbProjectCode").append('<option value="' + item.ProjectCode + '">' + item.ProjectCode + '</option>');
                });
                $(".cmbProjectCode").append('<option value="None of the above">None of the above</option>');
            }
            else {
                $(".cmbProjectCode").append('<option value="No Project code returned">No Project code returned</option>');
                $("#txtProjectCode").prop("disabled", "");
                $("#txtProjectCode").prop("required", "required");
                GetDCSServer(null);
            }
            $(".cmbProjectCode").append('<option value="Don’t have one">Don’t have one</option>');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            var err = $.parseJSON(jqXHR.responseText);
            AmAlert(err.Message + " while getting project code.");
            //HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Loading Project Code");
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

$(function () {

    //

    //*******************************************************************************
    $('#txtProjectCode').on('keyup', function (e) {

        if ($.inArray(e.keyCode, [20, 46, 8, 9, 27, 13, 110, 37, 39]) !== -1 ||
            // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
        (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+V
        (e.keyCode == 86 && e.ctrlKey === true) ||
            // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }

        var txt = $(this).val().trim().toUpperCase();

        //$(this).val(txt);

        if (txt.length == 1) {
            if ((/^P$/i.test(txt))) {
                //$(this).val(txt);
            }
            else {
                $(this).val('');
            }
        }
        else if (txt.length == 2) {
            if ((/^PC$/i.test(txt))) {
                //$(this).val(txt);
            }
            else {
                $(this).val('');
            }
        }
        else if (txt.length > 2) {
            if ((/^PC\d*$/i.test(txt))) {
                //$(this).val(txt);
            }
            else {
                $(this).val('');
            }
        }
    });

    $('#txtProjectCode').on('keydown', function (e) {
        //, 32 - Space, 37 - left arrow, 39 - right arrow
        if ($.inArray(e.keyCode, [20, 46, 8, 9, 27, 13, 110, 37, 39]) !== -1 ||
            // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
        (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+V
        (e.keyCode == 86 && e.ctrlKey === true) ||
            // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }

        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && (e.keyCode < 65 || e.keyCode > 90)) {
            e.preventDefault();
        }

        var txt = $(this).val().trim().toUpperCase();

        if (txt.length == 0) {
            if (e.keyCode != 80) {
                e.preventDefault();
            }
            else {
                //$(this).val(txt);
            }
        }
        else if (txt.length == 1) {
            if (e.keyCode != 67) {
                e.preventDefault();
            }
            else {
                //$(this).val(txt);
            }
        }
        else if (txt.length > 1) {
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
            else {
                //$(this).val(txt);
            }
        }
    });
    //*******************************************************************************

    //$("body").on("keydown", ".prjCode", (function (e) {

    //    //alert(e.keyCode);

    //    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 189, 190]) !== -1 ||
    //        // Allow: Ctrl+A
    //    (e.keyCode == 65 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+C
    //    (e.keyCode == 67 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+V
    //    (e.keyCode == 86 && e.ctrlKey === true) ||
    //        // Allow: home, end, left, right, down, up
    //    (e.keyCode >= 35 && e.keyCode <= 40)) {
    //        // let it happen, don't do anything
    //        return;
    //    }
    //    // Ensure that it is a number and stop the keypress
    //    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && (e.keyCode < 65 || e.keyCode > 90)) {
    //        e.preventDefault();
    //    }

    //})).on("keyup", ".prjCode", (function (e) {
    //    ///^\w+$/
    //    ///^\w*$/
    //    ///^[a-zA-Z0-9_]*$/
    //    alert("prjCode => keyup => " + $(this).val());
    //    if (/^[a-zA-Z0-9]+$/.test($(this).val())) {
    //        alert('Input is not alphanumeric');
    //        e.preventDefault();
    //        return false;
    //    }
    //}));

    //$('#txtProjectCode3').on('keydown', function (e) {
    //    //, 32 - Space
    //    if ($.inArray(e.keyCode, [20, 46, 8, 9, 27, 13, 110]) !== -1 ||
    //        // Allow: Ctrl+A
    //    (e.keyCode == 65 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+C
    //    (e.keyCode == 67 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+V
    //    (e.keyCode == 86 && e.ctrlKey === true) ||
    //        // Allow: home, end, left, right, down, up
    //    (e.keyCode >= 35 && e.keyCode <= 40)) {
    //        // let it happen, don't do anything
    //        return;
    //    }

    //    //if (e.ctrlKey || e.shiftKey) {
    //    //    return;
    //    //}

    //    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && (e.keyCode < 65 || e.keyCode > 90)) {
    //        e.preventDefault();
    //    }

    //    var txt = $(this).val().trim().toUpperCase();
    //    //alert(txt);

    //    if (txt.length == 0) {
    //        if (e.keyCode != 80) {
    //            e.preventDefault();
    //        }
    //        else {
    //            $(this).val(txt);
    //        }
    //    }
    //    else if (txt.length == 1) {
    //        if (e.keyCode != 67) {
    //            e.preventDefault();
    //        }
    //        else {
    //            $(this).val(txt);
    //        }
    //        //if ((/^P$/i.test(txt))) {
    //        //    $(this).val(txt);
    //        //}
    //        //else {
    //        //    e.preventDefault();
    //        //    $(this).val('');
    //        //}
    //    }
    //    else if (txt.length > 1) {
    //        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
    //            e.preventDefault();
    //        }
    //    }
    //    //else if (txt.length == 2) {
    //    //    if ((/^PC$/i.test(txt))) {
    //    //        $(this).val(txt);
    //    //    }
    //    //    else {
    //    //        $(this).val(txt.substring(0, txt.length - 1));
    //    //    }
    //    //}
    //    //    //else if (txt.length > 8) {
    //    //    //    alert(txt);
    //    //    //}
    //    //else if (txt.length > 2) {
    //    //    if ((/^PC\d*$/i.test(txt))) {
    //    //        $(this).val(txt);
    //    //    }
    //    //    else {
    //    //        e.preventDefault();
    //    //        return false;
    //    //        //$(this).val(txt.substring(0, txt.length - 1));
    //    //    }
    //    //}
    //    //alert((/^P$/i.test($(this).val())));
    //});

    //$('#txtProjectCode2').on('change keyup keydown', function (e) {

    //    if ($.inArray(e.keyCode, [32, 46, 8, 9, 27, 13, 110]) !== -1 ||
    //        // Allow: Ctrl+A
    //    (e.keyCode == 65 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+C
    //    (e.keyCode == 67 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+V
    //    (e.keyCode == 86 && e.ctrlKey === true) ||
    //        // Allow: home, end, left, right, down, up
    //    (e.keyCode >= 35 && e.keyCode <= 40)) {
    //        // let it happen, don't do anything
    //        return;
    //    }
    //    if (e.ctrlKey || e.shiftKey) {
    //        return;
    //    }
    //    var txt = $(this).val().toUpperCase();

    //    alert((/^P$/g.test(txt)));

    //    if (txt.length == 1) {
    //        if (!(/^P$/.test(txt))) {
    //            alert("gg");
    //            e.preventDefault();
    //            return false;
    //        }


    //        //$(this).val(/^P$/.test(txt) ? txt : txt.substring(0, txt.length - 1));
    //    }
    //    else if (txt.length == 2) {
    //        $(this).val(/^PC$/.test(txt) ? txt : txt.substring(0, txt.length - 1));
    //    }
    //    else if (txt.length > 2) {
    //        $(this).val(/^PC\d*$/.test(txt) & txt.length < 10 ? txt : txt.substring(0, txt.length - 1));
    //    }
    //    //$(this).val(/^PC\d*$/.test(txt) & txt.length < 9 ? txt : txt.substring(0, txt.length - 1));
    //});

    //$("#txtProjectCode").keydown((function (e) {
    //    //alert(e.keyCode);
    //    //32, space
    //    //190, .
    //    //, 189 -_
    //    if ($.inArray(e.keyCode, [32, 46, 8, 9, 27, 13, 110]) !== -1 ||
    //        // Allow: Ctrl+A
    //    (e.keyCode == 65 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+C
    //    (e.keyCode == 67 && e.ctrlKey === true) ||
    //        // Allow: Ctrl+V
    //    (e.keyCode == 86 && e.ctrlKey === true) ||
    //        // Allow: home, end, left, right, down, up
    //    (e.keyCode >= 35 && e.keyCode <= 40)) {
    //        // let it happen, don't do anything
    //        return;
    //    }
    //    // Ensure that it is a number and stop the keypress
    //    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && (e.keyCode < 65 || e.keyCode > 90)) {
    //        e.preventDefault();
    //    }

    //}));

    //$("#txtProjectCode").keyup((function (e) {
    //    ///^[a-zA-Z0-9_]*$/
    //    //if (!/^[a-zA-Z0-9- ]*$/.test($(this).val())) {
    //    //if (!/^[a-zA-Z0-9]*$/.test($(this).val())) {
    //    if (!/^[P]{1}[C]{1}[0-9]{7}$/.test($(this).val())) {
    //        $(this).val("");
    //        e.preventDefault(); 
    //        return false;
    //    } 
    //}));

    //


    if ($("#surveyRequestType").val().IsMatch("RF")) {

        $("#txtProjectCode").prop("disabled", "disabled");

        GetPrjCode(viewModel.selectedWs().WsProfile().EngNum(), viewModel.selectedWs().WsProfile().Client());

        $("#txtProjectCode").change(function () {

            //alert("change");

            if (IsNothing($(this).val())) {

                if ($("#cmbProjectCode").children('option').length == 0) {

                    //$("#txtProjectCode").prop("disabled", "");
                    //$("#txtProjectCode").prop("required", "required");

                }
                else {
                    $(".cmbProjectCode").prop("disabled", "");
                    $(".cmbProjectCode").prop("required", "required");
                }

                GetDCSServer(null);
            }
            else {

                if ((/^PC\d{7}$/i.test($(this).val()))) {

                    $(".cmbProjectCode").prop("required", "");
                    $(".cmbProjectCode").prop("disabled", "disabled");

                    GetDCSServer($(this).val());
                }
                else {
                    AmAlert("Invalid project code, Please enter project code in the format PCXXXXXXX");
                }
            }
        });

        //onpropertychange
        //$("#txtProjectCode").onpropertychange(function () {

        //    alert("onpropertychange");

        //    if (IsNothing($(this).val())) {

        //        if ($("#cmbProjectCode").children('option').length == 0) {

        //            //$("#txtProjectCode").prop("disabled", "");
        //            //$("#txtProjectCode").prop("required", "required");

        //        }
        //        else {
        //            $(".cmbProjectCode").prop("disabled", "");
        //            $(".cmbProjectCode").prop("required", "required");
        //        }

        //        GetDCSServer(null);
        //    }
        //    else {

        //        if ((/^PC\d{7}$/i.test($(this).val()))) {

        //            $(".cmbProjectCode").prop("required", "");
        //            $(".cmbProjectCode").prop("disabled", "disabled");



        //            GetDCSServer($(this).val());
        //        }
        //        else {
        //            AmAlert("Invalid project code, Please enter project code in the format PCXXXXXXX");
        //        }
        //    }
        //});

        $("#cmbProjectCode").change(function () {

            $("#txtProjectCode").val("");
            $(".cmbDCSServer").val("");

            if (IsNothing($(this).val())) {
                $("#txtProjectCode").prop("required", "");
                $("#txtProjectCode").prop("disabled", "disabled");
                GetDCSServer(null);
            }
            else if ($(this).val().IsMatch("None of the above")) {
                $("#txtProjectCode").prop("disabled", "");
                $("#txtProjectCode").prop("required", "required");
                GetDCSServer(null);
            }
                //No Project code returned
            else if ($(this).val().IsMatch("No Project code returned")) {
                $("#txtProjectCode").prop("disabled", "");
                $("#txtProjectCode").prop("required", "required");
                GetDCSServer(null);
            }
                //
            else if ($(this).val().IsMatch("Don’t have one")) {
                
                //$("#txtProjectCode").val("");
                //$(".cmbDCSServer").val("");

                $("#txtProjectCode").prop("required", "");
                $("#txtProjectCode").prop("disabled", "disabled");

                GetDCSServer(null);
            }
            else {
                $("#txtProjectCode").prop("required", "");
                $("#txtProjectCode").prop("disabled", "disabled");
                GetDCSServer($("#cmbProjectCode").val());
            }

            //$(".cmbDCSServer").prop("disabled", "");
            //$(".cmbDCSServer").empty();
        });

        //$("#chkProjectCode").on("click", function () {

        //    $(".cmbProjectCode").val("");
        //    $("#txtProjectCode").val("");
        //    $(".cmbDCSServer").val("");

        //    if ($(this).prop("checked")) {

        //        //$(".cmbProjectCode").val("");
        //        //$("#txtProjectCode").val("");

        //        $(".cmbProjectCode").prop("required", "");
        //        $("#txtProjectCode").prop("required", "");

        //        $(".cmbProjectCode").prop("disabled", "disabled");
        //        $("#txtProjectCode").prop("disabled", "disabled");

        //        GetDCSServer(null);
        //    }
        //    else {

        //        //$(".cmbDCSServer").empty();
        //        //$(".cmbDCSServer").prop("required", "");
        //        //$(".cmbDCSServer").prop("disabled", "disabled");

        //        if ($("#cmbProjectCode").children('option').length == 0) {
        //            $("#txtProjectCode").prop("disabled", "");
        //            $("#txtProjectCode").prop("required", "required");
        //        }
        //        else {
        //            $(".cmbProjectCode").prop("disabled", "");
        //            $(".cmbProjectCode").prop("required", "required");
        //        }
        //    }
        //});

    }

    $("input[name=radKPMGContEnt]:radio").on('change', function () {

        switch ($(this).val()) {
            case 'Yes':
                $("#txtADesc").prop("required", true);
                //$("#radKPMGContEnt_Q3").show();
                //alert("show");
                break;
            case 'No':
                $("#txtADesc").prop("required", false);
                //$("#radKPMGContEnt_Q3").hide();
                //alert("hide");
                break;
        }
    });


    //$("#frmSurveyRequest").removeData("validator")
    //$("#frmSurveyRequest").removeData("unobtrusiveValidation");

    //alert("focus");
    //$('#txtBusUnit').focus();

    $("#frmSurveyRequest").validate({

        //errorPlacement: function (error, element) {
        //    // if the input has a prepend or append element, put the validation msg after the parent div
        //    if (element.parent().hasClass('input-prepend') || element.parent().hasClass('input-append')) {
        //        error.insertAfter(element.parent());
        //        // else just place the validation message immediatly after the input
        //    } else {
        //        error.insertAfter(element);
        //    }
        //},
        //errorElement: "small", // contain the error msg in a small tag
        //wrapper: "div", // wrap the error message and small tag in a div
        //highlight: function (element) {
        //    $(element).closest('.form-group').addClass('error'); // add the Bootstrap error class to the control group
        //},
        //success: function (element) {
        //    $(element).closest('.form-group').removeClass('error'); // remove the Boostrap error class from the control group
        //}

        errorPlacement: function (error, element) {
            //alert(error);
            // if the input has a prepend or append element, put the validation msg after the parent div
            if (element.parent().parent().hasClass('input-prepend') || element.parent().parent().hasClass('input-append')) {
                error.insertAfter(element.parent().parent());
                element.parent().parent().addClass("float-left");
                error.addClass("margin-Lt-150");
                // else just place the validation message immediatly after the input
            } else {
                error.insertAfter(element);
            }
        },
        errorElement: "mark", // contain the error msg in a small tag
        wrapper: "div", // wrap the error message and small tag in a div
        highlight: function (element) {
            $(element).closest('.form-group').addClass('error'); // add the Bootstrap error class to the control group
        },
        success: function (element) {
            $(element).closest('.form-group').removeClass('error'); // remove the Boostrap error class from the control group
        }

    });

    $('#frmSurveyRequest').submit(function (e) {

        e.preventDefault();
        var $form = $(this);

        // check if the input is valid
        if (!$form.valid()) return false;

        if ($("#surveyRequestType").val().IsMatch("RF")) {

            var hid_projectCode_val = "";
            //if ($("#chkProjectCode").prop("checked")) {
            if ($("#cmbProjectCode").val().IsMatch("Don’t have one")) {
                hid_projectCode_val = "Don’t have one";
            }
            else {
                //.IsMatch("NotSelected")
                //None of the above
                //hid_projectCode_val = IsNothing($("#cmbProjectCode").val()) ? $("#txtProjectCode").val() : $("#cmbProjectCode").val();
                //hid_projectCode_val = $('#cmbProjectCode').is(':disabled') ? $("#txtProjectCode").val() : $("#cmbProjectCode").val();

                if ($('#cmbProjectCode').is(':disabled')) {
                    if (!IsNothing($("#txtProjectCode").val())) {
                        hid_projectCode_val = $("#txtProjectCode").val();
                    }
                }
                else {
                    //No Project code returned
                    if ($("#cmbProjectCode").val().IsMatch("None of the above") || $("#cmbProjectCode").val().IsMatch("No Project code returned")) {
                        if (IsNothing($("#txtProjectCode").val())) {
                            //hid_projectCode_val = "None of the above";
                        }
                        else {
                            hid_projectCode_val = $("#txtProjectCode").val();
                        }
                    }
                    else {
                        hid_projectCode_val = $("#cmbProjectCode").val();
                    }
                }

            }

            if (IsNothing(hid_projectCode_val)) {
                AmAlert("Project Code is required.");
                return false;
            }
            else {
                $("#hid_projectCode").val(hid_projectCode_val);
            }

            if (isValid($("#txtPrimWBName").val())) {

            }
            else {
                AmAlert("Invalid primary workbook name, name should not contain [\ / : * ? \" < > |].");
                return false;
            }
        }

        // Find disabled inputs, and remove the "disabled" attribute
        var disabled = $form.find(':input:disabled').removeAttr('disabled');

        // serialize the form
        var serialized = $form.serialize();

        //alert(serialized);

        // re-disabled the set of inputs that you previously enabled
        disabled.attr('disabled', 'disabled');

        $.ajax(
        {
            type: 'POST',
            url: '/api/Workspace/PostSurveyRequest',
            headers: {
                'RequestVerificationToken': GetTokenHeaderValue()
            },
            dataType: 'json',
            data: serialized,
            success: function (data, textStatus, xhr) {

                //alert(IsNothing(updateObj_Reprocess));

                if (!IsNothing(updateObj_Reprocess)) {

                    if (updateObj_Reprocess.IsPage_Act)
                        PostUpdateFileActivity_Act(updateObj_Reprocess, updateObj_Reprocess.EngNum, null);
                    else
                        PerformDeleteFinal(updateObj_Reprocess.wsId, updateObj_Reprocess.docObjId, updateObj_Reprocess.comment, false);

                    updateObj_Reprocess = "";

                    HandleSuccess("PostSurveyRequest-Reprocess", "");
                }
                else {
                    HandleSuccess("PostSurveyRequest-" + $("#surveyRequestType").val(), "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                HandleError(jqXHR, textStatus, errorThrown);
            },
            beforeSend: function () {
                myApp.showPleaseWait("Submitting " + $("#surveyRequestType").val() + " - " + viewModel.selectedFile().Number());
            },
            complete: function () {
                //alert("complete");
                myApp.hidePleaseWait();
            }
        });
    })
});
