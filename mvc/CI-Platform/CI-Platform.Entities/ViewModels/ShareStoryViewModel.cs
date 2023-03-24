using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public  class ShareStoryViewModel
    {
        public List<MissionApplication> GetMissionListofUser { get; set; } = null;

        public Story GetDraftedStory { get; set; } = null;

        [Required]
        public string? MissionTitle { get; set; }

        [Required]
        public string? StoryTitle { get; set; }

        [Required]
        public string? StoryDescription { get; set; }

        [Required]
        public DateTime? date;

        public string[] VideoUrls { get; set; } 

        public long MissionId { get; set; }

    }
}
