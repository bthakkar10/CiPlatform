using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform_web.Controllers
{
    public class StoryController : Controller
    {
        private readonly CiDbContext _db;
        private readonly IFilter _filterMission;
        private readonly IStoryListing _storyListing;
        public StoryController(IFilter filterMission, IStoryListing storyListing, CiDbContext db)
        {
            _db = db;
            _filterMission = filterMission;
            _storyListing = storyListing;
        }

        public IActionResult StoryListing()
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);

            var vm = new StoryListingViewModel();

            vm.Country = _filterMission.CountryList();
            vm.Theme = _filterMission.ThemeList();
            vm.Skill = _filterMission.SkillList();
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> StoryListing(string searchText, int? countryId, string? cityId, string? themeId, string? skillId)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            try
            {
                var response = _db.Stories.FromSql($"exec spFilterStory @searchText={searchText}, @countryId={countryId}, @cityId={cityId}, @themeId={themeId}, @skillId={skillId}");

                var items = await response.ToListAsync();

                var StoryId = items.Select(m => m.StoryId).ToList();
                var vm = new StoryListingViewModel();

                vm.DisplayStoryCard = _storyListing.DisplayStoryCard(StoryId).ToList();

                if (vm != null && vm.DisplayStoryCard.Any())
                {
                    return PartialView("_StoryListingPartial", vm);
                }
                else
                {
                    return PartialView("_NoMissionFound");
                }

            }
            catch (Exception ex)
            {
                return View(ex);

            }
            //return Ok(new { StatusCode = 200, PartialView = PartialView("_MissionDisplayPartial", vm) });

            //return Ok(); _MissionDisplayPartial
        }

        public IActionResult GetCitiesByCountry(int countryId)
        {
            var vm = new StoryListingViewModel();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
        }
    }
}
