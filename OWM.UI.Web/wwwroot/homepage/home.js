$(function () {
    var options = {
        useEasing: true,
        useGrouping: true,
        separator: ',',
        decimal: '.',
    };
    var milespledge = new CountUp('milespledge', 0, $('#mp-counter').val(), 1, 3, options);
    if (!milespledge.error) {
        milespledge.start();
    } else {
        console.error(milespledge.error);
    }
    var countries = new CountUp('countries', 0, $('#c-counter').val(), 0, 3, options);
    if (!countries.error) {
        countries.start();
    } else {
        console.error(countries.error);
    }
    var participant = new CountUp('participant', 0, $('#p-counter').val(), 0, 3, options);
    if (participant.error) {
        console.error(participant.error);
    } else {
        participant.start();
    }

    function parseDate(str) {
        var mdy = str.split('/');
        return new Date(mdy[2], mdy[0] - 1, mdy[1]);
    }
    function datediff(first, second) {
        return Math.round((second - first) / (1000 * 60 * 60 * 24));
    }
    $('.days').text((datediff(parseDate(new Date().toLocaleDateString()), parseDate('04/12/2019'))));
});