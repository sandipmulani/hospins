$(document).on('change','input[type="text"],textarea',function(){
    $(this).val($(this).val().trim());
})

function isNumberKey(event) {

    if ((event.which = 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}

$(document).on('hidden.bs.modal', '.modal', function () {
    if ($('.modal').hasClass('show')) {
        $('body').addClass('modal-open');
    }

    //setTimeout(function () {
    //    if (($.fn.dataTable != undefined) == true) {
    //        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    //    }
    //}, 500);
});

$(document).on('hide.bs.modal', '.modal', function () {
    $(this).find(".modal-dialog").removeClass("zoomIn");
    $(this).find(".modal-dialog").addClass("zoomOut");
});

$(document).on('show.bs.modal', '.modal', function () {
    $(".modal-dialog").addClass("zoomIn animated");
    $(".modal-dialog").removeClass("zoomOut");
    // run your validation... ( or shown.bs.modal )
    setTimeout(function () {
        if ($(".bootbox.modal .alert-success").length > 0) {
            $(".bootbox.modal .alert-success").parent().parent().modal('hide');
        }
    }, 5000);
});

function OpenPopup(modalId, modalContainId, url, FormId, val_Fn, callback) {
    loaderstart();
    $('#' + modalContainId + '').empty();
    $('#' + modalContainId + '').load(url, function (response, status, xhr) {
        if ((response || "") == "" || response.indexOf("<meta charset=\"utf-8\" />") != -1) {
            $('#' + modalId + '').modal('toggle');
            location.reload();
            return;
        }
        $('#' + modalId + '').modal({
            keyboard: true, backdrop: 'static'
        }, 'show');
        setTimeout(function() {
            $("input[type='text'], textarea").change(function(){
                $(this).val($(this).val().trim());
            });
            $("#"+ FormId +" input:text, #"+ FormId +" textarea , #"+ FormId +" select").first().focus();
            $("input, select, textarea").attr("autocomplete", "off");
            try {
                if($('#'+FormId).length > 0) {
                    if($.fn.validate) {
                        $.each($('#'+FormId).validate().settings.rules, function(i, item) {
                            if(item.required) {
                                item["normalizer"] = function(value) {return $.trim(value);}
                            }
                        });   
                    }
                }
            } catch (error) {
                console.log(error);
            }
        },500);
        bindForm(this, modalId, modalContainId, FormId, val_Fn, callback);
        loaderstop();
        //$(".drpdwn").selectpicker({
        //    windowPadding:80
        //});
    });
}
function OpenPopupWithFileUpload(modalId, modalContainId, url, FormId, val_Fn, callback) {
    loaderstart();
    $('#' + modalContainId + '').empty();
    $('#' + modalContainId + '').load(url, function (response, status, xhr) {
        if (response.indexOf("<meta charset=\"utf-8\" />") != -1) {
            $('#' + modalId + '').modal('toggle');
            location.reload();
            return;
        }
        $('#' + modalId + '').modal({
            keyboard: true, backdrop: 'static'
        }, 'show');
        setTimeout(function() {
            $("input[type='text'], textarea").change(function(){
                $(this).val($(this).val().trim());
            });
            $("#"+ FormId +" input:text, #"+ FormId +" textarea , #"+ FormId +" select").first().focus();
            $("input, select, textarea").attr("autocomplete", "off");
            try {
                if($('#'+FormId).length > 0) {
                    if($.fn.validate) {
                        $.each($('#'+FormId).validate().settings.rules, function(i, item) {
                            if(item.required) {
                                item["normalizer"] = function(value) {return $.trim(value);}
                            }
                        });   
                    }
                }
            } catch (error) {
                console.log(error);
            }
        },500);
        bindFormWithFileUpload(this, modalId, modalContainId, FormId, val_Fn, callback);
        loaderstop();
        $("select").selectpicker({
            windowPadding:80
        });
    });
}

function loaderstart() {
    $('.loader-main').show();
    $(".loader-main").css("z-index", "1051");
}

function loaderstop() {
    $('.loader-main').hide();
}

function UploadFile(disId, hdnId, clsId) {
    for (var i = 0; ; i++) {
        if ($("#imgModal_" + i + "")) {
            $('<div/>', {
                id: "imgModal_" + i,
                class: 'modal fade',
                role: "dialog"
            }).appendTo('body');
            $("#imgModal_" + i + "").html('<div class="modal-dialog"><div id=\"myModalContent_' + i + '\" class="modal-content modal-popup"></div></div>');
            $('#myModalContent_' + i + '').load(imageUploadUrl, function (response, status, xhr) {
                $('#imgModal_' + i + '').modal({
                    keyboard: true, backdrop: 'static'
                }, 'show');
                $("#hdnDispId").val(disId);
                $("#hdnHdnImgId").val(hdnId);
                $("#hdnbtnImgId").val(clsId);
                $("#hdnPopupImgId").val("imgModal_" + i);
            });
            break;
        }
    }
}
function closeImage(imgId, btnId) {
    $('.' + imgId).attr("src", "");
    $('.' + btnId).css("display", "none");
}

function bindForm(dialog, modalId, modalContainid, FormId, val_Fn, callback) {
    if (val_Fn) {
        val_Fn();
    }
    $('#' + FormId + '', dialog).submit(function () {
        try {

            if ($('#' + FormId + '').valid()) {
                loaderstart();
                $.ajax({
                    url: this.action,
                    type: this.method,
                    cache: false,
                    data: $(this).serialize(),
                    crossDomain: true,
                    success: function (html, status, xhr) {
                        if (html.success == "true") {
                            $('#' + modalId + '').modal('toggle');
                            setTimeout(function () {
                                if (typeof bootbox !== "undefined") {
                                    Popuphtml(html.returnMsg, 'success');
                                }
                            }, 200);
                            if (callback)
                                callback(html);
                        } else {
                            $('#' + modalContainid + '').html(html.PartialviewContent);

                            //if($('#' + modalContainid + '').find(".drpdwn").length > 0) {
                            //    $('#' + modalContainid + '').find(".drpdwn").selectpicker("refresh");
                            //}
                            bindForm(dialog, modalId, modalContainid, FormId, val_Fn, callback);
                            if (typeof bootbox !== "undefined") {
                                Popuphtml(html.returnMsg, 'error');
                            }
                            else {
                                if (callback)
                                    callback(html);
                            }
                        }
                        loaderstop();
                    },
                    error: function (e) {
                        loaderstop();
                    }
                });
                return false;
            }
            //else
            //    return false;
        } catch (e) {
            return false;
        }
    });
}

function bindFormWithFileUpload(dialog, modalId, modalContainid, FormId, val_Fn, callback) {
    if (val_Fn) {
        val_Fn();
    }
    $('#' + FormId + '', dialog).submit(function () {
        try {

            if ($('#' + FormId + '').valid()) {
                loaderstart();
                var formData = new FormData(document.getElementById(FormId));
                var arr = $('#' + FormId + '').serializeArray();
                arr.forEach(function (data, i) {
                    if (data.name != "__RequestVerificationToken") {
                        formData.append(data.name, data.value);
                    }
                });
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: formData,
                    cache: false,
                    processData: false,
                    contentType: false,
                    headers: {
                        '__RequestVerificationToken': $("#" + FormId + " input[name='__RequestVerificationToken']").val()
                    },
                    success: function (html, status, xhr) {
                        if (html.success == "true") {
                            $('#' + modalId + '').modal('toggle');
                            Popuphtml(html.returnMsg, 'success');
                            if (callback)
                                callback(html);
                            //location.reload();
                        } else {
                            $('#' + modalContainid + '').html(html.PartialviewContent);
                            bindForm(dialog, modalId, modalContainid, FormId, val_Fn, callback);
                            Popuphtml(html.returnMsg, 'error');
                        }
                        loaderstop();
                    },
                    error: function (e) {
                        loaderstop();
                    }
                });
                return false;
            }
            //else
            //    return false;
        } catch (e) {
            return false;
        }
    });
}
//var elPrevStatus = null;
var isConfirm = false;
function Delete(e, url, msg, yes, no, fn_callback) {

    var elPrevStatus = e;
    bootbox.confirm({
        message: msg,
        centerVertical: true,
        buttons: {
            confirm: {
                label: yes,
                className: 'btn-secondary'
            },
            cancel: {
                label: no,
                className: 'btn-primary'
            }
        },
        callback: function (result) {
            if (result) {
                //loaderstart();
                $.ajax({
                    url: url,
                    type: 'post',
                    data: {
                        Id: $(e).attr('data-id'),
                        '__RequestVerificationToken': GetAntiForgeryToken()
                    },
                    headers: {
                        '__isConfirm': isConfirm
                    },
                    success: function (data) {
                        isConfirm = false;
                        if (data.success) {
                            if (data.success == "true" && data.returnMsg) {
                                Popuphtml(data.returnMsg, 'success');
                                //set this timeout bcz befor showing alert msgs it executes the call back function.
                                if (fn_callback) {
                                    data["e"] = e;
                                    fn_callback(data);
                                }
                                else
                                    location.reload();
                            }
                            else if (data.success == "false" && (data.returnMsg || "") == "childExists") {
                                isConfirm = true;
                                Delete(e, url, "This record is being used by other records. Are sure to delete this record?", yes, no, fn_callback);
                            }
                            else if (data.success == "false" && data.returnMsg) {
                                setTimeout(function () {
                                    Popuphtml(data.returnMsg, 'error');
                                }, 200);
                            }
                        }
                    }
                });
                //loaderstop();
            }
            else {
                isConfirm = false;
                $(elPrevStatus).prop("checked", !$(elPrevStatus).prop("checked"));
                //reloadGrid();
            }
        }
    });
}

