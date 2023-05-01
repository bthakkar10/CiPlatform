using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class GoalViewModel
    {

        public long TimesheetId { get; set; } = 0;

        [Required(ErrorMessage = "Select Mission To add Data!!")]
        public long MissionId { get; set; }

        [Required(ErrorMessage = "Action is required!!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only digits are allowed")]
        public int Action { get; set; } 

        [Required(ErrorMessage = "Date Volunteered is required")]
        public DateTime TimeDate { get; set; }

        public string? TimeErrorMessage { get; set; }
        public bool IsValid()
        {

            var today = DateTime.Today;

            if (TimeDate.Date > today)
            {
                TimeErrorMessage = "The time entry date cannot be in the future.";
                return false;
            }
            else
            {
                TimeErrorMessage = "";
                return true;
            }
        }

        public string? GoalMessage { get; set; }
    }
}
