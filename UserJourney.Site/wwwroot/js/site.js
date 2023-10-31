function setFocusFirstControl(finder) {
    if ($(finder).find('input:visible,textarea:visible,select:visible').not('.focusDisable').not('.datepicker').not('.datetimepicker').not('.daterangepicker-text').not('input:button').length > 0) {
        $(finder).find('input:visible,textarea:visible,select:visible').not('.focusDisable').not('.datepicker').not('.datetimepicker').not('.daterangepicker-text').not('input:button').first().focus();
    }
    else {
        $(finder).find('input:visible,textarea:visible,select:visible,button:visible').not('.focusDisable').not('.datepicker').not('.datetimepicker').not('.daterangepicker-text').not('.close').first().focus();
    }
}

var divMessage = "divMessage";

function successMessage(message, divId) {
    if (divId == null || divId == undefined || divId == 'undefined' || divId == 'null' || divId == '') {
        divId = "divMessage";
    }
    divMessage = divId;
    $('#' + divMessage).removeAttr('style');
    var mess = "<div class='alert alert-success'><button type='button' class='close' data-dismiss='alert' aria-label='close'>&times;</button>" + message + "</div>";
    messageSetup(mess);
}

function errorMessage(message, divId) {
    if (divId == null || divId == undefined || divId == 'undefined' || divId == 'null' || divId == '') {
        divId = "divMessage";
    }
    divMessage = divId;
    $('#' + divMessage).removeAttr('style');
    var mess = "<div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert' aria-label='close'>&times;</button>" + message + "</div>";
    messageSetup(mess);
}

function messageSetup(mess) {
    if (messsageIntervalId) {
        clearTimeout(messsageIntervalId);
        messsageIntervalId = null;
    }

    $('#' + divMessage).fadeIn();
    $('#' + divMessage).html($.parseHTML(unescape(mess)));

    messsageIntervalId = setTimeout(clearMessageFadeOut, 5000);

    $('#' + divMessage).hover(function () {
        clearTimeout(messsageIntervalId);
    }, function () {
        messsageIntervalId = setTimeout(clearMessageFadeOut, 5000);
    });

    if (divMessage == "divMessage") {
        $('html, body').animate({ scrollTop: 0 }, 'fast');
    }
}
