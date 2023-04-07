//global
var currentUrl = window.location.href;
if (currentUrl.includes("HomePage")) {
    FilterSortPaginationSearch(1);
}
else if (currentUrl.includes("StoryListing")) {
    StoryFilter(1);

}


var SelectedsortCase = null;
var SelectedCountry = null;
var userId = ($("#user-id").text());
let allDropdowns = $('.dropdown ul');

//global search text selection
$('#searchText').on('keyup', function () {
    if (currentUrl.includes("HomePage")) {
        FilterSortPaginationSearch();
        /*console.log("first")*/
    }
    else if (currentUrl.includes("StoryListing")) {
        StoryFilter();
        /* console.log("second")*/
    }
});

//global dropdown selection for filters
allDropdowns.on('change', function () {
    if (currentUrl.includes("HomePage")) {
        FilterSortPaginationSearch();
    }
    else if (currentUrl.includes("StoryListing")) {
        StoryFilter();

    }
})

//for mission filters sorting stored procedure
function FilterSortPaginationSearch(pageNo) {
    var CountryId = SelectedCountry;
    var CityId = $('#CityList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var ThemeId = $('#ThemeList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var SkillId = $('#SkillList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    /* var SearchText = searchText;*/
    var SearchText = $("#searchText").val();
    var sortCase = SelectedsortCase;
    var pagesize = 9;

    $.ajax({
        type: 'POST',
        url: '/Home/HomePage',
        data: { CountryId: CountryId, CityId: CityId, ThemeId: ThemeId, SkillId: SkillId, SearchText: SearchText, sortCase: sortCase, userId: userId, pageNo: pageNo, pagesize: pagesize },
        success: function (data) {
            var view = $(".partialViews");
            view.empty();
            view.append(data);

            totalMission();

            if (document.getElementById('missionCount') != null) {
                var totalRecords = document.getElementById('missionCount').innerText;
            }
            let totalPages = Math.ceil(totalRecords / pagesize);

            if (totalPages <= 1) {
                $('#pagination-container').parent().parent().hide();
            }
            let paginationHTML = `
            <li class="page-item">
            <a class="pagination-link first-page" aria-label="Previous">
            <span aria-hidden="true"><img src="/images/previous.png" /></span>
            </a>
            </li>
            <li class="page-item">
            <a class="pagination-link previous-page" aria-label="Previous">
            <span aria-hidden="true"><img src="/images/left.png" /></span>
            </a>
            </li>`;

            for (let i = 1; i <= totalPages; i++) {
                let activeClass = '';
                if (i === (pageNo === undefined ? 1 : pageNo)) {
                    activeClass = ' active';
                }
                paginationHTML += `
                <li class="page-item ${activeClass}">
                <a class="pagination-link" data-page="${i}">${i}</a>
                </li>`;
            }

            paginationHTML += `
            <li class="page-item">
            <a class="pagination-link next-page" aria-label="Next">
            <span aria-hidden="true"><img src="/images/right-arrow1.png" /></span>
            </a>
            </li>
            <li class="page-item">
            <a class="pagination-link last-page" aria-label="Next">
            <span aria-hidden="true"><img src="/images/next.png" /></span>
            </a>
            </li>`;

            $('#pagination-container').empty()
            $('#pagination-container').append(paginationHTML)
            $('#pagination-container').parent().parent().show();



            // pagination
            let currentPage;

            $(document).on('click', '.pagination li', (function () {
                $('.pagination li').each(function () {
                    if ($(this).hasClass('active')) {

                        currentPage = $(this).find('a').data('page');
                        $(this).removeClass('active');
                    }
                })
                pageNo = currentPage;
                if ($(this).find('a').hasClass('first-page')) {
                    pageNo = 1;
                    currentPage = pageNo;
                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == 1) {
                            $(this).parent().addClass('active')
                        }
                    })
                }
                else if ($(this).find('a').hasClass('last-page')) {
                    pageNo = totalPages;
                    currentPage = pageNo;
                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == totalPages) {
                            $(this).parent().addClass('active')
                        }
                    })
                }
                else if ($(this).find('a').hasClass('previous-page')) {
                    if (currentPage - 1 > 0) {
                        pageNo = currentPage - 1;

                        currentPage = pageNo;
                    }
                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == pageNo) {
                            $(this).parent().addClass('active')
                        }
                    })

                } else if ($(this).find('a').hasClass('next-page')) {
                    if (currentPage < totalPages) {
                        pageNo = currentPage + 1;
                    }

                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == pageNo) {
                            $(this).parent().addClass('active')
                        }
                    })
                    currentPage = pageNo;

                } else {
                    $(this).addClass('active')

                    pageNo = $(this).find('a').data('page');
                    currentPage = pageNo;

                }
                FilterSortPaginationSearch(pageNo);
            }));
        },
        error: function (error) {
            console.log(error)
        }
    });
}