function CustomDelete(e, url, msg, yes, no, fn_callback) {
    bootbox.confirm({
        message: msg,
        centerVertical: true,
        buttons: {
            confirm: {
                label: yes,
                className: 'btn-secondary'
            },
            cancel: {
                label: no,
                className: 'btn-primary'
            }
        },
        callback: function (result) {
            if (result) {
                loaderstart();
                var data = { Id: $(e).attr('data-id'), '__RequestVerificationToken': GetAntiForgeryToken() }
                $.post(url, data, function (data) {
                    loaderstop();
                    if (data.success) {
                        if (data.success == "true" && data.returnMsg) {
                            if (fn_callback) {
                                data["e"] = e;
                                fn_callback(data);
                            }
                            else {
                                Popuphtml(data.returnMsg, 'success');
                            }
                        }
                        else if (data.success == "false" && data.returnMsg) {
                            setTimeout(function () {
                                Popuphtml(data.returnMsg, 'error');
                            }, 200);
                        }
                    }
                });
            }
        }
    });
}

var elPrevStatus = null;
function ChangeStatus(e, url, msg, yes, no, fn_callback) {
    elPrevStatus = e;
    bootbox.confirm({
        message: msg,
        centerVertical: true,
        buttons: {
            confirm: {
                label: yes,
                className: 'btn-secondary'
            },
            cancel: {
                label: no,
                className: 'btn-primary'
            }
        },

        callback: function (result) {
            if (result) {
                loaderstart();
                var data = { Id: parseInt($(e).attr('data-id')), '__RequestVerificationToken': GetAntiForgeryToken() }
                $.post(url, data, function (data) {
                    if (data && data.returnMsg) {
                        if (data["success"] == "true") {
                            data["e"] = e;
                            setTimeout(function(){
                                Popuphtml(data.returnMsg, 'success');
                            },100);
                        }
                        else {
                            setTimeout(function(){
                                Popuphtml(data.returnMsg, 'error');
                            },100);
                        }
                    }
                    if (fn_callback)
                        fn_callback(data);
                    else
                        location.reload();
                    loaderstop();
                });
            }
            else {
                $(elPrevStatus).prop("checked",!$(elPrevStatus).prop("checked"));
                //reloadGrid();
            }
        }
    });
}

