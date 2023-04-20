using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata.Ecma335;
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
        private readonly IAdminApproval _adminApproval;
        private readonly IAdminMission _adminMission;

        public AdminController(IAdminUser adminUser, IFilter filter, IAdminCms adminCms, IAdminTheme adminTheme, IAdminSkills adminSkills, IAdminApproval adminApproval, IAdminMission adminMission)
        {
            _adminUser = adminUser;
            _filter = filter;
            _adminCms = adminCms;
            _adminTheme = adminTheme;
            _adminSkills = adminSkills;
            _adminApproval = adminApproval;
            _adminMission = adminMission;
        }

        //------------------------------------------  User  Section Starts ---------------------------------------------------------- 

        [HttpGet]
        [Authorize(Roles = "admin")]
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
                if (_adminTheme.ThemeAdd(themevm) == "Added")
                {
                    TempData["success"] = "New Theme Added Successfully!!";
                    return RedirectToAction("MissionTheme");
                }
                else if(_adminTheme.ThemeAdd(themevm) == "Exists")
                {
                    TempData["error"] = "Theme Already Exists";
                    return RedirectToAction("MissionTheme");
                }
                else
                {
                    TempData["error"] = "Something went wrong!! Please try again later!!";
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
                if (_adminSkills.SkillsAdd(skillvm) == "Added")
                {
                    TempData["success"] = "New Skill Added Successfully!!";
                    return RedirectToAction("MissionSKill");
                }
                else if(_adminSkills.SkillsAdd(skillvm) == "Exists")
                {
                    TempData["error"] = "Skill Already Exists";
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
        //------------------------------------------  Mission Skill Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Mission Application Section Starts ---------------------------------------------------------- 

        public IActionResult MissionApplication()
        {
            AdminApprovalViewModel vm = new AdminApprovalViewModel();
            vm.MissionApplicationList = _adminApproval.MissionApplicationList();
            return View(vm);
        }

        public IActionResult ApproveOrDeclineApplication()
        {
            long MissionApplicationId = long.TryParse(Request.Form["HiddenApplicationId"], out long result) ? result : 0;
            long Status = long.TryParse(Request.Form["HiddenStatus"], out long resultStatus) ? resultStatus : 0;

            if(_adminApproval.ApproveDeclineApplication(MissionApplicationId, Status) == "APPROVE")
            {
                TempData["success"] = "Mission Application Approved Successfully";
                return RedirectToAction("MissionApplication");

            }
            else if(_adminApproval.ApproveDeclineApplication(MissionApplicationId, Status) == "DECLINE")
            {
                TempData["success"] = "Mission Application Declined Successfully";
                return RedirectToAction("MissionApplication");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("MissionApplication");
            }
        }
        //------------------------------------------  Mission Application Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Story Section Starts ---------------------------------------------------------- 
        public IActionResult Story()
        {
            AdminApprovalViewModel vm = new AdminApprovalViewModel();
            vm.StoryList = _adminApproval.StoryList();
            return View(vm);
        }
        public IActionResult ApproveOrDeclineStory()
        {
            long StoryId = long.TryParse(Request.Form["HiddenStoryId"], out long result) ? result : 0;
            long Status = long.TryParse(Request.Form["HiddenStatus"], out long resultStatus) ? resultStatus : 0;

            if (_adminApproval.ApproveDeclineStory(StoryId, Status) == "PUBLISHED")
            {
                TempData["success"] = "Story Published Successfully";
                return RedirectToAction("Story");

            }
            else if (_adminApproval.ApproveDeclineStory(StoryId, Status) == "DECLINED")
            {
                TempData["success"] = "Story Declined Successfully";
                return RedirectToAction("Story");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("Story");
            }
        }

        public IActionResult DeleteStory()
        {
            long StoryId = long.TryParse(Request.Form["HiddenStoryId"], out long result) ? result : 0;
            if (_adminApproval.IsStoryDeleted(StoryId))
            {
                TempData["succes"] = "Skill Deleted Successfully!!";
                return RedirectToAction("Story");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("Story");
            }
        }

        //------------------------------------------  Story Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Mission Section Starts ---------------------------------------------------------- 

        public IActionResult AdminMission()
        {
            AdminMissionViewModel missionvm = new AdminMissionViewModel();
            missionvm.GetMissionList = _adminMission.MissionList();
            return View(missionvm);
        }

        //add or update mission form get method to load the add and edit forms 
        [HttpGet]
        public IActionResult AddOrUpdateMission()
        {
            AdminMissionViewModel vm = new AdminMissionViewModel();
            return PartialView("_AddOrUpdateMission", vm);
        }

        //------------------------------------------  Mission Section Ends ---------------------------------------------------------- 

    }

}