//to count no of total missions for explore -- missions count in mission page
function totalMission() {
    var count = document.getElementById('missionCount').innerText;
    $('#exploreText').text("Explore " + count + " missions");
    if (count == 0) {
        $('.NoMissionFound').show();
    }
    else {
        $('.NoMissionFound').hide();
    }
}

//stories filters stored procedure
function StoryFilter(pageNo) {
    var CountryId = SelectedCountry;
    var CityId = $('#CityList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var ThemeId = $('.ThemeListMissionFilter input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var SkillId = $('#SkillList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var SearchText = $("#searchText").val();
    var pagesize = 3;
    $.ajax({
        type: 'POST',
        url: '/Story/StoryListing',
        data: { CountryId: CountryId, CityId: CityId, ThemeId: ThemeId, SkillId: SkillId, SearchText: SearchText, pageNo: pageNo, pagesize: pagesize },
        success: function (data) {
            var view = $(".StorypartialViews");
            view.empty();
            view.append(data);

            if (document.getElementById('storyCount') != null) {
                var totalRecords = document.getElementById('storyCount').innerText;
            }
            let totalPages = Math.ceil(totalRecords / pagesize);
            console.log(totalPages);
            if (totalPages <= 1) {
                $('#pagination-container').parent().parent().hide();
            }
            let paginationHTML = `
            <li class="page-item">
            <a class="pagination-link first-page" aria-label="Previous">
            <span aria-hidden="true"><img src="/images/previous.png" /></span>
            </a>
            </li>
            <li class="page-item">
            <a class="pagination-link previous-page" aria-label="Previous">
            <span aria-hidden="true"><img src="/images/left.png" /></span>
            </a>
            </li>`;

            for (let i = 1; i <= totalPages; i++) {
                let activeClass = '';
                if (i === (pageNo === undefined ? 1 : pageNo)) {
                    activeClass = ' active';
                }
                paginationHTML += `
                <li class="page-item ${activeClass}">
                <a class="pagination-link" data-page="${i}">${i}</a>
                </li>`;
            }

            paginationHTML += `
            <li class="page-item">
            <a class="pagination-link next-page" aria-label="Next">
            <span aria-hidden="true"><img src="/images/right-arrow1.png" /></span>
            </a>
            </li>
            <li class="page-item">
            <a class="pagination-link last-page" aria-label="Next">
            <span aria-hidden="true"><img src="/images/next.png" /></span>
            </a>
            </li>`;

            $('#pagination-container').empty()
            $('#pagination-container').append(paginationHTML)
            $('#pagination-container').parent().parent().show();



            // pagination
            let currentPage;

            $(document).on('click', '.pagination li', (function () {
                $('.pagination li').each(function () {
                    if ($(this).hasClass('active')) {

                        currentPage = $(this).find('a').data('page');
                        $(this).removeClass('active');
                    }
                })
                pageNo = currentPage;
                if ($(this).find('a').hasClass('first-page')) {
                    pageNo = 1;
                    currentPage = pageNo;
                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == 1) {
                            $(this).parent().addClass('active')
                        }
                    })
                }
                else if ($(this).find('a').hasClass('last-page')) {
                    pageNo = totalPages;
                    currentPage = pageNo;
                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == totalPages) {
                            $(this).parent().addClass('active')
                        }
                    })
                }
                else if ($(this).find('a').hasClass('previous-page')) {
                    if (currentPage - 1 > 0) {
                        pageNo = currentPage - 1;

                        currentPage = pageNo;
                    }
                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == pageNo) {
                            $(this).parent().addClass('active')
                        }
                    })

                } else if ($(this).find('a').hasClass('next-page')) {
                    if (currentPage < totalPages) {
                        pageNo = currentPage + 1;
                    }

                    $('.pagination li').find('a').each(function () {
                        if ($(this).data('page') == pageNo) {
                            $(this).parent().addClass('active')
                        }
                    })
                    currentPage = pageNo;

                } else {
                    $(this).addClass('active')

                    pageNo = $(this).find('a').data('page');
                    currentPage = pageNo;

                }
                StoryFilter(pageNo);
            }));
        },
        error: function (error) {
            console.log(error)
        }
    });
}

//for selecting sort dropdown in mission page
$("#sortByDropdown li").click(function () {
    var sortCase = $(this).val();
    SelectedsortCase = sortCase;
    console.log(sortCase);

    FilterSortPaginationSearch();
});

// for country selection
$(".CountryListMissionFilter li").click(function () {
    $(this).addClass('selected');

    var CountryId = $(this).val();
    console.log(CountryId)
    SelectedCountry = CountryId;

    GetCitiesByCountry(CountryId);

    if (currentUrl.includes("HomePage")) {
        FilterSortPaginationSearch();
        console.log("first")
    }
    else if (currentUrl.includes("StoryListing")) {
        StoryFilter();
        console.log("second")
    }
});

