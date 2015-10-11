var isInEdit = false;

var resetViewModel = function (vm) {

    //alert("resetViewModel");

    //vm.treeRoot(null);
    vm.selectedWs(null);
    vm.selectedFldr(null);
    vm.selectedFile(null);

    vm.currentActivity(null);

    vm.currentDataItem(null);
    vm.currentGrid(null);
    vm.selectedWsId(null);
    vm.toReloadSelectedWsId(false);

    $(".chkEngFldr").prop("checked", false);
    $(".activeTreeItem").removeClass("activeTreeItem");

    //alert(JSON.stringify(ko.mapping.toJS(vm.selectedWs())));
    //alert(JSON.stringify(ko.mapping.toJS(viewModel.selectedWs())));

    $("#myEngTab").tabs("option", "active", 0);
    $("#myEngTab").tabs("disable");
}

var viewModel = {

    originalTree: ko.observableArray(),

    treeRoot: ko.observableArray(),
    selectedWs: ko.observable(),
    selectedFldr: ko.observable(),
    selectedFile: ko.observable(),

    searchStr: ko.observable(),

    //wsActivity: ko.observableArray(),

    currentActivity: ko.observable(),

    currentDataItem: ko.observable(),
    currentGrid: ko.observable(),

    selectedWsId: ko.observable(),
    toReloadSelectedWsId: ko.observable(false),

    treeTemplate: function (item) {
        return "treeElementWChild_2";
    },

    tp_Ques: ko.observable(),

    availableRetPolicy: ko.observableArray(['10YEARS', '11YEARS', '12YEARS',
    '13YEARS', '14YEARS', '15YEARS',
    '1YEAR', '20YEARS', '25YEARS',
    '2YEARS', '30YEARS', '3YEARS',
    '4YEARS', '5YEARS', '6YEARS',
    '7YEARS', '8YEARS', '9YEARS',
    'HOLD', 'PERMANENT']),
};

ko.bindingHandlers.date = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        $(element).change(function () {
            var value = valueAccessor();
            value($(element).val());
        })
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {

        var value = valueAccessor();
        var date = ko.unwrap(value);
        //var date = value();

        if (IsNothing(date)) {
            $(element).val(date);
        }
        else {
            var strDate = date.ToDate();
            $(element).val(strDate);
        }
    }
};

var getAllFiles = function all(fldrItem, allF) {

    $.each(fldrItem, function (idxFldr, fItem) {

        //alert(JSON.stringify(ko.mapping.toJS(fItem)));
        if (!(fItem.FolderPath().indexOf(wsLog) > -1)) {
            $.each(fItem.WsFiles(), function (idxFile, fileItem) {

                if (ToBindFileInGrid(fileItem)) {
                    allF.push(fileItem);
                }

            })

            all(fItem.WsFldrs(), allF);
        }
    })
}

viewModel.allFiles = ko.computed(function () {
    var allF = ko.observableArray();
    getAllFiles(this.selectedWs().WsFldrs()(), allF);
    return allF;
}, viewModel, { deferEvaluation: true });

viewModel.selectedFldrFiles = ko.computed(function () {
    var allF = ko.observableArray();

    //alert(this.selectedFldr());

    if (!IsNothing(this.selectedFldr())) {
        $.each(this.selectedFldr().WsFiles(), function (idxFile, fileItem) {

            if (ToBindFileInGrid(fileItem)) {
                allF.push(fileItem);
            }
        })
    }

    return allF;

}, viewModel, { deferEvaluation: true });

//viewModel.kPMGOnly = ko.computed(function (kPMGOnly) {
//    //alert(kPMGOnly);
//    return "Yes";
//}, viewModel, { deferEvaluation: true });

ko.observable.fn.BoolToChar = function (kPMGOnly) {
    return kPMGOnly.ToChar();
};

