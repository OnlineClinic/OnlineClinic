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
ȭ�� ó�� ASPX�� Ŭ���̾�Ʈ�� Load�� �� �����ؾ� �� ���� ó��
********************************************************************/
function g_WindowOnLoad(strPortletID)
{
    //���� �޽����� �ִ� ��� �˾� ���
    var obj = eval("document.all.HErrorMessage_" + strPortletID);
    if (obj.value.length > 0) g_OpenErrorMessage(obj.value);

    //Information�� �ִ� ��� �˾� ���
    var obj = eval("document.all.HInformationMessage_" + strPortletID);
    if (obj.value.length > 0) g_OpenInformation(obj.value);

    // �齺���̽��� ���� �������� ���� ���� ���´�.
    document.onkeydown = g_PreventNavigateBack;

    try {
        // ���¹� Text ����
        window.status = "";
    }
    catch (exception) {
        alert(exception.message + " (g_WindowOnLoad) ");
    }

}

/********************************************************************
�ؽ�Ʈ �ڽ� �ܴ̿� backspace �Է��� �����Ѵ�.
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
���� �޽��� ���ڸ� ����.
********************************************************************/
function g_OpenErrorMessage(strErr)
{
    window.showModalDialog(COMMONPAGE_WEB_PATH + "ErrorMessage.aspx", strErr, "dialogWidth:405px;dialogHeight:260px;status=no;scroll=no");
}

/********************************************************************
�۾����� ���ڸ� ����.
********************************************************************/
function g_OpenInformation(sInfo)
{

    window.showModalDialog(COMMONPAGE_WEB_PATH + "InformationMessage.aspx", sInfo, "dialogWidth:405px;dialogHeight:260px;status=no;scroll=no");
}

/********************************************************************
���� ���ڸ� ����.
********************************************************************/
function g_OpenConfirm(sCfm)
{

    var res = window.showModalDialog(COMMONPAGE_WEB_PATH + "ConfirmMessage.aspx", sCfm, "dialogWidth:405px;dialogHeight:260px;status=no;scroll=no");
    return res;
}


/********************************************************************
�˾�â�� ����.
********************************************************************/
function g_OpenDialog(sUrl, sFrame, sFeature)
{
    return window.open(sUrl, sFrame, sFeature);
}

/********************************************************************
�˾�â�� ����.
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
�˾�â�� ����.
********************************************************************/
function g_Redirect(sUrl, sArg)
{
    var tmpUrl = sUrl + "?" + escape(sArg);
    alert(tmpUrl);
    document.location.replace(tmpUrl);
}


/********************************************************************
������ Ư�����ڸ� ���ù��ڷ� ��ȯ�Ѵ�.
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
�ش� �̹��� Control�� disable��Ű�� ���콺 ����� default�� �����Ѵ�.
********************************************************************/
function g_DisableControl(oImgBtn)
{
    oImgBtn.disabled = true;
    oImgBtn.style.cursor = "default";
    oImgBtn.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=30)";
    oImgBtn.onclick = "";
}

/********************************************************************
�ش� �̹��� Control�� disable��Ű�� ���콺 ����� default�� �����Ѵ�.
********************************************************************/
function g_EnableControl(oImgBtn)
{
    oImgBtn.disabled = false;
    oImgBtn.style.cursor = "default";
    oImgBtn.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=30)";
    oImgBtn.onclick = "";
}

/********************************************************************
�Ϲ� Calendar obj:�Է� ��Ʈ��
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
            g_OpenInformation("�ùٸ� ��¥ ������ �����Ͱ� �ƴϹǷ� ���ó����� �޷��� �����帳�ϴ�.");
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
Div Display Toggle �Լ� 
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
Progress bar start�� ���¿��� ����ڿ� ���� �̺�Ʈ�� ���� �Լ�
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
ID�� ��ü�� ��������� ��ü�� ���� ��������
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
g_GetObjID �� clone
*************************************************************************/
function $(objID)
{
    if (typeof (objID) == 'string')
        return document.getElementById(objID);
    else
        return objID;
}

/************************************************************************
������ư���� üũ�Ǿ��ִ°��� value����������´�.
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
����Ű ó�� : ����Ű�� ġ�� ����Ǿ���� ��ü�� Onclick�̺�Ʈ�� �����Ų��.
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
�ؽ�Ʈ �ڽ��� paste ����� ���´�. 
onpaste="_CheckPaste()"
*************************************************************************/
function g_CheckPaste()
{
    return false;
}

