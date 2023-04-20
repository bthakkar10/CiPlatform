using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public  class AdminMission : IAdminMission
    {
        private readonly CiDbContext _db;
        public AdminMission(CiDbContext db)
        {
            _db = db;    
        }
        public List<Mission> MissionList()
        {
            return _db.Missions.Where(mission => mission.DeletedAt == null).ToList();
        }

    }
}
