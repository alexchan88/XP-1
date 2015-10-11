function Draw_kGrid_Access_Help() {

    $("#kGrid_Access_Help").kendoGrid({
        dataSource: {
            data: usrAccessRights,
        },


        dataBound: function (e) {
            //alert('kGrid_Access_Help');
            SetGridSize('kGrid_Access_Help');
        },
        sortable: true,
        scrollable: true,

        columns: [
        {
            title: 'Action',
            field: 'Action',
            width: GetGridColWidth(gridColWidth.PT50, 'kGrid_Access_Help'),
        },

        {
            title: 'Administrators',
            field: 'Administrators',
        },
        {
            title: 'Read/Write',
            field: 'ReadWrite',
        },
        {
            title: 'Read Only',
            field: 'ReadOnly',
        },
        ]

    });
}

function Draw_kGrid_S2GuidMap_Search(data) {

    $("#kGrid_S2GuidMap_Search").kendoGrid({
        dataSource: {
            data: data,
        },


        dataBound: function (e) {
            //alert('kGrid_S2GuidMap_Search');
            SetGridSize('kGrid_S2GuidMap_Search');
        },
        sortable: true,
        scrollable: false,
        //navigatable: false,

        columns: [
            {
                title: "Workbook Name",
                field: "WorkbookName",
                template: "<p>#=GetLongTextToolTip(WorkbookName, 70)#</p>",
                width: GetGridColWidth(gridColWidth.PT60, 'kGrid_S2GuidMap_Search'),
            },
            {
                title: "Status",
                field: "WBStatus",
                width: GetGridColWidth(gridColWidth.FIXED, 'kGrid_S2GuidMap_Search'),
            },
        ]
    });

    $("#spanS2SearchResultGuid").text($("#txtMafGuid").val());
    $("#txtMafFileName").val(data[0].FileName);

    $("#btnLinkGuidAdd").click(function () {
        LinkMAF(this);
    });

    $("#btnLinkGuidCancel").click(function () {
        CloseMyDialog(this);
    });
}

function engNumFilter(element) {
}

function statusFilter(element) {
}

function Draw_kGrid_Eng_Activity() {

    $("#kGrid_Eng_Activity").kendoGrid({
        dataSource: {
            async: false,
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'GET',
                        //url: '/api/Workspace/GetFileActivity?fDate=' + GetDefaultDate() +
                        //    '&tDate=' + (new Date()).ToDate() + '&activityFilterType=' + "ALL" + '&engNum=' + viewModel.selectedWs().WsProfile().EngNum(),
                        url: '/api/Workspace/GetFileActivity?fDate=' + (new Date('01/01/1999')).ToDate() +
                            '&tDate=' + (new Date()).ToDate() + '&activityFilterType=' + "ALL" + '&engNum=' + viewModel.selectedWs().WsProfile().EngNum(),
                        headers: {
                            'RequestVerificationToken': GetTokenHeaderValue()
                        },
                        success: function (data, textStatus, xhr) {
                            options.success(data);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            HandleError(jqXHR, textStatus, errorThrown);
                        },
                        beforeSend: function () {
                            myApp.showPleaseWait("Retrieving Activity.");
                        },
                        complete: function () {
                            myApp.hidePleaseWait();
                        }
                    });
                }
            }
        },
        sortable: true,
        scrollable: true,
        filterable: {
            extra: false
        },
        dataBound: function (e) {

            $("#kGrid_Eng_Activity .k-pager-info").empty();
            $("#kGrid_Eng_Activity .k-pager-info").append($('#kGrid_Eng_Activity').data('kendoGrid').dataSource.total() + " items");

            resizeGrid("kGrid_Eng_Activity");
        },
        pageable: {
            numeric: false,
            previousNext: false,
            refresh: true,
            pageSizes: false,
            messages: {
                display: "{2} items",
            }
        },
        columns: [
        {
            title: 'File Type',
            field: 'RequestType',
            template: '#=RequestType.FileType2()#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Eng_Activity'),
            filterable: false,
        },
        {
            title: 'Status',
            field: 'Status',
            template: '#=GetDocumentStatus(DocumentStatus)#',
            width: GetGridColWidth(gridColWidth.PT15, 'kGrid_Eng_Activity'),
            filterable: {
                ui: statusFilter,
                operators: {
                    string: { contains: "Contains" }
                }
            }
        },
        {
            title: 'RET/RF File Name for Review',
            field: 'FileName',
            template: '#=GetLongTextToolTip(FileName, 15)#',
            width: GetGridColWidth(gridColWidth.PT15, 'kGrid_Eng_Activity'),
            filterable: false,
        },
        {
            title: 'Workbook Name',
            field: 'WorkBookName',
            template: '#=GetLongTextToolTip(WorkBookName, 15)#',
            width: GetGridColWidth(gridColWidth.PT15, 'kGrid_Eng_Activity'),
            filterable: false,
        },
        {
            title: 'eAudIT Engagement File Name',
            field: 'EngagementFileName',
            template: '#=GetLongTextToolTip(EngagementFileName, 15)#',
            width: GetGridColWidth(gridColWidth.PT15, 'kGrid_Eng_Activity'),
            filterable: false,
        },
        {
            title: 'Request',
            field: 'NSTID',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Eng_Activity'),
            filterable: false,
        },
        {
            title: '',
            template: '#=GetFileMenu_Act(DocumentStatus, RequestType.FileType(), \'kGrid_Eng_Activity\')#',
            width: GetGridColWidth(gridColWidth.FIXED, 'kGrid_Eng_Activity'),
            filterable: false,
        },
        ]
    });

}