//get cities based on countries for filters 
function GetCitiesByCountry(CountryId) {
    $.ajax({
        type: "GET",
        url: "/Home/GetCitiesByCountry",
        data: { CountryId: CountryId },
        success: function (data) {
            var dropdown = $(".CityListMissionFilter");
            dropdown.empty();
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li> <div class="dropdown-item mb-3 ms-3 form-check"> <input type="checkbox" class="form-check-input" id="exampleCheck1"  value=` + item.cityId + `><label class="form-check-label" for="exampleCheck1" value=` + item.cityId + `>` + item.cityName + `</label></div></li>`
            })
            dropdown.html(items);
        }
    });

    $.ajax({
        type: "GET",
        url: "/Home/GetCitiesByCountry",
        data: { CountryId: CountryId },
        success: function (data) {
            var dropdown = $(".CityListMissionFilter");
            dropdown.empty();
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li> <div class="dropdown-item mb-3 ms-3 form-check"> <input type="checkbox" class="form-check-input" id="exampleCheck1" value=` + item.cityId + `><label class="form-check-label" for="exampleCheck1" value=` + item.cityId + `>` + item.cityName + `</label></div></li>`
            })
            dropdown.html(items);
        }
    });
}

//for accordian function
function GetCitiesByCountry(CountryId) {
    $.ajax({
        type: "GET",
        url: "/Home/GetCitiesByCountry",
        data: { CountryId: CountryId },
        success: function (data) {
            var dropdown = $("#CityList");
            dropdown.empty();
            var items = "";
            $(data).each(function (i, item) {
                //items += "<option value=" + this.value + ">" + this.text + "</option>"
                items += `<li> <div class="dropdown-item mb-3 ms-3 form-check"> <input type="checkbox" class="form-check-input" id="exampleCheck1"  value=` + item.cityId + `><label class="form-check-label" for="exampleCheck1" value=` + item.cityId + `>` + item.cityName + `</label></div></li>`
            })
            dropdown.html(items);
        }
    });
}

//to display pills according to the filter selection
let filterPills = $('.filter-pills');
allDropdowns.each(function () {
    let dropdown = $(this);
    $(this).on('change', 'input[type="checkbox"]', function () {

        // if the check box is checked then add it to pill
        if ($(this).is(':checked')) {
            let selectedOptionText = $(this).next('label').text();
            let selectedOptionValue = $(this).val();
            const closeAllButton = filterPills.children('.closeAll');

            // creating a new pill
            let pill = $('<span></span>').addClass('pill');

            // adding the text to pill
            let pillText = $('<span></span>').text(selectedOptionText);
            pill.append(pillText);

            // add the close icon (bootstrap)
            let closeIcon = $('<span></span>').addClass('close').html(' x');
            pill.append(closeIcon);


            // for closing the pill when clicking on close icon
            closeIcon.click(function () {
                const pillToRemove = $(this).closest('.pill');
                pillToRemove.remove();
                // Uncheck the corresponding checkbox
                const checkboxElement = dropdown.find(`input[type="checkbox"][value="${selectedOptionValue}"]`);
                checkboxElement.prop('checked', false);
                if (currentUrl.includes("HomePage")) {
                    FilterSortPaginationSearch();

                }
                else if (currentUrl.includes("StoryListing")) {
                    StoryFilter();

                }


                if (filterPills.children('.pill').length === 1) {
                    filterPills.children('.closeAll').remove();
                }

            });

            // Add "Close All" button
            if (closeAllButton.length === 0) {
                filterPills.append('<span class=" closeAll"><span>Close All</span></span>');
                filterPills.children('.closeAll').click(function () {
                    allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                    filterPills.empty();
                    if (currentUrl.includes("HomePage")) {
                        FilterSortPaginationSearch();

                    }
                    else if (currentUrl.includes("StoryListing")) {
                        StoryFilter();

                    }



                });

                //add the pill before the close icon
                filterPills.prepend(pill);

            }
            else {
                filterPills.children('.closeAll').before(pill);
            }

        }
        // if the checkbox is not checked then we have to check for its value if it is exists in the pills section then we have to remove it
        else {
            let selectedOptionText = $(this).next('label').text() + ' x';
            let selectedOptionValue = $(this).val();
            $('.pill').each(function () {
                const pillText = $(this).text();
                if (pillText === selectedOptionText) {
                    $(this).remove();
                }
            });
            if ($('.pill').length === 1) {
                $('.closeAll').remove();
            }
        }

        if (currentUrl.includes("HomePage")) {
            FilterSortPaginationSearch();

        }
        else if (currentUrl.includes("StoryListing")) {
            StoryFilter();

        }

    });

})

