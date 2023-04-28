using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CI_Platform_web.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        
        private readonly IUserProfile _userProfile;
        private readonly IFilter _filter;
        public UserController(IUserProfile userProfile, IFilter filter)
        {
            _userProfile = userProfile;
            _filter = filter;
        }
        public IActionResult UserProfile()
        {
            try
            {
                if (HttpContext.Session.GetString("Id") != null)
                {
                    ViewBag.UserId = HttpContext.Session.GetString("Id");
                }
                var UserId = HttpContext.Session.GetString("Id");
                long userId = Convert.ToInt64(UserId);
                ViewBag.UserId = userId;
                UserProfileViewModel vm = _userProfile.GetUserDetails(userId);
                return View(vm);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
        [AllowAnonymous]
        public IActionResult GetCitiesByCountry(int CountryId)
        {
            var City = _filter.CityList(CountryId);
            return Json(City);
        }

        //change password for user modal
        [HttpPost]
        public IActionResult ChangePassword(string oldPass, string newPass)
        {
            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);

            //var userId = long.TryParse(HttpContext.Session.GetString("userId"), out var result) ? result : 0;
            bool isPasswordChanged = _userProfile.ChangePassword(userId, oldPass, newPass);
            if (isPasswordChanged)
            {
                return Ok(new { icon = "success", message = "Password updated successfully!!" });
            }
            else
            {
                return Ok(new { icon = "error", message = "Old Password is incorrect!!" });
            }
        }

        [HttpPost]
        public IActionResult EditUserProfile(UserProfileViewModel vm)
        {
            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);
            

            if (_userProfile.EditUserProfile(userId, vm) == "Updated")
            {
                TempData["success"] = "Your Profile is Updated Successfully!!";
                return RedirectToAction("UserProfile");
            }
            else if(_userProfile.EditUserProfile(userId, vm) == "Exists")
            {
                TempData["error"] = "User with same EmploeeId Already Exists!!";
                return RedirectToAction("UserProfile");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("UserProfile");
            }
        }

        [HttpPost]
        public IActionResult ContactUs(string ContactSubject, string ContactMessage)
        {

            var UserId = HttpContext.Session.GetString("Id");
            long userId = Convert.ToInt64(UserId);

            //var userId = long.TryParse(HttpContext.Session.GetString("userId"), out var result) ? result : 0;
            bool isMessageSent = _userProfile.ContactUs(userId, ContactSubject, ContactMessage);
            if (isMessageSent)
            {
                return Ok(new { icon = "success", message = "Your message is sent successfully!!" });
            }
            else
            {
                return Ok(new { icon = "error", message = "Some error occured!! Please try again later!!" });
            }
        }
    }
}

