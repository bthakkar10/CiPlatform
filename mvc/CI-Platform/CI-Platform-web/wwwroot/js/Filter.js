//global
$(document).ready(function () {
    FilterSortPaginationSearch(); 
});

var SelectedsortCase = null;
var SelectedCountry = null;

//for filters sorting stored procedure
function FilterSortPaginationSearch() {
    var CountryId = SelectedCountry;
    var CityId = $('#CityList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var ThemeId = $('#ThemeList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var SkillId = $('#SkillList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var SearchText = $("#search").val();
    var sortCase = SelectedsortCase;
    console.log(UserId);
    $.ajax({
        type: 'POST',
        url: '/Home/HomePage',
        data: { CountryId: CountryId, CityId: CityId, ThemeId: ThemeId, SkillId: SkillId, SearchText: SearchText, sortCase: sortCase, UserId: UserId },
        success: function (data) {

            console.log("Done");
            var view = $(".partialViews");
            view.empty();
            view.append(data);
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
/*    console.log(CountryId);*/

    //$('.card-div').each(function () {

    //    var cardCountry = $(this).find('.mission-country').text();


    //    if (CountryId == cardCountry) {
    //        $(this).show();
    //    } else {
    //        $(this).hide();
    //    }
    //});

    GetCitiesByCountry(CountryId);
    FilterSortPaginationSearch();
});
//get cities based on countries 
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
let allDropdowns = $('.dropdown ul');
allDropdowns.each(function () {
        let dropdown = $(this);
        $(this).on('change', 'input[type="checkbox"]', function () {

            // if the check box is checked then add it to pill
            if ($(this).is(':checked')) {
                let selectedOptionText = $(this).next('label').text();
                let selectedOptionValue = $(this).val();
                const closeAllButton = filterPills.children('.closeAll');

                // creating a new pill
                let pill = $('<span></span>').addClass('pill ');

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
                    FilterSortPaginationSearch();
                    if (filterPills.children('.pill').length === 1) {
                        filterPills.children('.closeAll').remove();
                    }

                });

                // Add "Close All" button
                if (closeAllButton.length === 0) {
                    filterPills.append('<div class=" closeAll"><span>Close All</span></div>');
                    filterPills.children('.closeAll').click(function () {
                        allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                        filterPills.empty();
                        FilterSortPaginationSearch();

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

            //FilterMissions();
            FilterSortPaginationSearch();
        });
    
    })

//to add or remove favourites
function favourite() {
    $('.favourite-button').on('click', function () {
        console.log("success");
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
                            alert("Mission added to favourites successfully!!!");
                        }
                        else {
                            $(this).addClass('bi-heart text-dark')
                            $(this).removeClass('bi-heart-fill text-danger')
                            text.empty();
                            text.append("Add to favourites");
                            console.log("remove");
                            alert("Mission removed from favourites successfully!!!");
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


//to add or update ratings(working on it)
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
            alert("ratings updated successfully!!");
        },
        error: function (error) {
            alert("Sessin Expired.");
            if (confirm("Please Login Again And Try Agaion")) {
                window.location.href = "/Home/Index";
            }
        }
    });
});

//comments
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
        console.log("done");
                $('.newComment').val('');
                alert("comment will be displayed after approval");
            },
            error: function (error) {
                console.log("error");
            }
        });
    }
    else {
        console.log("null");
    }
});

//recommend to co-worker invite 
$(document).on('click', '.model-invite-btn', function () {

    var ToUserId = $(this).data('user-id');
    var MissionId = $(this).data('mission-id');
    var FromUserId = $(this).data('from-user-id');
    var btn = $(this);
    console.log(FromUserId);
    $.ajax({
        type: "POST",
        url: "/Home/MissionInvite",
        data: { ToUserId: ToUserId, MissionId: MissionId, FromUserId: FromUserId },
        success: function () {
            console.log("success");
            var button = $('<button>').addClass('btn btn-success disabled')
                .append($('<span>').text('Already Invited '));
            btn.replaceWith(button);
           /* $('invited-' + ToUserId).html('<button class="btn btn-outline-success" data-mission-Id="@mission_id">Already Invited</button>');*/
        },
        error: function (error) {
            console.log(error);
        }
    });
});