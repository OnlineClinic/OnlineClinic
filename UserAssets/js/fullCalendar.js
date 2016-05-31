var gService = false;
var gTimeZone = 9;

function g_ptnCookie()
{
    if (typeof (ptnOther) == "function") {
        ptnOther();
    }
    else if (typeof (ptnPlugin) == "function") {
        ptnPlugin();
    }
    else if (typeof (ptnFPC) == "function") {
        ptnFPC(gTimeZone);
    }
}

function g_ptnGetCookie(name)
{
    var pos = document.cookie.indexOf(name + "=");
    if (pos != -1) {
        var start = pos + name.length + 1;
        var end = document.cookie.indexOf(";", start);
        if (end == -1) {
            end = document.cookie.length;
        }
        return unescape(document.cookie.substring(start, end));
    }
    return null;
}

function g_ptnGetCrumb(name, crumb)
{
    var aCookie = g_ptnGetCookie(name).split(":");
    for (var i = 0; i < aCookie.length; i++) {
        var aCrumb = aCookie[i].split("=");
        if (crumb == aCrumb[0]) {
            return aCrumb[1];
        }
    }
    return null;
}

function g_ptnGetIdCrumb(name, crumb)
{
    var cookie = g_ptnGetCookie(name);
    var id = cookie.substring(0, cookie.indexOf(":lv="));
    var aCrumb = id.split("=");
    for (var i = 0; i < aCrumb.length; i++) {
        if (crumb == aCrumb[0]) {
            return aCrumb[1];
        }
    }
    return null;
}

function g_ptnIsFpcSet(name, id, lv, ss)
{
    if (id == g_ptnGetCrumb(name, "id")) {
        if (lv == g_ptnGetCrumb(name, "lv")) {
            if (ss = g_ptnGetCrumb(name, "ss")) {
                return true;
            }
        }
    }
    return false;
}

function g_ShowErrorMessage(msg)
{
    var arrMsg = new Array(5);
    arrMsg[0] = msg;
    alert(arrMsg[0]);
}


/********************************************************************
화면 처리 ASPX가 클라이언트에 Load된 후 실행해야 될 로직 처리
********************************************************************/
function g_WindowOnLoad(strPortletID)
{
    //에러 메시지가 있는 경우 팝업 출력
    var obj = eval("document.all.HErrorMessage_" + strPortletID);
    if (obj.value.length > 0) g_OpenErrorMessage(obj.value);

    //Information이 있는 경우 팝업 출력
    var obj = eval("document.all.HInformationMessage_" + strPortletID);
    if (obj.value.length > 0) g_OpenInformation(obj.value);

    // 백스페이스로 이전 페이지로 가는 것을 막는다.
    document.onkeydown = g_PreventNavigateBack;

    try {
        // 상태바 Text 변경
        window.status = "";
    }
    catch (exception) {
        alert(exception.message + " (g_WindowOnLoad) ");
    }

}

/********************************************************************
텍스트 박스 이외는 backspace 입력을 제한한다.
********************************************************************/
function g_PreventNavigateBack()
{
    var strTagType;

    if (window.event.keyCode == 8) {
        if (window.event.srcElement.tagName.toUpperCase() == "INPUT") {
            strTagType = window.event.srcElement.getAttribute("type").toUpperCase();
            if (strTagType == "TEXT" || strTagType == "PASSWORD" || strTagType == "FILE")
                return;
        }
        else if (window.event.srcElement.tagName.toUpperCase() == "TEXTAREA")
            return;
        else
            window.event.returnValue = false;
    }
}


/********************************************************************
에러 메시지 상자를 띠운다.
********************************************************************/
function g_OpenErrorMessage(strErr)
{
    window.showModalDialog(COMMONPAGE_WEB_PATH + "ErrorMessage.aspx", strErr, "dialogWidth:405px;dialogHeight:260px;status=no;scroll=no");
}

/********************************************************************
작업정보 상자를 띠운다.
********************************************************************/
function g_OpenInformation(sInfo)
{

    window.showModalDialog(COMMONPAGE_WEB_PATH + "InformationMessage.aspx", sInfo, "dialogWidth:405px;dialogHeight:260px;status=no;scroll=no");
}

/********************************************************************
질문 상자를 띠운다.
********************************************************************/
function g_OpenConfirm(sCfm)
{

    var res = window.showModalDialog(COMMONPAGE_WEB_PATH + "ConfirmMessage.aspx", sCfm, "dialogWidth:405px;dialogHeight:260px;status=no;scroll=no");
    return res;
}


/********************************************************************
팝업창을 띠운다.
********************************************************************/
function g_OpenDialog(sUrl, sFrame, sFeature)
{
    return window.open(sUrl, sFrame, sFeature);
}

/********************************************************************
팝업창을 띠운다.
********************************************************************/
function g_OpenModalDialog(sUrl, sArg, sWith, sHeight)
{
    var strReturn = "";
    var sFeature = "dialogWidth:" + sWith + "px;dialogHeight:" + sHeight + "px;status:no;scroll:no;help:No;resizable:No;";
    strReturn = window.showModalDialog(sUrl, sArg, sFeature);

    return strReturn;
}


function g_OpenModalDialog2(wins, sUrl, sArg, sWith, sHeight)
{
    var strReturn = "";
    var sFeature = "dialogWidth:" + sWith + "px;dialogHeight:" + sHeight + "px;status:no;scroll:no;help:No;resizable:Yes;";
    var args = new Object;
    args.window = wins;
    args.value = sArg;

    strReturn = window.showModalDialog(sUrl, args, sFeature);

    return strReturn;
}

function g_OpenModalDialog3(sUrl, sArg, sFeature)
{
    var strReturn = "";

    strReturn = window.showModalDialog(sUrl, sArg, sFeature);

    return strReturn;
}

/********************************************************************
팝업창을 띠운다.
********************************************************************/
function g_Redirect(sUrl, sArg)
{
    var tmpUrl = sUrl + "?" + escape(sArg);
    alert(tmpUrl);
    document.location.replace(tmpUrl);
}


/********************************************************************
문자중 특정문자를 선택문자로 변환한다.
********************************************************************/
function g_setReplace(obj, searchStr, replaceStr)
{
    var str = ((typeof obj == "object") ? obj.value : obj);
    var len, i, tmpstr;

    len = str.length;
    tmpstr = "";

    for (var i = 0; i < len; i++) {
        if (str.charAt(i) != searchStr) {
            tmpstr = tmpstr + str.charAt(i);
        }
        else {
            tmpstr = tmpstr + replaceStr;
        }
    }
    return tmpstr;
}


function g_setReplaceString(obj, searchStr, replaceStr)
{
    var str = ((typeof obj == "object") ? obj.value : obj);
    while (str.indexOf(searchStr) > 0) {
        str = str.replace(searchStr, replaceStr);
    }


    return str;
}

function g_ReplaceQuot(val)
{
    return g_setReplace(val, "'", "\\\'");
}
function g_ReReplaceQuot(val)
{

    return g_setReplace(val, "\\", "");
}


function g_ReplaceQuot(val)
{
    return g_setReplace(val, "'", "\\\'");
}
function g_ReReplaceQuot(val)
{

    return g_setReplace(val, "\\", "");
}

/********************************************************************
해당 이미지 Control을 disable시키고 마우스 모양을 default로 설정한다.
********************************************************************/
function g_DisableControl(oImgBtn)
{
    oImgBtn.disabled = true;
    oImgBtn.style.cursor = "default";
    oImgBtn.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=30)";
    oImgBtn.onclick = "";
}

/********************************************************************
해당 이미지 Control을 disable시키고 마우스 모양을 default로 설정한다.
********************************************************************/
function g_EnableControl(oImgBtn)
{
    oImgBtn.disabled = false;
    oImgBtn.style.cursor = "default";
    oImgBtn.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=30)";
    oImgBtn.onclick = "";
}

