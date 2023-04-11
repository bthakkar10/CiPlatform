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
    public class UserProfileViewModel
    {
        public User? GetUserDetails { get; set; }

        [Required (ErrorMessage = "First Name is Required!!" )]
        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Surname is Required!!")]
        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string? Surname { get; set; }

        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string? EmployeeId  { get; set;}

        [MaxLength(255, ErrorMessage = "Only 255 characters are allowed!!")]
        public string? Manager { get; set;}

        [MaxLength(255, ErrorMessage = "Only 255 characters are allowed!!")]
        public string? Title { get; set; }

        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string? Department { get; set; }

        public string? ProfileText { get; set; }

        public string? WhyIVolunteer { get; set; }

        [RegularExpression(@"^https:\/\/(www\.)?linkedin\.com\/in\/[a-zA-Z0-9-]+$", ErrorMessage = "Please enter a valid LinkedIn URL.")]
        public string? LinkedInUrl { get; set; }

        
        public long? CountryId { get; set;}

        
        public long? CityId { get; set; }


        public string? Availibility { get; set; }

        public List<UserSkill>? UserSkills { get; set; }

        public List<Skill>? Skills { get; set; }

        public string? UpdatedUserSkills { get; set; }
        [Required(ErrorMessage = "Country is Required!!")]
        public List<Country>? Countries { get; set; }

        [Required(ErrorMessage = "City is Required!!")]
        public List<City>? Cities { get; set; }  

        public string? Avatar { get; set; }

        public IFormFile? UpdatedAvatar { get; set; }   
    }
}
