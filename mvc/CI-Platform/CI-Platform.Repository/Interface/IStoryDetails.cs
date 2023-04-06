using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryDetails
    {
        public Story GetStoryDetails(long MissionId, long UserId);

        public List<User> UserList(long UserId);

        public void StoryInvite(long ToUserId, long StoryId, long FromUserId, StoryDetailsViewModel vm);

        public void IncreaseViewCount(long UserId, long MissionId);
    }
}
