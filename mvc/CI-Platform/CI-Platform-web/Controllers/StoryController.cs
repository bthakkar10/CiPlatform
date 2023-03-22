using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Printing;

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
        public async Task<IActionResult> StoryListing(string? countryId, string? cityId, string? themeId, string? skillId, string searchText, int? pageNo, int? pagesize)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            try
            {
                IConfigurationRoot _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("spFilterStory", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@countryId", SqlDbType.VarChar).Value = countryId != null ? countryId : null;
                    command.Parameters.Add("@cityId", SqlDbType.VarChar).Value = cityId != null ? cityId : null;
                    command.Parameters.Add("@themeId", SqlDbType.VarChar).Value = themeId != null ? themeId : null;
                    command.Parameters.Add("@skillId", SqlDbType.VarChar).Value = skillId != null ? skillId : null;
                    command.Parameters.Add("@searchText", SqlDbType.VarChar).Value = searchText;
                    
                    command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pagesize;
                    command.Parameters.Add("@pageNo", SqlDbType.Int).Value = pageNo;
                    SqlDataReader reader = command.ExecuteReader();

                    List<long> StoryIds = new List<long>();
                    while (reader.Read())
                    {
                        long totalRecords = reader.GetInt32("TotalRecords");
                        ViewBag.totalRecords = totalRecords;
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        long storyId = reader.GetInt64("story_id");
                        StoryIds.Add(storyId);
                    }


                    var vm = new StoryListingViewModel();

                    var UserId = Convert.ToInt64(ViewBag.UserId);
                    vm.DisplayStoryCard = _storyListing.DisplayStoryCard(StoryIds);
                   
                    if (vm != null && vm.DisplayStoryCard.Any())
                    {
                        return PartialView("_StoryListingPartial", vm);
                    }
                    else
                    {
                        return PartialView("_NoMissionFound");
                    }
                }




            }
            catch (Exception ex)
            {
                return View(ex);

            }
          
        }

        public IActionResult GetCitiesByCountry(int countryId)
        {
            var vm = new StoryListingViewModel();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
        }
    }
}
