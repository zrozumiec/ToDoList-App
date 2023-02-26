$("#reminder").click(function () {
    if ($(this).is(":checked")) {
        $('#reminderDate').removeAttr('disabled');
    }
    else {
        $("#reminderDate").attr('disabled', 'disabled');

    }
});