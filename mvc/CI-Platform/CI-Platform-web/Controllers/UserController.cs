using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfile _userProfile;
        public UserController(IUserProfile userProfile)
        {
            _userProfile = userProfile;
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
    }
}
