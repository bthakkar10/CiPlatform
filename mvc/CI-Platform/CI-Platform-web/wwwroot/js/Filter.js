//console.log("hi")

$("#CountryList li").click(function () {
    $(this).addClass('selected');

    var CountryId = $(this).val();
    console.log(CountryId);

    $('.card-div').each(function () {
       
        var cardCountry = $(this).find('.mission-country').text();
      

        if (CountryId == cardCountry) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });

    GetCitiesByCountry(CountryId);
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
                FilterMissions();
            });

            // Add "Close All" button
            if (closeAllButton.length === 0) {
                filterPills.append('<div class=" closeAll"><span>Close All</span></div>');
                filterPills.children('.closeAll').click(function () {
                    allDropdowns.find('input[type="checkbox"]').prop('checked', false);
                    filterPills.empty();
                    FilterMissions();
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

        FilterMissions();
    });

})

function FilterMissions() { 

    var selectedCities = $('#CityList input[type="checkbox"]:checked').map(function () {
        return $(this).next('label').text();
    }).get();
    console.log(selectedCities);

    var selectedThemes = $('#ThemeList input[type="checkbox"]:checked').map(function () {
        return $(this).next('label').text();
    }).get();
    console.log(selectedThemes);

    var selectedSkills = $('#SkillList input[type="checkbox"]:checked').map(function () {
        return $(this).next('label').text();
    }).get();
    console.log(selectedSkills);

    if (selectedCities.length === 0 && selectedThemes.length === 0 && selectedSkills.length === 0) {
        $('.card-div').show();
    } else {
        //console.log(selectedCities);

        $('.card-div').each(function () {
            var cardCity = $(this).find('.mission-city').text();
            var cardTheme = $(this).find('.mission-theme').text();
            var cardSkill = $(this).find('.mission-skill').text();
            console.log(cardSkill);

            var cityFlag = selectedCities.some(function (selectedCity) {
                return selectedCity.trim().toUpperCase() == cardCity.trim().toUpperCase();
            });
            var themeFlag = selectedThemes.some(function (selectedTheme) {
                return selectedTheme.trim().toUpperCase() == cardTheme.trim().toUpperCase();
            });
            var skillFlag = selectedSkills.some(function (selectedSkill) {
                for (var i = 0; i < cardSkill.length; i++) {
                    if (selectedSkill.trim() == cardSkill[i]) {
                        
                        console.log(i);
                        return true;
                    }
                }
              
                //$(this).each(function () {
                //    var cs = cardSkill.trim().toUpperCase();
                //    var ss = selectedSkill.trim().toUpperCase();
                //    /*cardSkill.trim().toUpperCase().indexOf(selectedSkill.trim().toUpperCase()) > -1;*/
                //     return cs.indexOf(ss) > -1;
                //});
            });
           


            //if (cityFlag) {
            //    $(this).show();
            //} else {
            //    $(this).hide();
            //}
            if (selectedThemes.length === 0 && selectedSkills.length === 0) {
                if (cityFlag) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }
            else if (selectedSkills.length === 0) {
                if (cityFlag && themeFlag) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }
            else {
                if (skillFlag )  {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }
        });
    }

}


$('#sortByDropdown li').on('click', function () {
    selectedSortOption = $(this).find('a').text();

    let gridCardsContainer = $('.grid-card').parent().parent();
    let listCardsContainer = $('.list-card').parent();
    var dateArray = [];
    switch (selectedSortOption) {
        case 'Newest':
            let cardsDateForNewest = $('.card').find('.created-date');
            cardsDateForNewest.each(function (j) {
                dateArray.push($(this).text());
            });
            dateArray = $.unique(dateArray)
            // Arrange array Elemeny in Ascending order
            dateArray.sort();

            // Arrange Array Element In Descending order
            dateArray.reverse();
            dateArray = $.unique(dateArray)
            for (var i = 0; i < dateArray.length; i++) {
                $('.grid-card').each(function () {
                    if ($(this).find('.created-date').text() == dateArray[i]) {
                        $(this).parent().appendTo($(gridCardsContainer));
                    }
                });
            }
            for (var i = 0; i < dateArray.length; i++) {
                $('.list-card').each(function () {
                    if ($(this).find('.created-date').text() == dateArray[i]) {
                        $(this).parent().appendTo($(listCardsContainer));
                    }
                });
            }
            filter()
            break;
        case 'Oldest':
            let cardsDateForOldest = $('.card').find('.created-date')
            var dateArray = [];
            cardsDateForOldest.each(function (j) {
                dateArray.push($(this).text());
            });
            // Arrange array Elemeny in Ascending order
            dateArray = $.unique(dateArray)
            dateArray.sort(function (a, b) {
                return new Date(a) - new Date(b);
            });
            console.log(dateArray)

            for (var i = 0; i < dateArray.length; i++) {
                $('.grid-card').each(function () {
                    if ($(this).find('.created-date').text() == dateArray[i]) {
                        console.log(true)

                        $(this).parent().appendTo($(gridCardsContainer));
                    }
                });
            }
            for (var i = 0; i < dateArray.length; i++) {
                $('.list-card').each(function () {
                    if ($(this).find('.created-date').text() == dateArray[i]) {
                        console.log(true)

                        $(this).parent().appendTo($(listCardsContainer));
                    }
                });
            }
            filter()
            break;
        case 'Lowest available seats':

            console.log(selectedSortOption)
            break;
        case 'Highest available seats':
            console.log(selectedSortOption)
            break;
        case 'My favourites':
            console.log(selectedSortOption)
            break;
        case 'Registration deadline':
            let deadlines = $('.card').find('.deadline')
            var dateArray = [];
            deadlines.each(function (j) {
                dateArray.push($(this).text());
            });
            dateArray.sort(function (a, b) {
                var dateA = new Date(
                    parseInt(a.substring(6)),
                    parseInt(a.substring(3, 5)) - 1,
                    parseInt(a.substring(0, 2))
                );
                var dateB = new Date(
                    parseInt(b.substring(6)),
                    parseInt(b.substring(3, 5)) - 1,
                    parseInt(b.substring(0, 2))
                );
                return dateA - dateB;
            });
            //dateArray = $.unique(dateArray)
            // Arrange array Elemeny in Ascending order
            //dateArray.sort();
            console.log(dateArray)
            for (var i = 0; i < dateArray.length; i++) {
                $('.grid-card').each(function () {
                    if ($(this).find('.deadline').text() == dateArray[i]) {

                        $(this).parent().appendTo($(gridCardsContainer));
                    }
                });
            }
            for (var i = 0; i < dateArray.length; i++) {
                $('.list-card').each(function () {
                    if ($(this).find('.deadline').text() == dateArray[i]) {
                        console.log(true)

                        $(this).parent().appendTo($(listCardsContainer));
                    }
                });
            }
            filter()
            break;
    }
})




      
