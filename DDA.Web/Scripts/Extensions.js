
function CheckNumberKey(el, e) {

    if (e.shiftKey || e.charCode == 46)
        return false;

    if (e.charCode >= 48 && e.charCode <= 57)
        return true;

    if (e.which == 8)
        return true;

    if ((e.ctrlKey === true || e.metaKey === true) || (e.charCode == 8) ||

        (e.keyCode >= 35 && e.keyCode <= 39)) {

        return true;
    }

    if (((e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }

    return false;
}


function alphaOnlyName(el, evt, lang) {
    lang = typeof lang !== 'undefined' ? lang : '';

    var charCode = (evt.which) ? evt.which : evt.keyCode;

    var charKey = (evt.char) ? evt.char : evt.key;

    var validate;

    if (typeof evt.key == 'undefined')
        validate = true;

    if (charCode === 32)
        validate = true;

    if (lang == '') {

        if (charCode == 1567 || charCode == 1548) // ؟ ،
            validate = false;

        var letter = /^[-\_\'\’a-zA-Z\ \u0600-\u06FF]+$/i

        validate = letter.test(charKey);
    }
    else if (lang == 'en') {

        var en = /^[-\_\'a-z\ \A-Z]+$/;

        validate = en.test(charKey);

        if (!validate) {

            if ($(el).next().next().find('.lang-validation').length === 0)
                $(el).next().next().append('<span class="lang-validation">' + _EnglishFieldValidation + '</span>');

            if ($(el).next().next().find('.field-validation-error').length !== 0)
                $(el).next().next().find('.field-validation-error').html('');
        }
    }
    else if (lang == 'ar') {

        if (charCode == 1567 || charCode == 1548) // ؟ ،
            validate = false;

        var ar = /^[-\_\’\ \u0600-\u06FF]+$/i;

        validate = ar.test(charKey);

        if (!validate) {

            if ($(el).next().next().find('.lang-validation').length === 0)
                $(el).next().next().append('<span class="lang-validation">' + _ArabicFieldValidation + '</span>');

            if ($(el).next().next().find('.field-validation-error').length !== 0)
                $(el).next().next().find('.field-validation-error').html('');
        }
    }

    if (validate) {

        if ($(el).next().next().children().hasClass('lang-validation')) {

            $(el).next().next().find('.lang-validation').remove();
        }

    }

    return validate;
}