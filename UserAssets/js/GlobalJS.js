// JScript File
var browser = navigator.appName;
function numericOnly(e)
{
    var code;

    if (browser == 'Microsoft Internet Explorer')
        code = e.keyCode;
    else //Mozilla/Netscape
        code = e.charCode;

    if ((code >= 48 && code <= 57) || code == 0)
        return true;
    else
        return false;
}

function numericWithDotOnly(txtId, e)
{
    var code;
    var txt = document.getElementById(txtId).value;

    if (browser == 'Microsoft Internet Explorer')
        code = e.keyCode;
    else //Mozilla/Netscape
        code = e.charCode;

    if (txt.indexOf('.', 0) >= 0 && code == 46) {
        return false;
    }

    if ((code >= 48 && code <= 57) || code == 0 || code == 46)
        return true;
    else
        return false;
}

function numericWithColonOnly(txtId, e)
{   
    var code;
    var txt = document.getElementById(txtId).value;

    if (browser == 'Microsoft Internet Explorer')
        code = e.keyCode;
    else //Mozilla/Netscape
        code = e.charCode;

    if (txt.indexOf('.', 0) >= 0 && code == 58) {
        return false;
    }

    if ((code >= 48 && code <= 57) || code == 0 || code == 58)
        return true;
    else
        return false;
}


function disableKeyPress(e)
{
    if (e.which != 0) return false;
    else return true;
}

function selectCheckBoxes(chk)
{
    var someChecked = false;
    var grid = document.getElementById(chk.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.id);
    var arrCheckList = grid.getElementsByTagName('input');
    var count = 0;
    for (var i = 0; i < arrCheckList.length; i++) {
        if (arrCheckList[i].type == 'checkbox' && arrCheckList[i].name.indexOf('chkSelectMain', 0) < 0) {
            someChecked = true;
            arrCheckList[i].checked = chk.checked;
            if (chk.checked)
                document.getElementById(arrCheckList[i].id.replace('Select_chk', 'Select_dvchk')).className = 'checkBox horizontal checkState';
            else
                document.getElementById(arrCheckList[i].id.replace('Select_chk', 'Select_dvchk')).className = 'checkBox horizontal uncheckState';
        }
    }
    if (chk.checked && !someChecked) {
        alert('No Record Has Been Selected on Current Page');
    }
}

function verifyMainCheck(chk)
{
    var grid = document.getElementById(chk.parentNode.parentNode.parentNode.parentNode.parentNode.id);
    var arrCheckList = grid.getElementsByTagName('input');
    var allChecked = true;
    for (var i = 0; i < arrCheckList.length; i++) {
        if (arrCheckList[i].type == 'checkbox') {
            if (!arrCheckList[i].checked) {
                allChecked = false;
                break;
            }
        }
    }
    if (allChecked) {
        $('#toggle-all').addClass('toggle-checked');
    }
    else {
        $('#toggle-all').removeClass('toggle-checked');
    }
}

function getMonthNo(monthName)
{
    monthName = monthName.toString().toUpperCase();

    if (monthName == "JAN" || monthName == "ΙΑΝ")
        return "01";
    else if (monthName == "FEB" || monthName == "ΦΕΒ")
        return "02";
    else if (monthName == "MAR" || monthName == "ΜΑΡ")
        return "03";
    else if (monthName == "APR" || monthName == "ΑΠΡ")
        return "04";
    else if (monthName == "MAY" || monthName == "ΜΑΪ")
        return "05";
    else if (monthName == "JUN" || monthName == "ΙΟΥΝ")
        return "06";
    else if (monthName == "JUL" || monthName == "ΙΟΥΛ")
        return "07";
    else if (monthName == "AUG" || monthName == "ΑΥΓ")
        return "08";
    else if (monthName == "SEP" || monthName == "ΣΕΠ")
        return "09";
    else if (monthName == "OCT" || monthName == "ΟΚΤ")
        return "10";
    else if (monthName == "NOV" || monthName == "ΝΟΕ")
        return "11";
    else if (monthName == "DEC" || monthName == "ΔΕΚ")
        return "12";
    else
        return "0";
}

function getMonthName(monthNo)
{
    if (monthNo == "0")
        return "Jan";
    else if (monthNo == "1")
        return "Feb";
    else if (monthNo == "2")
        return "Mar";
    else if (monthNo == "3")
        return "Apr";
    else if (monthNo == "4")
        return "May";
    else if (monthNo == "5")
        return "Jun";
    else if (monthNo == "6")
        return "Jul";
    else if (monthNo == "7")
        return "Aug";
    else if (monthNo == "8")
        return "Sep";
    else if (monthNo == "9")
        return "Oct";
    else if (monthNo == "10")
        return "Nov";
    else if (monthNo == "11")
        return "Dec";
    else
        return "";
}

