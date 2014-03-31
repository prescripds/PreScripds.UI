$(document).ready(function () {
    $('#userLoginViewModel_0__UserName').on('blur', function () {
        var username = $(this).val();
        if (username != '') {
            ajaxPost("/Account/CheckUserName", { username: username }, function (data) {
                if (data != '' && data == "True") {
                    $('#emailExistsDiv').removeAttr('style');
                    $('#emailExistsDiv').attr('style', 'display:block');
                    $('#userNameExists').text('User Name already exists.');
                }
            });
        }
    });
});