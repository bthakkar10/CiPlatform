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
//to get id for delete
$(document).on('click', '#DeleteSkillBtn', function () {
    var SkillId = $(this).data('skill-id');
    $("#HiddenSkillId").val(SkillId);
});