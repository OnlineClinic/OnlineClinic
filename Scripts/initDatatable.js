$(document).ready(function () {
   try {
    var sitename = 'Investment';
    var flag = false;    
        if (getCookie("CookieCompliance_" + sitename) != null && getCookie("CookieCompliance_" + sitename) == 'true') {
            flag = true;
        }
    var oTable = $('.datatable').dataTable({
        "sPaginationType": "full_numbers"
        , "bJQueryUI": false
        , "bProcessing": true
        , "sDom": '<"search-box"r><"H"lf>t<"F"ip>'
        , "aLengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        , "iDisplayLength": 10, "bStateSave": flag, "aaSorting": [[0, 'asc']]
        , "oLanguage": { "oPaginate": { "sNext": '>', "sLast": '>>', "sFirst": '<<', "sPrevious": '<' } }
    });
    var oSettings = oTable.fnSettings(); if (oSettings != null)
    {
        oSettings.aoColumns[oTable.fnGetData(0).length - 1].bSortable = false; oSettings.aoColumns[oTable.fnGetData(0).length - 1].sClass = "";
    }

    var oTable2 = $('.datatableAllSort').dataTable({
        "sPaginationType": "full_numbers"
        , "bJQueryUI": false
        , "bProcessing": true
        , "sDom": '<"search-box"r><"H"lf>t<"F"ip>'
        , "aLengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        , "iDisplayLength": 10, "bStateSave": flag, "aaSorting": [[0, 'asc']]
        , "oLanguage": { "oPaginate": { "sNext": '>', "sLast": '>>', "sFirst": '<<', "sPrevious": '<' } }
    });    

    } catch (e) {
        
    }
});
function getCookie(key) {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}