function GetEngByEngNum(engNum, wsLoadType, isAdmin, isFromSrch) {

    $.ajax(
    {
        type: 'GET',
        async: false,
        url: '/api/Workspace/GetEngByEngNum?engNum=' + engNum + '&wsLoadType=' + wsLoadType + '&isAdmin=' + isAdmin,
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data, textStatus, xhr) {

            if (IsNothing(data)) {
                AmAlert(AmMsg.WS_Dont_Exists_4_WSNum);
                myApp.hidePleaseWait();
                return;
            }

            if (wsLoadType.IsMatch("ALL")) {

                var rootData = data;
                var mappedChildItem = ko.mapping.fromJS(data[0]);

                rootData[0].IsLoaded = false;
                rootData[0].WsProfile = null;
                rootData[0].WsFldrs = null;
                rootData[0].WsGroups = null;

                var rootDataMap = ko.mapping.fromJS(rootData[0]);

                //viewModel.treeRoot.push(rootDataMap);

                //var match = ko.utils.arrayFirst(viewModel.treeRoot(), function (item) {
                //    return data[0].ObjectID === item.ObjectID();
                //});

                var match = "";

                match = ko.utils.arrayFirst(viewModel.treeRoot(), function (item) {
                    return data[0].ObjectID === item.ObjectID();
                });

                //alert(IsNothing(match));

                if (IsNothing(match)) {
                    viewModel.treeRoot.push(rootDataMap);
                    match = ko.utils.arrayFirst(viewModel.treeRoot(), function (item) {
                        return data[0].ObjectID === item.ObjectID();
                    });
                }

                var idx = viewModel.treeRoot().indexOf(match);

                viewModel.treeRoot()[idx].WsProfile(mappedChildItem.WsProfile);
                viewModel.treeRoot()[idx].WsFldrs(mappedChildItem.WsFldrs);
                viewModel.treeRoot()[idx].WsGroups(mappedChildItem.WsGroups);

                viewModel.treeRoot()[idx].IsLoaded(mappedChildItem.IsLoaded);

                viewModel.selectedWs(viewModel.treeRoot()[idx]);
            }
            else if (wsLoadType.IsMatch("None")) {
                var mappedRoot = ko.mapping.fromJS(data);
                viewModel.treeRoot(mappedRoot());
            }

            //var mappedData = ko.utils.arrayMap(viewModel.treeRoot(), function (item) {
            //    return item.Name().split('-').pop().trim();
            //});

            //$("#engSrch").autocomplete({
            //    source: mappedData,
            //    change: function (event, ui) {

            //        viewModel.searchStr($(this).val().trim());
            //    },

            //    select: function (event, ui) {
            //    }
            //});

        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Loading Workspace - " + engNum);
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });

}

function GetSelectedFile(fileObjID) {
    //viewModel.allFiles()
}

function SetSelectedFile(fileObjID) {

    //var match = ko.utils.arrayFirst(viewModel.selectedFldr().WsFiles(), function (item) {
    var match = ko.utils.arrayFirst(viewModel.allFiles()(), function (item) {
        return fileObjID === item.ObjectID();
    });
    //alert(match.Number());
    //if (match != null) {
    //    var idx = viewModel.selectedFldr().WsFiles().indexOf(match);

    //    viewModel.selectedFile(viewModel.selectedFldr().WsFiles()[idx]);
    //}
    if (match)
    { viewModel.selectedFile(match); }
}

//function UpdateSelectedFile(fileObjID, fileObj) {

//    //var match = ko.utils.arrayFirst(viewModel.selectedFldr().WsFiles(), function (item) {
//    var match = ko.utils.arrayFirst(viewModel.allFiles()(), function (item) {
//        return fileObjID === item.ObjectID();
//    });
//    //alert(match.Number());
//    //if (match != null) {
//    //    var idx = viewModel.selectedFldr().WsFiles().indexOf(match);

//    //    ko.mapping.fromJS(fileObj, {}, viewModel.selectedFldr().WsFiles()[idx]);
//    //}

//    if (match) {
//        ko.mapping.fromJS(fileObj, {}, viewModel.selectedFile());
//    }
//}

//function UpdateFileStatus(fileObjID, status) {

//    var match = ko.utils.arrayFirst(viewModel.allFiles()(), function (item) {
//        return fileObjID === item.ObjectID();
//    });

//    //alert("status - " + status);
//    //alert("viewModel.selectedFile().DocumentStatus().Status() - " + viewModel.selectedFile().DocumentStatus.Status());

//    if (match) {
//        viewModel.selectedFile().DocumentStatus.Status(status);
//        //ko.mapping.fromJS(status, {}, viewModel.selectedFile().DocumentStatus.Status());
//    }
//}

//InUse
function UpdateFileStatusAfterActivity(status) {

    var dataItem = viewModel.currentDataItem();
    dataItem.DocumentStatus.Status = status;
    //viewModel.selectedFile().DocumentStatus.Status(status);
    dataItem.set("Status", GetDocumentStatus(dataItem.DocumentStatus));
    //viewModel.currentGrid().refresh();
}

//InUse
function RemoveDeletedFile(fileObjID, fileObj) {

    //var match = ko.utils.arrayFirst(viewModel.selectedFldr().WsFiles(), function (item) {
    var match = ko.utils.arrayFirst(viewModel.allFiles()(), function (item) {
        return fileObjID === item.ObjectID();
    });

    if (match) {

        viewModel.selectedFile().IsDeleted(true);

        //viewModel.allFiles().remove(match);
        //viewModel.currentGrid().removeRow(viewModel.currentDataItem());
    }
}

$(function () {
    ko.applyBindings(viewModel);
});