function Draw_kGrid_Activity() {

    var usrId = $("#txtUserId").val();

    usrId = IsNothing(usrId) ? "" : usrId;

    $("#kGrid_Activity").kendoGrid({
        dataSource: {
            async: false,
            transport: {
                read: function (options) {
                    $.ajax({
                        type: 'GET',
                        url: '/api/Workspace/GetFileActivity?fDate=' + $("#txtFrom").val() +
                            '&tDate=' + $("#txtTo").val() + '&activityFilterType=' + $("#txtRequestType").val() + '&usrId=' + usrId,
                        headers: {
                            'RequestVerificationToken': GetTokenHeaderValue()
                        },
                        success: function (data, textStatus, xhr) {
                            options.success(data);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            HandleError(jqXHR, textStatus, errorThrown);
                        },
                        beforeSend: function () {
                            myApp.showPleaseWait("Retrieving Activity.");
                        },
                        complete: function () {
                            myApp.hidePleaseWait();
                        }
                    });
                }
            }
            , filter: { field: "Status", operator: "contains", value: "Pending Approval" },
        },
        sortable: true,
        scrollable: true,
        filterable: {
            extra: false
        },
        dataBound: function (e) {
            $("#lblNoPendingApproval").empty();
            var filter = this.dataSource.filter();
            if (!IsNothing(filter)) {
                for (var i = 0; i < filter.filters.length; i++) {
                    if (filter.filters[i].field.IsMatch("Status")) {
                        if (filter.filters[i].value.IsMatch("Pending Approval")) {
                            if ($('#kGrid_Activity').data('kendoGrid').dataSource.total() == 0) {
                                $("#lblNoPendingApproval").append("There are no Requests Pending Approval");
                            }
                        }
                    }
                }
                $("#btnShowAll").empty();
                $("#btnShowAll").append("Show All");
            }

            $("#kGrid_Activity .k-pager-info").empty();
            $("#kGrid_Activity .k-pager-info").append($('#kGrid_Activity').data('kendoGrid').dataSource.total() + " items");

            resizeGrid("kGrid_Activity");
        },
        pageable: {
            numeric: false,
            previousNext: false,
            refresh: true,
            pageSizes: false,
            messages: {
                display: "{2} items",
            }
        },
        columns: [

        {
            title: 'Engagement',
            field: 'EngagementNumber',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: {
                ui: engNumFilter,
                operators: {
                    string: { contains: "Contains" }
                }
            }
        },
        {
            title: 'Engagement Name',
            field: 'EngagementName',
            template: '#=GetLongTextToolTip(EngagementName, 12)#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: false,
        },
        {
            title: 'File Type',
            field: 'RequestType',
            template: '#=RequestType.FileType2()#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: false,
        },

        {
            title: 'Status',
            field: 'Status',
            template: '#=GetDocumentStatus(DocumentStatus)#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: {
                ui: statusFilter,
                operators: {
                    string: { contains: "Contains" }
                }
            }
        },
        {
            title: 'RET/RF File Name for Review',
            field: 'FileName',
            template: '#=GetLongTextToolTip(FileName, 12)#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: false,
        },
        {
            title: 'DocNum',
            field: 'DocNum',
            width: GetGridColWidth(gridColWidth.PT05, 'kGrid_Activity'),
            filterable: false,
        },
        {
            title: 'Workbook Name',
            field: 'WorkBookName',
            template: '#=GetLongTextToolTip(WorkBookName, 12)#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: false,
        },
        {
            title: 'eAudIT Engagement File Name',
            field: 'EngagementFileName',
            template: '#=GetLongTextToolTip(EngagementFileName, 12)#',
            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_Activity'),
            filterable: false,
        },
        {
            title: 'Request',
            field: 'NSTID',
            width: GetGridColWidth(gridColWidth.PT05, 'kGrid_Activity'),
            filterable: false,
        },
        {
            title: '',
            template: '#=GetFileMenu_Act(DocumentStatus, RequestType.FileType(), \'kGrid_Activity\')#',
            width: GetGridColWidth(gridColWidth.FIXED, 'kGrid_Activity'),
            filterable: false,
        },
        ]
    });
}

