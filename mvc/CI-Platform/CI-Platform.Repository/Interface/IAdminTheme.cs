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

        public string ThemeAdd(AdminThemeViewModel themevm);

        public string ThemeDelete(long ThemeId);

        public AdminThemeViewModel GetThemeData(long ThemeId);

        public string EditTheme(AdminThemeViewModel themevm);


    }
}