/********************************************************************
일반 Calendar obj:입력 컨트롤
********************************************************************/
function g_NewCalendar_Open(obj)
{
    obj = (typeof obj == 'string') ? $(obj) : obj;
    var CurDate = ((typeof obj == "object") ? obj.value : obj);
    CurDate = g_Trim(CurDate);

    if (CurDate != "") {
        var CurDate = g_setReplace(CurDate, "-", "");

        var year = CurDate.substr(0, 4);
        var month = CurDate.substr(4, 2);
        var day = CurDate.substr(6, 2);

        if (g_isValidDay(year, month, day) == false) {
            g_OpenInformation("올바른 날짜 형식의 데이터가 아니므로 오늘날자의 달력을 보여드립니다.");
            CurDate = "";
        }
    }
    else CurDate = "";

    var sUrl = COMMONPAGE_WEB_PATH + "/ModalCalendar.aspx";
    var sArg = CurDate;
    var sFeature = "dialogWidth:285px;dialogHeight:263px;status:no;help:no;scroll:no";

    var ReturnValue = g_OpenModalDialog(sUrl, sArg, 285, 263);

    if (ReturnValue != null) {
        if (typeof obj == "object") obj.value = ReturnValue;
        else return ReturnValue;
    }

    return "";
}

/********************************************************************
Div Display Toggle 함수 
divID: div ID
isView: true, false, ''
********************************************************************/
function g_StyleDisplyToggle(ObjID, isView)
{
    var oObj = g_GetObjID(ObjID);

    if (isView == undefined || isView == '') {
        if (oObj.style.display == "")
            oObj.style.display = "none";
        else
            oObj.style.display = "";
    }
    else {
        if (isView == false)
            oObj.style.display = "none";
        else
            oObj.style.display = "";
    }
}


/************************************************************************
Progress bar Initialize		
*************************************************************************/
function g_ProgressBarInit()
{

    if (g_GetObjID('gPbar') != null || g_GetObjID('gPbar') != undefined) return;

    var sTop = (document.body.offsetHeight / 2) - 50;
    var sLeft = (document.body.offsetWidth / 2) - 100;


    var pframe = document.createElement("<iframe id='hiddenLayer' style='position:absolute;LEFT: " + sLeft + "px; TOP: " + sTop + "px; WIDTH: 268px; HEIGHT: 100px; DISPLAY: none; Z-INDEX: 3001;'></iframe>");
    var pDiv = document.createElement("<DIV id='gPbar' style='LEFT: " + sLeft + "px; TOP: " + sTop + "px; WIDTH: 268px; HEIGHT: 100px; DISPLAY: none; Z-INDEX: 3002; BORDER-LEFT-COLOR: darkgray; BORDER-BOTTOM-COLOR: darkgray; BORDER-TOP-STYLE: solid; BORDER-TOP-COLOR: darkgray; BORDER-RIGHT-STYLE: solid; BORDER-LEFT-STYLE: solid; POSITION: absolute; BACKGROUND-COLOR: #ffffff; BORDER-RIGHT-COLOR: darkgray; BORDER-BOTTOM-STYLE: solid; layer-background-color: #FFFFFF'></DIV>");
    var pTable = document.createElement("<TABLE height='100%' cellSpacing='0' cellPadding='0' width='100%' border='0' id='pTable'></TABLE>");
    var pTr = document.createElement("<TR></TR>");
    var pTd = document.createElement("<TD></TD>");

    document.forms[0].insertBefore(pframe);
    document.forms[0].insertBefore(pDiv);
    document.all.gPbar.appendChild(pTable);
    document.all.pTable.insertRow(pTr);
    document.all.pTable.rows[0].insertCell(pTd);
    document.all.pTable.rows[0].cells[0].innerHTML = "<IMG src='" + IMAGE_PATH + "process.gif'>";
}

/************************************************************************
Progress bar start and display on		
*************************************************************************/
function g_ProgressBarStart()
{

    if (g_GetObjID('gPbar') == null || g_GetObjID('gPbar') == undefined) return;
    var sTop = (document.forms[0].offsetHeight / 2) - 50;
    var sLeft = (document.forms[0].offsetWidth / 2) - 135;

    g_GetObjID("hiddenLayer").style.top = sTop;
    g_GetObjID("hiddenLayer").style.left = sLeft;
    g_GetObjID("hiddenLayer").style.display = "";
    g_GetObjID("gPbar").style.top = sTop;
    g_GetObjID("gPbar").style.left = sLeft;
    g_GetObjID("gPbar").style.display = "";
}

/************************************************************************
Progress bar end and display off			
*************************************************************************/
function g_ProgressBarEnd()
{
    if (g_GetObjID('gPbar') == null || g_GetObjID('gPbar') == undefined) return;

    g_GetObjID("gPbar").style.display = "none";
    g_GetObjID("hiddenLayer").style.display = "none";
}

function g_AttributeAdd(eventObj, attName, defaultValue)
{
    if (eventObj.attributes.getNamedItem(attName) == null) {
        var attribute = document.createAttribute(attName);
        attribute.value = defaultValue;
        eventObj.attributes.setNamedItem(attribute);
    }
}
/************************************************************************
Progress bar start된 상태에서 사용자에 의한 이벤트를 막는 함수
*************************************************************************/
function g_FE(sHandler)
{
    g_FireEvent(sHandler);
}

function g_FireEvent(sHandler)
{
    if (document.all.Pbar != undefined) {
        if (document.all.gPbar.style.display == "none")
            eval(sHandler);
    }
    else eval(sHandler);
}


/************************************************************************
ID의 객체를 가져오기및 객체의 값을 가져오기
*************************************************************************/
function g_GetObjID(objID)
{
    return document.getElementById(objID);
}

function g_GetValueID(objID)
{
    return fn_GetObjID(objID).value;
}

/************************************************************************
g_GetObjID 의 clone
*************************************************************************/
function $(objID)
{
    if (typeof (objID) == 'string')
        return document.getElementById(objID);
    else
        return objID;
}

/************************************************************************
라디오버튼에서 체크되어있는것의 value값을가지고온다.
*************************************************************************/
function g_GetRadioValue(objID)
{
    var i;
    var objRadio = objID;  //document.getElementById(objID);
    var checkedval;
    if (objRadio == null) { return; }
    for (i = 0; i < objRadio.length; i++) {
        if (objRadio[i].checked == true) {
            checkedval = objRadio[i].value;
            break;
        }
    }
    return checkedval;
}

/************************************************************************
엔터키 처리 : 엔터키를 치면 실행되어야할 객체의 Onclick이벤트를 실행시킨다.
*************************************************************************/
function g_EnterEvent(obj)
{
    if (event.keyCode == 13) {
        g_GetObjID(obj).fireEvent('onclick');

        event.returnValue = false;
        event.cancelBubble = true;
    }
}

/************************************************************************
텍스트 박스의 paste 기능을 막는다. 
onpaste="_CheckPaste()"
*************************************************************************/
function g_CheckPaste()
{
    return false;
}

/************************************************************************
텍스트입력박스에 숫자만 입력가는 하게 함.
onkeypress = "g_OnlyNumber(this)" 
*************************************************************************/
function g_OnlyNumber(obj)
{
    var minusValue = "";
    if (obj.value.length > 0) {
        if (obj.value.substring(0, 1) == "-") {
            minusValue = "-";
        }
    }

    var val = obj.value;
    var filter = /[^0-9]/g;
    if (filter.test(val)) {
        obj.value = val.replace(filter, "");
    }
}

/************************************************************************
텍스트입력박스에 숫자만 입력가는 하게 함.
onkeypress = "g_CheckNumber()" 
*************************************************************************/
function g_CheckNumber()
{
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
        event.cancelBubble = true;
    }
}