function ConvertToBase64String(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

function AjaxCall(_url, _method, _data, callback) {

    $.ajax({
        url: _url,
        dataType: "json",
        type: _method,
        data: _data,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        cache: false,
        success: function (html, status, xhr) {
            if (callback)
                callback(html);
            else
                return html;
        },
        error: function (e) {
            loaderstop();
        }
    });
}

function AjaxCallPost(_url, _method, _data, callback) {

    $.ajax({
        url: _url,
        //dataType: "json",
        type: _method,
        data: _data,
        //contentType: 'application/json; charset=utf-8',
        //processData: false,
        cache: false,
        success: function (html, status, xhr) {
            if (callback)
                callback(html);
            else
                return html;
        },
        error: function (e) {
            loaderstop();
        }
    });
}

function Rating(data, outof) {
    var str = "";
    var cnt = 0;
    for (var i = cnt; i < data; i++) {
        cnt++;
        str += '<i class="fa fa-star rating-star-yellow"></i>';
    }
    for (var i = cnt; i < outof; i++) {
        cnt++;
        str += '<i class="fa fa-star-o"></i>';
    }
    return str;
}

function SetActiveTab(elmId) {
    $($("#" + elmId + "").parent().parent().find('a')[0]).trigger("click");
    $("#" + elmId + "").addClass("active");
    $("#" + elmId + "").parent().parent("li").addClass("open");
    $("#" + elmId + "").parent("ul").show();
}

function MakeDataTableResponsive(tblId) {
    $("#" + tblId + " tr th").each(function (e, index) {
        $("#" + tblId + " tr td:nth-child(" + (e + 1) + ")").attr('title', $(this).text());;
    });
}

function Popuphtml(message, divclass) {
    if (message)
    {
        var strMsg = '';
        if (message.split('**').length > 0) {
            for (var i = 0; i < message.split('**').length; i++) {
                strMsg += '<p class="text-left">' + message.split('**')[i] + '</p>';
            }
        }
        else {
            strMsg += '<p class="text-left">' + message + '</p>';
        }
        //var dialog = bootbox.dialog({
        //    message: strMsg, //'<p class="text-center">' + message + '</p>',
        //    closeButton: true,
        //    //className: "my-bootbox-dialog",
        //    onEscape: function () {
        //        $('.bootbox.modal').modal('hide');
        //        //$('body').trigger("forcerefresh.scrtabs");
        //    }
        //});
        //$('.bootbox-body').parent().parent().addClass(divclass);

        toastr[divclass](strMsg);
    }
}
$(document).on("hidden.bs.modal", ".bootbox.modal,.modal", function (e) {
    //$('body').trigger("forcerefresh.scrtabs");
});

function PopuphtmlDescription(message, divclass) {
    message = $("#hfdesc" + message).val()
    if (message) {
        var strMsg = '';
        if (message.split('**').length > 0) {
            for (var i = 0; i < message.split('**').length; i++) {
                strMsg += '<p style="word-wrap: break-word;" class="text-center">' + message.split('**')[i] + '</p>';
            }
        }
        else {
            strMsg += '<p style="word-wrap: break-word;" class="text-center">' + message + '</p>';
        }
        var dialog = bootbox.dialog({
            message: strMsg, //'<p class="text-center">' + message + '</p>',
            closeButton: true,
            onEscape: function () {
                $('.bootbox.modal').modal('hide');
            }
        });
        $('.bootbox-body').parent().parent().addClass(divclass);
    }
}

function PopupOk(message) {
    bootbox.alert({
        //title: "Information",
        message: '<p style="word-wrap: break-word;" class="text-center">' + message + '</p>',
        onEscape: function () {
            $('.bootbox.modal').modal('hide');
        }
    });
    /*$('.BpopSmall').html('');
    $('.b-modal').attr("style", "");
    $('.SmallMessage').append(html);
    if (message.split('**').length > 0) {
        for (var i = 0; i < message.split('**').length; i++) {
            $('.Errormes').append('<h4 class="jlr_type_e popup_tt">' + message.split('**')[i] + '</h4>');
        }
    }
    else {
        $('.Errormes').append('<h4 class="jlr_type_e popup_tt">' + message + '</h4>');
    }
    $('.Errormes').parent().append('<div class="divbtn"><a id="targetref" href="#" class="btn btn-success" style="background:#0e502a;width: 75px;"><strong>OK</strong></a></div>');
    $("input[name='UserName']").focus();
    $("input[name='Email']").focus();
    $('#popup_this').bPopup();*/
}

function PopupMail(message) {
    bootbox.alert({
        size: 'large',
        message: '<p style="word-wrap: break-word;" class="text-center">' + message + '</p>',
        onEscape: function () {
            $('.bootbox.modal').modal('hide');
        }
    });
    /*$('.BpopSmall').html('');
    $('.b-modal').attr("style", "");
    $('.SmallMessage').append(html);
    if (message.split('**').length > 0) {
        for (var i = 0; i < message.split('**').length; i++) {
            $('.Errormes').append('<h4 class="jlr_type_e popup_tt">' + message.split('**')[i] + '</h4>');
        }
    }
    else {
        $('.Errormes').append('<h4 class="jlr_type_e popup_tt">' + message + '</h4>');
    }
    $('.Errormes').parent().append('<div class="divbtn"><a id="targetref" href="#" class="btn btn-success" style="background:#0e502a;width: 75px;"><strong>OK</strong></a></div>');
    $("input[name='UserName']").focus();
    $("input[name='Email']").focus();
    $('#popup_this').bPopup();*/
}

$('.btncolse').click(function () {
    if ($(".divbtn").length > 0) {
        $('.divbtn a').click();
    }
})

$(document).on("keypress", ".numberwithfloat", function (event) {
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

$(document).on("keypress", ".numberOnly", function (event) {
    if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});
$(".negativeNumbers").bind("keypress", function (event) {
    if ((event.which < 48 || event.which > 57) && event.which != 45) {
        return false;
    }
    if(event.target.value != undefined && event.target.value.indexOf('-') != -1 && event.which == 45){
        return false;
    }
    return true;
}).bind("paste", function(event) {
    var text = event.originalEvent.clipboardData.getData('Text');
    if (!$.isNumeric(text)) {
        event.preventDefault();
    }
});

//$(document).bind("paste", ".negativeNumbers", function (e) {
//    debugger
//    var text = e.originalEvent.clipboardData.getData('Text');
//    if (!$.isNumeric(text)) {
//        e.preventDefault();
//    }
//});
$('.numberRegEx').on('input', function (event) {
    this.value = this.value.replace(/[^0-9]/g, '');
});
$('.alphanumericRegEx').on('input', function (event) {
    this.value = this.value.replace(/[^a-zA-Z0-9 .@&_-]/g, '');
});
$(document).on('keypress', '.intOnly', function (event) {
    if (event.which < 47 || event.which > 59) {
        event.preventDefault();
    }
});
$('.numberOnly').bind("paste", function (e) {
    var text = e.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            e.preventDefault();
            $(this).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        e.preventDefault();
    }
});
$(document).on('keydown', '.textOnly', function (e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = e.keyCode;
        if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
    }
});
$(document).on('keypress', '.restrictingComma', function (event) {
    if (event.which == 44) {
        event.preventDefault();
    }
});
$(document).on('keypress', '.floatOnly', function (eve) {
    if ((eve.which != 46 || $(this).val().indexOf('.') != -1) && (eve.which < 48 || eve.which > 57) || (eve.which == 46 && $(this).caret().start == 0)) {
        eve.preventDefault();
    }
});

$(document).on('blur', '.from,.to', function (eve) {
    var _this = $(this).closest(".number-range")
    var mode = $(this).hasClass("to") ? "to" : "from";
    var from = $(_this).find(".from");
    var to = $(_this).find(".to");
    var fromValue = ~~($(from).val() || 0);
    var toValue = ~~($(to).val() || 0);
    if(mode == "from") {
        if((toValue != 0 && fromValue > toValue) || ((from.val() || "").length >= 10)) {
            $(from).val("");
        }
    }
    else {
        if((toValue < fromValue) || ((to.val() || "").length >= 10)) {
            $(to).val("");
        }
    }
});

function ShowMessage(obj, title) {

    bootbox.dialog({
        title: title,
        message: "<span style='white-space: pre-wrap;'>" + obj.title.slice(1, -1) + "</span>", // obj.title
        backdrop: 'true',
        onEscape: 'false'
    });


}
function PopMessage(strMessage) {
    $('#popMessage').text(strMessage);
    $.magnificPopup.open({
        removalDelay: 500, //delay removal by X to allow out-animation
        midClick: true, // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
        closeOnBgClick: false, //Background click close popup.
        modal: false, //Esc key  disable enable.
        type: 'inline',
        prependTo: document.forms[0],
        items: {
            src: '#message-popup'
        }
    });
}
function PopMessage2(strMessage2) {

    $('#popMessage2').html(strMessage2.replace(/[*]/g, "</br>"));
    $.magnificPopup.open({
        removalDelay: 500, //delay removal by X to allow out-animation
        midClick: true, // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
        closeOnBgClick: false, //Background click close popup.
        modal: false, //Esc key  disable enable.
        type: 'inline',
        prependTo: document.forms[0],
        items: {
            src: '#message-popup2'
        }
    });
}
$(".modalClose").click(function () {
    $.magnificPopup.close();

});
Array.prototype.removeByIndex = function (index) {
    this.splice(index, 1);
}

Object.defineProperty(String.prototype, '_toSplitArr', {
    value: function () {
        var val = this;
        return (val.replace(/(\r\n|\n|\r)/gm, ",").replace(/(\r\t|\t|\r)/gm, ",").replace(/;/g, ",").replace(/\n\s*\n/g, '\n') || "").split(",").filter(String);
    }
});

Object.defineProperty(Array.prototype, '_ToChunk', {
    value: function (chunkSize) {
        var array = this;
        return [].concat.apply([],
            array.map(function (elem, i) {
                return i % chunkSize ? [] : [array.slice(i, i + chunkSize)];
            })
        );
    }
});

function getUrlParams() {
    var params = {};
    decodeURI(window.location.search).replace(/[?&]+([^=&]+)=([^&]*)/gi, function (str, key, value) {
        params[key] = value;
    });
    return params;
}

function valNRIC(str) {
    /* check string alphanumeric or not */
    var acheckOK = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
    var acheckStr = str;
    var allstrValid = true;
    for (i = 0; i < acheckStr.length; i++) {
        ch = acheckStr.charAt(i);
        for (j = 0; j < acheckOK.length; j++)
            if (ch == acheckOK.charAt(j))
                break;

        if (j == acheckOK.length) {
            allstrValid = false;
            break;
        }
    }
    if (!allstrValid) {
        return (false);
    }

    /* check length which is 9*/
    if (str.length != 9) {
        return false;
    }

    /* for get first charecter of string */
    var firstchar = str.charAt(0).toUpperCase();
    //alert(firstchar);

    /* for get last charecter of string */
    var lastchar = str.charAt(8).toUpperCase();
    //alert(lastchar);

    /* checking first charecter is comes from STFG or Not */
    var firstcharflag = "0";
    var fcheckOK = "STFG";
    for (j = 0; j < fcheckOK.length; j++) {
        if (firstchar == fcheckOK.charAt(j)) {
            firstcharflag = "1";
            break;
        }
    }
    if (firstcharflag == 0) {
        return false;
    }

    /* checking last charecter is comes from alphabet or not */
    var lastcharflag = "0";
    var lastcharval = "";
    if (firstchar == "S" || firstchar == "T") /* Singaporean */ {
        var lcheckOK = "ABCDEFGHIJZ";
        for (j = 0; j < lcheckOK.length; j++) {
            if (lastchar == lcheckOK.charAt(j)) {
                lastcharflag = "1";
                break;
            }
        }

        /* Prefix checking */
        if (lastchar == "A") {
            lastcharval = "10";
        }
        else if (lastchar == "B") {
            lastcharval = "9";
        }
        else if (lastchar == "C") {
            lastcharval = "8";
        }
        else if (lastchar == "D") {
            lastcharval = "7";
        }
        else if (lastchar == "E") {
            lastcharval = "6";
        }
        else if (lastchar == "F") {
            lastcharval = "5";
        }
        else if (lastchar == "G") {
            lastcharval = "4";
        }
        else if (lastchar == "H") {
            lastcharval = "3";
        }
        else if (lastchar == "I") {
            lastcharval = "2";
        }
        else if (lastchar == "Z") {
            lastcharval = "1";
        }
        else if (lastchar == "J") {
            lastcharval = "0";
        }

    }
    if (firstchar == "F" || firstchar == "G") /* Foreigner */ {
        var lcheckOK = "KLMNPQRTUWX";
        for (j = 0; j < lcheckOK.length; j++) {
            if (lastchar == lcheckOK.charAt(j)) {
                lastcharflag = "1";
                break;
            }
        }

        /* Prefix checking */
        if (lastchar == "K") {
            lastcharval = "10";
        }
        else if (lastchar == "L") {
            lastcharval = "9";
        }
        else if (lastchar == "M") {
            lastcharval = "8";
        }
        else if (lastchar == "N") {
            lastcharval = "7";
        }
        else if (lastchar == "P") {
            lastcharval = "6";
        }
        else if (lastchar == "Q") {
            lastcharval = "5";
        }
        else if (lastchar == "R") {
            lastcharval = "4";
        }
        else if (lastchar == "T") {
            lastcharval = "3";
        }
        else if (lastchar == "U") {
            lastcharval = "2";
        }
        else if (lastchar == "W") {
            lastcharval = "1";
        }
        else if (lastchar == "X") {
            lastcharval = "0";
        }
    }
    if (lastcharflag == 0) {
        return false;
    }

    //2765432
    var d1 = str.charAt(1);
    var d2 = str.charAt(2);
    var d3 = str.charAt(3);
    var d4 = str.charAt(4);
    var d5 = str.charAt(5);
    var d6 = str.charAt(6);
    var d7 = str.charAt(7);

    if (firstchar == "S" || firstchar == "F") {
        var d0 = "0";
    }
    else if (firstchar == "T" || firstchar == "G") {
        var d0 = "4";
    }
    //alert(d0);

    /* 2765432 calculation is standard format from algoritham with nric number's each num */
    var acal = Math.abs(d0) + Math.abs((d1 * 2) + (d2 * 7) + (d3 * 6) + (d4 * 5) + (d5 * 4) + (d6 * 3) + (d7 * 2));
    //alert(acal);
    var bcal = acal % 11;
    //alert(bcal);
    //alert(lastcharval);

    if (lastcharval == bcal) {
        return true;
    }
    else {
        return false;
    }
}

function getContentFromHtml(s) {
    var span = document.createElement('span');
    span.innerHTML = s;
    return span.textContent || span.innerText;
};

if (!Array.prototype.find) {
    Object.defineProperty(Array.prototype, 'find', {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return kValue.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return kValue;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return undefined.
            return undefined;
        }
    });
}

