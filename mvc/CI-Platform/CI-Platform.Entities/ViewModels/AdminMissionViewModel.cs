using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionViewModel
    {
        public List<Mission> GetMissionList { get; set; }  =  new List<Mission>();  
        
        public string? MissionTitle { get; set; }   = string.Empty; 

        public string? ShortDescription { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

       
        [Required(ErrorMessage = "Country is Required!!")]
        public long? CountryId { get; set; }

        [Required(ErrorMessage = "City is Required!!")]
        public long? CityId { get; set; }

        public string? MissionType { get; set; } = string.Empty;

        public string? Deadline { get; set; } = string.Empty;

        public int? GoalValue { get; set; } 

        public string? GoalObjectiveText { get; set; } = string.Empty;

        public string? OrganizationName { get; set; } = string.Empty;

        public string? OrganizationDetail { get; set; } = string.Empty;     

        public DateTime? StartDate { get; set; }    

        public DateTime? EndDate { get; set;}

        public int ? TotalSeats { get; set; } = 0;

        public string? Avaliablity { get; set; } = string.Empty; 

       public List<string> YoutubeUrl { get; set; } = new List<string>();   

        public long? ThemeId { get; set; }

        public List<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();

        

    }
}