/************************************************************************
숫자 입력시 3자리마다 , 붙여줌
onkeyup = "g_currency(this)" 형태로 사용
*************************************************************************/
function g_currency(obj)
{
    if (event.keyCode != 9)
        obj.value = g_ConvertCurrency(Number(g_ConvertCurrency(obj.value, 'rcurrency')), 'currency');
}

/************************************************************************
수치값에 , 를 붙이거나 ,을 빼버리는 함수
fmt: currency -> ','를 붙임.  rcurrency -> ','를 제거.
*************************************************************************/
function g_ConvertCurrency(value, fmt)
{
    var strFmt, strSign;
    var i, nStart, nEnd;

    try {
        var sdata = new String(value);

        if (value == null || value == undefined || sdata == "null") {
            strFmt = "";
            return strFmt;
        }

        for (i = 0; i < sdata.length; i++)
            if (sdata.charAt(i) != " ") break;
        nStart = i;

        for (i = sdata.length - 1; i >= 0; i--)
            if (sdata.charAt(i) != " ") break;
        nEnd = i;

        if (sdata.substr(0, 1) == "-") {
            strSign = "-";
            sdata = sdata.substr(nStart + 1, nEnd - nStart + 2);
        }
        else {
            strSign = "";
            sdata = sdata.substr(nStart, nEnd - nStart + 1);
        }

        if (sdata.length == 0) {
            strFmt = "";
        }
        else if (fmt.toLowerCase() == "currency") {
            nEnd = sdata.length;
            for (i = 0; i < sdata.length; i++) {
                if (sdata.charAt(i) == ".") { nEnd = i; break; }
            }

            strFmt = "";
            for (i = 0; i < nEnd; i++) {
                strFmt = strFmt.concat(sdata.charAt(i));
                if ((nEnd - i - 1) > 0 && (nEnd - i - 1) % 3 == 0) strFmt = strFmt.concat(",");
            }
            strFmt = strSign + strFmt;
            strFmt = strFmt.concat(sdata.substr(nEnd, sdata.length - nEnd));
        }
        else if (fmt.toLowerCase() == "rcurrency") {
            strFmt = sdata.replace(/\,/g, "");
            strFmt = strSign + strFmt;
        }

        return (strFmt);
    }
    catch (exception) {
        alert(exception.message);
    }
}

/************************************************************************
반올림 함수
*************************************************************************/
function g_Round(str, n)
{
    var strvalue, i;
    var temp = "";
    var templen;
    var result;
    var minus = false;

    str = "" + str;

    //음수 체크
    if (str.substr(0, 1) == "-") {
        minus = true;
    }

    // default
    if (n == null) n = 4;

    if (n < 0) {
        //g_OpenInformation("반올림할 자릿수가 지정되지 않았습니다.");
        return str;
    }

    //숫자인지 체크
    if (isNaN(str)) {
        //g_OpenInformation("반올림할 수가 입력되지 않았습니다." + str);
        return 0;
    }

    // 소수점이 없는 경우
    if (str.indexOf(".") < 0) return str;

    //- 부호 제거
    if (minus) str = str.substr(1, str.length);

    // 소수점이 있는 경우
    strvalue = eval(str);

    // 소수점 이동 (오른쪽)
    for (i = 1; i < n; i++) {
        strvalue = strvalue * 10;
    }

    // 반올림을 한다.
    strvalue = Math.round(strvalue);

    // 소수점 이동 (왼쪽)
    result = "" + strvalue;
    if (n > 1) {
        /*
        if(minus)
        alert("minus");
        else
        alert("plus");
        */
        templen = result.length - n + 1;

        if (templen <= 0) {
            temp = minus ? "-0." : "0.";
        }
        else {
            temp = minus ? "-" + result.substr(0, templen) + "." : result.substr(0, templen) + ".";
        }

        temp = temp + result.substr(templen, n - 1);
        result = temp;

    }

    return result;
}

/************************************************************************
입력문자열의  substring 값을 리턴한다.
*************************************************************************/
function g_Substr(str, s, l)
{
    return str.substr(s, l);
}

/************************************************************************
수치값 입력시 정수만 입력하게 하는 함수
onkeypress = "g_CheckNumberSet()"
*************************************************************************/
var PrevNumber = 0;
function g_CheckNumberSet(objID)
{
    var targetObj = null;

    if (objID == undefined || objID == null)
        targetObj = event.srcElement;
    else
        targetObj = g_GetObjID(objID);

    targetObj.style.imeMode = "disabled";
    targetObj.onpaste = g_CheckPaste;
    targetObj.onkeydown = g_CheckNumberDown;
    targetObj.onkeyup = g_CheckNumberUp;
    targetObj.onkeypress = g_CheckNumberPress;
    targetObj.onfocus = g_CheckNumberFocus;
    targetObj.onblur = g_CheckNumberBlur;
    targetObj.select();

    g_EvalScript("focus");
}


// g_CheckNumberSet 서브함수(KeyDown 이벤트핸들러)
function g_CheckNumberDown()
{


    g_CheckkeyCode(false);
    g_EvalScript("keydown");
}

// g_CheckNumberSet 서브함수(KeyDown 이벤트핸들러)
function g_CheckNumberUp()
{
    //'-'기호가 중간에 들어오면 원복시킨다.
    var sValue = event.srcElement.value.replace(/\s/g, "").split("-");
    if (sValue.length > 1) {
        if (g_Trim(sValue[0]) != "") {
            event.srcElement.value = PrevNumber;
        }
    }

    g_CheckkeyCode(false);
    g_EvalScript("keyup");
}


function g_CheckNumberPress()
{
    PrevNumber = event.srcElement.value;
    g_EvalScript("keypress");
}

// g_CheckNumberSet 서브함수(KeyUp 이벤트핸들러)
function g_CheckNumberFocus()
{
    event.srcElement.value = g_ConvertCurrency(event.srcElement.value, 'rcurrency');
    PrevNumber = event.srcElement.value;
    g_EvalScript("focus");
}

// g_CheckNumberSet 서브함수(Blur 이벤트핸들러)
function g_CheckNumberBlur()
{
    if (g_Trim(event.srcElement.value) == "") event.srcElement.value = "0";

    if (g_Trim(event.srcElement.value) != "-") {
        if (isNaN(event.srcElement.value) == true) {
            event.srcElement.value = "0";
        }
    }


    var NowValue = Number(g_ConvertCurrency(event.srcElement.value, 'rcurrency'));
    event.srcElement.value = g_ConvertCurrency(Number(g_ConvertCurrency(event.srcElement.value, 'rcurrency')), 'currency');

    g_EvalScript("blur");
}

// g_CheckNumberSet 서브함수(키코드 체크함수)
function g_CheckkeyCode(isDotAble)
{
    var keyCode = event.keyCode; // 현재키값을 받는다.

    if (!(
		keyCode == 8 || //백스페이스 
		keyCode == 9 || //TAB키
		keyCode == 27 || //ESC키
		keyCode == 45 || //INS키
		keyCode == 46 || //DEL키
		(keyCode >= 33 && keyCode <= 40)/*page up,page down,home,end,방향키*/)) //이외의 다른키가 들어왔을때
    {
        //쉬프트키가 눌려진 상태라면 막는다.
        if (event.shiftKey || event.shiftKey) {
            event.returnValue = false;
            event.cancelBubble = true;
            return;
        }

        //수치아닌경우
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105))) {
            switch (keyCode) // '-','.'기호에 관련된 키가 아니면 막는다.
            {
                case 109:
                case 189:
                    var sValue = event.srcElement.value.replace(/\s/g, "").split("-");
                    // '-'기호에 관련된 키라도 텍스트박스에 '-' 존재한다면 막는다.
                    if (sValue.length > 1) {
                        event.returnValue = false;
                        event.cancelBubble = true;
                        return;
                    }
                    break;
                case 110:
                case 190:
                    //정수라면 막는다.
                    if (!isDotAble) {
                        event.returnValue = false;
                        event.cancelBubble = true;
                        return;
                    }
                    // '-'기호에 관련된 키라도 텍스트박스에 '-' 존재한다면 막는다.
                    if (event.srcElement.value.split(".").length > 1) {
                        event.returnValue = false;
                        event.cancelBubble = true;
                        return;
                    }
                    break;
                default:
                    event.returnValue = false;
                    event.cancelBubble = true;
                    return;
                    break;
            }
        }
    }
}

