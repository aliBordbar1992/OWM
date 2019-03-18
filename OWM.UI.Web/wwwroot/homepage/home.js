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
});