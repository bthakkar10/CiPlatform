using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class TimeViewModel
    {
        public long TimesheetId { get; set; } = 0;

        [Required(ErrorMessage = "Select Mission To add Data!!")]
        public long MissionId { get; set; }

        [Required(ErrorMessage = "Date Volunteered is required")]
        public DateTime TimeDate { get; set; }


        [Required(ErrorMessage = "Please enter the hours.")]
        [Range(0, 23, ErrorMessage = "Enter value between 0 and 23!")]
        public int TimeHours { get; set; }

        [Required(ErrorMessage = "Please enter the minutes.")]
        [Range(0, 59, ErrorMessage = "Enter value between 0 and 59!")]
        public int TimeMinutes { get; set; }



        public TimeSpan Time => new TimeSpan(TimeHours, TimeMinutes, 0);


        public string? TimeMessage { get; set; }

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
                TimeMessage = "";
                return true;
            }
        }

        public List<Timesheet> GetTimesheetData { get; set; } = new List<Timesheet>();  
        public string? TimesheetErrorMessage { get; set; }

        public bool IsTimesheetPresent()
        {
            if (GetTimesheetData != null)
            {
                foreach (var ts in GetTimesheetData)
                {
                    if (ts.DateVolunteered == TimeDate)
                    {
                        TimesheetErrorMessage = "You already have entered timesheet for this date!!";
                        return true;
                    }
                }

            }
            TimesheetErrorMessage = " ";
            return false;

        }
    }
}
