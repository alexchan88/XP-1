function GetFileTabAct() {
    $.ajax({
        type: 'GET',
        url: '/api/Workspace/GetFileActivity?fDate=' + (new Date('01/01/1999')).ToDate() +
            '&tDate=' + (new Date()).ToDate() + '&activityFilterType=' + "ALL" + '&engNum=' + viewModel.selectedWs().WsProfile().EngNum(),
        headers: {
            'RequestVerificationToken': GetTokenHeaderValue()
        },
        success: function (data, textStatus, xhr) {
            alert(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HandleError(jqXHR, textStatus, errorThrown);
        },
        beforeSend: function () {
            myApp.showPleaseWait("Retrieving File Activity.");
        },
        complete: function () {
            myApp.hidePleaseWait();
        }
    });
}

function SelectedFldrOnChange(selectedFolder) {
    //alert("selectedFolder");
    viewModel.selectedFldr('');
    viewModel.selectedFile('');

    $("#myEngTab").tabs("option", "active", 1);

    //GetFileTabAct();

    selectedFolder.parent().siblings("li").children(".chkEngFldr").prop("checked", false);
    selectedFolder.siblings("ul").children("li").children(".chkEngFldr").prop("checked", false);

    //selectedFolder.parent().parent().closest(".chkEngFldr").prop("checked", false);
    //selectedFolder.parent().parent().closest(".chkEngFldr").trigger("click");

    selectedFolder.prop("checked", true);

    //123-AKS

    $(".activeTreeItem").removeClass("activeTreeItem");
    selectedFolder.addClass("activeTreeItem");
    //alert(selectedFolder);

    var selectedFolderId = selectedFolder.attr("id");

    if (!(typeof viewModel.selectedWs() === 'undefined')) {

        //alert("viewModel.selectedWs()");

        var parentObjectID = selectedFolder.parent().parent().parent().find(".chkEngFldr").attr("id");

        if (parentObjectID === viewModel.selectedWs().ObjectID()) {

            //alert("parentObjectID");

            var match = ko.utils.arrayFirst(viewModel.selectedWs().WsFldrs()(), function (item) {
                return selectedFolderId === item.ObjectID();
            });

            if (match != null) {


                var idx = viewModel.selectedWs().WsFldrs()().indexOf(match);

                //alert("1");
                viewModel.selectedFldr(viewModel.selectedWs().WsFldrs()()[idx]);
                //alert("2");
            }
        }
        else {

            //alert("parentObjectID-not");

            var match = ko.utils.arrayFirst(viewModel.selectedWs().WsFldrs()(), function (item) {
                return parentObjectID === item.ObjectID();
            });

            if (match != null) {
                var idx = viewModel.selectedWs().WsFldrs()().indexOf(match);

                var match2 = ko.utils.arrayFirst(viewModel.selectedWs().WsFldrs()()[idx].WsFldrs(), function (item) {
                    return selectedFolderId === item.ObjectID();
                });

                if (match2 != null) {
                    var idx2 = viewModel.selectedWs().WsFldrs()()[idx].WsFldrs().indexOf(match2);

                    viewModel.selectedFldr(viewModel.selectedWs().WsFldrs()()[idx].WsFldrs()[idx2]);
                }
            }
        }
    }
}

$(function () {

    //myApp.showPleaseWait("Loading");
    //$.getJSON("/api/Workspace/GetWs/?wsId=", function (data) {

    //    var mappedRoot = ko.mapping.fromJS(data);
    //    viewModel.treeRoot(mappedRoot());
    //    viewModel.originalTree(mappedRoot());

    //    var mappedData = ko.utils.arrayMap(viewModel.treeRoot(), function (item) {
    //        return item.Name().split('-').pop().trim();
    //    });

    //    $("#engSrch").autocomplete({
    //        source: mappedData,

    //        change: function (event, ui) {

    //            if (IsNothing(ui.item)) {

    //                var srchTxt = $(this).val().trim();

    //                if (IsEngSearchValid()) {

    //                    viewModel.treeRoot(ko.utils.arrayFilter(viewModel.originalTree(), function (item) {
    //                        var txtTreeRoot = item.Name().split('-').pop().trim();
    //                        return txtTreeRoot.indexOf(srchTxt) > -1;
    //                    }));

    //                    resetViewModel(viewModel);

    //                    if (viewModel.treeRoot().length == 0) {

    //                    }
    //                    else {
    //                        EngSearch_RemoveFilter(false);
    //                    }

    //                }
    //            }

    //        },

    //        select: function (event, ui) {

    //            var srchTxt = ui.item.value;

    //            if (IsEngSearchValid()) {

    //                viewModel.treeRoot(ko.utils.arrayFilter(viewModel.originalTree(), function (item) {
    //                    var txtTreeRoot = item.Name().split('-').pop().trim();
    //                    return txtTreeRoot.indexOf(srchTxt) > -1;
    //                }));

    //                resetViewModel(viewModel);
    //                EngSearch_RemoveFilter(false);
    //            };
    //        }
    //    });

    //})
    //.fail(function (jqXHR, textStatus, errorThrown) {
    //    HandleError(jqXHR, textStatus, errorThrown);
    //})
    //.always(function () {
    //    myApp.hidePleaseWait();
    //    //document.writeln($(".chkEngFldr").first().attr("id"));
    //    //$(".chkEngFldr").first().trigger("click");
    //});

    //
    if (toLoadWs) {
        //alert(toLoadWs);
        $.ajax({
            type: 'GET',
            async: true,
            url: "/api/Workspace/GetWs/?wsId=",
            headers: {
                'RequestVerificationToken': GetTokenHeaderValue()
            },
            success: function (data, textStatus, xhr) {

                var mappedRoot = ko.mapping.fromJS(data);
                viewModel.treeRoot(mappedRoot());
                viewModel.originalTree(mappedRoot());

                var mappedData = ko.utils.arrayMap(viewModel.treeRoot(), function (item) {
                    return item.Name().split('-').pop().trim();
                });

                $("#engSrch").autocomplete({
                    source: mappedData,

                    change: function (event, ui) {

                        //alert("change");

                        if (IsNothing(ui.item)) {

                            var srchTxt = $(this).val().trim();

                            if (IsEngSearchValid()) {

                                viewModel.treeRoot(ko.utils.arrayFilter(viewModel.originalTree(), function (item) {
                                    var txtTreeRoot = item.Name().split('-').pop().trim();
                                    return txtTreeRoot.indexOf(srchTxt) > -1;
                                }));

                                resetViewModel(viewModel);

                                if (viewModel.treeRoot().length == 0) {

                                }
                                else {
                                    EngSearch_RemoveFilter(false);
                                }

                            }
                        }

                    },

                    select: function (event, ui) {

                        //alert("select");

                        var srchTxt = ui.item.value;

                        if (IsEngSearchValid()) {

                            viewModel.treeRoot(ko.utils.arrayFilter(viewModel.originalTree(), function (item) {
                                var txtTreeRoot = item.Name().split('-').pop().trim();
                                return txtTreeRoot.indexOf(srchTxt) > -1;
                            }));

                            resetViewModel(viewModel);
                            EngSearch_RemoveFilter(false);
                        };
                    }
                });

            },
            error: function (jqXHR, textStatus, errorThrown) {
                HandleError(jqXHR, textStatus, errorThrown);
            },
            beforeSend: function () {
                myApp.showPleaseWait("Loading");
            },
            complete: function () {
                myApp.hidePleaseWait();
            }
        });
    }
    //

    $("body").on("click", "ul.ulEngFarm > li > .chkEngFldr:not(:checked)", function () {

        //alert("1");

        $(".chkEngFldr").prop("checked", false);

        $(this).prop("checked", true);

        viewModel.selectedFldr('');
        viewModel.selectedFile('');

        //123-AKS
        $(".activeTreeItem").removeClass("activeTreeItem");
        $(this).addClass("activeTreeItem");

    });

    $("body").on("click", "ul.ulEngFarm > li > .chkEngFldr", function () {

        //alert("2");

        $("#myEngTab").tabs("option", "active", 0);

        //if (isInEdit) {
        //    //AmAlert(AmMsg.isInEdit_Msg + " 1");
        //}
        //else {
        //    $("#myEngTab").tabs("option", "active", 0);
        //    //EditToggle(false);
        //}
    });

    $("body").on("click", "ul.ulEngFarm > li > .chkEngFldr:checked", function () {

        //alert("3");

        if (isInEdit && !internalReload) {
            $(this).prop("checked", false);

            //123-AKS
            //$(this).removeClass("activeTreeItem");

            AmAlert(AmMsg.isInEdit_Msg);
        }
        else {

            if (!(typeof (EditToggle) == 'undefined') && !internalReload) {
                //alert("xx");
                EditToggle(false);
            }

            viewModel.selectedFldr('');
            viewModel.selectedFile('');

            $("#myEngTab").tabs("enable");

            $("#myEngTab").tabs("option", "active", 0);

            var selectedTree = $(this);

            $(".chkEngFldr").prop("checked", false);
            selectedTree.prop("checked", true);

            //123-AKS
            $(".activeTreeItem").removeClass("activeTreeItem");
            selectedTree.addClass("activeTreeItem");

            var selectedEngId = selectedTree.attr("id");

            var match = ko.utils.arrayFirst(viewModel.treeRoot(), function (item) {
                return selectedEngId === item.ObjectID();
            });

            var idx = viewModel.treeRoot().indexOf(match);

            //alert(match.IsLoaded());

            if (!match.IsLoaded()) {

                //alert("cc");

                //myApp.showPleaseWait("Loading Workspace " + match.Name());
                //$.getJSON("/api/Workspace/GetWs/?wsId=" + selectedEngId + '&newSession=' + newSession, function (data) {

                //    var mappedChildItem = ko.mapping.fromJS(data[0]);

                //    viewModel.treeRoot()[idx].WsProfile(mappedChildItem.WsProfile);
                //    viewModel.treeRoot()[idx].WsFldrs(mappedChildItem.WsFldrs);
                //    viewModel.treeRoot()[idx].WsGroups(mappedChildItem.WsGroups);

                //    viewModel.treeRoot()[idx].IsLoaded(mappedChildItem.IsLoaded);
                //})
                //.fail(function (jqXHR, textStatus, errorThrown) {
                //    HandleError(jqXHR, textStatus, errorThrown);
                //})
                //.always(function () {
                //    newSession = false;
                //    myApp.hidePleaseWait();
                //});
                //myApp.showPleaseWait("Loading Workspace " + match.Name());
                $.ajax(
                       {
                           type: 'GET',
                           async: true,
                           url: "/api/Workspace/GetWs/?wsId=" + selectedEngId + '&newSession=' + newSession,
                           headers: {
                               'RequestVerificationToken': GetTokenHeaderValue()
                           },
                           success: function (data, textStatus, xhr) {

                               var mappedChildItem = ko.mapping.fromJS(data[0]);

                               viewModel.treeRoot()[idx].WsProfile(mappedChildItem.WsProfile);
                               viewModel.treeRoot()[idx].WsFldrs(mappedChildItem.WsFldrs);
                               viewModel.treeRoot()[idx].WsGroups(mappedChildItem.WsGroups);

                               viewModel.treeRoot()[idx].IsLoaded(mappedChildItem.IsLoaded);

                               viewModel.selectedWs(viewModel.treeRoot()[idx]);

                           },
                           error: function (jqXHR, textStatus, errorThrown) {
                               HandleError(jqXHR, textStatus, errorThrown);
                           },
                           beforeSend: function () {
                               myApp.showPleaseWait("Loading Workspace " + match.Name());
                           },
                           complete: function () {
                               myApp.hidePleaseWait();

                               if (!IsNothing(selectedFolderId_CheckBoxId)) {
                                   uploadInProgress = false;

                                   //alert(selectedFolderId_CheckBoxId);

                                   //var ch = $("[id='" + viewModel.selectedWs().ObjectID() + "']");
                                   //ch.prop("checked", false);
                                   //ch.trigger("click");

                                   //var chk = $("*[id='" + selectedFolderId_CheckBoxId + "']");
                                   //chk.prop("checked", false);
                                   //chk.trigger("click");

                                   //alert($('.dmUp').length);

                                   //selectedFolderId_CheckBoxId = viewModel.selectedFldr().ObjectID();
                                   //var chk = $("*[id='" + selectedFolderId_CheckBoxId + "']");

                                   //alert(chk.parents("li").length);
                                   ////chkEngFldr

                                   //$.each(chk.parents("li"), function (idx, item) {
                                   //    alert($(item).find(".chkEngFldr"));
                                   //})

                                   //return;

                                   var chk = $("*[id='" + selectedFolderId_CheckBoxId + "']");
                                   //chk.prop("checked", false);
                                   //chk.trigger("click");

                                   var chkArray = [];

                                   $.each(chk.parents("li"), function (idx, item) {
                                       var chkItem = $(item).find(".chkEngFldr");
                                       //alert($(chkItem).attr("id"));
                                       chkArray.push(chkItem);
                                       //$(chkItem).prop("checked", false);
                                       //$(chkItem).trigger("click");
                                   })

                                   //chkArray = chkArray.reverse();

                                   $.each(chkArray.reverse(), function (idx, item) {
                                       //alert("myloop");
                                       //alert($(item).prop("id"));
                                       $(item).prop("checked", false);
                                       $(item).trigger("click");
                                   })

                                   selectedFolderId_CheckBoxId = "";
                               }
                           }
                       });
            }
            else {
                viewModel.selectedWs(viewModel.treeRoot()[idx]);
            }
        }

    });

    $("body").on("click", "ul.ulEngFldr > li > .chkEngFldr:not(:checked)", function () {

        //alert("4");

        var selectedFolder = $(this);

        SelectedFldrOnChange(selectedFolder);

        //$("#myEngTab").tabs("option", "active", 1);

        //selectedFolder.parent().siblings("li").children(".chkEngFldr").prop("checked", false);
        //selectedFolder.siblings("ul").children("li").children(".chkEngFldr").prop("checked", false);

        //selectedFolder.prop("checked", true);

    });

    $("body").on("click", "ul.ulEngFldr > li > .chkEngFldr:checked", function () {

        //alert("5");

        var selectedFolder = $(this);

        //alert("SelectedFldrOnChange");
        SelectedFldrOnChange(selectedFolder);

    });
});
