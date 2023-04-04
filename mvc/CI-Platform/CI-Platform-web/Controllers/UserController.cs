using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfile _userProfile;
        private readonly IFilter _filter;
        public UserController(IUserProfile userProfile, IFilter filter)
        {
            _userProfile = userProfile;
            _filter= filter;    
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

                UserProfileViewModel vm =_userProfile.GetUserDetails(userId);
                return View(vm);
            }
            catch(Exception ex)
            {
                return View(ex);
            }
        }

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
                return Ok(new {icon = "success", message= "Password updated successfully!!"});
            }
            else
            {
                 return Ok(new { icon = "error", message = "New Password and Old Password cannot be same!!" });

            }
        }
    }
}
