﻿$(function () {
    var i;
    $(".chosen-select").chosen({
        width: "100%"
    }), $("#RegistrationData_CityName").on("keyup", function () {
        "" == $(this).val() || $(this).val().length < 3 ? $("ul[name=cityfilter]").empty() : setTimeout(function () {
            jQuery.getJSON("http://gd.geobytes.com/AutoCompleteCity?callback=?&sort=size&q=" + $("#RegistrationData_CityName").val(), function (t) {
                var i = [];
                $("ul[name=cityfilter]").empty(), t.forEach(function (t) {
                    i.push("<li>" + t + "</li>")
                }), $("ul[name=cityfilter]").append(i.join(""))
            })
        }, 1500)
    }), $("ul[name=cityfilter]").on("click", "li", function () {
        var t;
        i = this.innerText, $("#RegistrationData_CityName").val(i), $("ul[name=cityfilter]").empty(), $("#RegistrationData_CityName").prop("readonly", !0), t = this, jQuery.getJSON("http://gd.geobytes.com/GetCityDetails?callback=?&fqcn=" + t.innerText, function (t) {
            $("#RegistrationData_CountryName").val(t.geobytescountry), $("#RegistrationData_CityId").val(t.geobytescityid)
        })
    }), $(".fa-times").click(function () {
        $("#RegistrationData_CityName").val(""), $("#RegistrationData_CountryName").val(""), $("#RegistrationData_CityName").prop("readonly", !1), $("#RegistrationData_CityId").val("")
    })
});