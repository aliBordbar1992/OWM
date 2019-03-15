$(function () {
  setTimeout(function () { $('#alertsuccess').fadeOut(700) }, 5000);    
    $('#interests').selectize({
        placeholder: 'Interests',
        persist: false,
        createOnBlur: true,
        create: true
    });
    var loading = $('.loading-sm');
    $('.chosen-select').chosen({ width: "100%" });
    $('#savechanges').on('click',
        function () {
            Checkinputs('inputarea');
        });
    $('#UploadUserImage').on('click',
        function () {
            if ($('#upfile').val() == "") {
                RedAlert('n', 'Please select a file');
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
                    $('#controls').hide();
                    $('#upfile').val('');
                    $('#UserProfilePictureModal').modal('toggle');
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