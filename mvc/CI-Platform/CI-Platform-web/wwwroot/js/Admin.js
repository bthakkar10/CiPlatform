//$('.validateAdminForm').removeData('validator').removeData('unobtrusiveValidation');
//$.validator.unobtrusive.parse('.validateAdminForm');


//SIDE NAV BAR
const shrink_btn = document.querySelector(".shrink-btn");
const search = document.querySelector(".search");
const sidebar_links = document.querySelectorAll(".sidebar-links a");
const active_tab = document.querySelector(".active-tab");
const shortcuts = document.querySelector(".sidebar-links h4");
const tooltip_elements = document.querySelectorAll(".tooltip-element");

let activeIndex;

shrink_btn.addEventListener("click", () => {
    document.body.classList.toggle("shrink");
    setTimeout(moveActiveTab, 400);

    shrink_btn.classList.add("hovered");

    setTimeout(() => {
        shrink_btn.classList.remove("hovered");
    }, 500);
});

//search.addEventListener("click", () => {
//    document.body.classList.remove("shrink");
//    search.lastElementChild.focus();
//});

function moveActiveTab() {
    let topPosition = activeIndex * 58 + 2.5;

    if (activeIndex > 3) {
        topPosition += shortcuts.clientHeight;
    }

    active_tab.style.top = `${topPosition}px`;
}

function changeLink() {
    sidebar_links.forEach((sideLink) => sideLink.classList.remove("active"));
    this.classList.add("active");

    activeIndex = this.dataset.active;

    moveActiveTab();
}

sidebar_links.forEach((link) => link.addEventListener("click", changeLink));

function showTooltip() {
    let tooltip = this.parentNode.lastElementChild;
    let spans = tooltip.children;
    let tooltipIndex = this.dataset.tooltip;

    Array.from(spans).forEach((sp) => sp.classList.remove("show"));
    spans[tooltipIndex].classList.add("show");

    tooltip.style.top = `${(100 / (spans.length * 2)) * (tooltipIndex * 2 + 1)}%`;
}

tooltip_elements.forEach((elem) => {
    elem.addEventListener("mouseover", showTooltip);
});

