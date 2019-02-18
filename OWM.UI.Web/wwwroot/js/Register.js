$(function () {
    var i;
    $('#interests').selectize({
        placeholder: 'Interests',
        persist: false,
        createOnBlur: true,
        create: true
    });
    $('#ValidationSummary').on('click',
        'li',
        function() {
            $(this).fadeOut();
        });
    setTimeout(function () { $('#ValidationSummary ul li').fadeOut(700) }, 5000);
    $(".chosen-select").chosen({
        width: "100%"
    }), $("#RegistrationData_CityName").on("keyup keydown", function () {
        $(this).val() === '' || $(this).val().length < 4 ? $("ul[name=cityfilter]").empty() :
            jQuery.getJSON("http://gd.geobytes.com/AutoCompleteCity?callback=?&sort=size&q=" + $("#RegistrationData_CityName").val(), function (t) {
                var i = [];
                $("ul[name=cityfilter]").empty(), t.forEach(function (t) {
                    i.push("<li>" + t + "</li>")
                }), $("ul[name=cityfilter]").append(i.join(""))
            })
    }), $("ul[name=cityfilter]").on("click", "li", function () {
        var t;
        i = this.innerText, $("#RegistrationData_CityName").val(i), $("ul[name=cityfilter]").empty(), $("#RegistrationData_CityName").prop("readonly", !0), t = this, jQuery.getJSON("http://gd.geobytes.com/GetCityDetails?callback=?&fqcn=" + t.innerText, function (t) {
            $("#RegistrationData_CountryName").val(t.geobytescountry), $("#RegistrationData_CityId").val(t.geobytescityid)
        })
    }), $(".fa-times").click(function () {
        $("#RegistrationData_CityName").val(""), $("#RegistrationData_CountryName").val(""), $("#RegistrationData_CityName").prop("readonly", !1), $("#RegistrationData_CityId").val("")
    })
});