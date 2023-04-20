using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminUserViewModel
    {

        public List<User> users = new List<User>();

        public User GetUserData { get; set; } = new User();

        public AdminUserViewModel() { }

        public long UserId { get; set; } = 0;

        public AdminUserViewModel(User GetUserData)
        {
            UserId = GetUserData.UserId;    
            FirstName = GetUserData.FirstName;
            Surname = GetUserData.LastName;
            Email = GetUserData.Email;
            PhoneNumber = GetUserData.PhoneNumber;
            EmployeeId = GetUserData.EmployeeId!;
            Department = GetUserData.Department!;
            Password = GetUserData.Password;
            Availibility = GetUserData.Availability!;
            CityId = GetUserData.CityId;   
            CountryId= GetUserData.CountryId;
            Password = GetUserData.Password;
            Status = GetUserData.Status;
            Role = GetUserData.Role;
        }

        [Required(ErrorMessage = "First Name is Required!!")]
        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surname is Required!!")]
        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string Surname { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter only valid email addresss!!")]
        [Required(ErrorMessage = "Email Address is Required!!")]
        public string? Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is Required!!")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "The phone number must be 10 digits and start with 6, 7, 8, or 9.")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string EmployeeId { get; set; } = string.Empty;

        [MaxLength(16, ErrorMessage = "Only 16 characters are allowed!!")]
        public string  Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required!!")]
        //[DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "The password must contain at least 8 characters including at least one uppercase letter, one lowercase letter, one digit and one special character!!")]
        public string Password { get; set; } = string.Empty;


        public string Availibility { get; set; } = string.Empty;

        public List<Country>? Countries { get; set; }

        public List<City>? Cities { get; set; }

        [Required(ErrorMessage = "Country is Required!!")]
        public long? CountryId { get; set; }

        [Required(ErrorMessage = "City is Required!!")]
        public long? CityId { get; set; }

        public bool? Status { get; set; } 

        public string? Role { get; set;  }
    }
}
