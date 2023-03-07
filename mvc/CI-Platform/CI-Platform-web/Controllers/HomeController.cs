using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
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
            

            var vm = new MissionListModel();

            vm.Country = _db.Countries.ToList();
            vm.Theme = _db.MissionThemes.ToList();
            vm.Skill = _db.Skills.ToList();
            vm.MissionList = _missionDisplay.DisplayMission();

            //var Skill = _db.Skills.ToList();
            //var skillall = new SelectList(Skill, "SkillId", "SkillName");
            return View(vm);

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
