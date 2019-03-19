$(function () {
    $('[counter]').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text().replace(',', '')
        },
            {
                duration: 1000,
                easing: 'swing',
                step: function (now) {
                    $(this).text(Math.ceil(now));
                    $(this).Digit();
                }
            });
    });
});

$.fn.Digit = function () {
    return this.each(function () {
        $(this).text($(this).text().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    });
}

function Checkinputs(n) {
    var t = !1,
        a = $("#" + n).find("input[required]");
    return 0 === a.length && (a = $("#" + n).find("textarea[required]")), a.each(function (n, a) {
        "" == a.value && (RedAlert(a, ""), t = !0)
    }), t
}

function RedAlert(t, e) {
    var n;
    n = "string" == typeof t ? $("#" + t) : t, $(n).addClass("is-invalid", 500), setTimeout(function () {
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
    n = "string" == typeof t ? $("#" + t) : t, $(n).addClass("is-valid", 500), setTimeout(function () {
        $(n).removeClass("is-valid")
    }, 4e3), $.notify(e, {
        className: "success",
        clickToHide: !1,
        autoHide: !0,
        position: "top left"
    })
}

function ClearFields(t) {
    $('#' + t).children('input:text, textarea').each(function () {
        var baseColor = $(this).css('color');
        $(this).animate({ color: $(this).css('background-color') }, function () {
            $(this).val('');
            $(this).css('color', baseColor);
        });
    }); $("#" + t).find("select").prop("selectedIndex", 0)
}

function AjaxGet(t) {
    $.ajax({
        type: "Get",
        url: t.url,
        data: t.param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: t.func,
        error: t.err
    })
}

function AjaxPost(t) {
    $.ajax({
        type: "Post",
        url: t.url,
        data: t.param,
        contentType: "application/json",
        success: t.func,
        error: t.err
    })
}

$(function () {
    setTimeout(function () { $('#ValidationSummary ul li').fadeOut(700) }, 5000);
    $('#ValidationSummary').on('click',
        'li',
        function () {
            $(this).fadeOut();
        });
})


var sideBarDonate;
$(function () {
    if ($(window).width() < 1060) {
        $('#LoginBtn').removeClass('owmbtn').css('background-color', 'transparent');
        $('#donateBtn').removeClass('owmbtn').css('background-color', 'transparent');
        $('#donateBtn1').removeClass('owmbtn').css('background-color', 'transparent');
    }
    switch (window.location.pathname.replace('/', '')) {
        case 'explore':
            $('.NavExplore').addClass('active');
            break;
        case 'index':
            $('.NavIndex').addClass('active');
            break;
        case 'helpus':
            $('.NavHelpus').addClass('active');
            break;
        case 'gallery':
            $('.NavGallery').addClass('active');
            break;
        case 'Register':
            $('.NavRegister').addClass('active');
            break;
        case 'Login':
            $('.btn-dark-green').addClass('active');
            break;
        default:
    }
    switch (window.location.pathname.split('/')[2]) {
        case 'Fun':
            $('.funstuff').addClass('active');
            break;
        case 'Video':
            $('.video').addClass('active');
            break;
        default:
    }
    switch (window.location.pathname.split('/')[1]) {
        case 'User':
        case 'Teams':
        case 'Profile':
            $('.NavUser').addClass('active');
            break;
    }
    sideBarDonate = $('#SideBarNavigation > div.donate')[0];
    RenderNavigation();
});

window.onresize = function () {
    if ($(window).width() <= 1060) {
        $('#LoginBtn').removeClass('owmbtn').css('background-color', 'transparent');
        $('#donateBtn').removeClass('owmbtn').css('background-color', 'transparent');
        $('#donateBtn1').removeClass('owmbtn').css('background-color', 'transparent');
    }
    if ($(window).width() > 1060) {
        $('#LoginBtn').addClass('owmbtn');
        $('#donateBtn').addClass('owmbtn');
        $('#donateBtn1').addClass('owmbtn');
        $("#wrapper").removeClass("toggled");
    }
    RenderNavigation();
}

function RenderNavigation() {
    if ($(window).width() > 1060) {
        $('#sideArea').show();
        $('#UserPanelSideArea').empty();
        $('#SideBarNavigation').show();
        $('#UserPanelMainArea').removeClass('col-md-12');
        $('#mainArea').removeClass('col-md-12');
    }
    if ($(window).width() <= 1060) {
        if ($('#sideArea')[0] == undefined) {
            $('#SideBarNavigation').show();
            return;
        }
        $('#sideArea').hide();
        $('#UserPanelMainArea').addClass('col-md-12');
        $('#mainArea').addClass('col-md-12');
        $('#UserPanelSideArea').empty().append(sideBarDonate).append($('#sideArea').html());
        $('#SideBarNavigation').hide();
    }
}

$(function () {
    $(document).on('click', '.mobile-open',
        function () {
            $('.sidebar-fade').show();
            $("#wrapper").addClass("toggled");
        })
    $(document).on('click', '.sidebar-fade',
        function () {
            $('.sidebar-fade').hide();
            $("#wrapper").removeClass("toggled");
        })
})

!function (a, n) { "function" == typeof define && define.amd ? define(n) : "object" == typeof exports ? module.exports = n(require, exports, module) : a.CountUp = n() }(this, function (a, n, t) { return function (a, n, t, e, i, r) { var u = this; if (u.version = function () { return "1.9.3" }, u.options = { useEasing: !0, useGrouping: !0, separator: ",", decimal: ".", easingFn: function (a, n, t, e) { return t * (1 - Math.pow(2, -10 * a / e)) * 1024 / 1023 + n }, formattingFn: function (a) { var n, t, e, i, r, o, s = a < 0; if (a = Math.abs(a).toFixed(u.decimals), n = (a += "").split("."), t = n[0], e = 1 < n.length ? u.options.decimal + n[1] : "", u.options.useGrouping) { for (i = "", r = 0, o = t.length; r < o; ++r)0 !== r && r % 3 == 0 && (i = u.options.separator + i), i = t[o - r - 1] + i; t = i } return u.options.numerals.length && (t = t.replace(/[0-9]/g, function (a) { return u.options.numerals[+a] }), e = e.replace(/[0-9]/g, function (a) { return u.options.numerals[+a] })), (s ? "-" : "") + u.options.prefix + t + e + u.options.suffix }, prefix: "", suffix: "", numerals: [] }, r && "object" == typeof r) for (var o in u.options) r.hasOwnProperty(o) && null !== r[o] && (u.options[o] = r[o]); "" === u.options.separator ? u.options.useGrouping = !1 : u.options.separator = "" + u.options.separator; for (var s = 0, l = ["webkit", "moz", "ms", "o"], m = 0; m < l.length && !window.requestAnimationFrame; ++m)window.requestAnimationFrame = window[l[m] + "RequestAnimationFrame"], window.cancelAnimationFrame = window[l[m] + "CancelAnimationFrame"] || window[l[m] + "CancelRequestAnimationFrame"]; function d(a) { return "number" == typeof a && !isNaN(a) } window.requestAnimationFrame || (window.requestAnimationFrame = function (a, n) { var t = (new Date).getTime(), e = Math.max(0, 16 - (t - s)), i = window.setTimeout(function () { a(t + e) }, e); return s = t + e, i }), window.cancelAnimationFrame || (window.cancelAnimationFrame = function (a) { clearTimeout(a) }), u.initialize = function () { return !!u.initialized || (u.error = "", u.d = "string" == typeof a ? document.getElementById(a) : a, u.d ? (u.startVal = Number(n), u.endVal = Number(t), d(u.startVal) && d(u.endVal) ? (u.decimals = Math.max(0, e || 0), u.dec = Math.pow(10, u.decimals), u.duration = 1e3 * Number(i) || 2e3, u.countDown = u.startVal > u.endVal, u.frameVal = u.startVal, u.initialized = !0) : (u.error = "[CountUp] startVal (" + n + ") or endVal (" + t + ") is not a number", !1)) : !(u.error = "[CountUp] target is null or undefined")) }, u.printValue = function (a) { var n = u.options.formattingFn(a); "INPUT" === u.d.tagName ? this.d.value = n : "text" === u.d.tagName || "tspan" === u.d.tagName ? this.d.textContent = n : this.d.innerHTML = n }, u.count = function (a) { u.startTime || (u.startTime = a); var n = (u.timestamp = a) - u.startTime; u.remaining = u.duration - n, u.options.useEasing ? u.countDown ? u.frameVal = u.startVal - u.options.easingFn(n, 0, u.startVal - u.endVal, u.duration) : u.frameVal = u.options.easingFn(n, u.startVal, u.endVal - u.startVal, u.duration) : u.countDown ? u.frameVal = u.startVal - (u.startVal - u.endVal) * (n / u.duration) : u.frameVal = u.startVal + (u.endVal - u.startVal) * (n / u.duration), u.countDown ? u.frameVal = u.frameVal < u.endVal ? u.endVal : u.frameVal : u.frameVal = u.frameVal > u.endVal ? u.endVal : u.frameVal, u.frameVal = Math.round(u.frameVal * u.dec) / u.dec, u.printValue(u.frameVal), n < u.duration ? u.rAF = requestAnimationFrame(u.count) : u.callback && u.callback() }, u.start = function (a) { u.initialize() && (u.callback = a, u.rAF = requestAnimationFrame(u.count)) }, u.pauseResume = function () { u.paused ? (u.paused = !1, delete u.startTime, u.duration = u.remaining, u.startVal = u.frameVal, requestAnimationFrame(u.count)) : (u.paused = !0, cancelAnimationFrame(u.rAF)) }, u.reset = function () { u.paused = !1, delete u.startTime, u.initialized = !1, u.initialize() && (cancelAnimationFrame(u.rAF), u.printValue(u.startVal)) }, u.update = function (a) { u.initialize() && (d(a = Number(a)) ? (u.error = "", a !== u.frameVal && (cancelAnimationFrame(u.rAF), u.paused = !1, delete u.startTime, u.startVal = u.frameVal, u.endVal = a, u.countDown = u.startVal > u.endVal, u.rAF = requestAnimationFrame(u.count))) : u.error = "[CountUp] update() - new endVal is not a number: " + a) }, u.initialize() && u.printValue(u.startVal) } });