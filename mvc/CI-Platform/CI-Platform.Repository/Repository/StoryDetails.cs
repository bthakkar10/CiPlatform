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
    public class StoryDetails : IStoryDetails
    {
        private readonly CiDbContext _db;

        public StoryDetails(CiDbContext db)
        {
            _db = db;
        }

        public Story GetStoryDetails(long MissionId,long UserId)
        {
            return _db.Stories.
            Include(s => s.StoryMedia).
            Include(s => s.StoryInvites).
            Include(s => s.User).
            ThenInclude(su => su.City).
            ThenInclude(su => su.Country).
            Where(s => s.MissionId == MissionId && s.UserId == UserId).
            FirstOrDefault();
        }

        public List<User> UserList(long UserId)
        {
            return _db.Users.Where(u => u.UserId != UserId).ToList();
            //return _db.Users.Where(u => u.UserId != UserId && !u.MissionApplications.Any(m => m.MissionId == MissionId && m.ApprovalStatus == "APPROVE")).ToList();
        }

        public void IncreaseViewCount(long UserId, long MissionId)
        {
            Story story = _db.Stories.Where(s => s.MissionId == MissionId && s.UserId == UserId && s.Status == "PUBLISHED").FirstOrDefault();
            if (story != null)
            {
                story.UserVisits = story.UserVisits + 1;
                story.UpdatedAt = DateTime.Now;
                _db.Update(story);
                _db.SaveChanges();
            }
        }
    }
}
