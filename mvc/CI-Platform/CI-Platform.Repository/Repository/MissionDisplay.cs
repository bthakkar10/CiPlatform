using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class MissionDisplay : IMissionDisplay
    {
        private readonly CiDbContext _db;
        public MissionDisplay(CiDbContext db)
        {
            _db = db;
        }
        public List<Mission> DisplayMission()
        {
            List<Mission> MissionList = _db.Missions
                .Include(m => m.City)
                .Include(m => m.Country)
                .Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill)
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionRatings)
                .Include(m => m.GoalMissions)
                .Include(m => m.MissionApplications)
                .Include(m => m.MissionMedia).ToList();
            return MissionList;
        }
    }
}
