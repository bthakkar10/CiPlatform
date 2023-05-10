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
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using PublicResXFileCodeGenerator;
using CI_Platform.Repository.Generic;
//using System.Web.Mvc;

namespace CI_Platform_web.Controllers
{
    [Authorize(Roles = "user")]

    public class HomeController : Controller
    {
        long UserId = 0;
        private readonly ILogger<HomeController> _logger;
        private readonly IFilter _filterMission;
        private readonly IMissionDisplay _missionDisplay;
        private readonly IMissionDetail _missionDetail;
        private readonly CiDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserNotifications _userNotifications;
 
        public HomeController(ILogger<HomeController> logger,  IMissionDisplay missionDisplay, CiDbContext db, IFilter filterMission, IMissionDetail missionDetail, IHttpContextAccessor httpContextAccessor, IUserNotifications userNotifications)
        {
            
            _logger = logger;
            _filterMission = filterMission;
            _missionDisplay = missionDisplay;
            _missionDetail = missionDetail;
            _db = db;
            _userNotifications = userNotifications; 
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


        //get method for homepage
       
        public IActionResult HomePage()
        {
            var vm = new PageListViewModel();

            vm.Country = _filterMission.CountryList();
            vm.Theme = _filterMission.ThemeList();
            vm.Skill = _filterMission.SkillList();


            return View(vm);

        }

        

        [HttpPost]
        public IActionResult HomePage(MissionFilterQueryParams queryParams)
        {
            var vm = _missionDisplay.FilterOnMission(queryParams, UserId);

            if (vm.Records.Count != 0)
            {
                return PartialView("_MissionDisplayPartial", vm);
            }
            else
            {
                return PartialView("_NoMissionFound");
            }
        }


        //add to favourites
        [HttpPost]
        public IActionResult AddToFavorites(int missionId)
        {
            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);

            _missionDisplay.AddToFavourites(userId, missionId);
            
            return Ok();
           
        }

        //get cities based on country filter
        public IActionResult GetCitiesByCountry(int countryId)
        {
            PageListViewModel vm = new();
            vm.City = _filterMission.CityList(countryId);
            return Json(vm.City);
        }

        //get all the notifications of a user 
        public IActionResult GetNotification()
        {
            NotificationViewModel vm = new()
            {
                userNotifications = _userNotifications.GetNotificationList(UserId),
                UserNotificationSetting = _userNotifications.userSettings(UserId),
                count = _userNotifications.GetNotificationList(UserId).Count(),
            };
            return PartialView("_NotificationSection", vm);
        }

        public int CountNotification()
        {
           int count = _userNotifications.GetNotificationList(UserId).Count();
           return count;
        }

        //notification clear all
        public IActionResult ClearNotification()
        {
            _userNotifications.ClearNotification(UserId);
            return Ok();
        }

        public IActionResult ChangeNotificationStatus(long NotificationId)
        {
            _userNotifications.ChangeNotificationStatus(NotificationId);
            return Ok();
        }

        public IActionResult NotificationSettings(long[] settingsArr)
        {
            if(_userNotifications.NotificationSettings(settingsArr, UserId))
            {
                return Ok(new { icon = "success", message = "Notification Settings " + Messages.Update });
            }
            else
            {
                return Ok(new { icon = "success", message = Messages.Error });
            }

        }

        //get method for mission details page
        [AllowAnonymous]
        public IActionResult MissionDetail(int MissionId)
        {
            try
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    string returnUrl = Url.Action("MissionDetail", "Home", new { MissionId = MissionId })!;
                    return RedirectToAction("Index", "Auth", new { returnUrl });
                }
                if (HttpContext.Session.GetString("Id") != null)
                {
                    ViewBag.UserId = HttpContext.Session.GetString("Id");
                  
                    ViewBag.Username = HttpContext.Session.GetString("Username");
                }
                ViewBag.MissionId = MissionId;
                long userId = Convert.ToInt64(HttpContext.Session.GetString("Id"));
                  
                MissionDetailViewModel vm = new()
                {
                    MissionDetails = _missionDetail.MissionDetails(MissionId),
                    ApprovedComments = _missionDetail.GetApprovedComments(MissionId),
                    RecentVolunteers = _missionDetail.GetRecentVolunteers(MissionId, userId),
                    RelatedMissions = _missionDetail.GetRelatedMissions(MissionId, userId),
                    UserList = _missionDetail.UserList(userId, MissionId),
                    totalVolunteers = _missionDetail.GetRecentVolunteers(MissionId, userId).Count()
                };

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

            var Updatedrating =  _missionDisplay.Ratings(rating, missionId, UserId);
            return Json(Updatedrating);
        }

        //to get the updated ratings 
        [HttpGet]
        public IActionResult GetMissionRating(long missionId)
        {
            var data = _missionDisplay.GetMissionRating(missionId);

            return Json(data);
        }

        //post comments in mission details
        [HttpPost]
        public IActionResult PostComment(string comment, long missionId)
        {
            string Id = HttpContext.Session.GetString("Id");
            long userId = long.Parse(Id);

            if(_missionDisplay.AddComment(comment, missionId, userId))
            {
                return Ok(new { icon = "success", message = "Comment  " + Messages.Add});
            }
            else
            {
                return Ok(new { icon = "error", message = "Please Apply First!!" });
            }
        }

        public IActionResult GetComments(long missionId) 
        {
            MissionDetailViewModel vm = new()
            {
                ApprovedComments = _missionDetail.GetApprovedComments(missionId),
            };
            return PartialView("_CommentsPartial", vm);
        }

        //necessary for displaying user list in recommended to co worker on grid and list view
        [HttpGet]
        public IActionResult UserList(long MissionId)
        {
            string Id = HttpContext.Session.GetString("Id");
            long userId = long.Parse(Id);
            var coworkers= _db.Users.Where(u => u.UserId != userId && u.DeletedAt==null && u.Status==true && u.Role==GenericEnum.Role.user.ToString() && !u.MissionApplications.Any(m => m.MissionId == MissionId && m.ApprovalStatus == "APPROVE")).ToList();
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
            UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == missionInvite.FromUserId && u.SettingId == (long)GenericEnum.notification.Recommended_Mission).FirstOrDefault();
            UserNotification userNotification = new()
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
                RecommendedMissionId = MissionId,
                UserSettingId = userSettingId.UserSettingId
            };
            _db.UserNotifications.Add(userNotification);
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

            if(_missionDisplay.ApplyToMission(UserId, MissionId))
            {
                return Ok(new { icon = "success", message = "Applied successfully!!"});
              
            }
            else
            {
                return Ok(new { icon = "error", message = Messages.Error });
            }
        }

        public bool CheckSession()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
