﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Generic;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PublicResXFileCodeGenerator;
using System.Data;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace CI_Platform_web.Controllers
{
    [Authorize(Roles = "user")]
    public class StoryController : Controller
    {
        long UserId = 0;
        private readonly CiDbContext _db;
        private readonly IFilter _filterMission;
        private readonly IStoryListing _storyListing;
        private readonly IShareStory _shareStory;
        private readonly IStoryDetails _storyDetails;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoryController(CiDbContext db, IFilter filterMission, IStoryListing storyListing, IShareStory shareStory, IStoryDetails storyDetails, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _filterMission = filterMission;
            _storyListing = storyListing;
            _shareStory = shareStory;
            _storyDetails = storyDetails;
            _httpContextAccessor = httpContextAccessor;
            string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = authorizationHeader?.Substring("Bearer ".Length).Trim();
            if (token is not null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);
                var claims = decodedToken.Claims;
                var customClaimString = decodedToken.Claims.FirstOrDefault(c => c.Type == "CustomClaimForUser")?.Value;
                var customClaimValue = JsonSerializer.Deserialize<User>(customClaimString);
                UserId = customClaimValue.UserId;
            }
        }

        //get method for story listing
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

        //post method for story listing sp
        [HttpPost]
        public async Task<IActionResult> StoryListing(MissionFilterQueryParams queryParams)
        {
           
            var UserId = Convert.ToInt64(ViewBag.UserId);
            StoryListingViewModel vm = _storyListing.SPStory(queryParams);
           
            //vm.DisplayStoryCard = _storyListing.DisplayStoryCard(StoryIds);

            if (vm != null)
            {
                return PartialView("_StoryListingPartial", vm);
            }
            else
            {
                return PartialView("_NoStoryFound");
            }
        }





        //get cities by country filter
        public IActionResult GetCitiesByCountry(int countryId)
        {
            var vm = new StoryListingViewModel();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
        }

        //get method for share your story page to fetch mission list where user has registered
        public IActionResult ShareStory(long userId)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
      
            ShareStoryViewModel vm = new()
            {
                GetMissionListofUser = _shareStory.GetMissionListofUser(userId)
            };
            return View(vm);
        }

        //get method for drafted story in share story page
        public IActionResult GetDraftedStory(long missionId)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var Id = Convert.ToInt64(ViewBag.UserId);
            var userId = (long)Id;

            Story DraftStory = _shareStory.GetDraftedStory(userId, missionId);
            if (DraftStory != null)
            {
                return Json(DraftStory);
            }
            else if (_shareStory.isPublishedStory(userId, missionId) == true)
            {
                return Ok(new { icon = "error", message = "Volunteers can publish only one story per mission!!" });
            }
            else
            {
                List<DateTime> dates = _shareStory.GetMissionDates(missionId, userId);
                return Json(dates);
            }


            //_shareStory.GetMissionDates(missionId, userId);

        }

        [HttpPost]
        //to save story in database in draft mode only 
        public IActionResult SaveStory(ShareStoryViewModel viewmodel)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var Id = Convert.ToInt64(ViewBag.UserId);
            var userId = (long)Id;

            Story DraftStory = _shareStory.GetDraftedStory(userId, viewmodel.MissionId);
            if (DraftStory == null)
            {
                if (_shareStory.isPublishedStory(userId, viewmodel.MissionId) == true)
                {

                    return Ok(new { icon = "error", message = "Volunteers can publish only one story per mission!!" });
                }
                else
                {
                    if(_shareStory.AddNewStory(viewmodel, userId))
                    {
                        return Ok(new { icon = "success", message = "New Story is added successfully in draft mode!!" });
                    }
                    else
                    {
                        return Ok(new { icon = "error", message = Messages.Error });
                    }
                   
                }
            }
            else
            {
                _shareStory.EditDraftedStory(viewmodel, userId);
                return Ok(new { icon = "success", message = "Story is edited successfully and saved in draft mode!!" });
            }
        }

        [HttpPost]
        //to submit story(now the story is in pending mode and will be published when admin approves it)
        public IActionResult SubmitStory(ShareStoryViewModel viewmodel)
        {
            if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("SEmail");
                ViewBag.UserId = HttpContext.Session.GetString("Id");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var Id = Convert.ToInt64(ViewBag.UserId);
            var userId = (long)Id;

            _shareStory.SubmitStory(viewmodel, userId);
            return Ok(new { icon = "success", message = "Story is submitted successfully and will be published when admin approves!!" });
        }

        [AllowAnonymous]
        public IActionResult StoryDetails(long MissionId, long UserId)
        {
            try
            {
                if (HttpContext.Session.GetString("Id") == null )
                {
                    string returnUrl = Url.Action("StoryDetails", "Story", new { MissionId = MissionId, UserId = UserId })!;
                    return RedirectToAction("Index", "Auth", new { returnUrl });
                }
                if (HttpContext.Session.GetString("Id") != null)
                {
                    ViewBag.UserId = HttpContext.Session.GetString("Id");
                }
                long userId = Convert.ToInt64(HttpContext.Session.GetString("Id"));

                StoryDetailsViewModel vm = new();

                vm.GetStoryDetails = _storyDetails.GetStoryDetails(MissionId, UserId);
                vm.UserList = _storyDetails.UserList(userId);
                _storyDetails.IncreaseViewCount(UserId, MissionId);
                return View(vm);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        //for story invite(recommended to coworker)
        [HttpPost]
        public async Task<IActionResult> StoryInvite(long ToUserId, long StoryId, long FromUserId, long MissionId, long SPUserId, StoryDetailsViewModel viewmodel)
        {
            StoryInvite storyInvite = new()
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
                StoryId = StoryId,
            };

            _db.StoryInvites.Add(storyInvite);
            UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == storyInvite.FromUserId && u.SettingId == (long)GenericEnum.notification.Recommended_Story).FirstOrDefault();
            UserNotification userNotifications = new()
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
                RecommendedStoryId = StoryId,
                UserSettingId = userSettingId.UserSettingId
            };
            _db.UserNotifications.Add(userNotifications);

            var StoryInviteLink = Url.Action("StoryDetails", "Story", new { MissionId = MissionId, UserId = SPUserId }, Request.Scheme);
            viewmodel.InviteLink = StoryInviteLink;

            await _db.SaveChangesAsync();

            await _storyDetails.SendInvitationToCoWorker(ToUserId, FromUserId, viewmodel);

            return Json(new { success = true });
        }



        public bool CheckSession()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

    }
}
