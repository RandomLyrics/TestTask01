// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//GLOBALS
const _DateFormat = 'DD.MM.YYYY hh:mm';
const _DateFormatToBack = 'MM-DD-YYYY';
const _DateFormatPL = 'DD.MM.YYYY';

// AJAX API
var _ajaxJson = (function () {
    'use strict';

    function BaseCall(url, rType, obj) {
        return $.ajax({
            url: url,
            type: rType,
            data: JSON.stringify(obj),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) { },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }

    return {
        get: function (url) { return BaseCall(url, "GET"); },
        post: function (url, obj) { return BaseCall(url, "POST", obj); },
        put: function (url, obj) { return BaseCall(url, "PUT", obj); },
        delete: function (url) { return BaseCall(url, "DELETE"); }
    };
}());


// DATES
function toDateFormat(date) {
    date = date + 'Z';
    date = moment(date).local().format(_DateFormat);
    return date;
}
function convertDateToUTC(date) {
    date = moment(date, _DateFormat);
    return date;
}