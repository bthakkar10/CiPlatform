using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IVolunteeringTimesheet
    {
        public List<MissionApplication> GetMissionTitles(long UserId);

        public string AddTimeBasedEntry(TimeViewModel vm, long UserId);

        public string AddGoalBasedEntry(GoalViewModel vm, long UserId);

        public Timesheet GetDataOnEdit(long TimeSheetId);

        public List<Timesheet> GetTimesheetData(long UserId);

        public string UpdateGoalBasedEntry(GoalViewModel vm, long UserId);

        public string UpdateTimeBasedEntry(TimeViewModel vm, long UserId);

        public bool DeleteTimeBasedEntry(long TimeSheetId);

        public bool DeleteGoalBasedEntry(long TimeSheetId);



    }
}
