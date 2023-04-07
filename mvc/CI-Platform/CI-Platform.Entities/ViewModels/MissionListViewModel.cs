﻿using CI_Platform.Entities.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public  class MissionListViewModel
    {
        public List<Country> Country { get; set; } = null;

        public List<City> City { get; set; } = null;

        public List<MissionTheme> Theme { get; set; } = null;

        public List<Skill> Skill { get; set; } = null;

        public List<MissionApplication> Application { get; set; } = null;

        public List<MissionRating> MissionRatings { get; set; } = null;

        public List<GoalMission> GoalMissions { get; set; } = null;

        public IEnumerable<Mission> DisplayMissionCardsDemo { get; set; } = null;

        public List<User> UserList { get; set; } = null;

        public string? link { get; set; }


    }
}
