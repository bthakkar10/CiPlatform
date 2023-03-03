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
        public IEnumerable<Mission> DisplayMission()
        {
            IEnumerable<Mission> MissionList = _db.Missions.Include(m => m.City).Include(m => m.Country).Include(m => m.MissionSkills).Include(m => m.MissionTheme).Include(m => m.MissionRatings).Include(m => m.GoalMissions);
            return MissionList;
        }
    }
}
