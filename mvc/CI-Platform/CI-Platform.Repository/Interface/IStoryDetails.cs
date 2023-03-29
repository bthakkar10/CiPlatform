using CI_Platform.Entities.DataModels;
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

        public void IncreaseViewCount(long UserId, long MissionId);
    }
}
