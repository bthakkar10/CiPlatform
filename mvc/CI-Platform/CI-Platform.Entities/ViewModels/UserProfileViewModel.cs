using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class UserProfileViewModel
    {
        public User? GetUserDetails { get; set; } 
        public string? FirstName { get; set; }

        public string? Surname { get; set; }

        public string? EmployeeId  { get; set;}

        public string? Manager { get; set;}

        public string? Title { get; set; }

        public string? Department { get; set; }

        public string? ProfileText { get; set; }

        public string? WhyIVolunteer { get; set; }

        public string? LinkedInUrl { get; set; }

        public long? CountryId { get; set;}

        public long? CityId { get; set; }

        public string? Avaliability { get; set; }

        public List<UserSkill>? UserSkills { get; set; }

        public List<Skill>? Skills { get; set; }

        public List<Country>? Countries { get; set; }

        public List<City>? Cities { get; set; }  

        public string? Avatar { get; set; }

    }
}
