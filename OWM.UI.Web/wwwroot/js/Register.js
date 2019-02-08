$(function () {
    $(".chosen-select").chosen({ width: "100%" });
    $('input[placeholder=City]').on('keyup', function () {
        if ($(this).val() == "" || $(this).val().length < 3) {
            $('ul[name=cityfilter]').empty();
            return;
        }
        setTimeout(
            function () {
                jQuery.getJSON(
                    "http://gd.geobytes.com/AutoCompleteCity?callback=?&sort=size&q=" + $('input[placeholder=City]').val(),
                    function (data) {
                        var d = [];
                        $('ul[name=cityfilter]').empty();
                        data.forEach(function (item) {
                            d.push('<li>' + item + '</li>');
                        });
                        $('ul[name=cityfilter]').append(d.join(''));
                    }
                );
            }, 1500);
    });
    var city;
    $('ul[name=cityfilter]').on('click', 'li',
        function () {
            city = this.innerText;
            $('input[placeholder=City]').val(city);
            $('ul[name=cityfilter]').empty();
            $('input[placeholder=City]').prop('disabled', true);
            GetCountry(this);
        });

    function GetCountry(e) {
        jQuery.getJSON("http://gd.geobytes.com/GetCityDetails?callback=?&fqcn=" + e.innerText,
            function (data) {
                $('input[placeholder=Country]').val(data.geobytescountry);
                $('input[name=cityid]').val(data.geobytescityid);
            }
        );
    }

    $('.fa-times').click(function () {
        $('input[placeholder=City]').val('');
        $('input[placeholder=Country]').val('');
        $('input[placeholder=City]').prop('disabled', false);
        $('input[name=cityid]').val('');
    });
});
