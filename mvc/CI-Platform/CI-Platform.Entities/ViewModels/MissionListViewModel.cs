using CI_Platform.Entities.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{

    
    public  class MissionListViewModel
    {
      
        public Mission MissionCard { get; set; }

        public string? CityName { get; set; }

        public string? ThemeTitle { get; set; }

        public bool Applied { get; set; }

        public float? avgRating { get; set; }

        public DateTime? startDate { get; set; }

        public DateTime? createdAt { get; set; }

        public int? seatsLeft { get; set; }

        public List<string> missionMedia { get; set; } = new List<string>();

        public List<GoalMission> goalMission { get; set; } = new List<GoalMission>();

        public List<MissionInvite> missionInvites { get; set; } = new List<MissionInvite>();        

        public bool favMission { get; set; }

        public bool IsOngoing { get; set; } 

        public bool HasEndDatePassed { get; set; }  

        public bool HasMissionStarted { get; set; } 

        public bool HasDeadlinePassed { get; set; } 

        public List<string> skillName { get; set; } = new List<string>();

        public List<FavouriteMission> favouriteMission { get; set; } = new List<FavouriteMission>();

     
        //public string? link { get; set; }


    }
}
