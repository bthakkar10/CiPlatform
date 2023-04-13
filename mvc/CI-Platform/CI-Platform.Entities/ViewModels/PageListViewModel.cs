using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class PageListViewModel
    {

      

        public List<Country> Country { get; set; } = null;

        public List<City> City { get; set; } = null;

        public List<MissionTheme> Theme { get; set; } = null;

        public List<Skill> Skill { get; set; } = null;


        public class PageList<MissionListViewModel>
        {
            public List<MissionListViewModel> Records { get; set; }
            public int TotalCount { get; set; }
            public int CurrentPage { get; set; }



            public PageList(List<MissionListViewModel> records, int totalCount, int page)
            {
                Records = records ?? new List<MissionListViewModel>();
                TotalCount = totalCount;
                CurrentPage = page;
            } 
        }


    }
}
