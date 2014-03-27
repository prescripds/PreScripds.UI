$(document).ready(function () {
    $('#userLoginViewModel_0__UserName').on('blur', function () {
        var username = $(this).val();
        if (username != '') {
            ajaxPost("/Account/CheckUserName", { username: username }, function (data) {
                if (data != '' && data == true) {
                    $(this).attr('style', 'border:red 1px solid');
                } else {
                    alert(data);
                }
            });
        }
    });
});