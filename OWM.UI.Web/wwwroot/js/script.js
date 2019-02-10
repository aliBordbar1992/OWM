function Checkinputs(n) {
    var t = !1,
        a = $("#" + n).find("input[required]");
    return 0 === a.length && (a = $("#" + n).find("textarea[required]")), a.each(function (n, a) {
        "" == a.value && (RedAlert(a, ""), t = !0)
    }), t
}

function RedAlert(t, e) {
    var n;
    n = "string" == typeof t ? $("#" + t) : t, $(n).addClass("is-invalid", 500), setTimeout(function() {
        $(n).removeClass("is-invalid")
    }, 4e3), $.notify(e, {
        globalPosition: "top left"
    })
}

function ElementRedalert(ele, pos, txt) {
    var element;
    if (typeof ele == "string") { element = $("#" + ele); } else { element = ele; }
    $(element).notify(txt, { position: pos, className: "error" });
}

function ElementGreenalert(ele, pos, txt) {
    var element;
    if (typeof ele == "string") { element = $("#" + ele); } else { element = ele; }
    $(element).notify(txt, { position: pos, className: "success" });
}

function GreenAlert(t, e) {
    var n;
    n = "string" == typeof t ? $("#" + t) : t, $(n).addClass("is-valid", 500), setTimeout(function() {
        $(n).removeClass("is-valid")
    }, 4e3), $.notify(e, {
        className: "success",
        clickToHide: !1,
        autoHide: !0,
        position: "top left"
    })
}

function ClearFields(t) {
    $("#" + t).find("input:text").val(""), $("#" + t).find("textarea").val(""), $("#" + t).find("select").prop("selectedIndex", 0)
}

function checkPastDate(t) {
    var e = $("#" + t).val();
    if ("" === e) return !1;
    var n = new Date,
        a = GtoJ(n.getFullYear(), n.getMonth() + 1, n.getDate()),
        o = a[0] + "/" + a[1] + "/" + a[2],
        i = o.split("/");
    1 === i[1].length && (i[1] = "0" + i[1]), 1 === i[2].length && (i[2] = "0" + i[2]);
    var s = e.split("/");
    return 1 === s[1].length && (s[1] = "0" + s[1]), 1 === s[2].length && (s[2] = "0" + s[2]), (o = i[0] + "/" + i[1] + "/" + i[2]) < s[0] + "/" + s[1] + "/" + s[2]
}

function CheckPastTime(t, e, n, a) {
    var o = t.split("/"),
        i = n.split("/");
    return new Date(JtoG(o[0], o[1], o[2], !0) + " " + e) < new Date(JtoG(i[0], i[1], i[2], !0) + " " + a)
}

function DaysBetween2Date(t, e) {
    var n = t.split("/"),
        a = e.split("/"),
        o = new Date(JtoG(n[0], n[1], n[2], !0) + " 12:00"),
        i = new Date(JtoG(a[0], a[1], a[2], !0) + " 12:00"),
        s = Math.floor(o.getTime() / 864e5);
    return Math.floor(i.getTime() / 864e5) - s
}

function DatesBetween2Date(t, e) {
    for (var n = [], a = t.split("/"), o = e.split("/"), i = new Date(JtoG(a[0], a[1], a[2], !0) + " 12:00"), s = new Date(JtoG(o[0], o[1], o[2], !0) + " 12:00"); i <= s;) n.push(new Date(i)), i.setDate(i.getDate() + 1);
    for (var r = 0; r < n.length; r++) n[r] = GtoJ(n[r].getFullYear(), n[r].getMonth() + 1, n[r].getDate()), n[r][1] = n[r][1] < 10 ? "0" + n[r][1] : n[r][1], n[r][2] = n[r][2] < 10 ? "0" + n[r][2] : n[r][2], n[r] = n[r][0] + "/" + n[r][1] + "/" + n[r][2];
    return n
}

function AjaxGet(t) {
    $.ajax({
        type: "Get",
        url: t.url,
        data: JSON.stringify(t.param),
        contentType: "application/json;",
        dataType: "json",
        success: t.func,
        error: function() {
            console.log("error")
        }
    })
}

function AjaxPost(t) {
    $.ajax({
        type: "Post",
        url: t.url,
        data: JSON.stringify(t.param),
        contentType: "application/json;",
        dataType: "json",
        success: t.func,
        error: function() {
            console.log("error")
        }
    })
}