function temp() {
    var cities = [];
    var countries = [];
    var themes = [];
    var skills = [];
    var div = document.getElementById("citydiv");
    var div1 = document.getElementById("countrydiv");
    var div2 = document.getElementById("themediv");
    var div3 = document.getElementById("skilldiv");

    var list = div.getElementsByTagName("input");
    //Getting cityId list
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            cities.push(list[i].value);
        }
    }
    //Getting countryId list
    var list1 = div1.getElementsByTagName("input");
    for (i = 0; i < list1.length; i++) {
        if (list1[i].checked) {
            countries.push(list1[i].value);
        }
    }
    //Getting themeId list
    var list2 = div2.getElementsByTagName("input");
    for (i = 0; i < list2.length; i++) {
        if (list2[i].checked) {
            themes.push(list2[i].value);
        }
    }
    //Getting skillId list
    var list3 = div3.getElementsByTagName("input");
    for (i = 0; i < list3.length; i++) {
        if (list3[i].checked) {
            skills.push(list3[i].value);
        }
    }

    //Search
    var search = document.getElementById("search").value;

    //sort-by
    var sort = document.getElementById("sort_by").value;
    console.log($("#grid-view").children.length);
    $.ajax({
        type: "POST", // POST
        url: '/Mission/Filter',
        data: {
            'cityId': cities,
            'countryId': countries,
            'themeId': themes,
            'skillId': skills,
            'search': search,
            'sort': sort

        },
        dataType: "html", // return datatype like JSON and HTML
        success: function (data) {
            if ($("#grid-view").hasClass("d-none"))
            {
                $("#list-grid").empty();
                console.log("Hii");
                $("#list-grid").html(data);
                $("#grid-view").addClass("d-none");
            }
            else {
                $("#list-grid").empty();
                console.log("Hii");
                $("#list-grid").html(data);
                $("#list-view").addClass("d-none");
            }

        },
        error: function (e) {
            console.log("Bye");
            alert('Error');
        },

    });
}



 public List < MissionCard > Filter(List < int >? cityId, List < int >? countryId, List < int >? themeId, List < int >? skillId, string ? search, int ? sort)
{
    List < MissionCard > cards = new List < MissionCard > ();
    var missioncards = GetMissionCards();
    var missionskill = _db.MissionSkills.ToList();
    List < int > temp = new List < int > ();
    #region Filter city
    if (cityId.Count != 0) {
        foreach(var n in cityId)
        {
            foreach(var item in missioncards)
            {
                        bool check = cards.Any(x => x.mission.MissionId == item.mission.MissionId);
                if (item.mission.CityId == n && check == false) {
                    cards.Add(item);
                }
            }
        }
    }
    #endregion

    #region Filter country
    if (countryId.Count != 0) {
        foreach(var m in countryId)
        {
            foreach(var item in missioncards)
            {
                        bool check = cards.Any(x => x.mission.MissionId == item.mission.MissionId);
                if (item.mission.CountryId == m && check == false) {
                    cards.Add(item);
                }
            }
        }
    }
    #endregion

    #region Filter theme
    if (themeId.Count != 0) {
        foreach(var m in themeId)
        {
            foreach(var item in missioncards)
            {
                        bool check = cards.Any(x => x.mission.MissionId == item.mission.MissionId);
                if (item.mission.ThemeId == m && check == false) {
                    cards.Add(item);
                }
            }
        }

    }
    #endregion

    #region Filter skill
    if (skillId.Count != 0) {
        foreach(var m in skillId)
        {
            foreach(var item in missionskill)
            {
                if (item.SkillId == m) {
                    temp.Add((int)item.MissionId);
                }
            }
            foreach(var item in temp)
            {
                        bool check = cards.Any(x => x.mission.MissionId == item);
                if (check == false) {
                    cards.Add(missioncards.FirstOrDefault(x => x.mission.MissionId == item));
                }
            }

        }

    }
    #endregion

    #region Filter search
    if (search != null) {
        foreach(var item in missioncards)
        {
            var title = item.mission.Title.ToLower();
            if (title.Contains(search.ToLower())) {
                        bool check = cards.Any(x => x.mission.MissionId == item.mission.MissionId);
                if (check == false) {
                    cards.Add(item);
                }
            }

        }
    }
    #endregion

    #region Default
    if (cityId.Count == 0 && countryId.Count == 0 && themeId.Count == 0 && skillId.Count == 0 && search == null) {
        foreach(var item in missioncards)
        {
            cards.Add(item);
        }

    }
    #endregion

    #region Sort - by
    if (sort != null) {
        if (sort == 1) {
            cards = cards.OrderByDescending(x => x.mission.CreatedAt).ToList();
        }
        if (sort == 2) {
            cards = cards.OrderBy(x => x.mission.CreatedAt).ToList();
        }
    }
    #endregion

    return cards;
}