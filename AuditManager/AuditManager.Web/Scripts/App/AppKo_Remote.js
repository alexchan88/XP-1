function GetEngByWsId(wsId, wsLoadType, from) {

    InitializeRemote();

    if (!(IsNothing(from)) && from.IsMatch("From_Upload")) {

    }
    else if (!(IsNothing(from)) && from.IsMatch("From_UsrMgmt")) {

    }
    else {
        $("#myEngTab").tabs("option", "active", 0);

        viewModel.selectedFldr('');
    }

    viewModel.selectedFile('');

    //alert((typeof (EditToggle) == 'undefined'));

    if (!(typeof (EditToggle) == 'undefined')) {
        EditToggle(false);
    }

    $.ajax(
    {
        type: 'GET',
        async: false,
        url: '/api/Workspace/GetEngByWsId?wsId=' + wsId + '&wsLoadType=' + wsLoadType + '&newSession=' + newSession,
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

            if (!(IsNothing(from)) && from.IsMatch("From_Upload")) {

                //alert(viewModel.selectedFldr().ObjectID());
                SetSelectedFldr(viewModel.selectedFldr().ObjectID());
            }

            //var mappedData = ko.utils.arrayMap(viewModel.treeRoot(), function (item) {
            //    return item.Name().split('-').pop();
            //});

            //$("#engSrch").autocomplete({
            //    source: mappedData,
            //    change: function () {
            //        viewModel.searchStr($(this).val());
            //    }
            //});

            //if (remote) {
            //    PostToRemote("upload", "completed");
            //}

        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Loading Workspace");
        },
        complete: function () {
            newSession = false;
            myApp.hidePleaseWait();
        }
    });
}

function SetSelectedFldr(fldrId) {

    InitializeRemote();

    viewModel.selectedFldr('');
    viewModel.selectedFile('');

    $("#myEngTab").tabs("option", "active", 1);

    var match = ko.utils.arrayFirst(viewModel.selectedWs().WsFldrs()(), function (item) {
        return fldrId === item.ObjectID();
    });

    if (match == null) {
        $.each(viewModel.selectedWs().WsFldrs()(), function (idx, item) {
            var match2 = ko.utils.arrayFirst(item.WsFldrs(), function (item2) {
                return fldrId === item2.ObjectID();
            });

            if (match2 != null) {
                var idx2 = viewModel.selectedWs().WsFldrs()()[idx].WsFldrs().indexOf(match2);

                viewModel.selectedFldr(viewModel.selectedWs().WsFldrs()()[idx].WsFldrs()[idx2]);

                //alert(idx);
                //alert(idx2);

                return false;
            }
        })
    }
    else {
        var idx = viewModel.selectedWs().WsFldrs()().indexOf(match);
        //alert(idx);
        viewModel.selectedFldr(viewModel.selectedWs().WsFldrs()()[idx]);
    }
}

function InitializeRemote() {
    $("#dmAlert").dialog('close');
}