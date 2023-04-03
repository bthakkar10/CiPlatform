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

//apply in mission button 
$('#ApplyBtnMission').click(function () {
    var MissionId = $(this).data('mission-id');
    var btn = $(this);
    $.ajax({
        type: 'POST',
        url: '/Home/ApplyMission',
        data: { MissionId: MissionId },
        success: function (result) {
            /*$('#ApplyBtnMission').addClass('disabled text-danger');*/

            var newButton = $('<a>').addClass('btn card-btn disabled text-danger')
                .append($('<span>').text('Approval Pending ')
                    .append($('<i>').addClass('bi bi-patch-exclamation-fill')));
            btn.replaceWith(newButton);
        },
        error: function (error) {
            console.log(error);
        },
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
                $('#text-input').text(result.description);
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


                        //var blob = new Blob([file.path], { type: 'image/png' });
                        //var files = new File([blob], file.path, { type: 'image/png' });





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
        console.log(allfiles);
        formData.append("MissionId", $('#missionTitle').val());
        formData.append("StoryTitle", $('#StoryTitle').val());
        formData.append("Date", $('#date').val());
        formData.append("StoryDescription", $('#text-input').text());
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

    for (var i = 0; i < allfiles.length; i++) {
        formData.append("Images", allfiles[i]);
    }
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
            var url = '/Story/StoryDetails?MissionId=' + MissionId + '&UserId=' + UserId;
            window.location.href = url;
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
    else if (url.length > 20) {
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

    //var images = [];
    //var maxImages = 20;
    //var maxImageSize = 4 * 1024 * 1024; // 4MB in bytes

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

