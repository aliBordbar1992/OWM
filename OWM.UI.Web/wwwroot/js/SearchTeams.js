$(function () {
    $('.chosen-select').chosen({ width: "100%" });
    $('#searchteams').focus();
    var loading = $('.loading-md-exp');
    var teamSerachResult = $('#teamsearchresult');

    $('input[name=teammembers]').on('change', function () {
        filterTeams();
    });

    $('input[name=milespledged]').on('change', function () {
        filterTeams();
    });

    $('.chosen-select').on('change', function () {
        filterTeams();
    });

    var take = 3, skip = 0, rows = 0;
    var elements = [];
    var isDataAvailable = true, isPreviousEventComplete = true;
    var load = '<img src="/img/loading.gif" id="loadteams" style="width: 20px; height: 20px; position: absolute; left: 48%; margin-top: -5px;"/>';
    var typingTimer;
    var doneTypingInterval = 1000;
    var $input = $('#searchteams');
    $input.on('keyup', function () {
        loading.show();
        clearTimeout(typingTimer);
        typingTimer = setTimeout(filterTeams, doneTypingInterval);
    });
    $input.on('keydown', function () {
        clearTimeout(filterTeams);
    });

    function filterTeams() {
        loading.show();
        teamSerachResult.empty();
        take = 3, skip = 0;
        sendRequest();
    }

    var html;
    $.get("/contents/TeamSearchTemplate.html", function (htmlString) {
        html = htmlString;
        sendRequest();
    }, 'html');

    function sendRequest() {
        var search = {};
        search.SearchExpression = $input.val();
        search.MembersOrder = $(document).find('input[name=teammembers]:checked').data().value;
        search.MilesOrder = $(document).find('input[name=milespledged]:checked').data().value;
        search.Occupation = $('#Occupations :selected').val() !== "" ? $('#Occupations :selected').val() : -1;
        search.SrchAgeRange = $('#Ages :selected').val();
        search.Take = take;
        search.Skip = skip;

        AjaxPost({
            url: '/api/Search/Count',
            param: JSON.stringify(search),
            func: counterr
        });

        function counterr(e) {
            $('[teamcount]').text(e);
        }

        AjaxPost({
            url: '/api/Teams/Search',
            param: JSON.stringify(search),
            func: createList
        });
    }


    function createList(e) {
        if (!e.length) {
            isDataAvailable = false;
            loading.hide();
            return;
        }
        elements = [];
        for (var i = 0; i < e.length; i++) {
            elements.push(html
                .replace("#m#", e[i].teamMembersCount)
                .replace("#mp#", e[i].totalMilesPledged)
                .replace('#mc#', e[i].totalMilesCompleted)
                .replace('#teamname#', e[i].teamName)
                .replace('#datecreated#', e[i].str_DateCreated)
                .replace("#desc#", e[i].description.substring(0, 150) + ' ...')
                .replace("#teamid#", e[i].teamId)
                .replace("#occ#", e[i].occupations.join(' / ')));
        }
        loading.hide();
        writeElementsToDom();
    }


    function writeElementsToDom() {
        if (elements.length > 0) {
            isDataAvailable = true;
            for (var i = 0; i < elements.length; i++) {
                $(elements[i]).hide().appendTo(teamSerachResult).show('slow');
                skip++;
            }
        } else {
            isDataAvailable = false;
        }
    }


    $(window).scroll(function () {
        if ($(document).height() - 400 <= $(window).scrollTop() + $(window).height()) {
            if (isPreviousEventComplete && isDataAvailable) {
                isPreviousEventComplete = false;
                var loadingElement = $(load);
                loadingElement.appendTo(teamSerachResult);
                setTimeout(function () {
                    sendRequest();
                    isPreviousEventComplete = true;
                    loadingElement.remove();
                }, 1000);
            }
        }
    });
});