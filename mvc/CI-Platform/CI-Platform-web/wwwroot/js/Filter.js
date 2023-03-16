//console.log("hi")
$(document).ready(function () {
    FilterSortPaginationSearch(); 
});

var SelectedsortCase = null;
var SelectedCountry = null;

function FilterSortPaginationSearch() {
    console.log(UserId);
    console.log("HEllo");
    var CountryId = SelectedCountry;
    var CityId = $('#CityList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var ThemeId = $('#ThemeList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    console.log(ThemeId);
    var SkillId = $('#SkillList input[type="checkbox"]:checked').map(function () { return $(this).val(); }).get().join();
    var SearchText = $('#search').val();
    var sortCase = SelectedsortCase;
    var UserId = $('.user-id').text();

    console.log(UserId);
    /*   var UserId = $()*/

    $.ajax({
        type: "POST",
        url: "/Home/HomePage",
        data: { CountryId: CountryId, CityId: CityId, ThemeId: ThemeId, SkillId: SkillId, SearchText: SearchText, sortCase: sortCase, UserId: UserId },
        success: function (data) {
            console.log("This works");
            var view = $(".partialViews");
            view.empty();
            view.append(data);

        },
        error: function (error) {
            // Show an error message or handle the error
            console.log("error");
        }
    });
}

$("#sortByDropdown li").click(function () {
    var sortCase = $(this).val();
    SelectedsortCase = sortCase;
    console.log(sortCase);

    FilterSortPaginationSearch();
});

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
                alert("Mission added to favourites successfully!!!");
                var allMissionId = $('.favourite-button')
                allMissionId.each(function () {
                    if ($(this).data('mission-id') === missionId) {
                        if ($(this).hasClass('bi-heart')) {
                            $(this).addClass('bi-heart-fill text-danger')
                            $(this).removeClass('bi-heart text-light')
                            console.log("added")
                        }
                        else {
                            $(this).addClass('bi-heart text-light')
                            $(this).removeClass('bi-heart-fill text-danger')
                            console.log("remove")
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

    //function FilterMissions() {

    //    var selectedCities = $('#CityList input[type="checkbox"]:checked').map(function () {
    //        return $(this).next('label').text();
    //    }).get();
    //    console.log(selectedCities);

    //    var selectedThemes = $('#ThemeList input[type="checkbox"]:checked').map(function () {
    //        return $(this).next('label').text();
    //    }).get();
    //    console.log(selectedThemes);

    //    var selectedSkills = $('#SkillList input[type="checkbox"]:checked').map(function () {
    //        return $(this).next('label').text();
    //    }).get();
    //    console.log(selectedSkills);

    //    if (selectedCities.length === 0 && selectedThemes.length === 0 && selectedSkills.length === 0) {
    //        $('.card-div').show();
    //    } else {
    //        //console.log(selectedCities);

    //        $('.card-div').each(function () {
    //            var cardCity = $(this).find('.mission-city').text();
    //            var cardTheme = $(this).find('.mission-theme').text();
    //            var cardSkill = $(this).find('.mission-skill').text();
    //            console.log(cardSkill);

    //            var cityFlag = selectedCities.some(function (selectedCity) {
    //                return selectedCity.trim().toUpperCase() == cardCity.trim().toUpperCase();
    //            });
    //            var themeFlag = selectedThemes.some(function (selectedTheme) {
    //                return selectedTheme.trim().toUpperCase() == cardTheme.trim().toUpperCase();
    //            });

    //            var skillFlag = selectedSkills.every(function (selectedSkill) {
    //                return cardSkill.indexOf(selectedSkill.trim()) >= 0;

    //            });


                //if (cityFlag) {
                //    $(this).show();
                //} else {
                //    $(this).hide();
                //}
                //if (selectedThemes.length === 0 && selectedSkills.length === 0) {
                //    if (cityFlag) {
                //        $(this).show();
                //    } else {
                //        $(this).hide();
                //    }
                //}
                //else if (selectedSkills.length === 0) {
                //    if (cityFlag && themeFlag) {
                //        $(this).show();
                //    } else {
                //        $(this).hide();
                //    }
                //}
                //else {
                //    if (skillFlag && cityFlag && themeFlag) {
                //        $(this).show();
                //    }
                //    else {
                //        $(this).hide();
                //    }
                //}
                //if (selectedCities.length === 0) {
                //    $(this).hide();
                //}
                //else if (cityFlag) {
                //    if (selectedThemes.length === 0) {
                //        $(this).show();
                //    }
                //    else if (themeFlag) {
                //        if (selectedSkills.length === 0) {
                //            $(this).show();
                //        }
                //        else if (skillFlag) {
                //            $(this).show();
                //        }
                //        else {
                //            $(this).hide();
                //        }

                //    }
                //    else {
                //        $(this).hide();
                //    }
                //}
                //else {
                //    $(this).hide();
                //}


    //        });
    //    }

    //}


