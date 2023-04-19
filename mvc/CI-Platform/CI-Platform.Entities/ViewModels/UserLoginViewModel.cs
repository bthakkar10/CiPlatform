using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class UserLoginViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter only valid email addresss!!")]
        [Required(ErrorMessage = "Email Address is Required!!")]
        public string Email { get; set;} = string.Empty;

        [Required(ErrorMessage = "Password is Required!!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "The password must contain at least 8 characters including at least one uppercase letter, one lowercase letter, one digit and one special character!!")]
        public string Password { get; set; } = string.Empty;

        //public UserLoginViewModel(User user)
        //{
        //    Email= user.Email;
        //    UserName = user.FirstName + " " + user.LastName;    
        //}

        //public string? FirstName { get; set; } 

        //public string? LastName { get; set; }

        //public string UserName { get; set; }   

        //public string Role { get; set; } = string.Empty;
    }
}
