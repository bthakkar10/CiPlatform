using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoryListingViewModel
    {
        public List<Country> Country { get; set; } = null;

        public List<City> City { get; set; } = null;

        public List<MissionTheme> Theme { get; set; } = null;

        public List<Skill> Skill { get; set; } = null;

        public List<Story> DisplayStoryCard { get; set; } = null; 

        public string? Avtaar { get; set; } = string.Empty;
    }
}
