using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First Name is Required!!")]
        [MaxLength(16, ErrorMessage = "Maximum 16 characters are allowed!!")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is Required!!")]
        [MaxLength(16, ErrorMessage = "Maximum 16 characters are allowed!!")]
        public string LastName { get; set;} = string.Empty;

        [Required(ErrorMessage = "Mobile number is Required!!")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "The phone number must be 10 digits and start with 6, 7, 8, or 9.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter only valid email addresss!!")]
        [Required(ErrorMessage = "Email Address is Required!!")]

        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is Required!!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "The password must contain at least 8 characters including at least one uppercase letter, one lowercase letter, one digit and one special character!!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is Required!!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password does not match!!")]
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}
