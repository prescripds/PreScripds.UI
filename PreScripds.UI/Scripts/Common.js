$(document).ready(function () {

});
function showProgressBar() {
    var viewportHeight = ($(window).height() / 2) - 50;
    var documentHt = $('body').height();
    $('.loadingDiv').removeAttr('style');
    $('.loadingDiv').css('height', documentHt);
    $('.progress-img').css('top', viewportHeight);
    $('.loadingDiv').show();
}
function hideProgressBar() {
    $('.loadingDiv').hide();
}

