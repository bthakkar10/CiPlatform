using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public  interface IAdminTheme
    {
        public List<MissionTheme> ThemeList();

        public bool ThemeAdd(AdminThemeViewModel themevm);

        public bool ThemeDelete(long ThemeId);

        public AdminThemeViewModel GetThemeData(long ThemeId);

        public bool EditTheme(AdminThemeViewModel themevm);


    }
}
