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
//change date format 
function ChangeDateFormat(date) {
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
}

//to check session has expired or not 
setInterval(checkSessionStatus, 1800000);
function checkSessionStatus() {

    $.ajax({
        type: "GET",
        url: "/Auth/SessionStatus",
        success: function (data) {
            if (!data.sessionExists) {
                // redirect the user to the login page
                window.location.href = "/Auth/Index";
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}
