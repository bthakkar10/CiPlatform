using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Generic;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class VolunteeringTimesheet : IVolunteeringTimesheet
    {
        private readonly CiDbContext _db;

        public VolunteeringTimesheet(CiDbContext db)
        {
            _db = db;
        }

        public List<MissionApplication> GetMissionTitles(long UserId)
        {   
            //return mission whose user id matches, who has applied, mission is not deleted and mission is ongoing 
            return _db.MissionApplications.Where(ms => ms.UserId == UserId && ms.ApprovalStatus == GenericEnum.ApplicationStatus.APPROVE.ToString() && ms.DeletedAt == null && ms.Mission.DeletedAt == null && (ms.Mission.StartDate<DateTime.Now && ms.Mission.EndDate>DateTime.Now))
                .Include(m => m.Mission).ToList();
        }
     
        public List<Timesheet> GetTimesheetData(long UserId)
        {
            return _db.Timesheets.Where(ms => ms.UserId == UserId && ms.DeletedAt == null && ms.User.DeletedAt == null && ms.Mission.DeletedAt == null).Include(m => m.Mission).ToList();
        }

        public string AddTimeBasedEntry(TimeViewModel vm, long UserId)
        {
            try
            {
                List<Timesheet> timesheet = _db.Timesheets.Where(t => t.UserId == UserId && t.MissionId == vm.MissionId && t.DeletedAt == null).ToList();
                foreach (var ts in timesheet)
                {
                    if (ts.DateVolunteered != vm.TimeDate)
                    {
                        return "Exists";
                    }
                }
                Timesheet TimeTs = new()
                {
                    MissionId = vm.MissionId,
                    UserId = UserId,
                    Time = vm.Time,
                    DateVolunteered = vm.TimeDate,
                    Notes = vm.TimeMessage,
                    CreatedAt = DateTime.Now,

                };
                _db.Add(TimeTs);
                _db.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AddGoalBasedEntry(GoalViewModel vm, long UserId)
        {
            try
            {
                List<Timesheet> timesheet = _db.Timesheets.Where(t => t.UserId == UserId && t.MissionId == vm.MissionId && t.DeletedAt == null).ToList();
                foreach (var ts in timesheet)
                {
                    if (ts.DateVolunteered == vm.TimeDate)
                    {
                        return "Exists";
                    }
                }
                Timesheet TimeTs = new()
                {
                    MissionId = vm.MissionId,
                    UserId = UserId,
                    Action = vm.Action,
                    DateVolunteered = vm.TimeDate,
                    Notes = vm.GoalMessage,
                    CreatedAt = DateTime.Now,

                };
                _db.Add(TimeTs);
                _db.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Timesheet GetDataOnEdit(long TimeSheetId)
        {
            return _db.Timesheets.Include(m => m.Mission).Where(m => m.DeletedAt == null && m.Mission.DeletedAt == null).FirstOrDefault(t => t.TimesheetId == TimeSheetId)!;
        }

        public bool UpdateGoalBasedEntry(GoalViewModel vm)
        {
            Timesheet ts = GetDataOnEdit(vm.TimesheetId);
            if (ts != null)
            {
                ts.Action = vm.Action;
                ts.Notes = vm.GoalMessage;
                ts.DateVolunteered = vm.TimeDate;
                ts.UpdatedAt = DateTime.Now;

                _db.Update(ts);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateTimeBasedEntry(TimeViewModel vm)
        {
            Timesheet ts = GetDataOnEdit(vm.TimesheetId);
            if (ts != null)
            {
                ts.Time = vm.Time;
                ts.Notes = vm.TimeMessage;
                ts.DateVolunteered = vm.TimeDate;
                ts.UpdatedAt = DateTime.Now;

                _db.Update(ts);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteTimeBasedEntry(long TimeSheetId)
        {
            try
            {
                Timesheet ts = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == TimeSheetId && t.DeletedAt == null)!;
                if (ts != null)
                {
                    ts.DeletedAt = DateTime.Now;
                    _db.Update(ts);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGoalBasedEntry(long TimeSheetId)
        {
            try
            {
                Timesheet ts = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == TimeSheetId && t.DeletedAt == null)!;
                if (ts != null)
                {
                    ts.DeletedAt = DateTime.Now;
                    _db.Update(ts);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
