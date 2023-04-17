//password show and hide 
$(document).on('click', '.bi-eye-slash', function () {
    $(this).parent().find('input').attr('type', 'text');
    $(this).addClass('d-none');
    $(this).prev().removeClass('d-none');
})
$(document).on('click', '.bi-eye-fill', function () {
    $(this).parent().find('input').attr('type', 'password');
    $(this).addClass('d-none');
    $(this).next().removeClass('d-none');
})