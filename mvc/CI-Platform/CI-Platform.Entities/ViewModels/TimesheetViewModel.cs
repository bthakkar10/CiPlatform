using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class TimesheetViewModel
    {
        public List<MissionApplication> GetMissionTitles { get; set; } = null;

        public TimeViewModel TimeViewModel { get; set; }
        public GoalViewModel GoalViewModel { get; set; }

        public List<Timesheet> GetTimesheetData { get; set; } = null;
       
    }

}
