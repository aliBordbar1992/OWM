var teamId, profielId, mprofileId;
function BlockMemberModalOpen(t, p, mp, th) {
    $('#modalUsername').text(($(th).parent().parent().find('a')[0].innerText));
    teamId = t;
    profielId = p;
    mprofileId = mp;
}
function UnBlockMemberModalOpen(t, p, mp, th) {
    $('#UnblockmodalUsername').text(($(th).parent().parent().find('a')[0].innerText));
    teamId = t;
    profielId = p;
    mprofileId = mp;
}

function blockMember() {
    AjaxGet({
        url: '/api/blockMember',
        param: {
            tId: teamId,
            pId: profielId,
            mpId: mprofileId
        },
        func: blockChangesApplied,
        err: blockErrorOccured
    });
}

function blockChangesApplied(e) {
    location.reload();
    $('#BlockMemberModal').modal('toggle');
    ElementGreenalert('statusmessage', 'top center', e.displayMessage);
    setTimeout(function () { location.reload(); }, 1000);
}

function blockErrorOccured(xhr, httpStatusMessage, customErrorMessage) {
    ElementRedalert('BlockMemberModal', 'top center', xhr.responseText);
}

function unblockMember() {
    AjaxGet({
        url: '/api/unblockMember',
        param: {
            tId: teamId,
            pId: profielId,
            mpId: mprofileId
        },
        func: unblockChangesApplied,
        err: unblockErrorOccured
    });
}

function unblockChangesApplied(e) {
    $('#UnblockMemberModal').modal('toggle');
    ElementGreenalert('statusmessage', 'top center', e.displayMessage);
    setTimeout(function () { location.reload(); }, 1000);
}

function unblockErrorOccured(xhr, httpStatusMessage, customErrorMessage) {
    ElementRedalert('BlockMemberModal', 'top center', xhr.responseText);
}

$(function () {
    $("#searchuser").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#search div.list-group-item").filter(function (index, ele) {
            $(ele).toggle($(ele).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $('input.success').change(function () {
        AjaxGet({
            url: '/api/Prevent',
            param: { tId: $('#TeamId').val(), open: this.checked },
            func: changesApplied,
            err: errorOccured
        });

        function changesApplied(e) {
            ElementGreenalert('alert', 'bottom center', e.displayMessage);
        }

        function errorOccured(xhr, httpStatusMessage, customErrorMessage) {
            ElementRedalert('alert', 'bottom center', xhr.responseText);
        }
    });
    $('#savechanges').on('click',
        function () {
            AjaxGet({
                url: '/api/SaveChanges',
                param: { tId: $('#TeamId').val(), description: $('#description').val() },
                func: saveChanges
            });

            function saveChanges(e) {
                ElementGreenalert('savechanges', 'right center', e.displayMessage);
            }
        });
    var x = 0;
    $('#badges span.badge').each(function () {
        $(this).addClass(getRandomClass());
        x++;
    });
    function getRandomClass() {
        var classes = ["badge-info", "badge-primary", "badge-secondary", "badge-success", "badge-danger", "badge-warning", "badge-dark"];
        if (x === classes.length - 1) {
            x = 0;
        }
        return classes[x];
    }
})