function isNullEmptyUndefined(_val) {
    return (_val == undefined || _val == null || _val == "" || _val == "null" || _val == "undefined") ? true : false;
}


///Function for update ckeditor value on submit
function UpdateCKEDITORInstance() {
    for (instance in CKEDITOR.instances) {
        CKEDITOR.instances[instance].updateElement();
    }
}

//multiple delete start
function chkAll(obj, name) {
    if (obj.checked) {
        if ($('#' + name + ' tbody input[ChekBoxType="DeleteCheckBox"]').length > 0) {
            $('#' + name + ' tbody input[ChekBoxType="DeleteCheckBox"]').prop('checked', true);
        }

    } else {
        if ($('#' + name + ' tbody input[ChekBoxType="DeleteCheckBox"]').length > 0) {
            $('#' + name + ' tbody input[ChekBoxType="DeleteCheckBox"]').prop('checked', false);
        }
    }
    $('input[ChekBoxType]').each(function () {
        AddItemInArray($(this).val(), obj.checked)
    });
}
//check box check in paging
var checkboxItem = [];
function AddItemInArray(item, chk) {
    chk = chk ? false : chk;
    if (item != undefined) {
        var idx = $.inArray(item, checkboxItem);
        if (idx == -1) {
            checkboxItem.push(item);
        }
        else if (!chk) {
            checkboxItem.splice(idx, 1);
        }
    }
    if(checkboxItem.length > 0) {
        $("#btnDelete").fadeIn("1000");
    }
    else {
        $("#btnDelete").fadeOut("1000");
    }
}
function DeleteGridData(obj, name, DeleteUrl, MessageTitle, reloadGrid) {
    var ids = "";
    $('#' + name + ' tbody input[ChekBoxType="DeleteCheckBox"]').each(function () {
        if ($(this).prop("checked")) {
            ids += ids == "" ? $(this).val() : "," + $(this).val()
        }
    });
    if (ids == "") {
        Popuphtml('Please select at least one record to delete', 'warning');
        return;
    }
    $(obj).attr("data-id", ids);
    Delete(obj, DeleteUrl, 'Are you sure want to delete this ' + (MessageTitle || "").toLocaleLowerCase() + '?', 'Yes', 'No', reloadGrid);
}
function chkChild(obj, name) {
    var table = $('#' + name + '').DataTable();
    var info = table.page.info();
    var showingEntries = (info.end - info.start);
    var checkedCount = $('[ChekBoxType="DeleteCheckBox"]:checked').length;
    var tooltipcount = $('.DeleteCheckBoxToolTip').length;
    if ((showingEntries - tooltipcount) == checkedCount)
        $('input[name="flowcheckall"]').prop('checked', true);
    else
        $('input[name="flowcheckall"]').prop('checked', false);
    AddItemInArray($(obj).val());
}
//multiple delete end
var filterArr = [];
var option_Mapping = {"input":"","select":" option:selected"};

