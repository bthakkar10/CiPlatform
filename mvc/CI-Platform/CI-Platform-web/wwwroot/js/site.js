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


//// drag and drop images in share your story page
//var allfiles = [];
//var fileInput = document.getElementById('mission-img-input');
//var fileList;
//function handleFiles(e) {
//    console.log("hi")

//    // Add dropped images or selected images to the list
//    var files = e.target.files || e.originalEvent.dataTransfer.files;

//    // Add selected images to the list
//    for (var i = 0; i < files.length; i++) {
//        var file = files[i];
//        var reader = new FileReader();

//        var imageType = /image\/(png|jpeg|jpg)/;
//        if (!file.type.match(imageType)) {
//            alert("Only png, jpg and jpeg image types are allowed.");
//            return false;
//        }

//        // Validate image size
//        if (file.size > 4 * 1024 * 1024) {
//            alert("An image should not be greater than 4MB.");
//            return false;
//        }

//        allfiles.push(files[i]);


//        // Create image preview and close icon
//        // Create image preview and close icon
//        reader.onload = (function (file) {
//            return function (e) {
//                var image = $('<img>').attr('src', e.target.result);
//                var closeIcon = $('<span>').addClass('close-icon').text('x');

//                // Add image and close icon to the list
//                var item = $('<div>').addClass('image').append(image).append(closeIcon);
//                imageList.append(item);

//                // Handle close icon click event
//                closeIcon.on('click', function () {
//                    item.remove();
//                    allfiles.splice(allfiles.indexOf(file), 1);
//                    console.log(allfiles);
//                    if (allfiles.length < 20) {
//                        fileInput.disabled = false;
//                    }
//                });
//            };
//        })(file);

//        // Read image file as data URL
//        reader.readAsDataURL(file);
//    }
//    if (allfiles.length > 20) {
//        alert("Maximum 20 images can be added.");
//        // Remove the last added file from the list
//        allfiles.splice(-1, 1);
//        // Remove the last added image preview from the list
//        imageList.children().last().remove();
//        //// Disable further file selection
//        fileInput.disabled = true;
//    }
//    // Create a new DataTransfer object
//    var dataTransfer = new DataTransfer();
//    // Create a new FileList object from the DataTransfer object
//    fileList = dataTransfer.files;
//}

////var allfiles = new DataTransfer().files;
//var dropzone = $('#mission-img-drop-area');
//var imageList = $('#mission-img-output');

//// Handle file drop event
//dropzone.on('drop', function (e) {
//    e.preventDefault();
//    e.stopPropagation();

//    // Remove dropzone highlight
//    dropzone.removeClass('dragover');
//    $('.note-dropzone').remove();
//    //$('.note-dropzone-message').remove();
//    handleFiles(e);
//});

//dropzone.on('click', function (e) {
//    //e.preventDefault();
//    $('#mission-img-input').click();
//})

//// Handle file dragover event
//dropzone.on('dragover', function (e) {
//    e.preventDefault();
//    e.stopPropagation();

//    // Highlight dropzone
//    dropzone.addClass('dragover');
//});

//// Handle file dragleave event
//dropzone.on('dragleave', function (e) {
//    e.preventDefault();
//    e.stopPropagation();

//    // Remove dropzone highlight
//    dropzone.removeClass('dragover');
//});


//// Handle file input change event
//$('#mission-img-input').on('change', function (e) {
//    handleFiles(e);
//    if (allfiles.length >= 20) {
//        fileInput.disabled = true;
//    }
//});
