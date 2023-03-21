using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IMissionDetail
    {
        public Mission MissionDetails(long MissionId);

        public List<Comment> GetApprovedComments(long MissionId);

        public List<MissionApplication> GetRecentVolunteers(long MissionId, long userId);

        public List<Mission> GetRelatedMissions(long MissionId);

        public List<User> UserList(long UserId, long MissionId);

        public Task SendInvitationToCoWorker(long ToUserId, long FromUserId, MissionDetailViewModel viewmodel);

    }
}
