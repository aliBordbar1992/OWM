$(document).mouseup(function (e) {
    var container = $(".popover");
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        $('#pledged').popover('hide');
        $('#complete').popover('hide');
    }
});

$(document).on('click', '.cancelp', function () {
    $('#pledged').popover('hide');
});

$("#pledged").popover({
    'title': 'Edit your pledged miles',
    'html': true,
    'placement': 'bottom',
    'content': $(".pledgeElements").html()
});

$(document).on('click', '.cancelc', function () {
    $('#complete').popover('hide');
});

$("#complete").popover({
    'title': 'Increase your completed miles',
    'html': true,
    'placement': 'bottom',
    'content': $(".completeElemnts").html()
});

function pledgeMiles() {
    if (!isNaN(parseFloat($($('.miles-pledged')[1]).val()))) {
        $('.loading-sm').show();
        AjaxGet({
            url: '/api/Miles/IncreaseMilesPledged',
            param: {
                tId: $('#teamId').val(),
                pId: $('#profileId').val(),
                miles: parseFloat($($('.miles-pledged')[1]).val())
            },
            func: preventCompleteOrPledge
        });
    } else {
        alert("error");
    }
}


function completeMiles() {
    if (!isNaN(parseFloat($($('.miles-complete')[1]).val()))) {
        $('.loading-sm').show();
        AjaxGet({
            url: '/api/Miles/CompleteMiles',
            param: {
                tId: $('#teamId').val(),
                pId: $('#profileId').val(),
                miles: parseFloat($($('.miles-complete')[1]).val())
            },
            func: preventCompleteOrPledge
        });
    } else {
        ElementRedalert('card', 'top center', 'Error');
    }
}

/////Check member can pledge or complete miles/////
function preventCompleteOrPledge(e) {
    if (!e.success) {
        ElementRedalert('card', 'top center', e.displayMessage);
        $('.loading-sm').hide();
        return;
    }
    if (e.data.canCompleteMiles) {
        $('#complete').removeClass('hidden');
        $('#pledged').addClass('hidden');
        $('.pledgeprogress').addClass('hidden');
    }
    if (e.data.canPledgeMiles) {
        $('#complete').addClass('hidden');
        $('#pledged').removeClass('hidden');
    }
    $('#complete').popover('hide');
    $('#pledged').popover('hide');
    updateProgressBar(e.data);
    ElementGreenalert('card', 'top center', e.displayMessage);
}

function updateProgressBar(e) {
    $('#myprogress').css('width', e.myCompletedMilesPercentage + '%').text(e.myCompletedMiles + ' miles');
    $('#teamprogress').css('width', e.teamCompletedMilesPercentage + '%').text(e.teamCompletedMiles + ' miles');
    $('#MilesUntilCanCompletePercentage').css('width', e.milesUntilCanCompletePercentage + '%').text(e.teamTotalMilesPledged + ' miles');
    $('#MyPledgedMiles').text(e.myPledgedMiles + ' miles');
    $($('.miles-pledged')[1]).val(1);
    $('#TeamTotalMilesPledged').text(e.teamTotalMilesPledged + ' miles');
    $('#MilesUntilCanComplete').text(parseFloat(e.milesUntilCanComplete.toFixed(1)));
}