function Draw_kGrid_S2GuidMap_MappedGuid() {

    $.ajax(
       {
           type: 'GET',
           url: '/api/WsS2Guid/GetMapped_WsS2Guid_ForEng?engNum=' + viewModel.selectedWs().WsProfile().EngNum(),
           headers: {
               'RequestVerificationToken': GetTokenHeaderValue()
           },
           success: function (data, textStatus, xhr) {

               $("#kGrid_S2GuidMap_MappedGuid").kendoGrid({
                   dataSource: {
                       data: data,
                   },

                   dataBound: function (e) {
                       //alert('kGrid_S2GuidMap_MappedGuid');
                       SetGridSize('kGrid_S2GuidMap_MappedGuid');
                   },
                   sortable: true,
                   scrollable: true,
                   //navigatable: false,
                   columns: [

                        {
                            field: "MAFGuid",
                            title: "eAudIT Engagement ID",
                            width: GetGridColWidth(gridColWidth.PT30, 'kGrid_S2GuidMap_MappedGuid'),
                        },
                        {
                            field: "FileName",
                            template: "<p>#=GetLongTextToolTip(FileName, 15)#</p>",
                            title: "MAF Name",
                            width: GetGridColWidth(gridColWidth.PT20, 'kGrid_S2GuidMap_MappedGuid'),
                        },
                        {
                            field: "UpdatedBy",
                            title: "Linked by",
                            width: GetGridColWidth(gridColWidth.PT15, 'kGrid_S2GuidMap_MappedGuid'),
                        },
                        {
                            field: "UpdateDate",
                            template: "#=UpdateDate.ToDate()#",
                            title: "Linked on",
                            width: GetGridColWidth(gridColWidth.PT10, 'kGrid_S2GuidMap_MappedGuid'),
                        },
                        {
                            command:
                                [
                                    {
                                        name: "Request to remove link",
                                        click: function (e) {

                                            if (CanTakeAction("DeLink Guid", WhoCanAct.ADMIN_N_MEMBERS, AmMsg.CantTakeAction_Ws_Msg)) {
                                                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                                AmConfirm(AmConfirmMsg.s2GuidDelinkMsg, DeLinkMAF, null, dataItem.MasterAuditFileId);
                                            }

                                        },
                                    },
                                ],
                            width: GetGridColWidth(gridColWidth.PT15, 'kGrid_S2GuidMap_MappedGuid'),
                        },
                   ]
               });

           },
           error: function (jqXHR, textStatus, errorThrown) {
               HandleError(jqXHR, textStatus, errorThrown);
           },
           beforeSend: function () {
               myApp.showPleaseWait("Retrieving Mapped MAF.");
           },
           complete: function () {
               myApp.hidePleaseWait();
           }
       });
}