// g_CheckNumberSet 서브함수(스크립트 실행함수)
function g_EvalScript(eventName)
{
    var eventHandler = event.srcElement.attributes.getNamedItem(eventName);
    if (eventHandler != null || eventHandler != undefined)
        eval(eventHandler.value);
}

/************************************************************************
수치값 입력시 실수만 입력하게 하는 함수
onkeypress = "g_CheckFloatSet()"
numberLen,isCurreny,objID
*************************************************************************/
var PrevFlotNumber = 0;

function g_CheckFloatSet(objID)
{
    var targetObj = null;

    if (objID == undefined || objID == null)
        targetObj = event.srcElement;
    else
        targetObj = g_GetObjID(objID);

    targetObj.style.imeMode = "disabled";
    targetObj.onpaste = g_CheckPaste;
    targetObj.onkeydown = g_CheckFloatDown;
    targetObj.onkeyup = g_CheckFloatUp;
    targetObj.onkeypress = g_CheckFloatPress;
    targetObj.onfocus = g_CheckFloatFocus;
    targetObj.onblur = g_CheckFloatBlur;
    targetObj.select();

    g_EvalScript("focus");
}

// g_CheckFloatSet 서브함수(keyDown 이벤트핸들러)
function g_CheckFloatDown()
{

    g_CheckkeyCode(true);
    g_EvalScript("keydown");

}

// g_CheckFloatSet 서브함수(Keyup 이벤트핸들러)
function g_CheckFloatUp()
{
    //'-'기호가 중간에 들어오면 원복시킨다.
    var sValue = event.srcElement.value.replace(/\s/g, "").split("-");
    if (sValue.length > 1) {
        if (g_Trim(sValue[0]) != "") {
            event.srcElement.value = PrevFlotNumber;
        }
    }

    g_CheckkeyCode(true);
    g_EvalScript("keyup");
}

function g_CheckFloatPress()
{
    PrevFlotNumber = event.srcElement.value;
    g_EvalScript("keypress");
}

function g_CheckFloatFocus()
{

    if (event.srcElement.value.split(".")[0] == "")
        event.srcElement.value = g_ConvertCurrency(event.srcElement.value, 'rcurrency')
    else {
        var temp_val = g_ConvertCurrency(event.srcElement.value.split(".")[0], 'rcurrency');
        if (event.srcElement.value.split(".")[1] != null && g_Trim(event.srcElement.value.split(".")[1]) != "")
            event.srcElement.value = temp_val + "." + g_Trim(event.srcElement.value.split(".")[1]);
    }

    PrevFlotNumber = event.srcElement.value;
    g_EvalScript("focus");
}


// g_CheckFloatSet 서브함수(Blur 이벤트핸들러)
function g_CheckFloatBlur()
{

    if (g_Trim(event.srcElement.value) == "") event.srcElement.value = "0";

    if (g_Trim(event.srcElement.value) != "-") {
        if (isNaN(event.srcElement.value) == true) {
            event.srcElement.value = "0";
        }
    }

    var dotCnt = event.srcElement.value.replace(/\s/g, "").split("-").length;
    if (dotCnt > 1) {
        event.srcElement.value = "0";
    }
    else if (dotCnt == 1) {
        if (event.srcElement.value.split(".")[0] == "")
            event.srcElement.value = vCheckFloat;
        else {
            var temp_val = g_ConvertCurrency(Number(g_ConvertCurrency(event.srcElement.value.split(".")[0], 'rcurrency')), 'currency');
            if (event.srcElement.value.split(".")[1] != null && g_Trim(event.srcElement.value.split(".")[1]) != "")
                event.srcElement.value = temp_val + "." + g_Trim(event.srcElement.value.split(".")[1]);
        }
    }

    g_EvalScript("blur");
}


/************************************ 드롭다운 시작 *************************/
function ut_setCombo(comboid, cd)
{
    var combo = g_GetObjID(comboid);
    for (var i = 0; i < combo.options.length; i++) {
        if (combo.options[i].value == (cd + "")) {
            combo.options[i].selected = true;
            break;
        }
    }
}

function g_getCombo(comboid)
{
    var val = "";
    var combo = g_GetObjID(comboid);

    if (combo.options.length > 0 && combo.selectedIndex >= 0) {
        val = combo.options[combo.selectedIndex].value;
    }

    return val;
}

// 드롭다운 setselected
function g_setComboIndex(comboid, idx)
{
    var combo = g_GetObjID(comboid);

    for (var i = 0; i < combo.options.length; i++) {
        if (i == idx) {
            combo.options[i].selected = true;
            break;
        }
    }
}


// 컨트롤의 값.
function g_getComboText(comboid)
{
    var val = "";
    var combo = g_GetObjID(comboid);

    if (combo.options.length > 0 && combo.selectedIndex >= 0) {
        val = combo.options[combo.selectedIndex].text;
    }

    return val;
}
/************************************ 드롭다운 종료 *************************/

/************************************ 체크박스 시작 *************************/

// 컨트롤의 값.
function g_setCbxChecked(objID, checkflag)
{
    g_GetObjID(objID).checked = checkflag;
}
function ut_getCbxChecked(objID)
{
    return ut_getobj(objID).checked;
}


// checkbox list의 선택된 값.
function g_setCheckBoxs(itemgroup, cd)
{
    var val = "";

    var aryChk = g_getItemList(itemgroup, "c");
    var cnt = aryChk.length;

    var i = 0;
    var j = 0;
    var arycd = cd.split(',');
    var cnt2 = arycd.length;

    for (i = 0; i < cnt; i++) {
        for (j = 0; j < cnt2; j++) {
            if (aryChk[i].value == arycd[j]) {
                aryChk[i].checked = true;
                break;
            }
        }
    }
}
// checkbox list의 선택된 값.
function g_getCheckBoxs(itemgroup)
{
    var val = "";

    var aryChk = ut_getItemList(itemgroup, "c");
    var i = 0;
    var cnt = aryChk.length;

    for (i = 0; i < cnt; i++) {
        if (aryChk[i].checked) {
            val += aryChk[i].value + ",";
        }
    }

    if (val.length > 0) {
        val = val.substr(0, (val.length - 1));
    }
    return val;
}


/************************************ 체크박스 종료 *************************/


/************************************ 라디오 시작 *************************/

// 라디오박스에 cd값을 선택함
function g_setRadio(itemgroup, cd)
{
    var aryChk = ut_getItemList(itemgroup, "r");

    for (var i = 0; i < aryChk.length; i++) {
        aryChk[i].checked = false;
        if (aryChk[i].value == cd) {
            aryChk[i].checked = true;
            break;
        }
    }
}
// 라디오리스트의 checked 값.
function g_getRadio(itemgroup)
{
    var val = "";

    var aryChk = g_getItemList(itemgroup, "r");

    for (var i = 0; i < aryChk.length; i++) {
        if (aryChk[i].checked) {
            val = aryChk[i].value;
            break;
        }
    }

    return val;
}

// 라디오리스트에 cd값을 선택함
function g_setRadioList(itemgroup, cd)
{
    var aryChk = g_getItemList(itemgroup, "r");

    for (var i = 0; i < aryChk.length; i++) {
        aryChk[i].checked = false;
        if (aryChk[i].value == cd) {
            aryChk[i].checked = true;
            break;
        }
    }
}
// 라디오리스트의 checked 값.
function g_getRadioList(itemgroup)
{
    var val = "";

    var aryChk = g_getItemList(itemgroup, "r");

    for (var i = 0; i < aryChk.length; i++) {
        if (aryChk[i].checked) {
            val = aryChk[i].value;
            break;
        }
    }

    return val;
}
/************************************ 라디오 종료 *************************/

