using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public  class MissionFilterQueryParams
    {
        public string CountryId { get; set; } = null!;

        public string CityIds { get; set; } = null!;

        public string ThemeIds { get; set; } = null!;

        public string SkillIds { get; set; } = null!;

        public string SearchText { get; set; } = null!;

        public int sortCase { get; set; }

        public int pageNo { get; set; } = 1;

        public int pagesize { get; set; }

    }
}
