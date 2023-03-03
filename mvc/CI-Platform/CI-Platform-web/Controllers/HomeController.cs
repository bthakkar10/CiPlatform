using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Diagnostics;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiDbContext _db;
        private readonly IMissionDisplay _missionDisplay;

        public HomeController(ILogger<HomeController> logger, CiDbContext db, IMissionDisplay missionDisplay)
        {
            _logger = logger;
            _db = db;   
            _missionDisplay = missionDisplay;
        }

        public IActionResult HomePage()
        {
            if (HttpContext.Session.GetString("SEmail") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
            }
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            var country = _db.Countries.ToList();
            var countryall = new SelectList(country, "CountryId", "Name");
            ViewBag.CountryList = countryall;

            var skill = _db.Skills.ToList();
            var skillall = new SelectList(skill, "SkillId", "SkillName");
            ViewBag.SkillList = skillall;

            var theme = _db.MissionThemes.ToList();
            var themeall = new SelectList(theme, "MissionThemeId", "Title");
            ViewBag.ThemeList = themeall;

            IEnumerable<Mission> MissionList = _missionDisplay.DisplayMission();
            ViewBag.MissionList = MissionList;

            return View();

        }



        public IActionResult GetCitiesByCountry(int countryId)
        {
            var cities = _db.Cities.Where(c => c.CountryId == countryId).ToList();
            return Json(cities);
        }

        public IActionResult MissionDetail()
        {
            return View();
        }
        public IActionResult StoryListing()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
