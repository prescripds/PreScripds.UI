$(document).ready(function () {
    $('#userLoginViewModel_0__UserName').on('blur', function () {
        var username = $(this).val();
        if (username != '') {
            ajaxPost("Account/CheckUserName", { loginName: username }, function (data) {
                if (data) {
                    $(this).attr('style', 'border:red 1px solid');
                }
            });
        }
    });
});