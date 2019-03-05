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
    })
    $('[data-toggle="popover"]').popover({
        trigger: 'focus'
    })
});