//$(".sidebar-links .nav-link").click(function () {
//    $(".nav-link").removeClass("active");
//    $(this).addClass("active");
//});
//to show active class in sidebar 
$(".sidebar-links .nav-link").each(function () {
    var url = $(this).attr('href');
    if (window.location.href.includes(url)) {
        $(this).addClass('active')
    }
})
//SIDE NAV BAR ENDS
//To show date and time in admin panel top header
function updateTime() {
    var dateTime = new Date();
    var formattedDateTime = dateTime.toLocaleString('en-US', { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric', hour: 'numeric', minute: 'numeric', hour12: true });
    $("#datetime").text(formattedDateTime);
}

//set interval will update time every seconds
setInterval(updateTime, 1000);
//APPEND PARTIAL VIEW FOR ADD OR EDIT USER  
$(document).on('click', '#AddNewUserBtn', function () {
    $.ajax({
        url: '/Admin/AddNewuser',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddNewUserForm').html(result);
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});

//DATA FETCHING FOR USER EDIT
$(document).on('click', '#EditBtnUserDataFetch', function () {
    var UserId = $(this).data('user-id');
    $.ajax({
        url: '/Admin/GetUserData',
        type: 'GET',
        data: { UserId: UserId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddNewUserForm').html(result);
            const isEditMode = true; // or false
            $('#AddHeader .FormHeading span').text(isEditMode ? 'Edit' : 'Add');
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//to get id for delete
$(document).on('click', '#DeleteUserBtn', function () {
    var UserId = $(this).data('user-id');
    $("#HiddenUserId").val(UserId);
});
//cities based on countries in admin panel user page
$(document).on('click', '#AdminCountrySelect', function () {
    var CountryId = $(this).val()
    UserGetCitiesByCountry(CountryId);
});
function UserGetCitiesByCountry(CountryId) {
    $.ajax({
        type: "GET",
        url: "/User/GetCitiesByCountry",
        data: { CountryId: CountryId },
        success: function (result) {
            var selectList = $("#UserCitySelect");
            var items = "";
            items = " <option selected>Enter your City</option>";
            $(result).each(function (index, item) {
                items += `<option value=` + item.cityId + `>` + item.cityName + `</option>`;
            })
            selectList.html(items);
        }
    });
}
//APPEND PARTIAL VIEW FOR ADD OR EDIT CMS PAGE
$(document).on('click', '#AddOrUpdateCms', function () {
    $.ajax({
        url: '/Admin/AddOrUpdatePrivacyPages',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdatePrivacyPages').html(result);
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//DATA FETCHING FOR CMS PAGE EDIT
$(document).on('click', '#EditBtnCmsPage', function () {

    var CmsId = $(this).data('cms-id');
    $.ajax({
        url: '/Admin/GetCmsData',
        type: 'GET',
        data: { CmsId: CmsId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdatePrivacyPages').html(result);

            const isEditMode = true; // or false
            $('#CmsHeader .FormHeading span').text(isEditMode ? 'Edit' : 'Add');
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//to get id for delete
$(document).on('click', '#DeleteCmsBtn', function () {
    var CmsId = $(this).data('cms-id');
    $("#HiddenCmsId").val(CmsId);
});
//APPEND PARTIAL VIEW FOR ADD OR EDIT MISSION THEME
$(document).on('click', '#AddOrUpdateThemeBtn', function () {
    $.ajax({
        url: '/Admin/AddOrUpdateTheme',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdateTheme').html(result);
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//DATA FETCHING FOR THEME EDIT
$(document).on('click', '#EditBtnTheme', function () {

    var ThemeId = $(this).data('theme-id');
    $.ajax({
        url: '/Admin/GetThemeData',
        type: 'GET',
        data: { ThemeId: ThemeId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdateTheme').html(result);
            const isEditMode = true; // or false
            $('#ThemeHeader .FormHeading span').text(isEditMode ? 'Edit' : 'Add');
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//to get id for delete
$(document).on('click', '#DeleteThemeBtn', function () {
    var ThemeId = $(this).data('theme-id');
    $("#HiddenThemeId").val(ThemeId);
});

//APPEND PARTIAL VIEW FOR ADD OR EDIT SKKILL
$(document).on('click', '#AddOrUpdateSkillBtn', function () {
    $.ajax({
        url: '/Admin/AddOrUpdateSkill',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdateSkill').html(result);
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//DATA FETCHING FOR Skill PAGE EDIT
$(document).on('click', '#EditBtnSkill', function () {
    var SkillId = $(this).data('skill-id');
    $.ajax({
        url: '/Admin/GetSkillData',
        type: 'GET',
        data: { SkillId: SkillId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdateSkill').html(result);
            const isEditMode = true; // or false
            $('#SkillHeader .FormHeading span').text(isEditMode ? 'Edit' : 'Add');
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//to get skill id for delete
$(document).on('click', '#DeleteSkillBtn', function () {
    var SkillId = $(this).data('skill-id');
    $("#HiddenSkillId").val(SkillId);
});

//to get id of mission application to decline or approve it 
$(document).on('click', '.ApproveOrDeclineApplication', function () {
    var MissionApplicationId = $(this).data('application-id');
    $("#HiddenApplicationId").val(MissionApplicationId);
});
//to fetch data for approval of mission application
$(document).on('click', '#ApprovalBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = false; // or false
    $('#ApplicationModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this mission application??' : 'Are you sure you want to approve this mission application??');
});
//to fetch data for decline of mission application 
$(document).on('click', '#DeclineBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = true; // or false
    $('#ApplicationModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this mission application??' : 'Are you sure you want to approve this mission application??');
});
//view story in story page of admin
$(document).on('click', '#ViewStoryBtn',function () {
    var MissionId = $(this).data('mission-id');
    $.ajax({
        method: 'GET',
        url: '/Admin/StoryDetailsAdmin',
        data: { MissionId: MissionId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#StoryDetailsAdmin').html(result);
        },
        error: function (error) {
            console.log(error);
        },

    });
});
//to get story id to decline, approve or delete it 
$(document).on('click', '.GetStoryIdBtn', function () {
    var StoryId = $(this).data('story-id');
    console.log(StoryId)
    $(".HiddenStoryId").val(StoryId);
});


//to fetch data for approval of story
$(document).on('click', '#StoryApprovalBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = false; // or false
    $('#StoryModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this story??' : 'Are you sure you want to approve this story??');
});
//to fetch data for decline of mission application 
$(document).on('click', '#StoryDeclineBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = true; // or false
    $('#StoryModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this story??' : 'Are you sure you want to approve this story??');
});

//APPEND PARTIAL VIEW FOR ADD OR EDIT Mission  
$(document).on('click', '#AddOrUpdateMissionBtn', function () {
    $.ajax({
        url: '/Admin/AddOrUpdateMission',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrEditMissionForm').html(result);
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
$(document).ready(function () {
    //start end and registration dates validations
    var CurrDate = new Date();
    console.log(ChangeDateFormat(CurrDate))
    $(".MissionStartDateValidate").prop('min', ChangeDateFormat(CurrDate))
})


// Add event listener to StartDate input
$(document).on('change', '#StartDate', function () {
    let CurrDate = new Date();
    console.log(ChangeDateFormat(CurrDate))
    $(".MissionStartDateValidate").prop('min', ChangeDateFormat(CurrDate))
    // Get selected value of StartDate
    var startDate = $(this).val();
    $("#MissionDeadline").attr('disabled', false);
    $("#MissionEndDate").attr('disabled', false);
    // Set min attribute of EndDate to startDate + 1 day
    $("#MissionEndDate").attr('min', addDays(startDate, 1));
    $("#MissionDeadline").attr('max', addDays(startDate, -1));
});


// Function to add days to a date
function addDays(dateString, days) {
    var date = new Date(dateString);
    date.setDate(date.getDate() + days);
    return date.toISOString().slice(0, 10);
}
//disable based on time or goal mission
$(document).on('change', '#MissionTypeSelection', function () {
    var MissionType = $(this).val();
    if (MissionType === "Time") {
        // Show/hide input elements
        $("#TimeBased").show();
        $("#GoalBased").hide();

        // Disable/enable input elements
        $("#MissionDeadline").attr('disabled', false);
        $("#TotalSeats").attr('disabled', false);
        $("#GoalValue").attr('disabled', true);
        $("#GoalObjectiveText").attr('disabled', true);
    }
    else if (MissionType === "Goal") {
        // Show/hide input elements
        $("#TimeBased").hide();
        $("#GoalBased").show();

        // Disable/enable input elements
        $("#MissionDeadline").attr('disabled', true);
        $("#TotalSeats").attr('disabled', true);
        $("#GoalValue").attr('disabled', false);
        $("#GoalObjectiveText").attr('disabled', false);
    }
});
//validate goal and time based values on the selection of type of mission
function validateGoalTime() {
    if ($('#MissionTypeSelection').val() === "Goal") {
        let isValidGoalObjective = validateGoalObjective();
        let isValidGoalValue = validateGoalValue();
        if (isValidGoalObjective && isValidGoalValue) {
            return true;
        }
        return false;
    } else if ($('#MissionTypeSelection').val() === "Time") {
        let isValidDeadline = validateDeadline();
        let isValidSeats = validateSeats();
        if (isValidDeadline && isValidSeats) {
            return true;
        }
        return false;
    }
}

function validateDeadline() {
    var deadline = $('#MissionDeadline').val();
    if (deadline == "" || deadline == null) {
        $('#DeadlineValidateText').text("Registration deadline is required!!");
        return false;
    }
    $('#DeadlineValidateText').text("");
    return true;
}

function validateSeats() {
    var totalSeats = $('#TotalSeats').val();
    if (totalSeats == "" || totalSeats == null || totalSeats <= 0) {
        $('#SeatsValidateText').text("Total seats is required!!");
        return false;
    }
    $('#SeatsValidateText').text("");
    return true;
}

function validateGoalObjective() {
    var goalObjective = $('#GoalObjectiveText').val();
    if (goalObjective == "" || goalObjective == null) {
        $('#GoalTextValidateText').text("Goal Objective is required!!");
        return false;
    }
    $('#GoalTextValidateText').text("");
    return true;
}

function validateGoalValue() {
    var goalValue = $('#GoalValue').val();
    if (goalValue == "" || goalValue == null || goalValue <= 0) {
        $('#GoalValueValidateText').text("Goal Value is required!!");
        return false;
    }
    $('#validateGoalValue').text("");
    return true;
}

//validate organisation details in admin mission
tinymce.init({
    selector: '#OrganizationDetail',
    init_instance_callback: function (editor) {
        // Code to be executed when the editor is initialized
        $(document).on('blur', '#OrganizationDetail', validateOrganisationDetail);
    }
});
/*$(document).on('blur','#OrganizationDetail', validateOrganisationDetail());*/
function validateOrganisationDetail() {
    var description = tinymce.activeEditor.getContent();
    var descText = $('<div>').html(description).text().trim();
    if (descText == "") {
        $("#HelpBlockOrgDetail").text("Organisation Details  is a required field!!");
        return false;
    }
    else if (descText.length > 40000) {
        $("#HelpBlockOrgDetail").text("Maximum 40000 characters are allowed!!!");
        return false;
    }
    else {
        $("#HelpBlockOrgDetail").text("");
        return true;
    }
}
tinymce.init({
    selector: '#Description',
    init_instance_callback: function (editor) {
        // Code to be executed when the editor is initialized
        $(document).on('blur', '#Description', validateMissionDes);
    }
});
//validate organisation details in admin mission
/*$('#Description').on('blur', validateMissionDes());*/
function validateMissionDes() {
    var description = tinymce.activeEditor.getContent();
    var descText = $('<div>').html(description).text().trim();
    if (descText == "") {
        $("#HelpBlockMissionDes").text("Mission Description is a required field!!");
        return false;
    }
    else if (descText.length > 40000) {
        $("#HelpBlockMissionDes").text("Maximum 40000 characters are allowed!!!");
        return false;
    }
    else {
        $("#HelpBlockMissionDes").text("");
        return true;
    }
}

// drag and drop images in admin mission page
let DefaultImage = null;
var allfiles = [];
var fileInput = document.getElementById('mission-img-input');
var fileList;
function handleFiles(e) {
    console.log(allfiles)

    // Add dropped images or selected images to the list
    var files = e.target.files || e.originalEvent.dataTransfer.files;

    // Add selected images to the list
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var reader = new FileReader();

        var imageType = /image\/(png|jpeg|jpg)/;
        if (!file.type.match(imageType)) {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Only png, jpg and jpeg image types are allowed.",
                showConfirmButton: false,
                timer: 4000
            });
            return false;
        }

        // Validate image size
        if (file.size > 4 * 1024 * 1024) {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Images should be less than 4MB!",
                showConfirmButton: false,
                timer: 4000
            });
            return false;
        }
        // Check if maximum image limit is reached
        if (allfiles.length >= 20) {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Maximum 20 images are allowed!",
                showConfirmButton: false,
                timer: 4000
            });
            return false;
        }

        allfiles.push(files[i]);



        // Create image preview and close icon
        reader.onload = (function (file, index) {
            return function (e) {
                const image = $('<img>').attr('src', e.target.result);
                const closeIcon = $('<span>').addClass('close-icon').text('x');

                // Add image and close icon to the list
                const item = $('<div>').addClass('image').append(image).append(closeIcon).data('file', file);
                /* item.data('file-index', index); // store index in data attribute*/
                $('#mission-img-output').append(item);

                item.on('click', function () {
                    if ($(this).hasClass('default-img')) {
                        // remove default image class from clicked image
                        $(this).removeClass('default-img');
                        // remove it from DefaultImage variable
                        DefaultImage = null;
                    } else {
                        // remove default image class from all other images
                        $(".image.default-img").removeClass('default-img');
                        // set default image class to clicked image
                        $(this).addClass('default-img');
                        // set it as DefaultImage variable
                        DefaultImage = file;
                    }
                });

                // Handle close icon click event
                closeIcon.on('click', function () {
                    const removedFile = $(this).parent().data('file');
                    item.remove();
                    /*   $(this).parent().remove();*/
                    console.log(allfiles.indexOf(removedFile))
                    allfiles.splice(allfiles.indexOf(removedFile), 1);
                    console.log(allfiles);

                    if (removedFile === DefaultImage) {
                        DefaultImage = null;
                    }

                    if (allfiles.length < 20) {
                        $('#mission-img-input').prop('disabled', false);
                    }
                });
            };
            console.log(allfiles)
            console.log(DefaultImage)
        })(file);

        // Read image file as data URL
        reader.readAsDataURL(file);
    }
    // Disable further file selection if maximum limit is reached
    if (allfiles.length >= 20) {
        fileInput.disabled = true;
    }
    // Create a new DataTransfer object
    var dataTransfer = new DataTransfer();
    // Create a new FileList object from the DataTransfer object
    fileList = dataTransfer.files;
}

//var allfiles = new DataTransfer().files;
var dropzone = $('#mission-img-drop-area');
var imageList = $('#mission-img-output');

// Handle file drop event
$(document).on('drop', '#mission-img-drop-area', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var dropzone = $('#mission-img-drop-area');
    // Remove dropzone highlight
    dropzone.removeClass('dragover');
    $('.note-dropzone').remove();
    //$('.note-dropzone-message').remove();
    handleFiles(e);
});

$(document).on('click', '#mission-img-drop-area', function () {
    //e.preventDefault();
    $('#mission-img-input').click();
})
/*$(document).on('change', '#mission-img-input', function () {*/
// Handle file dragover event
$(document).on('dragover', '#mission-img-drop-area', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Highlight dropzone
    dropzone.addClass('dragover');
});

// Handle file dragleave event
$(document).on('dragleave', '#mission-img-drop-area', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Remove dropzone highlight
    dropzone.removeClass('dragover');
});


// Handle file input change event
$(document).on('change', '#mission-img-input', function (e) {
    handleFiles(e);
    if (allfiles.length >= 20) {
        fileInput.disabled = true;
    }
});


// drag and drop documents in  admin mission page
var allDocfiles = [];
var DocfileInput = document.getElementById('mission-doc-input');
var DocfileList;
function handleDocFiles(e) {
    console.log(allDocfiles)

    // Add dropped images or selected images to the list
    var docfiles = e.target.files || e.originalEvent.dataTransfer.files;

    // Add selected images to the list
    for (var i = 0; i < docfiles.length; i++) {
        var docfile = docfiles[i];
        var reader = new FileReader();

        var doctype = /application\/(pdf)/;
        if (!docfile.type.match(doctype)) {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Only pdf files are allowed!!",
                showConfirmButton: false,
                timer: 4000
            });
            return false;
        }

        //Validate doc size
        if (docfile.size > 4 * 1024 * 1024) {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "pdfs should not be greater than 4MB!!",
                showConfirmButton: false,
                timer: 4000
            });
            return false;
        }

        allDocfiles.push(docfiles[i]);
        console.log(" file name" + docfiles[i].name)
        var docFileName = docfiles[i].name;
        console.log(docFileName)

        // Create image preview and close icon
        // Create image preview and close icon
        reader.onload = (function (file, name) {
            return function (e) {

                var doc = $('<a>').addClass('rounded-pill').attr('href', URL.createObjectURL(file)).attr('target', '_blank').text(file.name);

                /* doc.append(docfiles.name);*/
                var closeIcon = $('<span>').addClass('close-icon').text('x');

                // Add document and close icon to the list
                var item = $('<div>').addClass('document').append(doc).append(closeIcon);

                $('#mission-doc-output').append(item);

                // Handle close icon click event
                closeIcon.on('click', function () {
                    item.remove();
                    allDocfiles.splice(allDocfiles.indexOf(file), 1);
                    console.log(allDocfiles);
                    if (allDocfiles.length < 5) {
                        $('#mission-doc-input').disabled = false;
                    }
                });

            };
        })(docfile, docFileName);

        // Read image file as data URL
        reader.readAsDataURL(docfile);
    }
    if (allDocfiles.length > 5) {
        swal.fire({
            position: 'top-end',
            icon: "error",
            title: "Maximum 5 files are allowed!!",
            showConfirmButton: false,
            timer: 4000
        });
        // Remove the last added file from the list
        allDocfiles.splice(-1, 1);
        // Remove the last added image preview from the list
        $('#mission-doc-output').children().last().remove();
        //// Disable further file selection
        DocfileInput.disabled = true;
    }
    // Create a new DataTransfer object
    var dataTransfer = new DataTransfer();
    // Create a new FileList object from the DataTransfer object
    DocfileList = dataTransfer.docfiles;
}

//var allfiles = new DataTransfer().files;
var dropzone = $('#mission-doc-drop-area');
var imageList = $('#mission-doc-output');

// Handle file drop event
$(document).on('drop', '#mission-doc-drop-area', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var dropzone = $('#mission-doc-drop-area');
    // Remove dropzone highlight
    dropzone.removeClass('dragover');
    $('.note-dropzone').remove();
    //$('.note-dropzone-message').remove();
    handleDocFiles(e);
});

$(document).on('click', '#mission-doc-drop-area', function () {
    //e.preventDefault();
    $('#mission-doc-input').click();
})
/*$(document).on('change', '#mission-img-input', function () {*/
// Handle file dragover event
$(document).on('dragover', '#mission-doc-drop-area', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Highlight dropzone
    dropzone.addClass('dragover');
});

// Handle file dragleave event
$(document).on('dragleave', '#mission-doc-drop-area', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Remove dropzone highlight
    dropzone.removeClass('dragover');
});


// Handle file input change event
$(document).on('change', '#mission-doc-input', function (e) {
    handleDocFiles(e);
    if (allDocfiles.length >= 5) {
        DocfileInput.disabled = true;
    }
});
//submitting mission form
$(document).on('submit', '#MissionForm', function (e) {
    e.preventDefault();
    e.stopPropagation();
    $('#MissionDeadline').prop('disabled', false);
    $('#TotalSeats').prop('disabled', false);
    $('#GoalValue').prop('disabled', false);
    $('#GoalObjectiveText').prop('disabled', false);
    $("#MissionEndDate").prop('disabled', false);
    var isValidDes = validateMissionDes();
    var isValidDetails = validateOrganisationDetail();
    if (validateGoalTime() == true && isValidDetails && isValidDes && $(this).valid())  {
        var myform = document.getElementById("MissionForm");
        var MissionFormData = new FormData(myform);

        for (let i = 0; i < allfiles.length; i++) {
            MissionFormData.append('ImageList', allfiles[i]);
        }
        console.log(allfiles)
        for (let i = 0; i < allDocfiles.length; i++) {
            MissionFormData.append('DocumentList', allDocfiles[i]);
        }

        if (DefaultImage != null) {
            MissionFormData.append('DefaultMissionImg', DefaultImage);
        }
        console.log(DefaultImage)
        let urls = null;
        let u = $('#MissionYoutubeUrl').val();
        if (u != null) {
            urls = u.split('\n');
            for (var i = 0; i < urls.length; i++) {
                MissionFormData.append("YoutubeUrl", urls[i]);
            }
        }
        else {
            MissionFormData.append("YoutubeUrl", null);
        }
        let SelectedSkills = $('#MissionSkillList input[type="checkbox"][name="MissionSkill"]:checked ').map(function () { return $(this).val(); }).get().join();
        MissionFormData.append("UpdatedMissionSKills", SelectedSkills);
        console.log(MissionFormData)
        $.ajax({
            url: '/Admin/AddOrUpdateMissionPost',
            type: 'POST',
            processData: false,
            contentType: false,
            data: MissionFormData,
            success: function (result) {
                swal.fire({
                    position: 'top-end',
                    icon: result.icon,
                    title: result.message,
                    showConfirmButton: false,
                    timer: 4000
                });

            },
            error: function (error) {
                console.log(error)
            }
        });
    }
})

//DATA FETCHING FOR MISSION EDIT
$(document).on('click', '#EditBtnMissionDataFetch', function () {
    var MissionId = $(this).data('mission-id');
    $.ajax({
        url: '/Admin/GetMissionData',
        type: 'GET',
        data: { MissionId: MissionId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrEditMissionForm').html(result);
            const isEditMode = true; // or false
            $('#AddHeader .FormHeading span').text(isEditMode ? 'Edit' : 'Add');

            //for images 
            $('#mission-img-output').empty();
            var imageArr = $('.mission-img-edit').map(function () {
                return $(this).val();
            }).get();
            var defaultimg = $('.mission-img-edit-default').val();

            $.each(imageArr, async function (index, item) {
                var file = imageArr[index];
                var image = $('<img>').attr('src', '/images/Upload/Mission/' + imageArr[index]);
                var closebtn = $('<span>').text('x');
                var item = $('<div>').addClass('image').append(image).append(closebtn);
                $('#mission-img-output').append(item);

                const response = await fetch('/images/Upload/Mission/' + file);
                const blob = await response.blob();
                const files = new File([blob], file, { type: blob.type });
                if (defaultimg === imageArr[index]) {
                    item.addClass('default-img');
                    DefaultImage = files;
                    console.log("defaut " + DefaultImage);
                }

                item.on('click', function () {
                    if ($(this).hasClass('default-img')) {
                        // remove default image class from clicked image
                        $(this).removeClass('default-img');
                        // remove it from DefaultImage variable
                        DefaultImage = null;
                    } else {
                        // remove default image class from all other images
                        $(".image.default-img").removeClass('default-img');
                        // set default image class to clicked image
                        $(this).addClass('default-img');
                        // set it as DefaultImage variable
                        DefaultImage = files;
                    }
                   
                });
                // Handle close icon click event
                closebtn.on('click', function () {
                    const removedFile = $(this).parent().data('file');
                    const index = allfiles.findIndex(f => f.name === file)
                    allfiles.splice(index, 1);
                    console.log(allfiles);
                    /*console.log(allfiles.indexOf(removedFile))*/
                    item.remove();
                    if (removedFile === DefaultImage) {
                        DefaultImage = null;
                    }
                });

                console.log("defaut " + DefaultImage);
               
                allfiles.push(files);
            });
            //for youtubeurls 
            var url = '';
            $('#MissionYoutubeUrl').empty();
            var UrlRecords = $('.mission-url-edit').map(function () {
                return $(this).val();
            }).get();
            $.each(UrlRecords, async function (index, item) {
                url += item + '\n';
            });
            $('#MissionYoutubeUrl').append(url);
            //for documents 
            $('#mission-doc-output').empty();
            var docArr = $('.mission-doc-edit').map(function () {
                return $(this).val();
            }).get();
            $.each(docArr, async function (index, item) {
                var docfile = docArr[index];
                var link = $('<a>').addClass('rounded-pill').attr('href', '/documents/' + docfile).attr('target', '_blank').text(docfile);
                var closebtn = $('<span>').addClass('close-icon').text('x');
                var item = $('<div>').addClass('document').append(link).append(closebtn);
                $('#mission-doc-output').append(item);

                const response = await fetch('/documents/' + docfile);
                const blob = await response.blob();
                const docfiles = new File([blob], docfile, { type: blob.type });
                // Handle close icon click event
                closebtn.on('click', function () {
                    allDocfiles.splice(allDocfiles.indexOf(docfiles), 1);
                    item.remove();
                });
                allDocfiles.push(docfiles);
            });
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});

//to get mission id for delete
$(document).on('click', '#DeleteMissionBtn', function () {
    var MissionId = $(this).data('mission-id');
    $("#HiddenMissionId").val(MissionId);
});

//banner-img-change
$(document).on('click', '.edit-icon', function () {
    // Open file input dialog
    $('#BannerImg').click();
});

// Add change event listener to profile image file input
$(document).on('change', '#BannerImg', function () {
    if (this.files.length > 0) {
        // Read image file and display preview
        var file = this.files[0];
        var reader = new FileReader();
        var imageType = /image\/(png|jpeg|jpg)/;
        if (!file.type.match(imageType)) {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Only png, jpg and jpeg image types are allowed.",
                showConfirmButton: false,
                timer: 4000
            });
            return false;
        }
        reader.onload = function (e) {
            $('.image-edit').find('img').attr('src', e.target.result);
        }
        reader.readAsDataURL(file);
        $('.image-edit').show();
    } else {
        // No file selected, hide image container
        $('.image-edit').hide();
    }
});

//APPEND PARTIAL VIEW FOR ADD OR EDIT banner PAGE
$(document).on('click', '#AddOrUpdateBannerBtn', function () {
    $.ajax({
        url: '/Admin/AddOrUpdateBanner',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdateBanner').html(result);
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//DATA FETCHING FOR banner PAGE EDIT
$(document).on('click', '#EditBtnBanner', function () {
    var BannerId = $(this).data('banner-id');
    $.ajax({
        url: '/Admin/GetBannerData',
        type: 'GET',
        data: { BannerId: BannerId },
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddOrUpdateBanner').html(result);

            const isEditMode = true; // or false
            $('#BannerHeader .FormHeading span').text(isEditMode ? 'Edit' : 'Add');
        },
        error: function () {
            swal.fire({
                position: 'top-end',
                icon: "error",
                title: "Something went wrong!!!",
                showConfirmButton: false,
                timer: 4000
            });
        }
    });
});
//to get id for delete
$(document).on('click', '#DeleteBannerBtn', function () {
    var BannerId = $(this).data('banner-id');
    $("#HiddenBannerId").val(BannerId);
});

//to get id of comment to decline or approve it 
$(document).on('click', '.ApproveOrDeclineComment', function () {
    var CommentId = $(this).data('comment-id');
    $("#HiddenCommentId").val(CommentId);
});
//to fetch data for approval of comment
$(document).on('click', '#CommentApprovalBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = false; // or false
    $('#CommentModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this Comment??' : 'Are you sure you want to approve this Comment??');
});
//to fetch data for decline of comment
$(document).on('click', '#CommentDeclineBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = true; // or false
    $('#CommentModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this Comment??' : 'Are you sure you want to approve this Comment??');
});

//to get id of timesheet to decline or approve it 
$(document).on('click', '.ApproveOrDeclineTimesheet', function () {
    var TimesheetId = $(this).data('timesheet-id');
    $("#HiddenTimesheetId").val(TimesheetId);
});
//to fetch data for approval of timesheet
$(document).on('click', '#TimesheetApprovalBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = false; // or false
    $('#TimesheetModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this Timesheet??' : 'Are you sure you want to approve this Timesheet??');
});
//to fetch data for decline of timesheet
$(document).on('click', '#TimesheetDeclineBtn', function () {
    var Status = $(this).data('status');
    $("#HiddenStatus").val(Status);
    const isDeclineBtn = true; // or false
    $('#TimesheetModalBody div').text(isDeclineBtn ? 'Are you sure you want to decline this Timesheet??' : 'Are you sure you want to approve this Timesheet??');
});