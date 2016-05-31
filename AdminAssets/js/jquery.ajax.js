$(function () {
    window.ajaxParam = {
        'url': null,
        'requestType': "GET",
        'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
        'dataType': 'json',
        'data': null,
        'beforeSendCallbackFunction': null,
        'successCallbackFunction': null,
        'completeCallbackFunction': null,
        'errorCallBackFunction': null,
        'controlId':null
    };

    window.doAjax = function (doAjax_params) {

        var url = doAjax_params['url'];
        var requestType = doAjax_params['requestType'];
        var contentType = doAjax_params['contentType'];
        var dataType = doAjax_params['dataType'];
        var data = doAjax_params['data'];
        var beforeSendCallbackFunction = doAjax_params['beforeSendCallbackFunction'];
        var successCallbackFunction = doAjax_params['successCallbackFunction'];
        var completeCallbackFunction = doAjax_params['completeCallbackFunction'];
        var errorCallBackFunction = doAjax_params['errorCallBackFunction'];
        var controlId = doAjax_params['controlId'];
        if (controlId == null) controlId = '';

        $.ajax({
            url: url,
            crossDomain: true,
            type: requestType,
            contentType: contentType,
            dataType: dataType,
            data: data,
            beforeSend: function (jqXHR, settings) {
                if (typeof beforeSendCallbackFunction === "function") {
                    beforeSendCallbackFunction();
                }
            },
            success: function (data, textStatus, jqXHR) {
                if (typeof successCallbackFunction === "function") {                   
                    successCallbackFunction(data, controlId);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (typeof errorCallBackFunction === "function") {
                    errorCallBackFunction(errorThrown);
                }

            },
            complete: function (jqXHR, textStatus) {
                if (typeof completeCallbackFunction === "function") {
                    completeCallbackFunction();
                }
            }
        });
    }
});