$(document).ready(function () {
    if($.fn.selectpicker) {
        $.fn.selectpicker.Constructor.DEFAULTS.liveSearch = true;
        $(".drpdwn").selectpicker({
            windowPadding:80
        });
        $('.selectpicker-custom').selectpicker({
            liveSearch:false,
            windowPadding:80
        }).on("changed.bs.select", function(e, clickedIndex, newValue, oldValue) {
            $(".clrFilter").hide();
            $("."+$(".filter-selection option:selected").attr("option-class")).show();
        });
    }
    $(".clrFilter").hide();
    $("." + $(".filter-selection option:selected").attr("option-class")).show();

    if($.fn.daterangepicker) {
        $('.fromdatepickerfor-filter').daterangepicker({
            "showClose": true,
            "showClear": true,
            "showTodayButton": true,
            "locale": { "format": 'MMM DD, YYYY' }
        }).attr("autocomplete", "off");
    }

});

$(document).on("click","#btnAddFilter",function() {
    var FieldName = $(".filter-selection option:selected").val();
    var FieldText = $(".filter-selection option:selected").text();
    var opType = $(".filter-selection option:selected").attr("option-type");
    var opdtType = $(".filter-selection option:selected").attr("datatype");
    var opClassName = $(".filter-selection option:selected").attr("option-class");
    
    var FieldValueText = "";
    var FieldValue = "";
    switch (opType) {
        case "select":
            FieldValueText = $("."+opClassName +" "+option_Mapping[opType]).map(function () {
                return $(this).text();
            }).get().join(',');

            FieldValue = $("."+opClassName +" "+option_Mapping[opType]).map(function () {
                return $(this).val();
            }).get().join(',');

            break;
        case "input":
            FieldValueText = $("."+opClassName +" "+option_Mapping[opType]).val();

            FieldValue = $("."+opClassName +" "+option_Mapping[opType]).val();
            break;
    }
    
    if((FieldValue || "") != "") {
        var checkExists = filterArr.filter(function(v){ if(v["FieldName"] == FieldName && v["FieldValue"] == FieldValue) return v; });
        if(checkExists.length <= 0) {
            filterArr.push({"FieldName":FieldName,"FieldValue":FieldValue,"FieldValueText":FieldValueText,"FieldText":FieldText,"Datatype":opdtType});
            setFilterHistory();
            addFilterTag();
            rebuildFilterTag();
            switch (opType) {
            case "select":
                $("."+opClassName +" "+opType).val(null).trigger("change");
                break;
            case "input":
                $("."+opClassName).val("");
                break;
            }
        }
        else {
            Popuphtml('Selected filter is already added, Please select different filter.', 'error');
        }
    }
    else {
        Popuphtml('Please enter filter criteria.', 'error');
    }
});
$(document).on("click",".closed-filter",function() {
    idx = $(this).attr("data-index");
    filterArr.removeByIndex(~~idx);
    setFilterHistory();
    $(this).parent().remove();
    rebuildFilterTag();
});
$(document).on("click",".clear-filter",function(){
    filterArr = [];
    setFilterHistory();
    $("#isVisibleApplyFilter").hide();
    rebuildFilterTag();
});