function validateSelection(grd)
{
    var grid = document.getElementById(grd);
    var arrCheckList = grid.getElementsByTagName('input');
    var atLeastOneSelected = false;
    for (var i = 0; i < arrCheckList.length; i++) {
        if (arrCheckList[i].type == 'checkbox' && arrCheckList[i].checked) {
            atLeastOneSelected = true;
            break;
        }
    }
    if (!atLeastOneSelected) {
        alert('Please select atleast one record to proceed');
        return false;
    }
}

function trimVal(value)
{
    value = leftTrim(value);
    value = rightTrim(value);
    value = value.replace('\n', '');
    return value;
}

function leftTrim(value)
{
    var strVal = new String();
    strVal = value;

    while (strVal.charAt(1) == ' ') {
        strVal = strVal.substring(2, strVal.length);
    }
    if (strVal.charAt(0) == ' ') {
        strVal = strVal.substring(1, strVal.length);
    }
    return strVal;
}

function rightTrim(value)
{
    var strVal = new String();
    strVal = value;

    while (strVal.charAt(strVal.length - 1) == ' ') {
        strVal = strVal.substring(0, strVal.length - 2);
    }
    if (strVal.charAt(strVal.length - 1) == ' ') {
        strVal = strVal.substring(0, strVal.length - 2);
    }
    return strVal;
}

function isEmail(mail)
{
    var emailPattern = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    return emailPattern.test(mail);
}

function setMaxLength(txt, len, e)
{

    if (browser == 'Microsoft Internet Explorer')
        code = e.keyCode;
    else //Mozilla/Netscape
        code = e.charCode;

    if (txt.value.length > len) {
        if (code == 0) return true;
        else return false;
    }
    else {
        return true;
    }
}

function MaskTimeFormat(txtId)
{
    var objStartTime = document.getElementById(txtId);
    var strStartTimeValue = objStartTime.value;
    objStartTime.value = GiveCorrectTimeFormat(strStartTimeValue);
}


function GiveCorrectTimeFormat(strInputTime)
{
    var strReturnValue = "";
    if (strInputTime.length <= 5) {
        strInputTime = strInputTime.replace(":", "");
        //alert(strInputTime);
        if (strInputTime.length == 1) {
            strInputTime = "0" + strInputTime + ":" + "00";
        }
        else if (strInputTime.length == 2) {
            //alert("in 2");
            if (strInputTime <= 23) {
                strInputTime = strInputTime + ":" + "00";
            }
            else if (strInputTime <= 59) {
                strInputTime = "00" + ":" + strInputTime;
            }
            else {
                strInputTime = "00:00"
            }
        }
        else if (strInputTime.length == 3) {
            //alert("in 3");
            if (strInputTime < 959) {
                //alert(strInputTime.substring(1,3));
                if (strInputTime.substring(1, 2) <= 5) {
                    strInputTime = "0" + strInputTime.substring(0, 1) + ":" + strInputTime.substring(1, 3);
                }
                else {
                    strInputTime = "0" + strInputTime.substring(0, 1) + ":" + "00";
                }
            }
            else {
                strInputTime = "0" + strInputTime.substring(0, 1) + ":" + "00";
            }
        }
        else if (strInputTime.length == 4) {
            //alert("in 4");
            if (strInputTime < 2359) {
                //alert(strInputTime.substring(0,2));
                if (strInputTime.substring(0, 2) <= 23) {

                    if (strInputTime.substring(2, 3) <= 5) {
                        strInputTime = strInputTime.substring(0, 2) + ":" + strInputTime.substring(2, 4);
                    }
                    else {
                        strInputTime = strInputTime.substring(0, 2) + ":" + "00";
                    }
                }
                else {
                    strInputTime = "00:00"
                }

            }
            else {
                if (strInputTime.substring(0, 2) <= 23) {
                    if (strInputTime.substring(2, 3) <= 5) {
                        strInputTime = strInputTime.substring(0, 2) + ":" + strInputTime.substring(2, 4);
                    }
                    else {
                        strInputTime = strInputTime.substring(0, 2) + ":" + "00";
                    }
                }
                else {
                    strInputTime = "00:00"
                }
            }
        }
        else {
            strInputTime = "00:00"
        }
        //alert(strInputTime);
        strReturnValue = strInputTime;
        if (strReturnValue == "00:00") {
            strReturnValue = "";
        }
    }
    return strReturnValue;
}

$(document).ready(function ()
{
    var window_width = $(window).width() - 310;
    $("#chartdiv").css("width", window_width + "px");
    $("#graphdiv").css("width", window_width + "px");
});

$(window).resize(function ()
{
    var window_width = $(window).width() - 310;
    $("#chartdiv").css("width", window_width + "px");
    $("#graphdiv").css("width", window_width + "px");
});

function getRandomString()
{
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 5; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}