//to add or remove favourites
function favourite() {
    $('.favourite-button').on('click', function () {

        var missionId = $(this).data('mission-id');
        $.ajax({
            url: '/Home/AddToFavorites',
            type: 'POST',
            data: { missionId: missionId },
            success: function () {
                // Show a success message or update the UI
                console.log(missionId);
                var text = $('.favText');
                console.log(text);
                var allMissionId = $('.favourite-button')
                allMissionId.each(function () {
                    if ($(this).data('mission-id') === missionId) {
                        if ($(this).find('i').hasClass('bi-heart')) {
                            $(this).find('i').addClass('bi-heart-fill text-danger')
                            $(this).find('i').removeClass('bi-heart text-dark')
                            text.empty();
                            text.append("Added To favourites");
                            console.log("added");
                            /*  alert("Mission added to favourites successfully!!!");*/
                        }
                        else {
                            $(this).find('i').addClass('bi-heart text-dark')
                            $(this).find('i').removeClass('bi-heart-fill text-danger')
                            text.empty();
                            text.append("Add to favourites");
                            console.log("remove");
                            /* alert("Mission removed from favourites successfully!!!");*/
                        }
                    }
                })
            },
            error: function (error) {
                // Show an error message or handle the error
                console.log(error)

            }
        });
    });
}

//to add or update ratings
$('.rating-star i').on('click', function () {
    var rating = $(this).index() + 1;
    var missionId = $(this).data('mission-id');
    var selectedIcon = $(this).prevAll().addBack();
    var unselectedIcon = $(this).nextAll();


    $.ajax({
        method: 'POST',
        url: '/Home/Rating',
        data: { rating: rating, missionId: missionId },
        success: function () {
            selectedIcon.removeClass('bi-star').addClass('bi-star-fill text-warning');
            unselectedIcon.removeClass('bi-star-fill text-warning').addClass('bi-star');
            /*alert("ratings updated successfully!!");*/
        },
        error: function (error) {
            /* alert("Sessin Expired.");*/
            if (confirm("Please Login Again And Try Agaion")) {
                window.location.href = "/Home/Index";
            }
        }
    });
});

//comments in mission details page
$('.commentButton').click(function () {
    var comment = $('.newComment').val();
    console.log(comment);
    var missionId = $(this).data('mission-id');
    if (comment != " ") {
        $.ajax({
            method: 'POST',
            url: '/Home/PostComment',
            data: { comment: comment, missionId: missionId },
            success: function (result) {
                $('.newComment').val('');
                swal.fire({
                    position: 'top-end',
                    icon: result.icon,
                    title: result.message,
                    showConfirmButton: false,
                    timer: 4000
                })
                /*$('#CommentHelpBox').text("Comment will be published after approval!!");*/
            },
            error: function (error) {
                console.log("error");
            }
        });
    }
    else {
        //$('#CommentHelpBox').addClass('text-danger').text("Please enter any comment");
        //$('.newComment').focus();
    }
});

//recommend to co-worker invite for mission details page 
$(document).on('click', '.model-invite-btn', function () {

    var FromUserId = $(this).data('from-user-id');
    var MissionId = $(this).data('mission-id');
    var ToUserId = $(this).data('to-user-id');
    var btn = $(this);

    $.ajax({
        type: "POST",
        url: "/Home/MissionInvite",
        data: { ToUserId: ToUserId, MissionId: MissionId, FromUserId: FromUserId },
        success: function () {

            var button = $('<button>').addClass('btn btn-success disabled')
                .append($('<span>').text('Already Invited '));
            btn.replaceWith(button);

        },
        error: function (error) {
            console.log(error);
        }
    });
});

//recommend to co-worker invite for mission page through get function which will fetch user list here and go to above function again
$(document).on('click', '.add-on-img', function () {
    var MissionId = $(this).data('mission-id');
    var UserId = $(this).data('user-id');
    $.ajax({
        type: "GET",
        url: "/Home/UserList",
        data: { MissionId: MissionId },
        success: function (coworkers) {
            console.log(coworkers);
            var list = $('.grid-modal-body');
            list.empty();
            var items = " ";
            $(coworkers).each(function (index, coworker) {
                items +=
                    ` <div class="mt-2" style="display : flex; justify-content : space-between;">
                        <span class="mx-4 "> ` + coworker.firstName + ` ` + coworker.lastName + `</span>
                        <span  mailto:class="invited- `+ coworker.UserId + `"><button class="btn mx-3 btn-outline-primary model-button model-invite-btn" data-mission-id=" ` + MissionId + `" data-from-user-id=" ` + UserId + `" data-to-user-id=" ` + coworker.userId + `">Invite</button></span>

                    </div>`

            });
            list.html(items);
        },
        error: function (error) {
            console.log(error);
        }
    });
});

//apply in mission button 
$('#ApplyBtnMission').click(function () {
    var MissionId = $(this).data('mission-id');
    var btn = $(this);
    $.ajax({
        type: 'POST',
        url: '/Home/ApplyMission',
        data: { MissionId: MissionId },
        success: function (result) {

            var newButton = $('<a>').addClass('btn card-btn disabled text-danger')
                .append($('<span>').text('Approval Pending')
                    .append($('<i>').addClass('bi bi-patch-exclamation-fill')));
            btn.replaceWith(newButton);
        },
        error: function (error) {
            console.log(error);
        },
    });
});

//recent volunteers pagination
var rvPageNo = 1;
if (window.location.href.includes("MissionDetail")) {
    recentVolunteerNavigation(rvPageNo);
}

