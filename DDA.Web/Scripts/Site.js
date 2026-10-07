/* Calanders */
var calendarIslamic;
var calendarGregorian;
var calendarFormat = 'mm/dd/yyyy';
var calendarFormatHomeloan = 'dd/mm/yyyy';
/*convertFromGregToHijri: Convert Gregorian date to Hijri date auomaticlly*/
/*convertFromHijriToGreg: Convert Hijri date to Gregorian date auomaticlly*/
/*This assumes that date controls must have a class "datepickerfieldHijri" or "datepickerfieldGreg"  */
/*Also it assumes an id "xxxxx" and the same id for hijir with word "Hijri" appended to it "xxxx_Hijri"  */
function inilitizeCalanders(convertFromGregToHijri, convertFromHijriToGreg) {

    $(document).ready(function () {

        if (_SystemLanguge == 'en')
            $.calendarsPicker.setDefaults($.calendarsPicker.regionalOptions['']);
        else
            $.calendarsPicker.setDefaults($.calendarsPicker.regionalOptions['ar']);

        calendarIslamic = $.calendars.instance('ummalqura', _SystemLanguge);
        calendarGregorian = $.calendars.instance('gregorian', _SystemLanguge);

        //Inilize calanders
        $(function () {

            $('.datepickerfieldHijri').each(function () {
                //debugger;
                var maxDate = '';
                var minDate = '';
                //debugger;
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1;
                var yyyy = today.getFullYear();

                if (typeof $(this).attr('dataDate') != 'undefined') {

                    var date = mm + '/' + dd + '/' + (yyyy - $(this).attr('dataDate').slice(1, -1));

                    maxDate = convertGregoianToHijri(date);
                }

                if (typeof $(this).attr('today') != 'undefined') {

                    var date = $(this).attr('today');//  mm + '/' + dd + '/' + (yyyy);

                    maxDate = convertGregoianToHijri(date);
                }

                if (typeof $(this).attr('minDate') != 'undefined') {

                    var minDate1 = $(this).attr('minDate');//  mm + '/' + dd + '/' + (yyyy);

                    minDate = minDate1;
                }

                $(this).calendarsPicker(
                    {
                        calendar: calendarIslamic,
                        dateFormat: calendarFormat,
                        changeYear: true,
                        yearRange: 'c-65:c+10',
                        endDate: maxDate,
                        maxDate: maxDate,
                        minDate: minDate,
                        onSelect: function (date) {
                            if (typeof date[0] != "undefined") {
                                var dateHijri = $(this).val();
                                var dateGregorian = convertHijriToGregroian(dateHijri);
                                $("#" + $(this).attr('id').replace('Hijri', '')).val(dateGregorian);

                                if (typeof ChangeDateCallBack === "function")
                                    ChangeDateCallBack($(this).attr('id').replace('Hijri', ''), $("#" + $(this).attr('id').replace('Hijri', '')).val());
                            }
                            else {
                                $("#" + $(this).attr('id').replace('Hijri', '')).val('');
                            }
                        }
                    });
            });

            $('.datepickerfieldGreg').each(function () {
                //debugger; 
                var maxDate = $(this).attr('dataDate');
                var minDate = '';
                if (typeof $(this).attr('minDate') != 'undefined') {

                    minDate = $(this).attr('minDate');//  mm + '/' + dd + '/' + (yyyy);

                }

                if (typeof $(this).attr('today') != 'undefined') {

                    var date = $(this).attr('today');//  mm + '/' + dd + '/' + (yyyy);

                    maxDate = date;
                }
                $(this).calendarsPicker(
                    {
                        calendar: calendarGregorian,
                        dateFormat: calendarFormat,
                        changeYear: true,
                        yearRange: 'c-65:c+10',
                        endDate: maxDate,
                        maxDate: maxDate,
                        minDate: minDate,
                        onSelect: function (date) {
                            if (typeof date[0] != "undefined") {
                                var dateGregorian = $(this).val();
                                var dateHijri = convertGregoianToHijri(dateGregorian);
                                $("#" + $(this).attr('id') + "Hijri").val(dateHijri);

                                if (typeof ChangeDateCallBack === "function")
                                    ChangeDateCallBack($(this).attr('id'), $(this).val());
                            }
                            else {
                                $("#" + $(this).attr('id') + "Hijri").val('');
                            }
                        }
                    });
            });

        });

        $('.datepickerfieldGreg, .datepickerfieldHijri').mask("00/00/0000", { placeholder: calendarFormat, removeMaskOnSubmit: true });

        $('.datepickerfieldHijri').on("focusout", function () {
            //debugger;
            date = $(this).val();

            if (date != '' && date != null) {

                var splitDate = date.split("/");

                if (
                    ($.calendars.instance('ummalqura')._validateLevel === 0 &&
                        !$.calendars.instance('ummalqura').isValid(splitDate[2], splitDate[0], splitDate[1])
                    )
                    //||
                    //(splitDate[2] < (new Date().getFullYear() - 65) || splitDate[2] > (new Date().getFullYear() + 10))
                ) {

                    $(this).val('');
                    $("#" + $(this).attr('id').replace('Hijri', '')).val('');
                }
                else {

                    var dateHijri = $(this).val();
                    var dateGregorian = convertHijriToGregroian(dateHijri);
                    $("#" + $(this).attr('id').replace('Hijri', '')).val(dateGregorian);

                    if (typeof ChangeDateCallBack === "function")
                        ChangeDateCallBack($(this).attr('id').replace('Hijri', ''), $('#' + $(this).attr('id').replace('Hijri', '')).val());
                }
            }
            else {
                $("#" + $(this).attr('id').replace('Hijri', '')).val('');
            }
        });

        $('.datepickerfieldGreg').on("focusout", function () {
            //debugger;

            date = $(this).val();
            if (date != '' && date != null) {

                var splitDate = date.split("/");

                if (
                    ($.calendars.instance('gregorian')._validateLevel === 0 &&
                        !$.calendars.instance('gregorian').isValid(splitDate[2], splitDate[0], splitDate[1])
                    )
                    ||
                    (splitDate[2] < 1900 || splitDate[2] > 2070)
                ) {

                    $(this).val('');
                    $("#" + $(this).attr('id') + "Hijri").val('');
                }
                else {

                    var dateGregorian = $(this).val();
                    var dateHijri = convertGregoianToHijri(dateGregorian);
                    $("#" + $(this).attr('id') + "Hijri").val(dateHijri);

                    if (typeof ChangeDateCallBack === "function")
                        ChangeDateCallBack($(this).attr('id'), $(this).val());
                }
            }
            else {
                $("#" + $(this).attr('id') + "Hijri").val('');
            }
        });

    })
}

