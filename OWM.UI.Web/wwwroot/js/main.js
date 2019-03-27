$(function () {
    // CYCLE DELAY TIME
    $.fn.cycleDelay = function (delayTime) {
        var delay = 0,
            element = $(this);

        element.each(function () {
            $(this).css({
                '-webkit-transition-delay': delay + 's, ' + delay + 's',
                '-moz-transition-delay': delay + 's, ' + delay + 's',
                '-ms-transition-delay': delay + 's, ' + delay + 's',
                '-o-transition-delay': delay + 's, ' + delay + 's',
                'transition-delay': delay + 's, ' + delay + 's'
            });

            delay = delay + delayTime;
        })
    };

    // GO TO PLEDGE
    $(document).on('click', '.go-to-pledge',function (e) {
        e.preventDefault();

        $('html, body').animate({
            scrollTop: $('#pledge').offset().top
        }, 2000);
    });

    // RANGE
    $('.range').change(function () {
        var meters = $(this).val();

        var miles = meters / 1609.344;

        $('.range-number').val(meters);

        $('.miles-number').text(miles.toFixed(1));
    });

    // ANIMS
    $('#splash').find('.logo, .strap').cycleDelay(0.2);

    $('#header, #splash').addClass('anim');

    // READ MORE
    $(document).on('click','.block p',function () {
        var block = $(this).closest('.block');

        if (!block.hasClass('.anim')) {
            block.find('p:nth-of-type(n+2)').each(function () {
                var element = $(this);
                element.animate({ 'max-height': element[0].scrollHeight }, 200)
            })

            block.addClass('anim');
        }
    })

});