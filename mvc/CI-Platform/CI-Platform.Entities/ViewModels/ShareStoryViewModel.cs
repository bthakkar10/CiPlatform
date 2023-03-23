using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public  class ShareStoryViewModel
    {
        public DateTime? date;

        public List<MissionApplication> GetMissionListofUser { get; set; } = null;
        public string? MissionTitle { get; set; }
        public string? StoryTitle { get; set; }
        public string? StoryDescription { get; set; }
    }
}
