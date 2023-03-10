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
            return _db.Countries.ToList();
        }

        public List<City> CityList(int CountryId)
        {
            return _db.Cities.Where(c => c.CountryId == CountryId).ToList();
        }
        public List<Skill> SkillList()
        {
            return _db.Skills.ToList();
        }

        public List<MissionTheme> ThemeList()
        {
            return _db.MissionThemes.ToList();
        }

       
    }
}
