using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryListing
    {
        public List<Story> DisplayStoryCard(List<long> StoryIds);
    }
}