function convertHijriToGregroian(date) {
    var date = calendarIslamic.parseDate(calendarFormat, date);
    date = calendarGregorian.fromJD(date.toJD());
    return calendarGregorian.formatDate(calendarFormat, date);
};

function convertGregoianToHijri(date) {
    var date = calendarGregorian.parseDate(calendarFormat, date);
    date = calendarIslamic.fromJD(date.toJD());
    return calendarIslamic.formatDate(calendarFormat, date);
};


function convertHijriToGregroianHomeloan(date) {
    var date = calendarIslamic.parseDate(calendarFormatHomeloan, date);
    date = calendarGregorian.fromJD(date.toJD());
    return calendarGregorian.formatDate(calendarFormatHomeloan, date);
};

function convertGregoianToHijriHomeloan(date) {
    var date = calendarGregorian.parseDate(calendarFormatHomeloan, date);
    date = calendarIslamic.fromJD(date.toJD());
    return calendarIslamic.formatDate(calendarFormatHomeloan, date);
};




//Since easyWizard breaks bootstrap design
//This function handles form resize
$(window).resize(function () {

    if ($('#wizard').length > 0)
        $('#wizard').easyWizard('resize', $('#wizard').find('.active'))
});

/*Wizard (Steps)*/
//Set and activate wizard
function initlizeWizard(culture) {
    $(document).ready(function () {
        $('#wizard').easyWizard({
            culture: culture,
            showSteps: false,
            showButtons: false,
            submitButton: false,
            before: function (wizardObj, currentStepObj, nextStepObj) {

                var currentStep = currentStepObj.attr('data-step');
                var nextStep = nextStepObj.attr('data-step');

                if (nextStep > currentStep) {

                    if (!$('form').valid())
                        return false;
                }

                $('.product-step').val(nextStep);
                next(currentStep, nextStep);

            }
        });
        $('input.button.back').on('click', function (e) {



            $(this).prop('disabled', 'disabled');
            e.preventDefault();
            $('#wizard').easyWizard('prevStep');
            setTimeout("$('input.button.back').prop('disabled', false);", 1000);
        });

        //$('button.continue').on('click', function (e) {

        //    $(this).prop('disabled', 'disabled');

        //    e.preventDefault();
        //    $('#wizard').easyWizard('nextStep');

        //    if ($(".easyWizardWrapper div[data-step='" + 4 + "']").hasClass('active'))
        //        $('button.continue').html(_SaveAsDraft);
        //    else
        //        $('button.continue').html(_Continue);

        //    $("#step1 .bs-wizard-dot, #step2 .bs-wizard-dot, #step3 .bs-wizard-dot, #step4 .bs-wizard-dot").removeClass('active-step');
        //    $("#step1 .text-center, #step2 .text-center, #step3 .text-center, #step4 .text-center").removeClass('stepInfo-style');

        //    $("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .bs-wizard-dot").addClass('active-step');
        //    $("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .text-center").addClass('stepInfo-style');

        //    RemoveSession();

        //    setTimeout("$('button.continue').prop('disabled', false);", 1000);

        //    return false;
        //});
    });
}