/************************************************************************
함수명		: g_getItemList(itemgroup,ptype)
내  용		:  span 이나 div에서 체크박스 or 라디오버튼 컨트롤을 리턴함
parameter   : itemgroup - (td, span, div)컨트롤 ID
ptype = r : 라디오, c : 체크박스
*************************************************************************/
function g_getItemList(itemgroup, ptype)
{
    var form = document.getElementById(itemgroup);
    var aryChk = new Array();

    ptype = ptype.toLowerCase();
    if (ptype == "c")
        ptype = "checkbox";
    else if (ptype == "r")
        ptype = "radio";
    var chkTypeCnt = -1;
    for (var i = 0; i < form.all.length; i++) {
        if (form.all[i].type == ptype) {
            aryChk[++chkTypeCnt] = form.all[i];
        }
    }

    return aryChk;
}

/************************************************************************
콤보박스에 OPTION 추가 시킨다.
*************************************************************************/
function g_ComboBoxBind(ComboObj, StringValue)
{
    ComboObj.options.length = 0
    if (g_isEmpty(StringValue) == "") {
        var tVal = g_GetStringTerminator(StringValue);
        if (tVal.length != undefined) {
            for (var i = 0; i < tVal.length; i++) {
                if (g_isEmpty(tVal[i]) == false) {
                    var sVal = g_GetStringSeparator(tVal[i]);
                    if (sVal.length != undefined) {
                        if (g_isEmpty(sVal[1]) == false) {
                            ComboObj.options[i] = new Option(sVal[1], sVal[0])
                        }
                    }
                }
            }
        }
    }

    if (ComboObj.options.length > 0) {
        ComboObj.options[0].selected = true;
        ComboObj.selectedIndex = 0;
        if (ComboObj.options.length == 1 && ComboObj.options[0].value == "") {
            g_DisableControl(ComboObj);
        }
        else g_EnableControl(ComboObj);
    }
    else g_DisableControl(ComboObj);
}

/************************************************************************
콤보박스에 임의 값을 선택 시킨다.
*************************************************************************/
function g_SetComboValue(objID, value)
{
    var combo = g_GetObjID(objID);
    if (combo == null) return;

    var cnt = 0;
    for (cnt = 0; cnt < combo.options.length; cnt++) {
        if (combo.options[cnt].value == value) {
            combo.options[cnt].selected = true;
            combo.selectedIndex = cnt;
        }
        else {
            combo.options[cnt].selected = false;
        }
    }
}

/************************************************************************
컴보박스등의 select 박스의 selected되어진 값을 가져온다. value가 아닌 text값		
*************************************************************************/
function g_GetSelectText(selectBoxID)
{
    return g_GetObjID(selectBoxID).options[g_GetObjID(selectBoxID).selectedIndex].text;
}

/************************************************************************
컴보박스등의 select 박스의 selected되어진 값을 가져온다. text가 아닌 value값		
*************************************************************************/
function g_GetSelectValue(selectBoxID)
{
    return g_GetObjID(selectBoxID).options[g_GetObjID(selectBoxID).selectedIndex].value;
}


/************************************************************************
객체 또는 값의 길이를 리턴한다.			
*************************************************************************/
function g_GetLength(obj)
{
    var str = "";
    if (typeof (obj) == "object") str = obj.value;
    else str = obj;

    var iLen = 0;
    for (var inx = 0; inx < str.length; inx++) {
        var oneChar = escape(str.charAt(inx));
        if (oneChar.length == 1) {
            iLen++;
        } else if (oneChar.indexOf("%u") != -1) {
            iLen += 2;
        } else if (oneChar.indexOf("%") != -1) {
            iLen += oneChar.length / 3;
        }
    }
    return iLen;

    //return ( str.length + ( escape( str ) + "%u" ).match( /%u/g ).length - 1 );
}

/************************************************************************
객체 또는 값의 죄우 공객을 없에는 트림 작업을 한다.	
*************************************************************************/
function g_Trim(obj)
{
    var str = ((typeof obj == "object") ? obj.value : obj);
    if (str == null) return "";
    var len = str.length;

    for (var i = 0; i < len; i++) {
        if (str.charAt(i) != " ") {
            str = str.substr(i, len - i);
            break;
        }
    }

    len = str.length;
    for (var i = len - 1; i >= 0; i--) {
        if (str.charAt(i) != " ") {
            str = str.substr(0, i + 1);
            break;
        }
    }
    return str;
}

/********************************************************************
입력값이 NULL인지 체크
********************************************************************/
function g_isNull(obj)
{
    var str = ((typeof obj == "object") ? obj.value : obj);

    if (str == null || str == "") {
        return true;
    }
    return false;
}

/********************************************************************
입력값에 스페이스 이외의 의미있는 값이 있는지 체크
********************************************************************/
function g_isEmpty(obj)
{
    var str = ((typeof obj == "object") ? obj.value : obj);

    if (str == null || str.replace(/ /gi, "") == "") {
        return true;
    }
    return false;
}

/********************************************************************
입력값에 특정 문자(chars)가 있는지 체크
특정 문자를 허용하지 않으려 할 때 사용 inChar(form.name,"!,*&^%$#@~;")
********************************************************************/
function g_inChar(obj, chars)
{

    var str = ((typeof obj == "object") ? obj.value : obj);

    if (str == null) return false;
    if (str.length == null) return false;
    if (str.length == undefined) return false;

    for (var inx = 0; inx < str.length; inx++) {
        if (chars.indexOf(str.charAt(inx)) != -1)
            return true;
    }
    return false;
}

var gNumber = "0123456789";
var gCurrency = "0123456789,";
var gFloat = "0123456789,.";

/********************************************************************
입력값이 특정 문자(chars)만으로 되어있는지 체크
특정 문자만 허용하려 할 때 사용
********************************************************************/
function g_inCharsOnly(obj, chars)
{
    var str = ((typeof obj == "object") ? obj.value : obj);


    if (str.length == null) return false;

    for (var inx = 0; inx < str.length; inx++) {
        if (chars.indexOf(str.charAt(inx)) == -1)
            return false;
    }
    return true;
}

/********************************************************************
입력값에 숫자만 있는지 체크
Type: N: Normal Integer; C:Currency include , ; F:Float include , .
********************************************************************/
function g_isNumber(obj, Type)
{
    var chars = gNumber;
    if (Type == "C") chars = gCurrency;
    if (Type == "F") chars = gFloat;

    return g_inCharsOnly(obj, chars);
}

/********************************************************************
유효한(존재하는) 월(月)인지 체크
********************************************************************/
function g_isValidMonth(mm)
{
    var str = ((typeof mm == "object") ? mm.value : mm);
    var m = parseInt(str, 10);
    return (m >= 1 && m <= 12);
}

/********************************************************************
유효한(존재하는) 일(日)인지 체크
********************************************************************/
function g_isValidDay(yyyy, mm, dd)
{
    var m = parseInt(mm, 10) - 1;
    var d = parseInt(dd, 10);
    if (!g_isNumber(yyyy, "c"))
        return false;

    var end = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    if ((yyyy % 4 == 0 && yyyy % 100 != 0) || yyyy % 400 == 0) {
        end[1] = 29;
    }

    return (d >= 1 && d <= end[m]);
}

/********************************************************************
유효한(존재하는) 일(日)인지 체크
2006-07-23 또는 20060723
********************************************************************/
function g_isValidDate(obj)
{

    var str = ((typeof obj == "object") ? obj.value : obj);
    str = g_setReplace(str, "-", "");
    str = g_setReplace(str, ".", "");

    if (str.length != 8)
        return false;

    var year = str.substr(0, 4);
    var month = str.substr(4, 2);
    var day = str.substr(6, 2);

    return g_isValidDay(year, month, day);
}

