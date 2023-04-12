using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public  class MissionFilterQueryParams
    {
        public string CountryId { get; set; } = string.Empty;

        public string CityIds { get; set; } = string.Empty;

        public string ThemeIds { get; set; } = string.Empty;

        public string SkillIds { get; set; } = string.Empty;

        public string SearchText { get; set; } = string.Empty;

        public int sortCase { get; set; }

        public int pageNo { get; set; } = 1;

        public int pagesize { get; set; }

    }
}
