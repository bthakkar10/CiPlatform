using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;

namespace CI_Platform_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFilter _filterMission;
        private readonly IMissionDisplay _missionDisplay;
        private readonly IMissionDetail _missionDetail;
        private readonly CiDbContext _db;
 
        public HomeController(ILogger<HomeController> logger,  IMissionDisplay missionDisplay, CiDbContext db, IFilter filterMission, IMissionDetail missionDetail)
        {
            _logger = logger;
            _filterMission = filterMission;
            _missionDisplay = missionDisplay;
            _missionDetail = missionDetail;
            _db = db;

        }


        //get method for homepage
        public IActionResult HomePage()
        
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }

            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);

            var vm = new MissionListViewModel();

            vm.Country = _filterMission.CountryList();
            vm.Theme = _filterMission.ThemeList();
            vm.Skill = _filterMission.SkillList();
     

            return View(vm);

        }

        //post method for homepage
        [HttpPost]
        public async Task<IActionResult> HomePage(string searchText, int? countryId, string? cityId, string? themeId, string? skillId, int? sortCase, int? userId, int? pageNo, int? pagesize)
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
                    SqlCommand command = new SqlCommand("spFilterSortSearchPagination", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@countryId", SqlDbType.VarChar).Value = countryId != null ? countryId : null;
                    command.Parameters.Add("@cityId", SqlDbType.VarChar).Value = cityId != null ? cityId : null;
                    command.Parameters.Add("@themeId", SqlDbType.VarChar).Value = themeId != null ? themeId : null;
                    command.Parameters.Add("@skillId", SqlDbType.VarChar).Value = skillId != null ? skillId : null;
                    command.Parameters.Add("@searchText", SqlDbType.VarChar).Value = searchText;
                    command.Parameters.Add("@sortCase", SqlDbType.VarChar).Value = sortCase;
                    command.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId;
                    command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pagesize;
                    command.Parameters.Add("@pageNo", SqlDbType.Int).Value = pageNo;
                    SqlDataReader reader = command.ExecuteReader();

                    List<long> MissionIds = new List<long>();
                    while (reader.Read())
                    {
                        long totalRecords = reader.GetInt32("TotalRecords");
                        ViewBag.totalRecords = totalRecords;
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        long missionId = reader.GetInt64("mission_id");
                        MissionIds.Add(missionId);
                    }


                    var vm = new MissionListViewModel();

                     //userId = Convert.ToInt64(ViewBag.UserId);
                    vm.DisplayMissionCardsDemo = _missionDisplay.DisplayMissionCardsDemo(MissionIds);

                    if (vm != null && vm.DisplayMissionCardsDemo.Any())
                    {
                        return PartialView("_MissionDisplayPartial", vm);
                    }
                    else
                    {
                        return PartialView("_NoMissionFound");
                    }
                }
            }
            catch(Exception ex)
            {
                return View(ex);

            }
   
        }

        
        //add to favourites
        [HttpPost]
        public IActionResult AddToFavorites(int missionId)
        {
            //string Id = HttpContext.Session.GetString("Id");
            //long userId = long.Parse(Id);

            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);


            // Check if the mission is already in favorites for the user
            if (_db.FavouriteMissions.Any(fm => fm.MissionId == missionId && fm.UserId == userId))
            {
                // Mission is already in favorites, return an error message or redirect back to the mission page
                var FavouriteMissionId = _db.FavouriteMissions.Where(fm => fm.MissionId == missionId && fm.UserId == userId).FirstOrDefault();
                _db.FavouriteMissions.Remove(FavouriteMissionId);
                _db.SaveChanges();
                return Ok();

                //return BadRequest("Mission is already in favorites.");
            }

            // Add the mission to favorites for the user
            var favoriteMission = new FavouriteMission { MissionId = missionId, UserId = userId };
            _db.FavouriteMissions.Add(favoriteMission);
            _db.SaveChanges();

            return Ok();
        }

        //get cities based on country filter
        public IActionResult GetCitiesByCountry(int countryId)
        {
            var vm = new MissionListViewModel();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
        }
        
        //get method for mission details page
        public IActionResult MissionDetail(int MissionId)
        {
            try
            {
                if (HttpContext.Session.GetString("Id") != null)
                {
                    ViewBag.UserId = HttpContext.Session.GetString("Id");
                }
                ViewBag.MissionId = MissionId;
                long userId = Convert.ToInt64(HttpContext.Session.GetString("Id"));
                //long userId = long.Parse(Id);
                var vm = new MissionDetailViewModel();
                vm.MissionDetails = _missionDetail.MissionDetails(MissionId);
                vm.ApprovedComments = _missionDetail.GetApprovedComments(MissionId);
                vm.RecentVolunteers = _missionDetail.GetRecentVolunteers(MissionId, userId);
                vm.RelatedMissions = _missionDetail.GetRelatedMissions(MissionId);
                vm.UserList = _missionDetail.UserList(userId, MissionId);
                vm.totalVolunteers = _missionDetail.GetRecentVolunteers(MissionId, userId).Count();
                return View(vm);
            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }
        //RecentVolunteers fetch pagination
        [HttpGet]
        public IActionResult RecentVolunteers(long missionId, int pageNo)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("Id"));

            int pageSize = 3;
            int pageNumber = pageNo;
            var query = _missionDetail.GetRecentVolunteers(missionId, userId).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToList();
            MissionDetailViewModel vm = new MissionDetailViewModel()
            {
                RecentVolunteers = query

            };
            return PartialView("_RecentVolunteers", vm);
        }

        //ratings
        [HttpPost]
        public IActionResult Rating(byte rating, int missionId)
        {
            
            var userId = HttpContext.Session.GetString("Id");
            long UserId = Convert.ToInt64(userId);


            var alredyRated = _db.MissionRatings.SingleOrDefault(mr => mr.MissionId == missionId && mr.UserId == UserId);

            if (alredyRated != null)
            {
                alredyRated.Rating = rating;
                _db.SaveChanges();
            }
            else
            {
                var newRating = new MissionRating { UserId = UserId, MissionId = missionId, Rating = rating };
                _db.MissionRatings.Add(newRating);
                _db.SaveChanges();
            }

            return Json(rating);
        }

        //post comments in mission details
        [HttpPost]
        public IActionResult PostComment(string comment, long missionId)
        {
            string Id = HttpContext.Session.GetString("Id");
            long userId = long.Parse(Id);

            //var userId = HttpContext.Session.GetString("Id");
            //long UserId = Convert.ToInt64(userId);

            if (comment != null)
            {
                var newComment = new Comment { UserId = userId, MissionId = missionId, CommentText = comment };
                _db.Comments.Add(newComment);
                _db.SaveChanges();
            }

            return Ok();
        }

        //necessary for displaying user list in recommended to co worker on grid and list view
        [HttpGet]
        public IActionResult UserList(long MissionId)
        {
            string Id = HttpContext.Session.GetString("Id");
            long userId = long.Parse(Id);
            var coworkers= _db.Users.Where(u => u.UserId != userId && !u.MissionApplications.Any(m => m.MissionId == MissionId && m.ApprovalStatus == "APPROVE")).ToList();
            return Json(coworkers);  
        }

        //for mission invite(recommended to coworker)
        [HttpPost]
        public async Task<IActionResult> MissionInvite(long ToUserId, long MissionId, long FromUserId, MissionDetailViewModel viewmodel)
        {
            var missionInvite = new MissionInvite()
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
                MissionId = MissionId,
            };

            _db.MissionInvites.Add(missionInvite);

            var MissionLink = Url.Action("MissionDetail", "Home", new { MissionId = MissionId }, Request.Scheme);
            viewmodel.link = MissionLink;

            await _db.SaveChangesAsync();

            await _missionDetail.SendInvitationToCoWorker(ToUserId, FromUserId, viewmodel);

            return Json(new { success = true });
        }

        //for documents in mission details page
        public IActionResult DisplayDocument(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot/documents", fileName);

            var fileStream = new FileStream(filePath, FileMode.Open);

            return File(fileStream, "application/pdf");
        }

        public IActionResult ApplyMission(long MissionId)
        {
            var userId = HttpContext.Session.GetString("Id");
            long UserId = Convert.ToInt64(userId);

            var AlreadyApplied = _db.MissionApplications.FirstOrDefault(m=>m.MissionId== MissionId && m.UserId == UserId);
            if(AlreadyApplied == null)
            {
                var missionApplication = new MissionApplication()
                {
                    MissionId= MissionId,
                    UserId = UserId,
                    AppliedAt= DateTime.Now,
                    CreatedAt= DateTime.Now,
                    ApprovalStatus = "PENDING"
                };
                _db.Add(missionApplication);
                _db.SaveChanges();
            }

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
