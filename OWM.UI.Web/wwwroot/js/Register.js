$(function () {
    var i;
    $('#interests').selectize({
        placeholder: 'Interests',
        persist: false,
        createOnBlur: true,
        create: true
    });
    $(".chosen-select").chosen({
        width: "100%"
    }), $("#RegistrationData_CityName").on("keyup",
        function () {
            $(this).val() == '' || $(this).val().length < 4
                ? $("ul[name=cityfilter]").empty()
                : AjaxGet({
                    url: '/api/FilterCountriesByCities',
                    param: { city: $("#RegistrationData_CityName").val() },
                    func: ct
                });

            function ct(e) {
                $("ul[name=cityfilter]").empty();
                var t = JSON.parse(e.substring(e.indexOf("(") + 1, e.lastIndexOf(")")));
                var i = [];
                t.forEach(function (t) {
                    i.push("<li>" + t + "</li>")
                });
                $("ul[name=cityfilter]").append(i.join(""));
            }
        });
    $("ul[name=cityfilter]").on("click", "li", function () {
        var t;
        i = this.innerText,
            $("#RegistrationData_CityName").val(i.substring(0, i.indexOf(',')).trim()),
            $("ul[name=cityfilter]").empty(),
            $("#RegistrationData_CityName").prop("readonly", true),
            AjaxGet({
                url: '/api/GetCountry',
                param: { country: i },
                func: co
            });

        function co(e) {
            var t = JSON.parse(e.substring(e.indexOf("(") + 1, e.lastIndexOf(")")));
            $('#RegistrationData_CountryName').val(t.geobytescountry);
        }
    });
    $(".fa-times").click(function () {
        $("#RegistrationData_CityName").val(""), $("#RegistrationData_CountryName").val(""), $("#RegistrationData_CityName").prop("readonly", !1), $("#RegistrationData_CityId").val("")
    })
});