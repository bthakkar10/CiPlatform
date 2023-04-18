using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Mvc;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminUser _adminUser;
        private readonly IFilter _filter;
        private readonly IAdminCms _adminCms;
        private readonly IAdminTheme _adminTheme;
        private readonly IAdminSkills _adminSkills;

        public AdminController(IAdminUser adminUser, IFilter filter, IAdminCms adminCms, IAdminTheme adminTheme, IAdminSkills adminSkills)
        {
            _adminUser = adminUser;
            _filter = filter;
            _adminCms = adminCms;
            _adminTheme = adminTheme;
            _adminSkills = adminSkills;
        }

        //------------------------------------------  User  Section Starts ---------------------------------------------------------- 

        [HttpGet]
        public IActionResult User()
        {
            try
            {
                AdminUserViewModel vm = new AdminUserViewModel();
                vm.users = _adminUser.users();
                return View(vm);

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
        [HttpPost]
        public IActionResult DeleteUser()
        {
            long UserId = long.TryParse(Request.Form["HiddenUserId"], out long result) ? result : 0;
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


        //fetching user data on edit 
        public IActionResult GetUserData(long UserId)
        {
            AdminUserViewModel vm = _adminUser.GetDataOnEdit(UserId);
            vm.Countries = _filter.CountryList();
            vm.Cities = _filter.AllCityList();
            return PartialView("_AddNewUser", vm);
        }

        [HttpGet]
        public IActionResult AddNewUser()
        {
            AdminUserViewModel vm = new AdminUserViewModel();
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
        //------------------------------------------  User  Section Ends ---------------------------------------------------------- 

        //------------------------------------------  CMS  Section Starts ---------------------------------------------------------- 

        //cms page
        public IActionResult CmsPage()
        {
            try
            {
                AdminCmsViewModel cmsvm = new AdminCmsViewModel();
                cmsvm.CmsList = _adminCms.CmsList();
                return View(cmsvm);

            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }

        //fetching privacy policy page data on edit button click 
        public IActionResult GetCmsData(long CmsId)
        {
            AdminCmsViewModel cmsvm = _adminCms.GetCmsData(CmsId);
            return PartialView("_AddOrUpdateCms", cmsvm);
        }

        //add or update cms form get method to load the add and edit forms 
        [HttpGet]
        public IActionResult AddOrUpdatePrivacyPages()
        {
            AdminCmsViewModel vm = new AdminCmsViewModel();
            return PartialView("_AddOrUpdateCms", vm);
        }

        //add or update privacy pages
        [HttpPost]
        //[Microsoft.AspNetCore.Mvc.DisableRequestValidation]
        [System.Web.Mvc.ValidateInput(false)]
        public IActionResult AddOrUpdatePrivacyPages(AdminCmsViewModel cmsvm)
        {
            //to add
            if (cmsvm.CmsId == 0)
            {
                if (_adminCms.CmsAdd(cmsvm))
                {
                    TempData["success"] = "Privacy Page Added Successfully!!";
                    return RedirectToAction("CmsPage");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("CmsPage");
                }
            }
            // to Update 
            else
            {
                if (_adminCms.EditCms(cmsvm))
                {
                    TempData["success"] = "Privacy Policy Updated Successfully!!";
                    return RedirectToAction("CmsPage");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("CmsPage");
                }
            }
        }
        [HttpPost]
        public IActionResult DeleteCmsPage()
        {
            long CmsId = long.TryParse(Request.Form["HiddenCmsId"], out long result) ? result : 0;
            if (_adminCms.CmsDelete(CmsId))
            {
                TempData["succes"] = "Privacy Page  Deleted Successfully!!";
                return RedirectToAction("CmsPage");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("CmsPage");
            }
        }
        //------------------------------------------  Cms  Section Ends ---------------------------------------------------------- 

        //------------------------------------------ Mission Theme Section starts ---------------------------------------------------------- 

        //Mission theme page
        public IActionResult MissionTheme()
        {
            try
            {
                AdminThemeViewModel themevm = new AdminThemeViewModel();
                themevm.ThemeList = _adminTheme.ThemeList();
                return View(themevm);

            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }

        //fetching mission theme data on edit button click 
        public IActionResult GetThemeData(long ThemeId)
        {
            AdminThemeViewModel themevm = _adminTheme.GetThemeData(ThemeId);
            return PartialView("_AddOrUpdateTheme", themevm);
        }

        //add or update theme form get method to load the add and edit forms 
        [HttpGet]
        public IActionResult AddOrUpdateTheme()
        {
            AdminThemeViewModel themevm = new AdminThemeViewModel();
            return PartialView("_AddOrUpdateTheme", themevm);
        }

        //add or update theme
        [HttpPost]
        public IActionResult AddOrUpdateTheme(AdminThemeViewModel themevm)
        {
            //to add
            if (themevm.ThemeId == 0)
            {
                if (_adminTheme.ThemeAdd(themevm))
                {
                    TempData["success"] = "New Theme Added Successfully!!";
                    return RedirectToAction("MissionTheme");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("MissionTheme");
                }
            }
            // to Update 
            else
            {
                if (_adminTheme.EditTheme(themevm))
                {
                    TempData["success"] = "Theme Updated Successfully!!";
                    return RedirectToAction("MissionTheme");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("MissionTheme");
                }
            }
        }
        [HttpPost]
        public IActionResult DeleteTheme()
        {
            long ThemeId = long.TryParse(Request.Form["HiddenThemeId"], out long result) ? result : 0;
            if (_adminTheme.ThemeDelete(ThemeId))
            {
                TempData["succes"] = "Theme Deleted Successfully!!";
                return RedirectToAction("MissionTheme");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("MissionTheme");
            }
        }
        //------------------------------------------  Mission Theme Section Ends ---------------------------------------------------------- 

        //------------------------------------------ Mission Skills Section starts ---------------------------------------------------------- 

        //Mission skill page
        public IActionResult MissionSKill()
        {
            try
            {
                AdminSkillsViewModel skillvm = new AdminSkillsViewModel();
                skillvm.SkillList = _adminSkills.SkillList();
                return View(skillvm);

            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }

        //fetching mission skill data on edit button click 
        public IActionResult GetSkillData(long SkillId)
        {
            AdminSkillsViewModel skillvm = _adminSkills.GetSkills(SkillId);
            return PartialView("_AddOrUpdateSkill", skillvm);
        }

        //add or update skil form get method to load the add and edit forms 
        [HttpGet]
        public IActionResult AddOrUpdateSkill()
        {
            AdminSkillsViewModel skillvm = new AdminSkillsViewModel();
            return PartialView("_AddOrUpdateSkill", skillvm);
        }

        //add or update skill
        [HttpPost]
        public IActionResult AddOrUpdateSkill(AdminSkillsViewModel skillvm)
        {
            //to add
            if (skillvm.SkillId == 0)
            {
                if (_adminSkills.SkillsAdd(skillvm))
                {
                    TempData["success"] = "New Skill Added Successfully!!";
                    return RedirectToAction("MissionSKill");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("MissionSKill");
                }
            }
            // to Update 
            else
            {
                if (_adminSkills.EditSkill(skillvm))
                {
                    TempData["success"] = "Skill Updated Successfully!!";
                    return RedirectToAction("MissionSKill");
                }
                else
                {
                    TempData["error"] = "Something went wrong!!";
                    return RedirectToAction("MissionSKill");
                }
            }
        }

        public IActionResult DeleteSkill()
        {
            long SkillId = long.TryParse(Request.Form["HiddenSkillId"], out long result) ? result : 0;
            if (_adminSkills.SkillDelete(SkillId))
            {
                TempData["succes"] = "Skill Deleted Successfully!!";
                return RedirectToAction("MissionSKill");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("MissionSKill");
            }
        }
        //------------------------------------------  Mission Theme Section Ends ---------------------------------------------------------- 


    }

}