function SaveAndContinue() {

    $(this).prop('disabled', 'disabled');

    $('#wizard').easyWizard('nextStep');

    if ($(".easyWizardWrapper div[data-step='" + 4 + "']").hasClass('active')) {
        $('#btn_Continue').hide(); //.addClass('hide');
        $('#btn_Submit').show(); //.removeClass('hide');
        $('#btn-Cancel').hide(); //.addClass('hide');
        //$('#btn_SaveAsDraft').hide(); //.addClass('hide');
    }
    else {
        $('#btn_Continue').show(); //.removeClass('hide');
        $('#btn_Submit').hide(); //.addClass('hide');
        $('#btn-Cancel').show(); //.removeClass('hide');
        //$('#btn_SaveAsDraft').show(); //.removeClass('hide');
    }


    if (!$(".easyWizardWrapper div[data-step='" + 1 + "']").hasClass('active'))
        $('#btnPrevious').removeClass('hide');
    else
        $('#btnPrevious').addClass('hide');

    $("#step1 .bs-wizard-dot, #step2 .bs-wizard-dot, #step3 .bs-wizard-dot, #step4 .bs-wizard-dot").removeClass('active-step');
    $("#step1 .text-center, #step2 .text-center, #step3 .text-center, #step4 .text-center").removeClass('stepInfo-style');

    $("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .bs-wizard-dot").addClass('active-step');
    $("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .text-center").addClass('stepInfo-style');

    RemoveSession();

    setTimeout("$('#btn_Continue').prop('disabled', false);", 1000);
}

//For form resize
var waitForFinalEvent = (function () {
    var timers = {};
    return function (callback, ms, uniqueId) {
        if (!uniqueId) {
            uniqueId = "Don't call this twice without a uniqueId";
        }
        if (timers[uniqueId]) {
            clearTimeout(timers[uniqueId]);
        }
        timers[uniqueId] = setTimeout(callback, ms);
    };
});

