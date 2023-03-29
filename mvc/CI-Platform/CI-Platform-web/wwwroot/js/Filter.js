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
var UserId = ($("#user-id").text());
let allDropdowns = $('.dropdown ul');

//global search text selection
$('#searchText').on('keyup', function () {
    if (currentUrl.includes("HomePage")) {
        FilterSortPaginationSearch();
        console.log("first")
    }
    else if (currentUrl.includes("StoryListing")) {
        StoryFilter();
        console.log("second")
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
        data: { CountryId: CountryId, CityId: CityId, ThemeId: ThemeId, SkillId: SkillId, SearchText: SearchText, sortCase: sortCase, UserId: UserId, pageNo: pageNo, pagesize: pagesize },
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
    var ThemeId = $('#ThemeList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
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
$("#CountryList li").click(function () {
    $(this).addClass('selected');

    var CountryId = $(this).val();
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

    $.ajax({
        type: "GET",
        url: "/Home/GetCitiesByCountry",
        data: { CountryId: CountryId },
        success: function (data) {
            var dropdown = $("#CityListAccordian");
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
                        if ($(this).hasClass('bi-heart')) {
                            $(this).addClass('bi-heart-fill text-danger')
                            $(this).removeClass('bi-heart text-dark')
                            text.empty();
                            text.append("Remove from favourites");
                            console.log("added");
                            /*  alert("Mission added to favourites successfully!!!");*/
                        }
                        else {
                            $(this).addClass('bi-heart text-dark')
                            $(this).removeClass('bi-heart-fill text-danger')
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
                console.log("error")

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
    if (comment != null) {
        $.ajax({
            method: 'POST',
            url: '/Home/PostComment',
            data: { comment: comment, missionId: missionId },
            success: function () {

                $('.newComment').val('');
                /* alert("comment will be displayed after approval");*/
            },
            error: function (error) {
                console.log("error");
            }
        });
    }
    else {
        alert("null comment not allowed");
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
    $.ajax({
        type: "GET",
        url: "/Home/UserList",
        data: { MissionId: MissionId },
        success: function (coworkers) {

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
                console.log(result);
                const date = new Date(result.createdAt);
                const yyyy = date.getFullYear();
                const mm = String(date.getMonth() + 1).padStart(2, '0');
                const dd = String(date.getDate()).padStart(2, '0');
                const formattedDate = `${yyyy}-${mm}-${dd}`;

                $('#date').val(formattedDate);
                $('#text-input').text(result.description);
                console.log(result.storyMedia);


                var UrlRecords = '';
                $.each(result.storyMedia, function (index, item) {
                    if (item.type === "videos") {
                        UrlRecords += item.path + '\n';
                    }
                });
                console.log(UrlRecords);
                $('#videoUrls').val(UrlRecords);

                /*   console.log(result.storyMedia[i].path);*/
                $.each(result.storyMedia, function (index, item) {
                    if (item.type === "images") {
                        var image = $('<img>').attr('src', '/images/Upload/' + item.path);
                        var closeIcon = $('<button>').text('x').click(function () {
                            $(this).parent().remove(); // remove the parent div containing both the image and the close button
                        });
                        var img = $('<span>').addClass('image').append(image).append(closeIcon);
                        $('#img-output').append(img);

                    }
                })

                $('#previewButton').removeClass('disabled');
                $('#submitButton').removeClass('disabled');
            }
            else {
                $('#StoryTitle').val(' ');
                $('#date').val(' ');
                $('#text-input').text(' ');
                $('#videoUrls').val(' ');
                $('#img-output').val(' ');
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
    if (validateStoryTitle() == true && validateDate() == true && validateStoryDes() == true && validateYoutubeUrls() == true) {
        isValid = true;
    }
    if (isValid) {
        console.log("validation");
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

        var input = $('#img-input');
        var files = input[0].files;
        for (var i = 0; i < files.length; i++) {
            formData.append("Images", files[i]);
        }
        console.log(files);
        formData.append("MissionId", $('#missionTitle').val());
        formData.append("StoryTitle", $('#StoryTitle').val());
        formData.append("Date", $('#date').val());
        formData.append("StoryDescription", $('#text-input').text());

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
    else {
        $('#error-message').text("Please enter all the fields as required.");
    }
});

//to submit story 
$('#submitButton').click(function () {
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

    var input = $('#img-input');
    var files = input[0].files;
    for (var i = 0; i < files.length; i++) {
        formData.append("Images", files[i]);
    }
    console.log(files);
    formData.append("MissionId", $('#missionTitle').val());
    formData.append("StoryTitle", $('#StoryTitle').val());
    formData.append("Date", $('#date').val());
    formData.append("StoryDescription", $('#text-input').text());

    $.ajax({
        url: '/Story/SubmitStory',
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
        },
        error: function (error) {
            console.log(error);
        }

    });
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
            /* window.location.href = "/Story/StoryDetails";*/
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
        //} else if (url.length > 20) {
        //    $("#HelpBlock-urls").text("Maximum 20 URLs are allowed!!");
        //    return false;
        //} else {
            $("#HelpBlock-urls").text(" ");
            return true;
          
    }
    
}
function isYoutubeUrl(url) {
    var pattern = /^.*(youtube.com\/|youtu.be\/|\/v\/|\/e\/|u\/\w+\/|embed\/|v=)([^#\&\?]*).*/;
    return pattern.test(url);
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

$('.editor').on('blur', validateStoryDes);
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


    var images = [];
    var maxImages = 20;
    var maxImageSize = 4 * 1024 * 1024; // 4MB in bytes

    // Bind drop event to the drag and drop area
    //$('#drop-area').on('drop', function (e) {
    //    e.preventDefault();
    //    var files = e.originalEvent.dataTransfer.files;

    //    for (var i = 0; i < files.length; i++) {
    //        var file = files[i];
    //        var fileType = file.type;
    //        var fileSize = file.size;

    //        // Check if file is an image and not more than 4MB
    //        if (!fileType.startsWith('image/')) {
    //            alert('Please upload only images.');
    //            return;
    //        }
    //        if (fileSize > maxImageSize) {
    //            alert('Please upload images smaller than 4MB.');
    //            return;
    //        }

    //        // Add image to array and display in drag and drop area
    //        images.push(file);
    //        $('#drop-area').append('<img src="' + URL.createObjectURL(file) + '">');

    //        // Limit the number of images to 20
    //        if (images.length >= maxImages) {
    //            alert('You can upload a maximum of 20 images.');
    //            return;
    //        }
    //    }
    //});

    // Prevent default behavior on dragover event
    $('#drop-area').on('dragover', function (e) {
        e.preventDefault();
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

// function format(){
//     var id = document.getElementById("textformat");
//     id.style.textDecoration="none";
// }


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

// share story
let files = [],
    dragArea = document.querySelector('.drag-area'),
    input = document.querySelector('.drag-area input'),
    button = document.querySelector('.drag-card button'),
    select = document.querySelector('.drag-area .select'),
    container = document.querySelector('.container-img');

/** CLICK LISTENER */
select.addEventListener('click', () => input.click());

/* INPUT CHANGE EVENT */
input.addEventListener('change', () => {
    let file = input.files;

    // if user select no image
    if (file.length == 0) return;

    for (let i = 0; i < file.length; i++) {
        if (file[i].type.split("/")[0] != 'image') continue;
        if (!files.some(e => e.name == file[i].name)) files.push(file[i])
    }

    showImages();
});

/** SHOW IMAGES */
function showImages() {
    container.innerHTML = files.reduce((prev, curr, index) => {
        return `${prev}
                <div class="image">
                    <span class="closeIconImg" onclick="delImage(${index})">&times;</span>
                    <img src="${URL.createObjectURL(curr)}" />
                </div>`
    }, '');
}

/* DELETE IMAGE */
function delImage(index) {
    files.splice(index, 1);
    showImages();
}

/* DRAG & DROP */
dragArea.addEventListener('dragover', e => {
    e.preventDefault()
    dragArea.classList.add('dragover')
})

/* DRAG LEAVE */
dragArea.addEventListener('dragleave', e => {
    e.preventDefault()
    dragArea.classList.remove('dragover')
});

var maxImageSize = 4 * 1024 * 1024; 
/* DROP EVENT */
dragArea.addEventListener('drop', e => {
    e.preventDefault()
    dragArea.classList.remove('dragover');

    let file = e.dataTransfer.files;
    for (let i = 0; i < file.length; i++) {
        var files = files[i];
/*        var fileType = file.type;*/
        var fileSize = files.size;
        /** Check selected file is image */
        if (file[i].type.split("/")[0] != 'image') continue;
        if (fileSize > maxImageSize) {
            alert('Please upload images smaller than 4MB.');
            return;
        }
        if (!files.some(e => e.name == file[i].name)) files.push(file[i]);

        $('#drop-area').append('<img src="' + URL.createObjectURL(file) + '">');

        // Limit the number of images to 20
        if (images.length >= maxImages) {
            alert('You can upload a maximum of 20 images.');
            return;
        }
    }
    showImages();
});