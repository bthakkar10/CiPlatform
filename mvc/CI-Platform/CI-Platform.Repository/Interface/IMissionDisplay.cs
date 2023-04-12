using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IMissionDisplay
    {
       
        public IEnumerable<Mission> DisplayMissionCardsDemo(List<long> MissionIds);

        public PageListViewModel.PageList<MissionListViewModel> FilterOnMission(MissionFilterQueryParams queryParams, long UserId);



    }
}