//Advance the slider
var _currentStep = 1;
function next(currentStep, nextStep) {
    //debugger;
    if (nextStep > currentStep) {
        if (!$('#step' + currentStep).hasClass("complete")) {
            $('#step' + currentStep).removeClass("active").addClass("complete");
            $(".easyWizardWrapper div[data-step='" + currentStep + "']").removeClass("active").css('display', 'none');
            // nextStep.css('display', 'block');
        }

        _currentStep++;

        if (!$('#step' + nextStep).hasClass("active"))
            $('#step' + nextStep).removeClass("disabled").addClass("active");

        $('#wizard').easyWizard('goToStep', _currentStep);
        $(".easyWizardWrapper div[data-step='" + _currentStep + "']").addClass("active");

        $("#step1 .bs-wizard-dot, #step2 .bs-wizard-dot, #step3 .bs-wizard-dot").removeClass('active-step');
        $("#step1 .text-center, #step2 .text-center, #step3 .text-center").removeClass('stepInfo-style');

        $("#step" + _currentStep + " .bs-wizard-dot").addClass('active-step');
        $("#step" + _currentStep + " .text-center").addClass('stepInfo-style');

        if ($(".easyWizardWrapper div[data-step='" + 4 + "']").hasClass('active')) {
            $('#btn_Continue').hide(); //.addClass('hide');
            $('#btn_Submit').show(); //.removeClass('hide');
            $('#btn-Cancel').hide(); //.addClass('hide');
            $('#btn_Approve').show(); 
            $('#btn_Reject').show(); 
            
            
            //$('#btn_SaveAsDraft').hide(); //.addClass('hide');
        }
        else {
            $('#btn_Continue').show(); //.removeClass('hide');
            $('#btn_Submit').hide(); //.addClass('hide');
            $('#btn-Cancel').show(); //.removeClass('hide');
            //$('#btn_SaveAsDraft').show(); //.removeClass('hide');
            $('#btn_Approve').hide();
            $('#btn_Reject').hide(); 
        }

        if (!$(".easyWizardWrapper div[data-step='" + 1 + "']").hasClass('active'))
            $('#btnPrevious').show(); //.removeClass('hide');
        else
            $('#btnPrevious').hide(); //.addClass('hide');

        //$("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .bs-wizard-dot").addClass('active-step');
        //$("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .text-center").addClass('stepInfo-style');
    }
}
function goTo(stepNumber) {
   // debugger;
    // $('.status-type').val(_ContinueStatus);
    if ($(".easyWizardWrapper div[data-step='" + stepNumber + "']").hasClass('active'))
        return

    _currentStep = stepNumber;
    $('#wizard').easyWizard('goToStep', stepNumber);

    if ($(".easyWizardWrapper div[data-step='" + 4 + "']").hasClass('active')) {
        $('#btn_Continue').hide(); //.addClass('hide');
        $('#btn_Submit').show(); //.removeClass('hide');
        $('#btn-Cancel').hide(); //.addClass('hide');
        //$('#btn_SaveAsDraft').hide(); //.addClass('hide');
        $('#btn_Approve').show();
        $('#btn_Reject').show(); 
    }
    else {
        $('#btn_Continue').show(); //.removeClass('hide');
        $('#btn_Submit').hide(); //.addClass('hide');
        $('#btn-Cancel').show(); //.removeClass('hide');
        //$('#btn_SaveAsDraft').show(); //.removeClass('hide');
        $('#btn_Approve').hide();
        $('#btn_Reject').hide(); 
    }

    if (!$(".easyWizardWrapper div[data-step='" + 1 + "']").hasClass('active'))
        $('#btnPrevious').show(); //.removeClass('hide');
    else
        $('#btnPrevious').hide(); //.addClass('hide');

    $("#step1 .bs-wizard-dot, #step2 .bs-wizard-dot, #step3 .bs-wizard-dot").removeClass('active-step');
    $("#step1 .text-center, #step2 .text-center, #step3 .text-center").removeClass('stepInfo-style');

    $("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .bs-wizard-dot").addClass('active-step');
    $("#step" + $(".easyWizardWrapper .step.active").attr("data-step") + " .text-center").addClass('stepInfo-style');
}

function PreviousStep() {
   // debugger;
    $('#btnPrevious').prop('disabled', 'disabled');

    setTimeout("$('#btnPrevious').prop('disabled', false);", 1000);

    var currentStep = $(".easyWizardWrapper .step.active").attr("data-step"); // - 1;
    nextStep = currentStep - 1;

    $('#step' + currentStep).removeClass("active");
    $(".easyWizardWrapper div[data-step='" + currentStep + "']").removeClass("active").css('display', 'none');
    // nextStep.css('display', 'block');


    if (!$('#step' + nextStep).hasClass("active"))
        $('#step' + nextStep).removeClass("disabled").removeClass("complete").addClass("active");

    $('#wizard').easyWizard('goToStep', nextStep);
    $(".easyWizardWrapper div[data-step='" + nextStep + "']").addClass("active");

    $("#step1 .bs-wizard-dot, #step2 .bs-wizard-dot, #step3 .bs-wizard-dot").removeClass('active-step');
    $("#step1 .text-center, #step2 .text-center, #step3 .text-center").removeClass('stepInfo-style');

    $("#step" + nextStep + " .bs-wizard-dot").addClass('active-step');
    $("#step" + nextStep + " .text-center").addClass('stepInfo-style');
    _currentStep--;

    if ($(".easyWizardWrapper div[data-step='" + 4 + "']").hasClass('active')) {
        $('#btn_Continue').hide(); //.addClass('hide');
        $('#btn_Submit').show(); //.removeClass('hide');
        $('#btn-Cancel').hide(); //.addClass('hide');
        //$('#btn_SaveAsDraft').hide(); //.addClass('hide');
        $('#btn_Approve').show();
        $('#btn_Reject').show(); 
    }
    else {
        $('#btn_Continue').show(); //.removeClass('hide');
        $('#btn_Submit').hide(); //.addClass('hide');
        $('#btn-Cancel').show(); //.removeClass('hide');
        //$('#btn_SaveAsDraft').show(); //.removeClass('hide');
        $('#btn_Approve').hide();
        $('#btn_Reject').hide(); 
    }

    if (!$(".easyWizardWrapper div[data-step='" + 1 + "']").hasClass('active'))
        $('#btnPrevious').show(); //.removeClass('hide');
    else
        $('#btnPrevious').hide(); //.addClass('hide');
    // goTo(stepNumber);
}