/********************************************************************
유효하는(존재하는) Time 인지 체크
ex)
var time = form.time.value; //'200102310000'
if (!isValidTime(time)) {
alert("올바른 날짜가 아닙니다.");
}
********************************************************************/
function g_isValidDateTime(Datex)
{
    var str = ((typeof Datex == "object") ? Datex.value : Datex);

    var year = str.substring(0, 4);
    var month = str.substring(4, 6);
    var day = str.substring(6, 8);
    var hour = str.substring(8, 10);
    var min = str.substring(10, 12);

    if (parseInt(year, 10) >= 1900 && g_isValidMonth(month) &&
		g_isValidDay(year, month, day)) {
        return true;
    }
    return false;
}

/********************************************************************
자바스크립트 Date 객체를 Time 스트링으로 변환
parameter date: JavaScript Date Object
********************************************************************/
function g_toTimeString(date)
{
    var str = date;
    var year = str.getFullYear();
    var month = str.getMonth() + 1; // 1월=0,12월=11이므로 1 더함
    var day = str.getDate();
    var hour = str.getHours();
    var min = str.getMinutes();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }
    if (("" + hour).length == 1) { hour = "0" + hour; }
    if (("" + min).length == 1) { min = "0" + min; }

    return ("" + year + month + day + hour + min)
}

/********************************************************************
오늘날짜
********************************************************************/
function g_getToDay()
{
    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth() + 1; // 1월=0,12월=11이므로 1 더함
    var day = date.getDate();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }

    return ("" + year + month + day)
}

/********************************************************************
오늘날짜 미들바 있는
********************************************************************/
function g_getToDay2()
{
    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth() + 1; // 1월=0,12월=11이므로 1 더함
    var day = date.getDate();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }

    return ("" + year + "-" + month + "-" + day)
}

/********************************************************************
오늘날짜에서 한달 빠진 날짜
********************************************************************/
function g_getToDay3()
{
    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth(); // 1월=0,12월=11이므로 1 더함
    var day = date.getDate();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }

    return ("" + year + "-" + month + "-" + day)
}

/********************************************************************
현재 시각을 Time 형식으로 리턴
********************************************************************/
function g_getCurrentTime()
{
    return g_toTimeString(new Date());
}

/********************************************************************
현재 年을 YYYY형식으로 리턴
********************************************************************/
function g_getYear()
{

    return g_getCurrentTime().substr(0, 4);
}

/********************************************************************
현재 月을 MM형식으로 리턴
********************************************************************/
function g_getMonth()
{

    return g_getCurrentTime().substr(4, 2);
}

/********************************************************************
현재 日을 DD형식으로 리턴
********************************************************************/
function g_getDay()
{

    return g_getCurrentTime().substr(6, 2);
}

/********************************************************************
월의 끝 일자 얻기
********************************************************************/
function g_getEndDate(sDate)
{
    //널체크
    if (g_isEmpty(sDate)) {
        return null;
    }

    //숫자체크
    if (!g_isNumber(sDate, "C")) {
        return null;
    }

    //길이가 8자리 체크
    if (sDate.length != 6) {
        return null;
    }

    var str = ((typeof sDate == "object") ? sDate.value : sDate);
    var yy = Number(str.substring(0, 4));
    var mm = Number(str.substring(4, 6));

    //윤년 검증
    var boundDay = "";

    if (mm != 2) {
        var mon = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        boundDay = mon[mm - 1];
    }
    else {
        if (yy % 4 == 0 && yy % 100 != 0 || yy % 400 == 0)
            boundDay = 29;
        else
            boundDay = 28;
    }
    return boundDay;
}

/********************************************************************
오늘의 요일을 리턴
********************************************************************/
function g_getDayOfWeek()
{
    var now = new Date();

    var day = now.getDay();
    var week = ["일", "월", "화", "수", "목", "금", "토"];

    return week[day];
}

/********************************************************************
특정날짜의 요일을 구한다.
********************************************************************/
function g_getDayOfWeek(time)
{
    var now = g_toTimeObject(time);

    var day = now.getDay();
    var week = ["일", "월", "화", "수", "목", "금", "토"];

    return week[day];
}

/********************************************************************
문자(20060901) -> 날짜형식(2006-09-01)
********************************************************************/
function g_frmDate(obj)
{
    var str = ((typeof obj == "object") ? obj.value : obj);

    if (g_isEmpty(str) == true) return "";

    str = str.substring(0, 4) + "-"
				+ str.substring(4, 6) + "-"
				+ str.substring(6, 8);
    return str;
}

/********************************************************************
날짜(2006-09-01) -> 문자(20060901)
********************************************************************/
function g_unFrmDate(obj)
{

    var str = ((typeof obj == "object") ? obj.value : obj);

    str = g_setReplace(str, "-", "");
    return str;
}

/********************************************************************
두개의 날짜를 비교하는 함수
********************************************************************/
function g_isValidDateTime(DateS, DateE)
{
    var strS = ((typeof DateS == "object") ? DateS.value : DateS);
    var strE = ((typeof DateE == "object") ? DateE.value : DateE);
    strS = g_setReplace(strS, "-", "");
    strE = g_setReplace(strE, "-", "");
    strS = g_setReplace(strS, ".", "");
    strE = g_setReplace(strE, ".", "");


    if ((parseInt(strE, 10) - parseInt(strS, 10)) >= 0) {
        return true;
    }
    else {
        return false;
    }

}

/********************************************************************
두개의 날짜를 비교하는 함수
********************************************************************/
function g_isValidDateFromTo(DateS, DateE)
{
    if (!g_isValidDate(DateS) || !g_isValidDate(DateE)) {
        alert('날짜형식이 잘못되었습니다. 확인해주세요.')
        return false;
    }

    if (!g_isValidDateTime(DateS, DateE)) {
        alert('뒤의 날짜가 앞의 날짜보다 앞설수 없습니다.');
        return false;
    }
    return true;
}

/********************************************************************
대문자변환
********************************************************************/
function g_setToUCase(obj)
{

    var str = ((typeof obj == "object") ? obj.value : obj);
    if (g_isEmpty(str)) return str;

    return str.toUpperCase();
}

/********************************************************************
소문자변환
********************************************************************/
function g_setToLCase(obj)
{
    if (g_isEmpty(str)) return str;

    var str = ((typeof obj == "object") ? obj.value : obj);
    return str.toLowerCase();
}

/********************************************************************
분리 구분자의 값을 나누어 배열로 리턴
********************************************************************/
function g_GetStringSeparator(strVar)
{
    try {

        if (strVar.indexOf('⊥') > -1) return strVar.split('⊥');
        return strVar;
    }
    catch (Exception) {
        return strVar;
    }
}

/********************************************************************
종결 구분자의 값을 나누어 배열로 리턴
********************************************************************/
function g_GetStringTerminator(strVar)
{
    try {
        //return strVar.split('⌒');
        if (strVar.indexOf('⌒') > -1) return strVar.split('⌒');
        return strVar;
    }
    catch (Exception) {
        return strVar;
    }
}

/********************************************************************
구분자를 입력받아 배열로 리턴
********************************************************************/
function g_GetStringSplit(strVar, strSplit)
{
    try {
        if (strVar.indexOf(strSplit) > -1) return strVar.split(strSplit);
        return strVar;
    }
    catch (Exception) {
        return strVar;
    }
}

