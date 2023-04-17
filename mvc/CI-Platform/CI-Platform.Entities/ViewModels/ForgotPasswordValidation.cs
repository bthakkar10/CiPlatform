using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class ForgotPasswordValidation
    {
        [Required(ErrorMessage = "Email is required!!")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address!!")]
        public string Email { get; set; } 
    }
}
