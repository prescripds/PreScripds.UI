// ajax.js

//highlight animation
(function ($) {
    $.fn.glow = function () {
        this.animate({ backgroundColor: "#ffffcc" }, 1).animate({ backgroundColor: "#EDEEEF" }, 1500);
    };
    
})(jQuery);

var port = $(location).attr('protocol');
var host = $(location).attr('host');
var basePath = (typeof (appSettings) != 'undefined' && appSettings.AppPath) || '';

var buildUrl = function (actionurl) {
    return actionurl[0] != '/' ? basePath + actionurl : actionurl;
};

var ajaxLoad = function (url, args, $container, onSuccess) {
    $.get(buildUrl(url),
            args,
            function (data) {
                if ($container)
                    $container.html(data);
                if (onSuccess)
                    onSuccess(data);
            },
            'html'
        );
};

var ajaxPost = function (url, args, onSuccess) {
    var fullUrl = buildUrl(url);

    $.post(
            fullUrl,
            args,
            function (data) {
                if (onSuccess)
                    onSuccess(data);
            }
        );
};

var ajaxGetJson = function (url, args, onSuccess) {

    var fullUrl = buildUrl(url);
    $.ajax({
        url: fullUrl,
        dataType: 'json',
        data: args,
        success: onSuccess
    });
};

var ajaxPostJson = function (url, args, onSuccess, onError) {

    var fullUrl = buildUrl(url);
    $.ajax({
        type: 'POST',
        url: fullUrl,
        dataType: 'json',
        data: args,
        success: onSuccess,
        error: onError
    });
};

function redirect(url) {
    location.href = url;
}