/********************************************************************
입력 폼에 시스템 문자를 입력 했는지 검사
********************************************************************/
function g_FormDataValidate(formId)
{
    var form = g_GetObjID(formId);

    for (var e = 0; e < form.elements.length; e++) {
        var el = form.elements[e];
        if (el.type == 'text') {

            el.value = g_setReplace(el.value, "\"", "'");

            if (el.value.match(/["⊥⌒]/g) != null) {
                if (el.value.match(/["⊥⌒]/g).length != undefined && el.value.match(/["⊥⌒]/g).length > 0) {
                    g_OpenInformation("시스템에서 사용하는 문자가 사용되었습니다.<BR> 이 문자를 제거해주시기 바랍니다.");
                    el.focus();
                    return false;
                }
            }
        }
    }

    if (g_GetObjID("JwEditor") != null) {
        // g_GetObjID("JwEditor").AllText =g_setReplace(g_GetObjID("JwEditor").AllText,"\"","'");
        if (g_GetObjID("JwEditor").AllText.match(/[⊥⌒]/g) != null) {
            if (g_GetObjID("JwEditor").AllText.match(/[⊥⌒]/g).length != undefined && g_GetObjID("JwEditor").AllText.match(/[⊥⌒]/g).length > 0) {
                g_OpenInformation("시스템에서 사용하는 문자가 사용되었습니다.<BR> 이 문자를 제거해주시기 바랍니다.");
                g_GetObjID("JwEditor").focus();
                return false;
            }
        }
    }

    return true;
}

/********************************************************************
조건 입력시 입력 길이 체크 하는 함수 조회 조건에 들어갈 값은 2자 이상이어야 한다.
이 함수는 Like 검색 조건식에 필 수 로 사용해야 한다.
********************************************************************/
function g_CheckQueryLength(obj)
{
    if (g_GetLength(obj) == 0 || g_GetLength(obj) > 2) return true;
    g_OpenInformation("검색 조건식의 입력 값은 3자리 이상이어야 합니다.");
    return false;
}

/********************************************************************
일반적인 Ajax 기능을 이용할때 리턴된 값의 에러를 체크하고 메세지 처리해준다.
********************************************************************/
function g_Ajax_Error(response)
{
    if (response.error != null) {
        alert(response.error);
        return false;
    }

    if (response.value == null) {
        alert("데이터를 처리 중 오류가 발생하였습니다.");
        return false;
    }

    if (g_Trim(response.value[0]) != SUCCESS) {
        alert(g_Trim(response.value[1]));
        return false;
    }
    return true;
}

var gUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
var gLowerCase = "abcdefghijklmnopqrstuvwxyz";
var gNumber = "0123456789";
/********************************************************************
입력값이 알파벳,숫자로 되어있는지 체크
********************************************************************/
function g_isAlphaNum(obj)
{
    var chars = gUpperCase + gLowerCase + gNumber + " ";
    return g_inCharsOnly(obj, chars);
}

/********************************************************************
구분자로 구분된 자료에서 원하는 값이 있는지 검사한다.
********************************************************************/
function g_isCheckSeparatorDatas(strSData, strKey)
{
    var tVal = g_GetStringSeparator(strSData);
    if (tVal.length == null) return false;
    if (tVal.length <= 0) return false;

    for (var i = 0; i < tVal.length; i++) {
        if (g_Trim(g_setToUCase(tVal[i])) == g_Trim(g_setToUCase(strKey))) {
            g_OpenInformation("이미 등록된 정보입니다.");
            return true;
        }
    }

    return false;
}

/********************************************************************
Acuate 실행
frameID: Actuate로 보낼 파라미터가 있는 폼
frameID: Actuate가 그려질 Iframe 명
********************************************************************/
function g_ExcuteActuate(formID, frameID)
{

    var frm = document.all(formID);
    var frame = document.all(frameID);

    // From을 전송하기위한 정보를 설정한다.
    frm.target = frameID;
    frm.submit();
}

/********************************************************************
Acuate 암호 함수 
********************************************************************/
function fn_ActuateEncode(s, charEnc)
{
    s = new String(s);
    if (charEnc) {
        charEnc = new String(charEnc);
    }
    else {
        charEnc = "%";
    }

    var sbuf = new String("");

    var hexArray = new Array(
		"00", "01", "02", "03", "04", "05", "06", "07",
		"08", "09", "0a", "0b", "0c", "0d", "0e", "0f",
		"10", "11", "12", "13", "14", "15", "16", "17",
		"18", "19", "1a", "1b", "1c", "1d", "1e", "1f",
		"20", "21", "22", "23", "24", "25", "26", "27",
		"28", "29", "2a", "2b", "2c", "2d", "2e", "2f",
		"30", "31", "32", "33", "34", "35", "36", "37",
		"38", "39", "3a", "3b", "3c", "3d", "3e", "3f",
		"40", "41", "42", "43", "44", "45", "46", "47",
		"48", "49", "4a", "4b", "4c", "4d", "4e", "4f",
		"50", "51", "52", "53", "54", "55", "56", "57",
		"58", "59", "5a", "5b", "5c", "5d", "5e", "5f",
		"60", "61", "62", "63", "64", "65", "66", "67",
		"68", "69", "6a", "6b", "6c", "6d", "6e", "6f",
		"70", "71", "72", "73", "74", "75", "76", "77",
		"78", "79", "7a", "7b", "7c", "7d", "7e", "7f",
		"80", "81", "82", "83", "84", "85", "86", "87",
		"88", "89", "8a", "8b", "8c", "8d", "8e", "8f",
		"90", "91", "92", "93", "94", "95", "96", "97",
		"98", "99", "9a", "9b", "9c", "9d", "9e", "9f",
		"a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7",
		"a8", "a9", "aa", "ab", "ac", "ad", "ae", "af",
		"b0", "b1", "b2", "b3", "b4", "b5", "b6", "b7",
		"b8", "b9", "ba", "bb", "bc", "bd", "be", "bf",
		"c0", "c1", "c2", "c3", "c4", "c5", "c6", "c7",
		"c8", "c9", "ca", "cb", "cc", "cd", "ce", "cf",
		"d0", "d1", "d2", "d3", "d4", "d5", "d6", "d7",
		"d8", "d9", "da", "db", "dc", "dd", "de", "df",
		"e0", "e1", "e2", "e3", "e4", "e5", "e6", "e7",
		"e8", "e9", "ea", "eb", "ec", "ed", "ee", "ef",
		"f0", "f1", "f2", "f3", "f4", "f5", "f6", "f7",
		"f8", "f9", "fa", "fb", "fc", "fd", "fe", "ff"
	);

    var len = s.length;
    for (var i = 0; i < len; i++) {
        var ch = s.charAt(i);
        var chCode = s.charCodeAt(i);
        if ('A' <= ch && ch <= 'Z') {  // 'A'..'Z'
            sbuf += ch;
        }
        else if ('a' <= ch && ch <= 'z') // 'a'..'z'
        {
            sbuf += ch;
        }
        else if ('0' <= ch && ch <= '9') // '0'..'9'
        {
            sbuf += ch;
        }
        else if (chCode <= 0x007f) // other ASCII
        {
            sbuf += charEnc + hexArray[chCode];
        }
        else if (chCode <= 0x07FF)  // non-ASCII <= 0x7FF
        {
            sbuf += charEnc + (hexArray[0xc0 | (chCode >> 6)]);
            sbuf += charEnc + (hexArray[0x80 | (chCode & 0x3F)]);
        }
        else     // 0x7FF < ch <= 0xFFFF
        {
            sbuf += charEnc + (hexArray[0xe0 | (chCode >> 12)]);
            sbuf += charEnc + (hexArray[0x80 | ((chCode >> 6) & 0x3F)]);
            sbuf += charEnc + (hexArray[0x80 | (chCode & 0x3F)]);
        }
    }
    return sbuf;
}

/********************************************************************
Excel Upload 에러 처리 함수 
********************************************************************/
function g_ExcelUploadError(strMsg)
{
    if (strMsg.indexOf("[File 에러!!!]") >= 0) {
        g_OpenInformation(strMsg);
        return false;
    }
    return true;

}

/********************************************************************
IFrame 리사이즈 함수
********************************************************************/
function g_IframesResize()
{
    var s = self;

    while (window.top != s) {
        s = s.parent;

        var ifs = s.document.getElementsByTagName('IFRAME');

        for (var i = 0; i < ifs.length; i++) {
            g_IframeResize(ifs[i]);
        }
    }
}

function g_IframeResize(iframe)
{
    try {
        if (window.navigator.appName.indexOf("Explorer") != -1) {
            var doc = iframe.contentWindow.document;
            var h = doc.body.scrollHeight;
        }
        else {
            var doc = iframe.contentWindow.document;
            var s = doc.body.appendChild(document.createElement('DIV'));
            s.style.clear = 'both';

            var h = s.offsetTop;
            s.parentNode.removeChild(s);
        }

        iframe.style.height = h + 'px';

        //setTimeout(function(){ g_IframeResize(iframe) }, 500); // for IE bug
    }
    catch (e) {
        //alert(e);
    }

    /*
    try
    {	
    var oBody = iframe.document.body;
			
    iframe.style.height = oBody.scrollHeight + (oBody.offsetHeight - oBody.clientHeight);
    }
    catch(ex)
    {
    window.status =	'Error: ' + ex.number + '; ' + ex.description;
    }
    */
}

/********************************************************************
문자(20060901) -> 날짜형식(2006-09-01)
type : 'Y' -> 년도
type : 'M' -> 년월
type : 'D' -> 년월일
********************************************************************/
function g_frmDateType(obj, type)
{
    if (obj.value == "") return;
    var sValue = obj.value.replace(/-/gi, "");
    //debugger;
    if (isNaN(sValue)) {
        obj.value = "";
        return;
    }

    if (type == "Y") {
        if (sValue.length != 4) {
            obj.value = "";
            alert("날짜형식이 안맞습니다.");
            return;
        }
        else {
            obj.value = sValue;
            return;
        }
    }
    else if (type == "M") {
        if (sValue.length != 6) {
            obj.value = "";
            alert("날짜형식이 안맞습니다.");
            return;
        }
        else {
            if (Number(sValue.substring(4, 6)) > 12) {
                obj.value = "";
                return;
            }
            else {
                obj.value = sValue.substring(0, 4) + "-"
				    + sValue.substring(4, 6);
            }
        }
    }
    else if (type == "D") {
        if (sValue.length != 8) {
            obj.value = "";
            alert("날짜형식이 안맞습니다.");
            return;
        }
        else {
            var year = sValue.substr(0, 4);
            var month = sValue.substr(4, 2);
            var day = sValue.substr(6, 2);

            if (g_isValidDay(year, month, day) == false) {
                obj.value = "";
                return;
            }
            else {
                obj.value = sValue.substring(0, 4) + "-"
				        + sValue.substring(4, 6) + "-"
				        + sValue.substring(6, 8);
            }
        }
    }
}

/************************************************************************
함수명		: fn_BriefPrint_Open()
내  용		: 요약지 인쇄
parameter   : 
작 성 자	: KYG
작 성 일	: 2011.09.27
수 정 일	: 
수정내역	:
*************************************************************************/
function fn_BriefPrint_Open(Patients_User_Seq)
{
    fn_BriefPrintSelect_Open(Patients_User_Seq);
    /*
    var strURL = "../Common/PatientsDetailBriefPrint_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq;
    var strURLb = "../Common/PatientsDetailBriefPrintb_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq;
    var strURL2 = "../Common/PatientsDetailBriefDeseasePrint_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq;
    var strURLb2 = "../Common/PatientsDetailBriefDeseasePrintb_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq;
    var sParam, sParam2 = "";
    var aReturnValue = new Array();
    aReturnValue = "";

    returnType = g_OpenModalDialog3("../Common/PatientsDetailBriefPrintSelect_Pop.aspx", aReturnValue, "dialogWidth:395px;dialogHeight:150px;status:no;scroll:yes;help:No;resizable:Yes;");
    if (returnType == null)
    return;
    else if (returnType == "11")
    returnType = g_OpenModalDialog3(strURL, aReturnValue, "dialogWidth:675px;dialogHeight:700px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "12")
    returnType = g_OpenModalDialog3(strURLb, aReturnValue, "dialogWidth:675px;dialogHeight:700px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "21")
    returnType = g_OpenModalDialog3(strURL2, aReturnValue, "dialogWidth:675px;dialogHeight:700px;status:no;scroll:yes;help:No;resizable:Yes;");
    else
    returnType = g_OpenModalDialog3(strURLb2, aReturnValue, "dialogWidth:675px;dialogHeight:700px;status:no;scroll:yes;help:No;resizable:Yes;");
    */
}

function fn_BriefPrintSelect_Open(Patients_User_Seq)
{ //1.5차
    var returnType;
    var aReturnValue = new Array();
    aReturnValue = "";

    returnType = g_OpenModalDialog3("../Common/PatientsCCPrintSelect_Pop.aspx", aReturnValue, "dialogWidth:650px;dialogHeight:280px;status:no;scroll:yes;help:No;resizable:Yes;");
    if (returnType == null)
        return;
    //질환관리        
    else if (returnType == "11")
        returnType = g_OpenModalDialog3("../Common/PatientsCommonPrint_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "1b")
        returnType = g_OpenModalDialog3("../Common/PatientsCommonPrintB_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    //관리내역(일반)
    else if (returnType == "21")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23Print_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "2b")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23PrintB_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    //관리내역(혈당)    
    else if (returnType == "31")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23Print_Pop.aspx?mcode=003&PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "3b")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23PrintB_Pop.aspx?mcode=003&PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    //관리내역(온라인)
    else if (returnType == "41")
        returnType = g_OpenModalDialog3("../Common/PatientsCodePrint_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "4b")
        returnType = g_OpenModalDialog3("../Common/PatientsCodePrintB_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");

}

/************************************************************************
함수명		: fn_BriefMeasure_Open()
내  용		: 측정결과 분석
parameter   : 
작 성 자	: KYG
작 성 일	: 2011.09.20
수 정 일	: 
수정내역	:
*************************************************************************/
function fn_BriefMeasure_Open(sType, sSeq, sABNORMAL_YN)
{
    var strURL = "../Common/PatientsMeasureBrief_Pop.aspx?TYPE=" + sType + "&SEQ=" + sSeq + "&ABNORMAL_YN=" + sABNORMAL_YN;
    var sParam, sParam2 = "";
    var aReturnValue = new Array();
    //sParam2 = "TopMenu=" + "S" + "&LeftMenu=" + "2";
    aReturnValue = "";

    try {
        var astrurl = strURL;
        var height = "245px";
        if (sType == "CHOLESTEROL")
            height = "400px";

        g_OpenModalDialog3(astrurl, aReturnValue, "dialogWidth:742px;dialogHeight:" + height + ";status:no;scroll:no;help:No;resizable:Yes;");
    }
    catch (exception) {
        alert("fn_BriefMeasure_Open : " + exception.description);
    }
}

function popUpCalendarCB(ctl, ctl2, format)
{
    if (window.navigator.userAgent.toLocaleLowerCase().indexOf('msie') == -1) {

        showCalendarPopup(ctl2);

    }
    else {
        popUpCalendar(ctl, ctl2, format)

    }
}



function showCalendarPopup(ymd)
{

    var _argObj = new Object();
    _argObj.item = new Object();

    _argObj.format = "DD-MM-YYYY";

    var return_val = window.showModalDialog("cal1.htm", _argObj, "dialogWidth:400px; dialogHeight:400px; resizable=no; help:no; status:no; scroll=no;center=yes; ");

    if (typeof (return_val) == "undefined" || return_val == null) {
        return;
    }

    ymd.value = return_val;
    return return_val;
}
