$(function () {
    var loading = $('.loading-sm');
    $('.chosen-select').chosen({ width: "100%" });
    $('#savechanges').on('click',
        function () {
            if (Checkinputs('inputarea')) {
                ElementRedalert('savechanges', 'right center', 'please fill out fields');
            }
        });
    $("#RegistrationData_CityName").on("keydown keyup", function () {
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
    $('#UploadUserImage').on('click',
        function () {
            if ($('#upfile').val() == "") {
                RedAlert('n', 'Please Select a File');
                return;
            }
            loading.show();
            UploadFile({
                form: 'imageform',
                url: "/user/profileimage",
                func: profilePictureUploadResult
            });
        });

    function profilePictureUploadResult(e) {
        switch (e.status) {
            case "error":
                {
                    RedAlert('n', e.message);
                    loading.hide();
                    break;
                }
            case "success":
                {
                    GreenAlert('n', e.message);
                    $('#upfile').val('');
                    $('#views').empty();
                    loading.hide();
                    $("#imgProfile").prop("src", e.picAddress);
                    $("#imgProfileAddress").val(e.picAddress);
                    break;
                }
            default:
                {
                    RedAlert('n', "Unknown error.");
                    loading.hide();
                    break;
                }
        }
    }
})