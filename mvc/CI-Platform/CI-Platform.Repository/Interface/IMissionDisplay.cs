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
       
        //public IEnumerable<Mission> DisplayMissionCardsDemo(List<long> MissionIds);

        public PageListViewModel.PageList<MissionListViewModel> FilterOnMission(MissionFilterQueryParams queryParams, long UserId);

        public string AddToFavourites(long MissionId, long UserId);

        public byte Ratings(byte rating, long MissionId, long UserId);

        public (int rating, int VolunteersRated) GetMissionRating(long missionId);

        public bool AddComment(string comment, long MissionId, long UserId);

        public bool ApplyToMission(long MissionId, long UserId);

    }
}
