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
//SIDE NAV BAR ENDS

//APPEND PARTIAL VIEW FOR ADD USER  
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
$(document).on('click', '.EditButtonDataFetch', function () {
    var UserId = $(this).data('user-id');  
    $.ajax({
        url: '/Admin/GetUserData',
        type: 'GET',
        data: { UserId: UserId },
        success: function (result) {
            var heading = '<h4>Edit</h4>';
            $('.FormHeading').empty().append($(heading).text("Edit"));
            $('.table-responsive').empty();
            $('#AddNewUserForm').html(result);
        },
        error: function () {
            alert('Error ');
        }
    });
});
//cities based on countries in user profile
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
//cms pages add
$(document).on('click', '#AddOrUpdateCms', function () {
    $.ajax({
        url: '/Admin/AddOrUpdatePrivacyPages',
        type: 'GET',
        success: function (result) {
            $('.table-responsive').empty();
            $('#AddNewUserForm').html(result);
            $('#AddOrUpdatePrivacyPages').html(result);
        },
        error: function () {
            alert('Error ');
        }
    });
});