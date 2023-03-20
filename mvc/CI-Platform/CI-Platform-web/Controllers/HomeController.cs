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
using System.Diagnostics;

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



        public IActionResult HomePage()
        
        {
            if (HttpContext.Session.GetString("SEmail") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
            }
            if (HttpContext.Session.GetString("Id") != null)
            {
                ViewBag.UserId = HttpContext.Session.GetString("Id");
            }
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }


            var vm = new MissionListModel();

            vm.Country = _filterMission.CountryList();
            vm.Theme = _filterMission.ThemeList();
            vm.Skill = _filterMission.SkillList();
            //vm.MissionList = _missionDisplay.DisplayMission();

             return View(vm);

        }



       

        [HttpPost]
        public async Task<IActionResult> HomePage(string searchText, int? countryId, string? cityId, string? themeId, string? skillId, int? sortCase, int? userId)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            //try { 
                var response = _db.Missions.FromSql($"exec spFilterSortSearchPagination @searchText={searchText}, @countryId={countryId}, @cityId={cityId}, @themeId={themeId}, @skillId={skillId}, @sortCase = {sortCase}, @userId = {userId}");

                var items = await response.ToListAsync();

                var MissionIds = items.Select(m => m.MissionId).ToList();

                var vm = new MissionListModel();

                vm.DisplayMissionCardsDemo = _missionDisplay.DisplayMissionCardsDemo(MissionIds).OrderBy(ml => MissionIds.IndexOf(ml.MissionId)).ToList();

                if (vm != null && vm.DisplayMissionCardsDemo.Any())
                {
                    return PartialView("_MissionDisplayPartial", vm);
                }
                else
                {
                    return PartialView("_NoMissionFound");
                }

            //}
            //catch(Exception ex)
            //{
            //    return View(ex);

            //}
            //return Ok(new { StatusCode = 200, PartialView = PartialView("_MissionDisplayPartial", vm) });
            
            //return Ok(); _MissionDisplayPartial
        }

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

        public IActionResult GetCitiesByCountry(int countryId)
        {
            var vm = new MissionListModel();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
        }

        public IActionResult MissionDetail(int MissionId)
        {
            //try
            //{
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
                vm.UserList = _missionDetail.UserList(userId);
                return View(vm);
            //}
            //catch (Exception ex)
            //{
            //    return View("Error");
            //}

        }


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

        //[HttpPost]
        //public IActionResult PostComment(string comment, long missionId)
        //{
        //    string Id = HttpContext.Session.GetString("UserId");
        //    long userId = long.Parse(Id);

        //    if (comment != null)
        //    {
        //        var newComment = new Comment { UserId = userId, MissionId = missionId, CommentText = comment };
        //        _db.Comments.Add(newComment);
        //        _db.SaveChanges();
        //    }

        //    return Json(missionId);
        //}

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
            await _db.SaveChangesAsync();

            var MissionLink = Url.Action("MissionDetail", "Home", new { MissionId = MissionId }, Request.Scheme);
            viewmodel.link = MissionLink;

            await _missionDetail.SendInvitationToCoWorker(ToUserId, FromUserId, viewmodel);

            return Json(new { success = true });
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