$('.volunteerBtn a').click(function () {
    let totalVolunteers = $('#totalVolunteers').val();
    let totalPages = Math.ceil(totalVolunteers / 3);
    if ($(this).hasClass('prevVolunteersBtn')) {
        if (rvPageNo > 1) {
            rvPageNo--;
            recentVolunteerNavigation(rvPageNo)
        }
    } else {
        if (rvPageNo < totalPages) {
            rvPageNo++;
            recentVolunteerNavigation(rvPageNo)
        }
    }
})

function recentVolunteerNavigation(pageNo) {
    var missionId = $('#MissionId').val();
    $.ajax({
        type: "GET",
        url: '/Home/RecentVolunteers',
        data: { missionId: missionId, pageNo: pageNo },

        success: function (data) {
            $('.volunteerd-images').empty();
            $('.volunteerd-images').append(data);
            let totalVolunteers = $('#totalVolunteers').val();
            let totalPages = Math.ceil(totalVolunteers / 3);
            var start = (((pageNo - 1) * 3) + 1);
            var end = start + 2;
            if (totalPages === pageNo) {
                end = start + (totalVolunteers % 3) - 1;
            }
            $('.volunteerBtn .pagination-vol-info').html(start + ` - ` + end + ` of ` + totalVolunteers + ` Recent Volunteers`)
        }
    })
}

//story details recommened to coworker
//recommend to co-worker invite for mission details page 
$(document).on('click', '.model-invite-btn-story', function () {

    var FromUserId = $(this).data('from-user-id');
    var StoryId = $(this).data('story-id');
    var ToUserId = $(this).data('to-user-id');
    var btn = $(this);
    var MissionId = $(this).data('mission-id');
    var SPUserId = $(this).data('sp-user-id');
    $.ajax({
        type: "POST",
        url: "/Story/StoryInvite",
        data: { ToUserId: ToUserId, StoryId: StoryId, FromUserId: FromUserId, MissionId: MissionId, SPUserId: SPUserId },
        success: function () {
            var button = $('<button>').addClass('btn btn-success disabled')
                .append($('<span>').text('Already Invited '));
            btn.replaceWith(button);

            //var url = '/Story/StoryDetails?MissionId=' + MissionId + '&UserId=' + FromUserId;
            //window.location.href = url;
        },
        error: function (error) {
            console.log(error);
        }
    });
});

//for share story page(ck-editor and drag and drop functionality)
let optionsButtons = document.querySelectorAll(".option-button");
let writingArea = document.getElementById("text-input");
let formatButtons = document.querySelectorAll(".format");
let scriptButtons = document.querySelectorAll(".script");


// Initial Setting
const initializer = () => {
    highlighter(formatButtons, false);
    highlighter(scriptButtons, true);
};

// main logic
const modifyText = (command, defaultUi, value) => {
    document.execCommand(command, defaultUi, value);
};

// button operations
optionsButtons.forEach(button => {
    button.addEventListener("click", () => {
        modifyText(button.id, false, null);
    });
});

// function for highlight selected options
const highlighter = (className, needsRemoval) => {
    className.forEach((button) => {
        button.addEventListener("click", () => {
            if (needsRemoval) {
                let alreadyActive = false;

                // clicked button is active
                if (button.classList.contains("active")) {
                    alreadyActive = true;
                }

                highlighterRemover(className);
                if (!alreadyActive) {
                    // highlight clicked button
                    button.classList.add("active");
                }
            }
            else {
                // other button can highlighted
                button.classList.toggle("active");
            }
        });
    });
};

// remove highlighter
const highlighterRemover = (className) => {
    className.forEach((button) => {
        button.classList.remove("active");
    });
}

window.onload = initializer();

// drag and drop images in share your story page
var allfiles = [];
var fileInput = document.getElementById('img-input');
var fileList;
function handleFiles(e) {

    // Add dropped images or selected images to the list
    var files = e.target.files || e.originalEvent.dataTransfer.files;

    // Add selected images to the list
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var reader = new FileReader();
        allfiles.push(files[i]);
        //formData.append('file', file);

        // Create image preview and close icon
        // Create image preview and close icon
        reader.onload = (function (file) {
            return function (e) {
                var image = $('<img>').attr('src', e.target.result);
                var closeIcon = $('<span>').addClass('close-icon').text('x');

                // Add image and close icon to the list
                var item = $('<div>').addClass('image').append(image).append(closeIcon);
                imageList.append(item);

                // Handle close icon click event
                closeIcon.on('click', function () {
                    item.remove();
                    allfiles.splice(allfiles.indexOf(file), 1);


                    console.log(allfiles);
                });
            };
        })(file);

        // Read image file as data URL
        reader.readAsDataURL(file);
    }
    // Create a new DataTransfer object
    var dataTransfer = new DataTransfer();
    // Create a new FileList object from the DataTransfer object
    fileList = dataTransfer.files;
}

//var allfiles = new DataTransfer().files;
var dropzone = $('#drop-area');
var imageList = $('#img-output');

