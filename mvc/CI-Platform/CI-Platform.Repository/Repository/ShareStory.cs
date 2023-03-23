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
   
    public class ShareStory : IShareStory
    {
        private readonly CiDbContext _db;

        public ShareStory(CiDbContext db)
        {
            _db = db;
        }

        public List<MissionApplication> GetMissionListofUser(long userId)
        {
            return _db.MissionApplications.Where(m => m.UserId == userId && m.ApprovalStatus == "APPROVE").Include(m => m.Mission).ToList();
        }
    }
}
