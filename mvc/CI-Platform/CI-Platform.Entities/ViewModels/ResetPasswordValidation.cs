using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class ResetPasswordValidation
    {
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
