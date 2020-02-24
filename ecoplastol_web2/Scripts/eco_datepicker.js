//https://stackoverflow.com/questions/12053022/mvc-datetime-validation-uk-date-format/

$(function () {
    // This will make every element with the class "date-picker" into a DatePicker element
    $('.date-picker').datepicker(
        {
            dateFormat: 'dd.mm.yy',
            //locale: 'pl',
            changeMonth: true,
            changeYear: true
        });
})
jQuery(function ($) {
    $.validator.addMethod('date',
        function (value, element) {
            if (this.optional(element)) {
                return true;
            }

            var ok = true;
            try {
                $.datepicker.parseDate('dd.mm.yy', value);
            }
            catch (err) {
                ok = false;
            }
            return ok;
        });
});