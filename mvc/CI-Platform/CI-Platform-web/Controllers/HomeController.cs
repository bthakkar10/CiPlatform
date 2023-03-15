﻿using CI_Platform.Entities.DataModels;
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
        private readonly CiDbContext _db;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IFilter filterMission, IMissionDisplay missionDisplay, CiDbContext db, IConfiguration config)
        {
            _logger = logger;
            _filterMission = filterMission;   
            _missionDisplay = missionDisplay;
            _db = db;
            _config = config;
        }



        public IActionResult HomePage()
        {
            if (HttpContext.Session.GetString("SEmail") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");

            }
            if (HttpContext.Session.GetString("Id") != null)
            {
                ViewBag.Id = HttpContext.Session.GetString("Id");
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
        public async Task<IActionResult> HomePage(string searchText, int? countryId, string? cityId, string? themeId, string? skillId, int? sortCase, string? userId)
        {
            var response = _db.Missions.FromSql($"exec spFilterSortSearchPagination @searchText={searchText}, @countryId={countryId}, @cityId={cityId}, @themeId={themeId}, @skillId={skillId}, @sortCase = {sortCase}, @userId = {userId}");

            var items = await response.ToListAsync();

            var MissionIds = items.Select(m => m.MissionId).ToList();

            var vm = new MissionListModel();

            vm.DisplayMissionCardsDemo = _missionDisplay.DisplayMissionCardsDemo(MissionIds).OrderBy(ml => MissionIds.IndexOf(ml.MissionId)).ToList();

 
            return PartialView("_MissionDisplayPartial", vm);
        }

        [HttpPost]
        public IActionResult AddToFavorites(int missionId)
        {
            string Id = HttpContext.Session.GetString("Id");
            long userId = long.Parse(Id);

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

        public IActionResult AddToFavourites(int missionId)
        {
            return View();
        }


        

        //[HttpPost]
        //public IActionResult AddToFavourites()
        //{
        //    // insert the favourite mission into the database
        //    var sql = "INSERT INTO FavouriteMissions (UserId, MissionName, MissionDescription) VALUES (@UserId, @MissionName, @MissionDescription)";
        //    var parameters = new { UserId = favouriteMissionData.UserId, MissionName = favouriteMissionData.MissionName, MissionDescription = favouriteMissionData.MissionDescription };
        //    var affectedRows = _db.Execute(sql, parameters);
        //    if (affectedRows > 0)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to add mission to favourites");
        //    }
        //}

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
