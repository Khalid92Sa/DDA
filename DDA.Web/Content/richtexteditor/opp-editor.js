var editorScopeOfWork = new RichTextEditor(document.getElementById("ScopeOfWork_editor"));
editorScopeOfWork.attachEvent("change", function () {
    document.getElementById("ScopeOfWork").value = editorScopeOfWork.getHTMLCode();
});

var editorScopeOfWorkAr = new RichTextEditor(document.getElementById("ScopeOfWorkAr_editor"));
editorScopeOfWorkAr.attachEvent("change", function () {
    document.getElementById("ScopeOfWorkAr").value = editorScopeOfWorkAr.getHTMLCode();
});

var editorEligibilityAndDocuments = new RichTextEditor(document.getElementById("EligibilityAndDocuments_editor"));
editorEligibilityAndDocuments.attachEvent("change", function () {
    document.getElementById("EligibilityAndDocuments").value = editorEligibilityAndDocuments.getHTMLCode();
});

var editorEligibilityAndDocumentsAr = new RichTextEditor(document.getElementById("EligibilityAndDocumentsAr_editor"));
editorEligibilityAndDocumentsAr.attachEvent("change", function () {
    document.getElementById("EligibilityAndDocumentsAr").value = editorEligibilityAndDocumentsAr.getHTMLCode();
});


function ValidateDocument() {
    $("#ValFile").addClass("d-none");
    var proposalFile = $("#RFPFile");
    if (proposalFile.val() == "") {
        $("#ValFile").removeClass("d-none");
        return false;
    }
    else {
        $("#ValFile").addClass("d-none");
        return true;
    }
}

function ValidationDate(withFile) {
    $("#ValDate").addClass("d-none");
    var startDate = new Date(document.getElementById('txt_RFPStartDateTime').value);
    var endDate = new Date(document.getElementById('txt_RFPEndDateTime').value);

    var expectedDate = new Date(document.getElementById('txt_RFPExpectedAwardedDateTime').value);
    if (expectedDate < endDate) {
        $("#ValExpDate").removeClass("d-none");
        return false;
    }
    else {
        $("#ValExpDate").addClass("d-none");
    }

    if (startDate < endDate) {
        $("#ValDate").addClass("d-none");
        if (withFile) {
            return ValidateDocument();
        }
        else {
            return true;
        }
    } else {
        $("#ValDate").removeClass("d-none");
        return false;
    }
}

$(document).ready(function () {

    //$('#ScopeOfWork, #ScopeOfWorkAr, #EligibilityAndDocuments, #EligibilityAndDocumentsAr');

    $("#btnClose").on('click', function () {
        window.location = '@Url.Action("Index", "Home")';
    });

    $('#txt_RFPStartDateTime, #txt_RFPEndDateTime, #txt_RFPExpectedAwardedDateTime').on('change', function () {
        ValidationDate(false);
    });
});
function showMainPDF(selectedId) {
    var splitedId = selectedId;
    var file = document.getElementById(selectedId).files[0];
    //var imageType = 'application/pdf';
    var imageType = "application/pdf";
    if (file.type.match(imageType)) {
        var img = document.getElementById("thumbnil_" + splitedId);
        $("#close_" + splitedId).show();
        $("#plusIcon_" + splitedId).hide();
        img.file = file;
        var reader = new FileReader();
        reader.onload = (function (aImg) {
            return function (e) {
                aImg.src = '../../Content/images/upload-success.svg';
            };
        })(img);
        reader.readAsDataURL(file);
    }
    else {
        var file = document.getElementById(selectedId).files[0];
        var img = document.getElementById("thumbnil_" + splitedId);
        var reader = new FileReader();
        reader.onload = (function (aImg) {
            return function (e) {
                aImg.src = '../../Content/images/upload-failed.svg';
            };
        })(img);
        reader.readAsDataURL(file);
    }

    if (splitedId[1] == "Logo") {
        $("#IsDeletedLogo").val(false);
    }
    if (splitedId[1] == "CoverImage") {
        $("#IsDeletedCoverImg").val(false);
    }
    if (splitedId[1] == "RFPFile") {
        $("#IsDeletedCategoryImg").val(false);
    }

    ValidateDocument();
}
function removeMainPDF(selectedId, imageType) {
    var splitedId = String(selectedId).split('_');
    var img = document.getElementById("thumbnil_" + splitedId[1]);
    $("#close_" + splitedId[1]).hide();
    $("#plusIcon_" + splitedId[1]).show();
    img.file = "";
    if (imageType == coverImageTypeCode) {
        img.src = baseUrl + "/Content/images/UploadCover.png";
    }
    else
        img.src = baseUrl + "/Content/images/upload-default.svg";
    var file = document.getElementById(splitedId[1]);
    if (file != null && file != undefined && file != "") {
        $("#" + splitedId[1]).val('');
    }

    if (splitedId[1] == "Logo") {
        $("#IsDeletedLogo").val(true);
    }
    if (splitedId[1] == "CoverImage") {
        $("#IsDeletedCoverImg").val(true);
    }
    if (splitedId[1] == "RFPFile") {
        $("#IsDeletedCategoryImg").val(true);
    }
}







