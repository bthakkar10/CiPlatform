using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class FilterModel
    {
        public long CountryId { get; set; }

        public string Name { get; set; } = null!;

        public long CityId { get; set; }

        public string? CityName { get; set; } = null!;

        public long SkillId { get; set; }

        public string? SkillName { get; set; } = null!;

        public long MissionThemeId { get; set; }

        public string? Title { get; set; } = null!; 

    }
}