function Draw_kGrid_S2GuidMap_NonClosedWorkbook() {

    $.ajax(
    {
        type: 'GET',
        url: '/api/Workspace/GetS2NonClosedWb?engNum=' + viewModel.selectedWs().WsProfile().EngNum(),
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data, textStatus, xhr) {

            if (data === null || data.length == 0) {
                CheckIfRETExists();
            }
            else {

                AmConfirm(AmConfirmMsg.s2NonClosedWbGrid_Html, CheckIfRETExists, null, null);

                $("#kGrid_S2GuidMap_NonClosedWorkbook").kendoGrid({
                    dataSource: {
                        data: data,
                    },

                    dataBound: function (e) {
                        //alert('kGrid_S2GuidMap_NonClosedWorkbook');
                        SetGridSize('kGrid_S2GuidMap_NonClosedWorkbook');
                    },
                    sortable: true,
                    scrollable: false,
                    //navigatable: false,
                    columns: [
                        {
                            field: "WorkbookName",
                            template: "<p>#=GetLongTextToolTip(WorkbookName, 50)#</p>",
                            title: "Workbook Name",
                            width: GetGridColWidth(gridColWidth.PT50, 'kGrid_S2GuidMap_NonClosedWorkbook'),
                        },
                        {
                            field: "Status",
                            title: "Workbook Status",
                            width: GetGridColWidth(gridColWidth.PT40, 'kGrid_S2GuidMap_NonClosedWorkbook'),
                        },
                    ]
                });

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Validating Workbook - " + viewModel.selectedWs().Name());
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

function Draw_kGrid_Access_SearchUser() {

    var lName = $("#txtLName").val().trim();
    var fName = $("#txtFName").val().trim();
    var email = $("#txtEmail").val().trim();

    if (IsNothing(lName) && IsNothing(fName) && IsNothing(email)) {
        AmAlert("Please enter Last Name Or First Name Or Email to do the search.");
        return;
    }
    else if (IsNothing(email) && IsNothing(lName)) {
        AmAlert("Last Name is required to do the search.");
        return;
    }

    var searchStr = '';
    var usrSearchBy = '';

    if ($("#txtLName").prop('disabled')) {
        usrSearchBy = 'Email';

        searchStr = email;

    }
    else {
        usrSearchBy = 'Name';
        if (IsNothing(lName))
            searchStr = fName;
        else if (IsNothing(fName))
            searchStr = lName;
        else
            searchStr = lName + ', ' + fName;
    }

    $.ajax(
        {
            type: 'Get',
            async: false,
            url: '/api/WsUsrMgmt/GetSearchUsr?searchStr=' + searchStr + '&usrSearchBy=' + usrSearchBy + '&isExactSrch=' + $("#chkExactSrch").is(":checked") + '&imDbType=' + 'Active',
            headers: {
                'RequestVerificationToken': GetTokenHeaderValue()
            },
            success: function (data, textStatus, xhr) {

                $("#kGrid_Access_SearchUser").kendoGrid({
                    dataSource: {
                        data: data,
                    },

                    dataBound: function (e) {
                        //alert('kGrid_Access_SearchUser');
                        SetGridSize('kGrid_Access_SearchUser');
                    },
                    sortable: true,
                    scrollable: true,
                    //navigatable: false,
                    columns: [

                         {
                             field: "FullName",
                             template: "#=GetUsrName(FullName, UsrNameType.Last)#",
                             title: "Last Name",
                             width: GetGridColWidth(gridColWidth.PT20, 'kGrid_Access_SearchUser'),
                         },
                         {
                             field: "FullName",
                             template: "#=GetUsrName(FullName, UsrNameType.First)#",
                             title: "First Name",
                             width: GetGridColWidth(gridColWidth.PT20, 'kGrid_Access_SearchUser'),
                         },
                         {
                             field: "Email",
                             title: "Email",
                             width: GetGridColWidth(gridColWidth.PT35, 'kGrid_Access_SearchUser'),
                         },
                         {
                             command:
                                 [
                                     {
                                         name: "Add",
                                         click: function (e) {

                                             var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

                                             var match = ko.utils.arrayFirst(viewModel.selectedWs().WsGroups()(), function (item) {
                                                 return selectedGrp === item.Name();
                                             });

                                             if (match != null) {

                                                 var match2 = ko.utils.arrayFirst(match.GrpUsers(), function (item) {
                                                     return dataItem.Name === item.Name();
                                                 });

                                                 if (match2 != null) {

                                                     AmAlert("User " + dataItem.Name + " is already a member of the " + UsrGrpNameToDisplay(selectedGrp) + ".");

                                                     return;
                                                 }
                                             }

                                             AmConfirm("Are you sure you want to add " + dataItem.Name + " to the " + UsrGrpNameToDisplay(selectedGrp) + "?", AddUsrToGrp, null, dataItem.Name, null);

                                         },
                                     },
                                 ],
                             width: GetGridColWidth(gridColWidth.PT15, 'kGrid_Access_SearchUser'),
                         },
                    ]
                });

            },
            error: function (jqXHR, textStatus, errorThrown) {
                HandleError(jqXHR, textStatus, errorThrown);
            },
            beforeSend: function () {
                myApp.showPleaseWait("Searching User - ");
            },
            complete: function () {
                myApp.hidePleaseWait();
            }
        });

}


function Draw_kGrid_RequestAccess_AdminUser(engNum) {

    $.ajax(
        {
            type: 'Get',
            async: true,
            url: '/api/Workspace/GetEngByEngNum?engNum=' + engNum + '&wsLoadType=Groups&isAdmin=true',
            headers: {
                'RequestVerificationToken': GetTokenHeaderValue()
            },
            success: function (data, textStatus, xhr) {

                var adminUser = "";

                $.each(data[0].WsGroups, function (idx, item) {
                    if (item.Name.match(/E_ADMIN/i)) {
                        adminUser = item.GrpUsers;
                    }
                })

                $("#kGrid_RequestAccess_AdminUser").kendoGrid({
                    dataSource: {
                        data: adminUser,
                    },

                    dataBound: function (e) {
                        SetGridSize('kGrid_RequestAccess_AdminUser');
                        ResizeDialog();
                        //ResizeGrid2();
                    },
                    sortable: true,
                    scrollable: true,
                    //navigatable: false,
                    columns: [

                         {
                             field: "FullName",
                             title: "Name",
                             width: GetGridColWidth(gridColWidth.PT40, 'kGrid_RequestAccess_AdminUser'),
                         },
                         {
                             field: "Email",
                             title: "Email",
                             width: GetGridColWidth(gridColWidth.PT40, 'kGrid_RequestAccess_AdminUser'),
                         },
                          {
                              title: '&nbsp;&nbsp;<input id=\'chkAllEmailAdminUsr\' name=\'chkAllEmailAdminUsr\' type=\'checkbox\' class=\'chkAllEmailAdminUsr\' title=\'Select or deselect All\' >',
                              template: '<input id=\'chkEmailAdminUsr\' name=\'chkEmailAdminUsr\'  class=\'chkEmailAdminUsr\' type=\'checkbox\' value=\'#=Email#\' >',
                              headerAttributes: { style: 'text-align: center;' },
                              attributes: { style: 'text-align: center;' },
                              width: GetGridColWidth(gridColWidth.PT10, 'kGrid_RequestAccess_AdminUser'),
                          },
                    ]
                });



            },
            error: function (jqXHR, textStatus, errorThrown) {
                HandleError(jqXHR, textStatus, errorThrown);
            },
            beforeSend: function () {
                myApp.showPleaseWait("Getting members of the workspace Administrators group");
            },
            complete: function () {
                myApp.hidePleaseWait();
            }
        });

}