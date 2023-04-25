using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class Filter : IFilter
    {
        private readonly CiDbContext _db;
        public Filter(CiDbContext db)
        {
            _db = db;
        }

        public List<Country> CountryList()
        {
            return _db.Countries.Where(country=> country.DeletedAt == null).ToList();
        }

        public List<City> CityList(int CountryId)
        {
            return _db.Cities.Where(c => c.CountryId == CountryId && c.DeletedAt == null).ToList();
        }
        public List<Skill> SkillList()
        {
            return _db.Skills.Where(skills => skills.DeletedAt == null).ToList();
        }

        public List<MissionTheme> ThemeList()
        {
            return _db.MissionThemes.Where(theme => theme.DeletedAt == null).ToList();
        }
        
        public List<City> AllCityList()
        {
            return _db.Cities.Where(city => city.DeletedAt == null).ToList();
        }

        public List<MissionSkill> MissionSkillList()
        {
            return _db.MissionSkills.Where(skills => skills.DeletedAt == null).ToList();  
        }

    }
}
