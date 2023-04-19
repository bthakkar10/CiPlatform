using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class AdminApproval : IAdminApproval
    {
        private readonly CiDbContext _db;
        public AdminApproval(CiDbContext db)
        {
            _db = db;
        }

       
        public List<MissionApplication> MissionApplicationList()
        {
            return _db.MissionApplications.Where(missionapplication=> missionapplication.DeletedAt == null && missionapplication.ApprovalStatus == "PENDING").Include(missionapplication=> missionapplication.Mission).Include(missionapplication => missionapplication.User).ToList();    
        }

        public string ApproveDeclineApplication(long MissionApplicationId, long Status)
        {
            try
            {
                MissionApplication missionApplication = _db.MissionApplications.Find(MissionApplicationId)!;
                if (missionApplication == null)
                {
                    return "error";
                }
                else
                {
                    if(Status == 1)
                    {
                        missionApplication.ApprovalStatus = "APPROVE";
                        missionApplication.UpdatedAt= DateTime.Now; 
                        _db.MissionApplications.Update(missionApplication);
                        _db.SaveChanges();
                        return missionApplication.ApprovalStatus;
                    }
                    else
                    {
                        missionApplication.ApprovalStatus = "DECLINE";
                        missionApplication.UpdatedAt = DateTime.Now;
                        _db.MissionApplications.Update(missionApplication);
                        _db.SaveChanges();
                        return missionApplication.ApprovalStatus;
                    }
                }
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }


        public List<Story> StoryList()
        {
            return _db.Stories.Where(story=>story.DeletedAt == null && story.Status == "PENDING").Include(story=>story.Mission).Include(story=>story.User).ToList();
        }
    }
}
