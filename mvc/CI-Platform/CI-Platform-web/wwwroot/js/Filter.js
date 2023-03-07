//console.log("hi")

$("#CountryList li").click(function () {
    $(this).addClass('selected');

    var countryId = $(this).val();
    console.log(countryId);

    $('.card-div').each(function () {
        console.log("hello");
        var cardCountry = $(this).find('.mission-country').text();
        console.log(cardCountry);

        if (countryId == cardCountry) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });

    GetCitiesByCountry(countryId);
});

function GetCitiesByCountry(countryId) {
    $.ajax({
        type: "GET",
        url: "/Home/GetCitiesByCountry",
        data: { countryId: countryId },
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
        data: { countryId: countryId },
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
    });

})


$(".dropdown .CardsFilter").on('change', 'input[type="checkbox"]', function () {
    console.log('hello');

    var selectedCities = $('#CityList input[type="checkbox"]:checked').map(function () {
        return $(this).next('label').text();
    }).get();
    console.log(selectedCities);

    var selectedThemes = $('#ThemeList input[type="checkbox"]:checked').map(function () {
        return $(this).next('label').text();
    }).get();
    console.log(selectedThemes);

    if (selectedCities.length === 0 && selectedThemes.length === 0) {
        $('.card-div').show();
    } else {
        //console.log(selectedCities);

        $('.card-div').each(function () {
            var cardCity = $(this).find('.mission-city').text();
            var cardTheme = $(this).find('.mission-theme').text();

            var cityFlag = selectedCities.some(function (selectedCity) {
                return selectedCity.trim().toUpperCase() == cardCity.trim().toUpperCase();
            });
            var themeFlag = selectedThemes.some(function (selectedTheme) {
                return selectedTheme.trim().toUpperCase() == cardTheme.trim().toUpperCase();
            });

            if (cityFlag) {
                $(this).show();
            } else {
                $(this).hide();
            }
            if (selectedThemes.length === 0) {
                if (cityFlag) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            } else {
                if (cityFlag && themeFlag) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }
        });
    }
});

//$(".dropdown #ThemeList").on('change', 'input[type="checkbox"]', function () {
//    console.log('hello');
//    var selectedThemes = $('input[type="checkbox"]:checked').map(function () {
//        return $(this).next('label').text();
//    }).get();
//    console.log(selectedThemes);

//    $('.card-div').each(function () {
//        var cardTheme = $(this).find('.mission-theme').text();
//        var flag = selectedThemes.some(function (selectedTheme) {
//            return selectedTheme.trim().toUpperCase() == cardTheme.trim().toUpperCase();
//        });
//        if (flag) {
//            $(this).show();
//        } else {
//            $(this).hide();
//        }
//    });
//});



      
