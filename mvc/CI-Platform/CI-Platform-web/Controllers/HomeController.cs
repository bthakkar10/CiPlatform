using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
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
        private readonly IFilter _filterMission;
        private readonly IMissionDisplay _missionDisplay;

        public HomeController(ILogger<HomeController> logger, IFilter filterMission, IMissionDisplay missionDisplay)
        {
            _logger = logger;
            _filterMission = filterMission;   
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

            vm.Country = _filterMission.CountryList();
            vm.Theme = _filterMission.ThemeList();
            vm.Skill = _filterMission.SkillList();
            vm.MissionList = _missionDisplay.DisplayMission();

            //var Skill = _db.Skills.ToList();
            //var skillall = new SelectList(Skill, "SkillId", "SkillName");
            return View(vm);

        }



        public IActionResult GetCitiesByCountry(int countryId)
        {
            var vm = new MissionListModel();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
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