// Handle file drop event
dropzone.on('drop', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Remove dropzone highlight
    dropzone.removeClass('dragover');
    $('.note-dropzone').remove();
    //$('.note-dropzone-message').remove();
    handleFiles(e);
});

dropzone.on('click', function (e) {
    //e.preventDefault();
    $('#img-input').click();
})

// Handle file dragover event
dropzone.on('dragover', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Highlight dropzone
    dropzone.addClass('dragover');
});

// Handle file dragleave event
dropzone.on('dragleave', function (e) {
    e.preventDefault();
    e.stopPropagation();

    // Remove dropzone highlight
    dropzone.removeClass('dragover');
});


// Handle file input change event
$('#img-input').on('change', function (e) {
    handleFiles(e);
});


//to show story details when in draft
$('#missionTitle').click(function () {
    var missionId = $(this).val();

    $.ajax({
        type: 'GET',
        url: '/Story/GetDraftedStory',
        data: { missionId: missionId },
        success: function (result) {
            if (result != null) {
                $('#StoryTitle').val(result.title);

                const date = new Date(result.createdAt);
                const yyyy = date.getFullYear();
                const mm = String(date.getMonth() + 1).padStart(2, '0');
                const dd = String(date.getDate()).padStart(2, '0');
                const formattedDate = `${yyyy}-${mm}-${dd}`;

                $('#date').val(formattedDate);
                tinymce.get('storyEditor').setContent(result.description);
                /*  $('#storyEditor').text(result.description);*/
                console.log(result.storyMedia);


                var UrlRecords = '';
                $.each(result.storyMedia, async function (index, item) {
                    if (item.type === "videos") {
                        UrlRecords += item.path + '\n';
                    }
                    else {
                        var file = result.storyMedia[index];
                        var image = $('<img>').attr('src', '/images/Upload/Story/' + result.storyMedia[index].path);
                        var closebtn = $('<span>').text('x');
                        var item = $('<div>').addClass('image').append(image).append(closebtn);
                        $('#img-output').append(item);

                        const response = await fetch('/images/Upload/Story/' + file.path);
                        const blob = await response.blob();
                        const files = new File([blob], file.path, { type: blob.type });

                        allfiles.push(files);

                        closebtn.on('click', function () {
                            var index = $(this).parent().index();
                            allfiles.splice(index, 1);
                            $(this).parent().remove();
                            console.log(allfiles);
                        });
                    }

                });

                $('#videoUrls').val(UrlRecords);
                $('#previewButton').removeClass('disabled');
                $('#submitButton').removeClass('disabled');
            }
            else {
                $('#StoryTitle').val(' ');
                $('#date').val(' ');
                tinymce.get('storyEditor').setContent('');
                $('#videoUrls').val(' ');
                $('#img-output').empty();
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
});

//to save story in database in draft mode 
$('#saveStory').click(function (e) {
    e.preventDefault();
    let isValid = false;
    if (validateStoryTitle() == true && validateDate() == true && validateYoutubeUrls() == true && validateMissionTitle() == true) {
        isValid = true;
    }
    if (isValid) {
        /*  console.log("validation");*/
        var formData = new FormData();
        var urls = null;
        var u = $('#videoUrls').val();
        if (u != null) {
            urls = u.split('\n');
            for (var i = 0; i < urls.length; i++) {
                formData.append("VideoUrls", urls[i]);
            }
        }
        else {
            formData.append("VideoUrls", null);
        }
        for (var i = 0; i < allfiles.length; i++) {
            formData.append("Images", allfiles[i]);
        }
        var StoryDescription = tinymce.get('storyEditor').getContent();
        console.log(allfiles);
        formData.append("MissionId", $('#missionTitle').val());
        formData.append("StoryTitle", $('#StoryTitle').val());
        formData.append("Date", $('#date').val());
        formData.append("StoryDescription", StoryDescription);
        /*console.log(formData.val());*/
        $.ajax({
            url: '/Story/SaveStory',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData,

            success: function (result) {
                swal.fire({
                    position: 'top-end',
                    icon: result.icon,
                    title: result.message,
                    showConfirmButton: false,
                    timer: 4000
                })
                $('#previewButton').removeClass('disabled');
                $('#submitButton').removeClass('disabled');
            },
            error: function (error) {
                console.log(error);
            }

        });
    }
});

//to submit story 
$('#submitButton').click(function (e) {
    e.preventDefault();
    let isValid = false;
    if (validateStoryTitle() == true && validateDate() == true && validateYoutubeUrls() == true && validateMissionTitle() == true) {
        isValid = true;
    }
    if (isValid) {
        var formData = new FormData();
        var urls = null;
        var u = $('#videoUrls').val();
        if (u != null) {
            urls = u.split('\n');
            for (var i = 0; i < urls.length; i++) {
                formData.append("VideoUrls", urls[i]);
            }
        }
        else {
            formData.append("VideoUrls", null);
        }

        for (var i = 0; i < allfiles.length; i++) {
            formData.append("Images", allfiles[i]);
        }
        var StoryDescription = tinymce.get('storyEditor').getContent();
        formData.append("MissionId", $('#missionTitle').val());
        formData.append("StoryTitle", $('#StoryTitle').val());
        formData.append("Date", $('#date').val());
        formData.append("StoryDescription", StoryDescription);

        $.ajax({
            url: '/Story/SubmitStory',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData,

            success: function (result) {
                /* console.log(result.message);*/

                swal.fire({
                    position: 'top-end',
                    icon: result.icon,
                    title: result.message,
                    showConfirmButton: false,
                    timer: 4000
                })
            },
            error: function (error) {
                console.log(error);
            }

        });
    }
});

//for preview button
$('#previewButton').click(function () {
    var UserId = $(this).data('user-id');
    var MissionId = $('#missionTitle').val();
    $.ajax({
        method: 'GET',
        url: '/Story/StoryDetails',
        data: { UserId: UserId, MissionId: MissionId },
        success: function (result) {
            console.log(result)
            var url = '/Story/StoryDetails?MissionId=' + MissionId + '&UserId=' + UserId;
            /*  window.location.href = url;*/
            var win = window.open(url, '_blank');
            win.focus();
            /*window.location.href = "/Story/StoryDetails?UserId=UserId&MissionId=MissionId";*/
        },
        error: function (error) {
            console.log(error);
        },

    });
});

//validate youtube urls 
$('#videoUrls').on('blur', validateYoutubeUrls);
function validateYoutubeUrls() {
    var urls = $('#videoUrls').val().split('\n');

    for (var i = 0; i < urls.length; i++) {
        var url = urls[i].trim();
        if (url.length > 0 && !isYoutubeUrl(url)) {
            $("#HelpBlock-urls").text("Please enter only valid youtube urls");
            $('#videoUrls').focus();
            return false;
        }
        else if (url.length <= 20) {
            $("#HelpBlock-urls").text("Maximum 20 URLs are allowed!!");
            return false;
        }
        else {
            $("#HelpBlock-urls").text(" ");
            return true;

        }
    }

}
function isYoutubeUrl(url) {
    var pattern = /^.*(youtube.com\/|youtu.be\/|\/v\/|\/e\/|u\/\w+\/|embed\/|v=)([^#\&\?]*).*/;
    return pattern.test(url);
}

$('#missionTitle').on('blur', validateMissionTitle);
function validateMissionTitle() {
    if ($('#missionTitle').val() === '') {
        $("#HelpBlock-missionTitle").text("Please select the title of the mission!");
        $('#missionTitle').focus();
        return false;
    }
    $("#HelpBlock-date").text("");
    return true;
}

$('#StoryTitle').on('blur', validateStoryTitle);
function validateStoryTitle() {
    if ($('#StoryTitle').val() === '') {
        $("#HelpBlock-storyTitle").text("Story Title is a required field!!");
        $('#StoryTitle').focus();
        return false;
    } else if ($('#StoryTitle').val().length > 255) {
        $("#HelpBlock-storyTitle").text("Maximum 255 characters are allowed!!");
        $('#StoryTitle').focus();
        return false;
    }
    $("#HelpBlock-storyTitle").text("");
    return true;
}

$('#date').on('blur', validateDate);
function validateDate() {
    if ($('#date').val() === '') {
        $("#HelpBlock-date").text("Date is a required field!!");
        $('#date').focus();
        return false;
    }

    $("#HelpBlock-date").text("");
    return true;
}

$('#storyEditor').on('blur', validateStoryDes);
function validateStoryDes() {
    if ($('#text-input').text().trim() === '') {
        $("#HelpBlock-storyDes").text("Story Description is a required field!!");
        return false;
    }
    else if ($('#text-input').text().length > 40000) {
        $("#HelpBlock-storyDes").text("Maximum 40000 characters are allowed!!");
        return false;
    }
    $("#HelpBlock-storyDes").text("");
    return true;
}

//user-profile-img-change
$('.edit-icon').click(function () {
    // Open file input dialog
    $('#profile-image-input').click();
});

// Add change event listener to profile image file input
$('#profile-image-input').change(function () {
    // Read image file and display preview
    var reader = new FileReader();
    reader.onload = function (e) {
        console.log(e.target.result)
        $('.image-edit').find('img').attr('src', e.target.result);
    }
    reader.readAsDataURL(this.files[0]);
});

// Adding skills to Profile
// Add selected skill to the right div on label click
$(document).on('click', '.all-skills-label', function () {
    $(this).toggleClass('highlight');
});

//adding skills to the right side of the container 
$('#addSkillsToRight').click(function () {

    $('.selected-skills-list').empty();
    var selectedUserSkills = "";
    $('.all-skills-label').each(function () {
        if ($(this).hasClass('highlight')) {
            selectedUserSkills += $(this).text() + "\n";
        }
    })

    var skillsArray = selectedUserSkills.split("\n");
    /*    console.log(skillsArray);*/

    skillsArray.forEach(function (skill, index) {
        var label = $('<label>');
        label.addClass('all-skills-label');
        label.text(skill);
        $('.selected-skills-list').append(label);
    })
})

//removing skills from the right container
$('#RemoveSkillsFromRight').click(function () {
    var selectedUserSkills = "";
    $('.selected-skills-list .all-skills-label').each(function () {
        if ($(this).hasClass('highlight')) {
            $(this).remove();
            selectedUserSkills += $(this).text() + "\n";
        }
    })

    var skillsArray = selectedUserSkills.split("\n");
    /*    console.log(skillsArray);*/

    skillsArray.forEach(function (skill, index) {
        $('.all-skills-label').each(function () {
            if ($(this).hasClass('highlight') && $(this).text() === skill) {
                $(this).toggleClass('highlight');
            }
        })
    })
})

//global skills array 
let skillsArr = []
$(".selected-skills-list .all-skills-label").each(function () {
    $(".user-skills-container").append(`<label >` + $(this).text() + `</label>`)
    skillsArr.push($(this).text());
});
$("#UpdatedUserSkills").val(skillsArr.join(','));

//to save skill and display them in the my skills container
$('#SaveSkillBtn').click(function () {
    skillsArr = []
    $(".user-skills-container").empty();
    $(".selected-skills-list .all-skills-label").each(function () {
        $(".user-skills-container").append(`<label >` + $(this).text() + `</label>`)
        skillsArr.push($(this).text());
    });

    $("#UpdatedUserSkills").val(skillsArr.join(','));
})

//cities based on countries in user profile
$("#UserCountrySelect").click(function () {
    var CountryId = $(this).val()
    UserGetCitiesByCountry(CountryId);
});

//get cities based on countries for filters 
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

//validations for change password in user profile
$('#OldPassword').on('blur', validateOldPassword);
function validateOldPassword() {
    var oldPass = $('#OldPassword').val();
    if (oldPass === "" || oldPass === null) {
        $('#validateOldPass').text("Old Password is required!");
        $('#OldPassword').focus();
        return false;
    } else {
        $('#validateOldPass').text("");
    }
    return true;
}

$('#NewPassword').on('blur', validateNewPassword);
function validateNewPassword() {
    var newPass = $('#NewPassword').val();
    if (newPass === "" || newPass === null) {
        $('#validateNewPass').text("New Password is required!");
        $('#NewPassword').focus();
        return false;
    } else if (newPass === $('#OldPassword').val()) {
        $('#validateNewPass').text("Old and New Password cannot be same!");
        $('#NewPassword').focus();
        return false;
    }
    //} else if (newPass.length < 8) {
    //    $('#validateNewPass').text("New Password must be 8 characters long!");
    //} else {
    else {
        $('#validateNewPass').text("");
    }
    return true;
}

$('#ConfirmPassword').on('blur', validateConfirmPassword);
function validateConfirmPassword() {
    var confirmPass = $('#ConfirmPassword').val();
    if (confirmPass === "" || confirmPass === null) {
        $('#validateConfirmPass').text("Re-enter your New Password!");
        $('#ConfirmPassword').focus();
        return false;
    } else if (confirmPass !== $('#NewPassword').val()) {
        $('#validateConfirmPass').text("New Password and Confirm Password must be same!");
        $('#ConfirmPassword').focus();
        return false;
    } else {
        $('#validateConfirmPass').text("");
    }
    return true;
}

//change password in user profile
$('#ChangePasswordBtn').on('click', function () {
    let isValid = false;
    if (validateOldPassword() == true && validateNewPassword() == true && validateConfirmPassword() == true) {
        isValid = true;
    }
    if (isValid) {
        var oldPass = $('#OldPassword').val();
        var newPass = $('#NewPassword').val();

        $.ajax({
            type: "POST",
            url: "/User/ChangePassword",
            data: { oldPass: oldPass, newPass: newPass },
            success: function (result) {
                swal.fire({
                    position: 'top-end',
                    icon: result.icon,
                    title: result.message,
                    showConfirmButton: false,
                    timer: 4000
                })
            },
            error: function (error) {
                console.log(error)
            }
        })
    } else {

    }
})

//password show and hide in user profile 
$('.bi-eye-slash').on('click', function () {
    $(this).parent().find('input').attr('type', 'text');
    $(this).addClass('d-none');
    $(this).prev().removeClass('d-none');
})
$('.bi-eye-fill').on('click', function () {
    $(this).parent().find('input').attr('type', 'password');
    $(this).addClass('d-none');
    $(this).next().removeClass('d-none');
})

//contact us form
$("#ContactUsBtn").on('click', function () {
    var ContactName = $("#ContactName").val();
    var ContactEmail = $('#ContactEmail').val();
    var ContactSubject = $('#ContactSubject').val();
    var ContactMessage = $('#ContactMessage').val();

    $.ajax({
        type: "POST",
        url: "/User/ContactUs",
        data: { ContactName: ContactName, ContactEmail: ContactEmail, ContactSubject: ContactSubject, ContactMessage: ContactMessage },
        success: function (result) {

        },
        error: function (error) {

        },
    });

})