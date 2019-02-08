$(function () {
    var i;
    $(".chosen-select").chosen({
        width: "100%"
    }), $("#registrationData_CityName").on("keyup", function () {
        "" == $(this).val() || $(this).val().length < 3 ? $("ul[name=cityfilter]").empty() : setTimeout(function () {
            jQuery.getJSON("http://gd.geobytes.com/AutoCompleteCity?callback=?&sort=size&q=" + $("#registrationData_CityName").val(), function (t) {
                var i = [];
                $("ul[name=cityfilter]").empty(), t.forEach(function (t) {
                    i.push("<li>" + t + "</li>")
                }), $("ul[name=cityfilter]").append(i.join(""))
            })
        }, 1500)
    }), $("ul[name=cityfilter]").on("click", "li", function () {
        var t;
        i = this.innerText, $("#registrationData_CityName").val(i), $("ul[name=cityfilter]").empty(), $("#registrationData_CityName").prop("readonly", !0), t = this, jQuery.getJSON("http://gd.geobytes.com/GetCityDetails?callback=?&fqcn=" + t.innerText, function (t) {
            $("#registrationData_CountryName").val(t.geobytescountry), $("#registrationData_CityId").val(t.geobytescityid)
        })
    }), $(".fa-times").click(function () {
        $("#registrationData_CityName").val(""), $("#registrationData_CountryName").val(""), $("#registrationData_CityName").prop("readonly", !1), $("#registrationData_CityId").val("")
    })
});