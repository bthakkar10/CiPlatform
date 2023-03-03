using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionDisplay
    {
        public long MissionId { get; set; }

        public long MissionThemeId { get; set; }

        public string? ThemeName{ get; set; }

        public long SkillId { get; set; }

        public string? SkillName { get; set; }

        public long CityId { get; set; }

        public string? CityName { get; set; }

        public long CountryId { get; set; }

        public string Name { get; set; }

        public string MissionTitle { get; set; }

        public string? ShortDescription { get; set; }

        public string? OrganizationName { get; set; }

        public byte Rating { get; set; }

        public string MissionType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int GoalValue { get; set; }

    }
}
