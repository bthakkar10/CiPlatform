using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return _db.MissionApplications.Where(ms=>ms.UserId == UserId && ms.ApprovalStatus == "APPROVE").Include(m=>m.Mission).ToList();
        }

        public bool AddTimeBasedEntry(TimeViewModel vm, long UserId)
        {
            try
            {
                Timesheet TimeTs = new Timesheet()
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
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


    }
}
