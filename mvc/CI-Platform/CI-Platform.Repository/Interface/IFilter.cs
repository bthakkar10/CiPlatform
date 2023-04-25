using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IFilter
    {
       public List<Country> CountryList();

        public List<City> CityList(int CountryId);

        public List<MissionTheme> ThemeList();

        public List<Skill> SkillList();

        public List<City> AllCityList();

        public List<MissionSkill> MissionSkillList();
    }
}
