using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminUser _adminUser;

        public AdminController(IAdminUser adminUser)
        {
            _adminUser= adminUser;  
        }

        [HttpGet]
        public IActionResult User()
        {
            try
            {
                var vm = new AdminUserViewModel();
                vm.users = _adminUser.users();
                return View(vm);

            }
            catch (Exception ex) 
            { 
                  return View(ex);
            }
        }

        public IActionResult DeleteUser(long UserId) 
        { 
            if(UserId > 0) 
            {
                if(_adminUser.IsDeleted(UserId))
                {
                    TempData["succes"] = "User Deleted Successfully!!";
                    return RedirectToAction("User");
                }
                else
                {
                    TempData["error"] = "Something went wrong!! Please try again later!!";
                    return RedirectToAction("User");
                }
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("User");
            }
        }

        
    }
}
