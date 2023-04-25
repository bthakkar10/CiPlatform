using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
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
        public AdminMissionViewModel() { }

        public List<Mission> GetMissionList { get; set; }  =  new List<Mission>();

        public long MissionId { get; set; } = 0;

        [Required(ErrorMessage= "Mission Title is required")]
        public string MissionTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mission Short Description is required")]
        [MaxLength(255, ErrorMessage = "Maximum 255 characters are allowed!!")]
        public string? ShortDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mission Description is required")]
        [MaxLength(40000, ErrorMessage = "Maximum 40000 characters are allowed!!")]
        public string? Description { get; set; } = string.Empty;

       
        [Required(ErrorMessage = "Country is Required!!")]
        public long CountryId { get; set; }

        [Required(ErrorMessage = "City is Required!!")]
        public long CityId { get; set; }

        [Required(ErrorMessage = "Mission Type is Required!!")]
        public string MissionType { get; set; } = string.Empty;

        public DateTime? Deadline { get; set; }

        public int GoalValue { get; set; }

        public string GoalObjectiveText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Organization Name is Required!!")]
        public string? OrganizationName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Organization Detail is Required!!")]
        public string? OrganizationDetail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is Required!!")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required!!")]
        public DateTime? EndDate { get; set;}

        public int ? TotalSeats { get; set; } = 0;

        [Required(ErrorMessage = "Status is Required!!")]
        public bool? Status { get; set; }

        [Required(ErrorMessage = "Avaliablity is Required!!")]
        public string? Avaliablity { get; set; } = string.Empty;

        [RegularExpression(@"^(https?\:\/\/)?(www\.youtube\.com|youtu\.?be)\/.+$", ErrorMessage = "Only Youtube Urls are allowed!!")]
        public string[]? YoutubeUrl { get; set; } = null!;

        [Required(ErrorMessage ="Theme is Required!!")]
        public long ThemeId { get; set; }

        public string? UpdatedMissionSKills { get; set; } 

        public List<Skill> SkillsList { get; set; } = new List<Skill>();

        public List<Country> CountryList { get; set; } = new List<Country> ();       

        public List<City> CityList { get; set; } = new List<City>();    

        public List<MissionTheme> ThemeList { get; set; } = new List<MissionTheme>();

        public List<MissionSkill> MissionSkills { get; set; }   = new List<MissionSkill>(); 
        
        public List<IFormFile> ImageList { get; set; } = new List<IFormFile>();

        public List<IFormFile> DocumentList { get; set; } = new List<IFormFile>();

        public IFormFile? DefaultMissionImg { get; set; }

        public Mission GetMissionData { get; set; } = new Mission();

        public string[]? MissionUrlLinks { get; set; }

        public string[]? MissionImagesList{ get; set; }

        public string[]? MissionDocumentsList { get; set; }  

        public string? MissionDefaultImage { get; set; } 

    public AdminMissionViewModel(Mission GetMissionData)
        {
            MissionId = GetMissionData.MissionId;
            MissionTitle = GetMissionData.Title;
            OrganizationName = GetMissionData.OrganizationName;
            OrganizationDetail= GetMissionData.OrganizationDetail;  
            Status= GetMissionData.Status;  
            StartDate= GetMissionData.StartDate;    
            EndDate= GetMissionData.EndDate;    
            Deadline= GetMissionData.Deadline;
            TotalSeats= GetMissionData.TotalSeats;  
            Description= GetMissionData.Description;    
            ShortDescription= GetMissionData.ShortDescription;  
            CountryId= GetMissionData.CountryId;    
            CityId= GetMissionData.CityId;
            ThemeId = GetMissionData.MissionThemeId;
            MissionType= GetMissionData.MissionType;
            Avaliablity = GetMissionData.Availability;
            
            //ImageList = GetMissionData.MissionMedia.
            
        }
    }
}
