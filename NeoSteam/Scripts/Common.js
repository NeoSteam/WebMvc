// 组装分页底栏totalpage 总页数，recordcount 总记录数， currentpage 当前页，分页函数
function paginationlocal(totalpage, recordcount, currentpage, funname) {
    var strPage = "";
    strPage += "<div class=\"dataTables_info\">共 " + recordcount + " 条记录，共 " + totalpage + " 页；当前第 " + currentpage + " 页 </div>";
    strPage += "  <div class=\"dataTables_paginate paging_full_numbers\">";
    if (currentpage == "1" || recordcount <= 0) {
        strPage += "<span class=\"first paginate_button paginate_button_disabled\">首页</span><span class=\"previous paginate_button paginate_button_disabled\">上一页</span>";
    } else {
        strPage += " <span class=\"first paginate_button\" onclick=\"" + funname + "('1')\">首页</span><span class=\"previous paginate_button\" onclick=\"" + funname + "('" + (parseInt(currentpage, 10) - 1) + "')\">上一页</span>";
    }

    strPage += "<span>";
    var s_page = 0;
    var e_page = 0;
    if (currentpage < 5) {
        s_page = 1;
        e_page = 10;
    } else {
        s_page = parseInt(currentpage) - 4;
        e_page = parseInt(currentpage) + 5;
    }
    if (e_page > totalpage) e_page = totalpage;

    for (var i = s_page; i <= e_page; i++) {
        if (i == currentpage) {
            strPage += "<span class=\"paginate_active\">" + i + "</span>";
        } else {
            strPage += "<span class=\"paginate_button\" onclick=\"" + funname + "('" + (parseInt(i, 10)) + "')\">" + i + "</span>";
        }
    }

    strPage += "</span>";

    if (currentpage == totalpage) {
        strPage += " <span class=\"next paginate_button paginate_button_disabled\">下一页</span><span class=\"last paginate_button paginate_button_disabled\">末页</span>";
    } else {
        strPage += "<span class=\"next paginate_button\" onclick=\"" + funname + "('" + (parseInt(currentpage, 10) + 1) + "')\">下一页</span><span class=\"last paginate_button\" onclick=\"" + funname + "('" + totalpage + "')\">末页</span>";
    }
    if (recordcount <= 0) {

    } else {
        strPage += " <input id=\"indexpage\" type=\"text\" value=" + currentpage + " style=\"width: 30px; height: 16px; margin-left: 5px; margin-top: 1px; border: none;\" /><input class=\"paginate_button\" style=\"border-left: 1px solid rgba(255, 255, 255, 0.15); border: 1px solid rgba(0, 0, 0, 0.5); display: block; float: right; height: 20px; background-color: #444444; color: white;\" type=\"button\" value=\"跳\"   onclick=\"" + funname + "('jump')\"/>";
    }

    strPage += "</div>";
    return strPage;
}


function onlyNumbers(inputString) {
    var str = "1234567890";
    var i;
    for (i = 0; i < inputString.length; i++) {
        if (str.indexOf(inputString.substr(i, 1), 0) < 0)
            return false;
    }
    return true;
}

// 对Date的扩展，将 Date 转化为指定格式的String 
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}