function setFilterHistory() {
    localStorage.removeItem('DataTables_'+$(self).attr("id")+"_"+window.location.pathname);
    setCookie($(self).attr("id"), JSON.stringify(filterArr));
}

function addFilterTag() {
    if(filterArr.length > 0) {
        var tags="";
        $.each(filterArr, function(i, item) {
            tags += '<div class="float-left add-filter"><span class="">'+ item["FieldText"] +': '+ item["FieldValueText"] +' </span><a href="javascript:void(0);" class="closed-filter" data-index="'+ i +'"><img src="'+ ImagePath +'/closed.svg"></a></div>';
        });
        $("#appliedFilter").html(tags);
        $("#isVisibleApplyFilter").show();
    }
    else {
        $("#isVisibleApplyFilter").hide();
    }
}

function rebuildFilterTag() {
    $(".add-filter").each(function(i,e) {
        $(e).find(".closed-filter").attr("data-index",i);
    });
    if (filterArr.length < 1) {
        $("#isVisibleApplyFilter").hide();
    }

    if($.isFunction(filterCallBack)) {
        filterCallBack();
    }
}

function BindGrid() {
    var qString = getUrlParams();
    $.each(qString,function(index, value){
        var selectedEl;
        $(".filter-selection option").each(function(i,e){
            if((index || "").toLowerCase() == ($(e).val() || "").toLowerCase()) {
                selectedEl = $(e);
            }
        });
        if(selectedEl) {
            var FieldName = selectedEl.val();
            var FieldText = selectedEl.text();
            var opType = selectedEl.attr("option-type");
            var opdtType = selectedEl.attr("datatype");
            var opClassName = selectedEl.attr("option-class");

            var FieldValueText = "";
            switch (opType) {
            case "select":
                $("."+ opClassName +" option").each(function(i,e){
                    if((value || "").toString().toLowerCase() == ($(e).val() || "").toString().toLowerCase()) {
                        FieldValueText = $(e).text();
                    }
                });
                break;
            case "input":
                FieldValueText = value;
                break;
            }
            var FieldValue = value;
            if((FieldValue || "") != "") {
                var checkExists = filterArr.filter(function(v){ if(v["FieldName"] == FieldName && v["FieldValue"] == FieldValue) return v; });
                if(checkExists.length <= 0) {
                    filterArr.push({"FieldName":FieldName,"FieldValue":FieldValue,"FieldValueText":FieldValueText,"FieldText":FieldText,"Datatype":opdtType});
                }
            }
        }
    });
    addFilterTag();
    rebuildFilterTag();
}