/************************************************************************
�ؽ�Ʈ�Է¹ڽ��� ���ڸ� �Է°��� �ϰ� ��.
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
�ؽ�Ʈ�Է¹ڽ��� ���ڸ� �Է°��� �ϰ� ��.
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
���� �Է½� 3�ڸ����� , �ٿ���
onkeyup = "g_currency(this)" ���·� ���
*************************************************************************/
function g_currency(obj)
{
    if (event.keyCode != 9)
        obj.value = g_ConvertCurrency(Number(g_ConvertCurrency(obj.value, 'rcurrency')), 'currency');
}

/************************************************************************
��ġ���� , �� ���̰ų� ,�� �������� �Լ�
fmt: currency -> ','�� ����.  rcurrency -> ','�� ����.
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
�ݿø� �Լ�
*************************************************************************/
function g_Round(str, n)
{
    var strvalue, i;
    var temp = "";
    var templen;
    var result;
    var minus = false;

    str = "" + str;

    //���� üũ
    if (str.substr(0, 1) == "-") {
        minus = true;
    }

    // default
    if (n == null) n = 4;

    if (n < 0) {
        //g_OpenInformation("�ݿø��� �ڸ����� �������� �ʾҽ��ϴ�.");
        return str;
    }

    //�������� üũ
    if (isNaN(str)) {
        //g_OpenInformation("�ݿø��� ���� �Էµ��� �ʾҽ��ϴ�." + str);
        return 0;
    }

    // �Ҽ����� ���� ���
    if (str.indexOf(".") < 0) return str;

    //- ��ȣ ����
    if (minus) str = str.substr(1, str.length);

    // �Ҽ����� �ִ� ���
    strvalue = eval(str);

    // �Ҽ��� �̵� (������)
    for (i = 1; i < n; i++) {
        strvalue = strvalue * 10;
    }

    // �ݿø��� �Ѵ�.
    strvalue = Math.round(strvalue);

    // �Ҽ��� �̵� (����)
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
�Է¹��ڿ���  substring ���� �����Ѵ�.
*************************************************************************/
function g_Substr(str, s, l)
{
    return str.substr(s, l);
}

/************************************************************************
��ġ�� �Է½� ������ �Է��ϰ� �ϴ� �Լ�
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


// g_CheckNumberSet �����Լ�(KeyDown �̺�Ʈ�ڵ鷯)
function g_CheckNumberDown()
{


    g_CheckkeyCode(false);
    g_EvalScript("keydown");
}

// g_CheckNumberSet �����Լ�(KeyDown �̺�Ʈ�ڵ鷯)
function g_CheckNumberUp()
{
    //'-'��ȣ�� �߰��� ������ ������Ų��.
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

// g_CheckNumberSet �����Լ�(KeyUp �̺�Ʈ�ڵ鷯)
function g_CheckNumberFocus()
{
    event.srcElement.value = g_ConvertCurrency(event.srcElement.value, 'rcurrency');
    PrevNumber = event.srcElement.value;
    g_EvalScript("focus");
}

// g_CheckNumberSet �����Լ�(Blur �̺�Ʈ�ڵ鷯)
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

// g_CheckNumberSet �����Լ�(Ű�ڵ� üũ�Լ�)
function g_CheckkeyCode(isDotAble)
{
    var keyCode = event.keyCode; // ����Ű���� �޴´�.

    if (!(
		keyCode == 8 || //�齺���̽� 
		keyCode == 9 || //TABŰ
		keyCode == 27 || //ESCŰ
		keyCode == 45 || //INSŰ
		keyCode == 46 || //DELŰ
		(keyCode >= 33 && keyCode <= 40)/*page up,page down,home,end,����Ű*/)) //�̿��� �ٸ�Ű�� ��������
    {
        //����ƮŰ�� ������ ���¶�� ���´�.
        if (event.shiftKey || event.shiftKey) {
            event.returnValue = false;
            event.cancelBubble = true;
            return;
        }

        //��ġ�ƴѰ��
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105))) {
            switch (keyCode) // '-','.'��ȣ�� ���õ� Ű�� �ƴϸ� ���´�.
            {
                case 109:
                case 189:
                    var sValue = event.srcElement.value.replace(/\s/g, "").split("-");
                    // '-'��ȣ�� ���õ� Ű�� �ؽ�Ʈ�ڽ��� '-' �����Ѵٸ� ���´�.
                    if (sValue.length > 1) {
                        event.returnValue = false;
                        event.cancelBubble = true;
                        return;
                    }
                    break;
                case 110:
                case 190:
                    //������� ���´�.
                    if (!isDotAble) {
                        event.returnValue = false;
                        event.cancelBubble = true;
                        return;
                    }
                    // '-'��ȣ�� ���õ� Ű�� �ؽ�Ʈ�ڽ��� '-' �����Ѵٸ� ���´�.
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

// g_CheckNumberSet �����Լ�(��ũ��Ʈ �����Լ�)
function g_EvalScript(eventName)
{
    var eventHandler = event.srcElement.attributes.getNamedItem(eventName);
    if (eventHandler != null || eventHandler != undefined)
        eval(eventHandler.value);
}

/************************************************************************
��ġ�� �Է½� �Ǽ��� �Է��ϰ� �ϴ� �Լ�
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

// g_CheckFloatSet �����Լ�(keyDown �̺�Ʈ�ڵ鷯)
function g_CheckFloatDown()
{

    g_CheckkeyCode(true);
    g_EvalScript("keydown");

}

// g_CheckFloatSet �����Լ�(Keyup �̺�Ʈ�ڵ鷯)
function g_CheckFloatUp()
{
    //'-'��ȣ�� �߰��� ������ ������Ų��.
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


// g_CheckFloatSet �����Լ�(Blur �̺�Ʈ�ڵ鷯)
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


/************************************ ��Ӵٿ� ���� *************************/
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

// ��Ӵٿ� setselected
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


// ��Ʈ���� ��.
function g_getComboText(comboid)
{
    var val = "";
    var combo = g_GetObjID(comboid);

    if (combo.options.length > 0 && combo.selectedIndex >= 0) {
        val = combo.options[combo.selectedIndex].text;
    }

    return val;
}
/************************************ ��Ӵٿ� ���� *************************/

/************************************ üũ�ڽ� ���� *************************/

// ��Ʈ���� ��.
function g_setCbxChecked(objID, checkflag)
{
    g_GetObjID(objID).checked = checkflag;
}
function ut_getCbxChecked(objID)
{
    return ut_getobj(objID).checked;
}


// checkbox list�� ���õ� ��.
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
// checkbox list�� ���õ� ��.
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


/************************************ üũ�ڽ� ���� *************************/


/************************************ ���� ���� *************************/

// �����ڽ��� cd���� ������
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
// ��������Ʈ�� checked ��.
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

// ��������Ʈ�� cd���� ������
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
// ��������Ʈ�� checked ��.
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
/************************************ ���� ���� *************************/

/************************************************************************
�Լ���		: g_getItemList(itemgroup,ptype)
��  ��		:  span �̳� div���� üũ�ڽ� or ������ư ��Ʈ���� ������
parameter   : itemgroup - (td, span, div)��Ʈ�� ID
ptype = r : ����, c : üũ�ڽ�
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
�޺��ڽ��� OPTION �߰� ��Ų��.
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
�޺��ڽ��� ���� ���� ���� ��Ų��.
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
�ĺ��ڽ����� select �ڽ��� selected�Ǿ��� ���� �����´�. value�� �ƴ� text��		
*************************************************************************/
function g_GetSelectText(selectBoxID)
{
    return g_GetObjID(selectBoxID).options[g_GetObjID(selectBoxID).selectedIndex].text;
}

/************************************************************************
�ĺ��ڽ����� select �ڽ��� selected�Ǿ��� ���� �����´�. text�� �ƴ� value��		
*************************************************************************/
function g_GetSelectValue(selectBoxID)
{
    return g_GetObjID(selectBoxID).options[g_GetObjID(selectBoxID).selectedIndex].value;
}


/************************************************************************
��ü �Ǵ� ���� ���̸� �����Ѵ�.			
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
��ü �Ǵ� ���� �˿� ������ ������ Ʈ�� �۾��� �Ѵ�.	
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
�Է°��� NULL���� üũ
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
�Է°��� �����̽� �̿��� �ǹ��ִ� ���� �ִ��� üũ
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
�Է°��� Ư�� ����(chars)�� �ִ��� üũ
Ư�� ���ڸ� ������� ������ �� �� ��� inChar(form.name,"!,*&^%$#@~;")
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
�Է°��� Ư�� ����(chars)������ �Ǿ��ִ��� üũ
Ư�� ���ڸ� ����Ϸ� �� �� ���
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
�Է°��� ���ڸ� �ִ��� üũ
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
��ȿ��(�����ϴ�) ��(��)���� üũ
********************************************************************/
function g_isValidMonth(mm)
{
    var str = ((typeof mm == "object") ? mm.value : mm);
    var m = parseInt(str, 10);
    return (m >= 1 && m <= 12);
}

/********************************************************************
��ȿ��(�����ϴ�) ��(��)���� üũ
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
��ȿ��(�����ϴ�) ��(��)���� üũ
2006-07-23 �Ǵ� 20060723
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
��ȿ�ϴ�(�����ϴ�) Time ���� üũ
ex)
var time = form.time.value; //'200102310000'
if (!isValidTime(time)) {
alert("�ùٸ� ��¥�� �ƴմϴ�.");
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
�ڹٽ�ũ��Ʈ Date ��ü�� Time ��Ʈ������ ��ȯ
parameter date: JavaScript Date Object
********************************************************************/
function g_toTimeString(date)
{
    var str = date;
    var year = str.getFullYear();
    var month = str.getMonth() + 1; // 1��=0,12��=11�̹Ƿ� 1 ����
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
���ó�¥
********************************************************************/
function g_getToDay()
{
    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth() + 1; // 1��=0,12��=11�̹Ƿ� 1 ����
    var day = date.getDate();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }

    return ("" + year + month + day)
}

/********************************************************************
���ó�¥ �̵�� �ִ�
********************************************************************/
function g_getToDay2()
{
    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth() + 1; // 1��=0,12��=11�̹Ƿ� 1 ����
    var day = date.getDate();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }

    return ("" + year + "-" + month + "-" + day)
}

/********************************************************************
���ó�¥���� �Ѵ� ���� ��¥
********************************************************************/
function g_getToDay3()
{
    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth(); // 1��=0,12��=11�̹Ƿ� 1 ����
    var day = date.getDate();

    if (("" + month).length == 1) { month = "0" + month; }
    if (("" + day).length == 1) { day = "0" + day; }

    return ("" + year + "-" + month + "-" + day)
}

/********************************************************************
���� �ð��� Time �������� ����
********************************************************************/
function g_getCurrentTime()
{
    return g_toTimeString(new Date());
}

/********************************************************************
���� Ҵ�� YYYY�������� ����
********************************************************************/
function g_getYear()
{

    return g_getCurrentTime().substr(0, 4);
}

/********************************************************************
���� ���� MM�������� ����
********************************************************************/
function g_getMonth()
{

    return g_getCurrentTime().substr(4, 2);
}

/********************************************************************
���� ���� DD�������� ����
********************************************************************/
function g_getDay()
{

    return g_getCurrentTime().substr(6, 2);
}

/********************************************************************
���� �� ���� ���
********************************************************************/
function g_getEndDate(sDate)
{
    //��üũ
    if (g_isEmpty(sDate)) {
        return null;
    }

    //����üũ
    if (!g_isNumber(sDate, "C")) {
        return null;
    }

    //���̰� 8�ڸ� üũ
    if (sDate.length != 6) {
        return null;
    }

    var str = ((typeof sDate == "object") ? sDate.value : sDate);
    var yy = Number(str.substring(0, 4));
    var mm = Number(str.substring(4, 6));

    //���� ����
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
������ ������ ����
********************************************************************/
function g_getDayOfWeek()
{
    var now = new Date();

    var day = now.getDay();
    var week = ["��", "��", "ȭ", "��", "��", "��", "��"];

    return week[day];
}

/********************************************************************
Ư����¥�� ������ ���Ѵ�.
********************************************************************/
function g_getDayOfWeek(time)
{
    var now = g_toTimeObject(time);

    var day = now.getDay();
    var week = ["��", "��", "ȭ", "��", "��", "��", "��"];

    return week[day];
}

/********************************************************************
����(20060901) -> ��¥����(2006-09-01)
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
��¥(2006-09-01) -> ����(20060901)
********************************************************************/
function g_unFrmDate(obj)
{

    var str = ((typeof obj == "object") ? obj.value : obj);

    str = g_setReplace(str, "-", "");
    return str;
}

/********************************************************************
�ΰ��� ��¥�� ���ϴ� �Լ�
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
�ΰ��� ��¥�� ���ϴ� �Լ�
********************************************************************/
function g_isValidDateFromTo(DateS, DateE)
{
    if (!g_isValidDate(DateS) || !g_isValidDate(DateE)) {
        alert('��¥������ �߸��Ǿ����ϴ�. Ȯ�����ּ���.')
        return false;
    }

    if (!g_isValidDateTime(DateS, DateE)) {
        alert('���� ��¥�� ���� ��¥���� �ռ��� �����ϴ�.');
        return false;
    }
    return true;
}

/********************************************************************
�빮�ں�ȯ
********************************************************************/
function g_setToUCase(obj)
{

    var str = ((typeof obj == "object") ? obj.value : obj);
    if (g_isEmpty(str)) return str;

    return str.toUpperCase();
}

/********************************************************************
�ҹ��ں�ȯ
********************************************************************/
function g_setToLCase(obj)
{
    if (g_isEmpty(str)) return str;

    var str = ((typeof obj == "object") ? obj.value : obj);
    return str.toLowerCase();
}

/********************************************************************
�и� �������� ���� ������ �迭�� ����
********************************************************************/
function g_GetStringSeparator(strVar)
{
    try {

        if (strVar.indexOf('��') > -1) return strVar.split('��');
        return strVar;
    }
    catch (Exception) {
        return strVar;
    }
}

/********************************************************************
���� �������� ���� ������ �迭�� ����
********************************************************************/
function g_GetStringTerminator(strVar)
{
    try {
        //return strVar.split('��');
        if (strVar.indexOf('��') > -1) return strVar.split('��');
        return strVar;
    }
    catch (Exception) {
        return strVar;
    }
}

/********************************************************************
�����ڸ� �Է¹޾� �迭�� ����
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
�Է� ���� �ý��� ���ڸ� �Է� �ߴ��� �˻�
********************************************************************/
function g_FormDataValidate(formId)
{
    var form = g_GetObjID(formId);

    for (var e = 0; e < form.elements.length; e++) {
        var el = form.elements[e];
        if (el.type == 'text') {

            el.value = g_setReplace(el.value, "\"", "'");

            if (el.value.match(/["�ѡ�]/g) != null) {
                if (el.value.match(/["�ѡ�]/g).length != undefined && el.value.match(/["�ѡ�]/g).length > 0) {
                    g_OpenInformation("�ý��ۿ��� ����ϴ� ���ڰ� ���Ǿ����ϴ�.<BR> �� ���ڸ� �������ֽñ� �ٶ��ϴ�.");
                    el.focus();
                    return false;
                }
            }
        }
    }

    if (g_GetObjID("JwEditor") != null) {
        // g_GetObjID("JwEditor").AllText =g_setReplace(g_GetObjID("JwEditor").AllText,"\"","'");
        if (g_GetObjID("JwEditor").AllText.match(/[�ѡ�]/g) != null) {
            if (g_GetObjID("JwEditor").AllText.match(/[�ѡ�]/g).length != undefined && g_GetObjID("JwEditor").AllText.match(/[�ѡ�]/g).length > 0) {
                g_OpenInformation("�ý��ۿ��� ����ϴ� ���ڰ� ���Ǿ����ϴ�.<BR> �� ���ڸ� �������ֽñ� �ٶ��ϴ�.");
                g_GetObjID("JwEditor").focus();
                return false;
            }
        }
    }

    return true;
}

/********************************************************************
���� �Է½� �Է� ���� üũ �ϴ� �Լ� ��ȸ ���ǿ� �� ���� 2�� �̻��̾�� �Ѵ�.
�� �Լ��� Like �˻� ���ǽĿ� �� �� �� ����ؾ� �Ѵ�.
********************************************************************/
function g_CheckQueryLength(obj)
{
    if (g_GetLength(obj) == 0 || g_GetLength(obj) > 2) return true;
    g_OpenInformation("�˻� ���ǽ��� �Է� ���� 3�ڸ� �̻��̾�� �մϴ�.");
    return false;
}

/********************************************************************
�Ϲ����� Ajax ����� �̿��Ҷ� ���ϵ� ���� ������ üũ�ϰ� �޼��� ó�����ش�.
********************************************************************/
function g_Ajax_Error(response)
{
    if (response.error != null) {
        alert(response.error);
        return false;
    }

    if (response.value == null) {
        alert("�����͸� ó�� �� ������ �߻��Ͽ����ϴ�.");
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
�Է°��� ���ĺ�,���ڷ� �Ǿ��ִ��� üũ
********************************************************************/
function g_isAlphaNum(obj)
{
    var chars = gUpperCase + gLowerCase + gNumber + " ";
    return g_inCharsOnly(obj, chars);
}

/********************************************************************
�����ڷ� ���е� �ڷῡ�� ���ϴ� ���� �ִ��� �˻��Ѵ�.
********************************************************************/
function g_isCheckSeparatorDatas(strSData, strKey)
{
    var tVal = g_GetStringSeparator(strSData);
    if (tVal.length == null) return false;
    if (tVal.length <= 0) return false;

    for (var i = 0; i < tVal.length; i++) {
        if (g_Trim(g_setToUCase(tVal[i])) == g_Trim(g_setToUCase(strKey))) {
            g_OpenInformation("�̹� ��ϵ� �����Դϴ�.");
            return true;
        }
    }

    return false;
}

/********************************************************************
Acuate ����
frameID: Actuate�� ���� �Ķ���Ͱ� �ִ� ��
frameID: Actuate�� �׷��� Iframe ��
********************************************************************/
function g_ExcuteActuate(formID, frameID)
{

    var frm = document.all(formID);
    var frame = document.all(frameID);

    // From�� �����ϱ����� ������ �����Ѵ�.
    frm.target = frameID;
    frm.submit();
}

/********************************************************************
Acuate ��ȣ �Լ� 
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
Excel Upload ���� ó�� �Լ� 
********************************************************************/
function g_ExcelUploadError(strMsg)
{
    if (strMsg.indexOf("[File ����!!!]") >= 0) {
        g_OpenInformation(strMsg);
        return false;
    }
    return true;

}

/********************************************************************
IFrame �������� �Լ�
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
����(20060901) -> ��¥����(2006-09-01)
type : 'Y' -> �⵵
type : 'M' -> ���
type : 'D' -> �����
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
            alert("��¥������ �ȸ½��ϴ�.");
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
            alert("��¥������ �ȸ½��ϴ�.");
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
            alert("��¥������ �ȸ½��ϴ�.");
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
�Լ���		: fn_BriefPrint_Open()
��  ��		: ����� �μ�
parameter   : 
�� �� ��	: KYG
�� �� ��	: 2011.09.27
�� �� ��	: 
��������	:
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
{ //1.5��
    var returnType;
    var aReturnValue = new Array();
    aReturnValue = "";

    returnType = g_OpenModalDialog3("../Common/PatientsCCPrintSelect_Pop.aspx", aReturnValue, "dialogWidth:650px;dialogHeight:280px;status:no;scroll:yes;help:No;resizable:Yes;");
    if (returnType == null)
        return;
    //��ȯ����        
    else if (returnType == "11")
        returnType = g_OpenModalDialog3("../Common/PatientsCommonPrint_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "1b")
        returnType = g_OpenModalDialog3("../Common/PatientsCommonPrintB_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    //��������(�Ϲ�)
    else if (returnType == "21")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23Print_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "2b")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23PrintB_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    //��������(����)    
    else if (returnType == "31")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23Print_Pop.aspx?mcode=003&PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "3b")
        returnType = g_OpenModalDialog3("../Common/PatientsCode23PrintB_Pop.aspx?mcode=003&PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    //��������(�¶���)
    else if (returnType == "41")
        returnType = g_OpenModalDialog3("../Common/PatientsCodePrint_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");
    else if (returnType == "4b")
        returnType = g_OpenModalDialog3("../Common/PatientsCodePrintB_Pop.aspx?PATIENTS_USER_SEQ=" + Patients_User_Seq, aReturnValue, "dialogWidth:890px;dialogHeight:750px;dialogTop:2px;dialogLeft:2px;status:no;scroll:yes;help:No;resizable:Yes;");

}

/************************************************************************
�Լ���		: fn_BriefMeasure_Open()
��  ��		: ������� �м�
parameter   : 
�� �� ��	: KYG
�� �� ��	: 2011.09.20
�� �� ��	: 
��������	:
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
