using CI_Platform.Entities.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public  class MissionListModel
    {
        public List<Country> Country { get; set; } = null;

        public List<City> City { get; set; }

        public List<MissionTheme> Theme { get; set; }

        public List<Skill> Skill { get; set; }

        public List<MissionApplication> Application { get; set; }


        public List<MissionRating> MissionRatings { get; set; }
        

        public List<GoalMission> goalMissions { get; set; }

        //public List<Mission> MissionList { get; set; }

        public List<Mission> DisplayMissionCardsDemo { get; set; }


    }
}
