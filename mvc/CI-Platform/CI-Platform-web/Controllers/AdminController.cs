using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminUser _adminUser;
        private readonly IFilter _filter;

        public AdminController(IAdminUser adminUser, IFilter filter)
        {
            _adminUser = adminUser;
            _filter = filter;
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
            if (UserId > 0)
            {
                if (_adminUser.IsUserDeleted(UserId))
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
        [HttpGet]
        public IActionResult AddNewUser()
        {
            var vm = new AdminUserViewModel();
            vm.Countries = _filter.CountryList();
            vm.Cities = _filter.AllCityList();
            return PartialView("_AddNewUser", vm);
        }

        [HttpPost]
        public IActionResult AddOrUpdateUser(AdminUserViewModel vm)
        {
            //to add
            if (vm.UserId == 0)
            {
                if (_adminUser.IsUserAdded(vm) == "Added")
                {
                    TempData["success"] = "User Added Successfully!!";
                    return RedirectToAction("User");
                }
                else if (_adminUser.IsUserAdded(vm) == "Exists")
                {
                    TempData["error"] = "User Already Exists!!";
                    return RedirectToAction("User");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("User");
                }
            }
            // to Update 
            else
            {
                if (_adminUser.EditUser(vm))
                {
                    TempData["success"] = "User Updated Successfully!!";
                    return RedirectToAction("User");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("User");
                }
            }
        }

        //fetching user data on edit 
        public IActionResult GetUserData(long UserId)
        {
            var vm = _adminUser.GetDataOnEdit(UserId);
            vm.Countries = _filter.CountryList();
            vm.Cities = _filter.AllCityList();
            return PartialView("_AddNewUser", vm);
        }
       
        //cms page
        public IActionResult CmsPage()
        {
            return View();  
        }

        //add or update form
        [HttpGet]
        public IActionResult AddOrUpdatePrivacyPages()
        {

            var vm = new AdminCmsViewModel();
            return PartialView("_AddOrUpdateCms", vm);
        }

    }
}
