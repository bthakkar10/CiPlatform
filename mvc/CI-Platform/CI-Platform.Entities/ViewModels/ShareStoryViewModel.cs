using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
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
        public List<MissionApplication>? GetMissionListofUser { get; set; } = null;

        [Required]
        public string? MissionTitle { get; set; } = null;

        [Required] 
        public string? StoryTitle { get; set; } = null;

        [Required] 
        public string? StoryDescription { get; set; } = null; 

        [Required]
        public DateTime? Date { get; set; } = null;

        public string[]? VideoUrls { get; set; } = null;

        public List<IFormFile>? Images { get; set; } = null;

        public long MissionId { get; set; }

    }
}
