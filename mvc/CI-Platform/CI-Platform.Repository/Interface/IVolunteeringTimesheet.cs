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

        public bool AddTimeBasedEntry(TimeViewModel vm, long UserId);

    }
}
