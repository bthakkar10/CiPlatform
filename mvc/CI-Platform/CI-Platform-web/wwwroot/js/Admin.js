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
            alert('Error ');
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
            alert('Error ');
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
            alert('Error ');
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
            alert('Error ');
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
            alert('Error ');
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
            alert('Error ');
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
            alert('Error ');
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
            alert('Error ');
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
$('#ViewStoryBtn').click(function () {
    var UserId = $(this).data('user-id');
    var MissionId = $(this).data('mission-id');
    $.ajax({
        method: 'GET',
        url: '/Story/StoryDetails',
        data: { UserId: UserId, MissionId: MissionId },
        success: function (result) {
            console.log(result)
            var url = '/Story/StoryDetails?MissionId=' + MissionId + '&UserId=' + UserId;

            var win = window.open(url, '_blank');
            win.focus();

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

//to get id of mission application to decline or approve it 
//$(document).on('click', '.ApproveOrDeclineApplication', function () {
//    var MissionApplicationId = $(this).data('application-id');
//    $("#HiddenApplicationId").val(MissionApplicationId);
//});
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
            $('#MissionForm').html(result);
        },
        error: function () {
            alert('Error ');
        }
    });
});

// drag and drop images in admin mission page
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
            alert("Only png, jpg and jpeg image types are allowed.");
            return false;
        }

        // Validate image size
        if (file.size > 4 * 1024 * 1024) {
            alert("An image should not be greater than 4MB.");
            return false;
        }

        allfiles.push(files[i]);


        // Create image preview and close icon
        // Create image preview and close icon
        reader.onload = (function (file) {
            return function (e) {
                var image = $('<img>').attr('src', e.target.result);
                var closeIcon = $('<span>').addClass('close-icon').text('x');

                // Add image and close icon to the list
                var item = $('<div>').addClass('image').append(image).append(closeIcon);
                $('#mission-img-output').append(item);

                // Handle close icon click event
                closeIcon.on('click', function () {
                    item.remove();
                    allfiles.splice(allfiles.indexOf(file), 1);
                    console.log(allfiles);
                    if (allfiles.length < 20) {
                        $('mission-img-input').disabled = false;
                    }
                });
            };
        })(file);

        // Read image file as data URL
        reader.readAsDataURL(file);
    }
    if (allfiles.length > 20) {
        alert("Maximum 20 images can be added.");
        // Remove the last added file from the list
        allfiles.splice(-1, 1);
        // Remove the last added image preview from the list
        $('#mission-img-output').children().last().remove();
        //// Disable further file selection
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
function handleFiles(e) {
    console.log(allDocfiles)

    // Add dropped images or selected images to the list
    var docfiles = e.target.files || e.originalEvent.dataTransfer.files;

    // Add selected images to the list
    for (var i = 0; i < docfiles.length; i++) {
        var docfile = docfiles[i];
        var reader = new FileReader();

        var doctype = /application\/(pdf)/;
        if (!docfile.type.match(doctype)) {
            alert("Only pdf files are allowed.");
            return false;
        }

        // Validate image size
        //if (file.size > 4 * 1024 * 1024) {
        //    alert("An image should not be greater than 4MB.");
        //    return false;
        //}

        allDocfiles.push(docfiles[i]);


        // Create image preview and close icon
        // Create image preview and close icon
        reader.onload = (function (file) {
            return function (e) {
                var doc = $('<div>').addClass('rounded-pill').attr('name', e.target.result);
                var closeIcon = $('<span>').addClass('close-icon').text('x');

                // Add image and close icon to the list
                var item = $('<div>').addClass('image').append(doc).append(closeIcon);
                $('#mission-doc-output').append(item);

                // Handle close icon click event
                closeIcon.on('click', function () {
                    item.remove();
                    allDocfiles.splice(allDocfiles.indexOf(file), 1);
                    console.log(allDocfiles);
                    if (allDocfiles.length < 20) {
                        $('#mission-doc-input').disabled = false;
                    }
                });
            };
        })(docfile);

        // Read image file as data URL
        reader.readAsDataURL(docfile);
    }
    if (allDocfiles.length > 20) {
        alert("Maximum 20 images can be added.");
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
    handleFiles(e);
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
    handleFiles(e);
    if (allDocfiles.length >= 20) {
        DocfileInput.disabled = true;
    }
});