function getTimepickiTime(obj) {
    var TimeValue = obj.val();
    var timeSlt = [];
    if((TimeValue || "") != "") {
        var time = TimeValue.split(" ");
        if(time.length > 0) {
            var resultSplit = time[0].split(':');
            if(resultSplit.length > 0) {
                timeSlt.push(resultSplit[0]);
                timeSlt.push(resultSplit[1]);
                timeSlt.push(time[1]);
            }
        }
    }
    else {
        timeSlt = formatAMPM(new Date);
    }
    return timeSlt;
}

function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0'+minutes : minutes;
    var timeSlt = [];
    timeSlt.push(hours);
    timeSlt.push(minutes);
    timeSlt.push(ampm);
    return timeSlt;
}

function fnDecodeHtml(html) {
    var result = '';
    if (html != undefined && html != '') {
        var txt = document.createElement("textarea");
        txt.innerHTML = html;
        result = txt.value;
    }
    return result;
}

function setCookie(cname, cvalue) {
    var d = new Date();
    d.setTime(d.getTime() + (1*24*60*60*1000));
    var expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
        c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
        }
    }
    return "";
}

setTimeout(function(){
    var filterExists = JSON.parse(getCookie($(self).attr("id")) || "[]");
    if(filterExists.length > 0) {
        filterArr = filterExists;
        addFilterTag();
        rebuildFilterTag();
    }
},100);

function ConfrimBoxWithCallback(title,htmlMessage,yesLable,NoLable,callbackfunction,yesclass){
    bootbox.confirm({
        title:title,
        message: htmlMessage,
        centerVertical: true,
        buttons: {
            cancel: {
                label: NoLable,
                className: 'btn-secondary'
            },
            confirm: {
                label: yesLable,
                className: yesclass==""?'btn-primary':yesclass
            }            
        },
        callback: function(result){
            callbackfunction(result);
        }
    });
}