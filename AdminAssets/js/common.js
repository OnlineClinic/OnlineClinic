includeJsFile('/Scripts/jquery-2.1.4.js');
includeJsFile('/Scripts/jquery-ui-1.11.4.js');
includeJsFile('/Scripts/jquery-ui.multidatespicker.js');
includeJsFile('/AdminAssets/js/validator/validator.js');
includeJsFile('/AdminAssets/js/jquery.ajax.js');

includeCssFile('/AdminAssets/css/jquery-ui.css');
includeCssFile('/css/mdp.css');

function includeJsFile(url) {
    document.write('<script type="text/javascript" src="' + url + '"></script>');
}
function includeCssFile(url) {
    document.write('<link href="' + url + '" rel="stylesheet" />');
}

$(function () {

    window.commonSaprator = '¶';
    // initialize the validator function
    validator.message['date'] = 'not a real date';

    // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
    $('form')
        .on('blur', 'input[required], input.optional, select.required', validator.checkField)
        .on('change', 'select.required', validator.checkField)
        .on('keypress', 'input[required][pattern]', validator.keypress);

    $('.multi.required')
        .on('keyup blur', 'input', function () {
            validator.checkField.apply($(this).siblings().last()[0]);
        });

    // bind the validation to the form submit event
    //$('#send').click('submit');//.prop('disabled', true);

    $('form').submit(function (e) {
        e.preventDefault();
        var submit = true;
        // evaluate the form using generic validaing
        if (!validator.checkAll($(this))) {
            submit = false;
        }

        if (submit)
            this.submit();
        return false;
    });

    /* FOR DEMO ONLY */
    $('#vfields').change(function () {
        $('form').toggleClass('mode2');
    }).prop('checked', false);

    $('#alerts').change(function () {
        validator.defaults.alerts = (this.checked) ? false : true;
        if (this.checked)
            $('form .alert').remove();
    }).prop('checked', false);

    window.getSelectedCheckBoxValue = function (controlId) {
        var selectedCheckBoxIds = '';
        $('#' + controlId + ' input:checked').each(function () {
            selectedCheckBoxIds += $(this).attr('value') + ',';
        });

        selectedCheckBoxIds = selectedCheckBoxIds.slice(0, -1);
        return selectedCheckBoxIds;
    }
});

