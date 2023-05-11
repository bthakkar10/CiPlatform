using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using CI_Platform_web.Utility;
using System.Reflection.Metadata.Ecma335;
using PublicResXFileCodeGenerator;
using CI_Platform.Repository.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.Web.Mvc;

namespace CI_Platform_web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminUser _adminUser;
        private readonly IFilter _filter;
        private readonly IAdminCms _adminCms;
        private readonly IAdminTheme _adminTheme;
        private readonly IAdminSkills _adminSkills;
        private readonly IAdminApproval _adminApproval;
        private readonly IAdminMission _adminMission;
        private readonly IAdminBanner _adminBanner;

        public AdminController(IAdminUser adminUser, IFilter filter, IAdminCms adminCms, IAdminTheme adminTheme, IAdminSkills adminSkills, IAdminApproval adminApproval, IAdminMission adminMission, IAdminBanner adminBanner)
        {
            _adminUser = adminUser;
            _filter = filter;
            _adminCms = adminCms;
            _adminTheme = adminTheme;
            _adminSkills = adminSkills;
            _adminApproval = adminApproval;
            _adminMission = adminMission;
            _adminBanner = adminBanner;
        }

        //------------------------------------------  User  Section Starts ---------------------------------------------------------- 

        [HttpGet]
        
        public IActionResult User()
        {
            try
            {
                AdminUserViewModel vm = new();
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
            bool returnvalue = _adminUser.IsUserDeleted(UserId);
            if (returnvalue)
            {
                TempData["succes"] = "User " + Messages.Delete ;
                return RedirectToAction("User");
            }
            else
            {
                TempData["error"] = Messages.Error;
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
                string returnvalue = _adminUser.IsUserAdded(vm);
                if (returnvalue == "Added")
                {
                    TempData["success"] = "User " + Messages.Add;
                    return RedirectToAction("User");
                }
                else if (returnvalue == "Exists")
                {
                    TempData["error"] = "User with same Email or Employee Id " + Messages.Exists;
                    return RedirectToAction("User");
                }
                else
                {
                    TempData["error"] = Messages.Error;
                    return RedirectToAction("User");
                }
            }
            // to Update 
            else
            {
                string returnvalue = _adminUser.EditUser(vm);
                if (returnvalue == "Updated")
                {
                    TempData["success"] = "User " + Messages.Update;
                    return RedirectToAction("User");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "User with same Email or Employee Id " + Messages.Exists;
                    return RedirectToAction("User");
                }
                else
                {
                    TempData["error"] = Messages.Error;
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
                AdminCmsViewModel cmsvm = new();
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
                string returnvalue = _adminCms.CmsAdd(cmsvm);
                if (returnvalue == "Added")
                {
                    TempData["success"] = "Privacy Page " + Messages.Add;
                    return RedirectToAction("CmsPage");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Privacy Page " + Messages.Exists;
                    return RedirectToAction("CmsPage");
                }
                else
                {
                    TempData["error"] = Messages.Error;
                    return RedirectToAction("CmsPage");
                }
            }
            // to Update 
            else
            {
                string returnvalue = _adminCms.EditCms(cmsvm);
                if (returnvalue == "Updated")
                {
                    TempData["success"] = "Privacy Policy " + Messages.Update;
                    return RedirectToAction("CmsPage");
                }
                else if (returnvalue == "Exists")
                {
                    TempData["error"] = "Privacy Page " + Messages.Exists;
                    return RedirectToAction("CmsPage");
                }
                else
                {
                    TempData["error"] = Messages.Error;
                    return RedirectToAction("CmsPage");
                }
            }
        }
        [HttpPost]
        public IActionResult DeleteCmsPage()
        {
            long CmsId = long.TryParse(Request.Form["HiddenCmsId"], out long result) ? result : 0;
            bool returnvalue = _adminCms.CmsDelete(CmsId);
            if (returnvalue)
            {
                TempData["succes"] = "Privacy Page " + Messages.Delete;
                return RedirectToAction("CmsPage");
            }
            else
            {
                TempData["error"] = Messages.Error;
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
                AdminThemeViewModel themevm = new();
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
                string returnvalue = _adminTheme.ThemeAdd(themevm);
                if (returnvalue == "Added")
                {
                    TempData["success"] = "New Theme " + Messages.Add;
                    return RedirectToAction("MissionTheme");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Theme " + Messages.Exists;
                    return RedirectToAction("MissionTheme");
                }
                else
                {
                    TempData["error"] = Messages.Error ;
                    return RedirectToAction("MissionTheme");
                }
            }
            // to Update 
            else
            {
                string returnvalue = _adminTheme.EditTheme(themevm);
                if (returnvalue == "Updated")
                {
                    TempData["success"] = "Theme " + Messages.Update;
                    return RedirectToAction("MissionTheme");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Theme " + Messages.Exists;
                    return RedirectToAction("MissionTheme");
                }
                else
                {
                    TempData["error"] = Messages.Error;
                    return RedirectToAction("MissionTheme");
                }
            }
        }
        [HttpPost]
        public IActionResult DeleteTheme()
        {
            long ThemeId = long.TryParse(Request.Form["HiddenThemeId"], out long result) ? result : 0;
            string returnvalue = _adminTheme.ThemeDelete(ThemeId);
            if (returnvalue == "Deleted")
            {
                TempData["succes"] = "Theme " + Messages.Add;
                return RedirectToAction("MissionTheme");
            }
            else if(returnvalue == "Exists")
            {
                TempData["error"] = "Theme " + Messages.Use;
                return RedirectToAction("MissionTheme");
            }
            else
            {
                TempData["error"] = Messages.Error;
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
                AdminSkillsViewModel skillvm = new();
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
                string returnvalue = _adminSkills.SkillsAdd(skillvm);
                if (returnvalue == "Added")
                {
                    TempData["success"] = "New Skill " + Messages.Add;
                    return RedirectToAction("MissionSKill");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Skill " + Messages.Exists;
                    return RedirectToAction("MissionSKill");
                }
                else
                {
                    TempData["error"] = Messages.Error;
                    return RedirectToAction("MissionSKill");
                }
            }
            // to Update 
            else
            {
                string returnvalue = _adminSkills.EditSkill(skillvm);
                if (returnvalue == "Updated")
                {
                    TempData["success"] = "Skill " + Messages.Update;
                    return RedirectToAction("MissionSKill");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Skill " + Messages.Exists;
                    return RedirectToAction("MissionSKill");
                }
                else
                {
                    TempData["error"] = Messages.Error;
                    return RedirectToAction("MissionSKill");
                }
            }
        }

        public IActionResult DeleteSkill()
        {
            long SkillId = long.TryParse(Request.Form["HiddenSkillId"], out long result) ? result : 0;
            string returnvalue = _adminSkills.SkillDelete(SkillId);
            if (returnvalue == "Deleted")
            {
                TempData["succes"] = "Skill " + Messages.Delete;
                return RedirectToAction("MissionSKill");
            }
            else if(returnvalue == "Exists")
            {
                TempData["error"] = "Skill " + Messages.Use;
                return RedirectToAction("MissionSKill");
            }
            else 
            {
                TempData["error"] = Messages.Error;
                return RedirectToAction("MissionSKill");
            }
        }
        //------------------------------------------  Mission Skill Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Mission Application Section Starts ---------------------------------------------------------- 

        public IActionResult MissionApplication()
        {
            AdminApprovalViewModel vm = new();
            vm.MissionApplicationList = _adminApproval.MissionApplicationList();
            return View(vm);
        }

        public IActionResult ApproveOrDeclineApplication(long MissionApplicationId, long ApplicationStatus)
        {
           
            string returnvalue = _adminApproval.ApproveDeclineApplication(MissionApplicationId, ApplicationStatus);
            if (returnvalue == GenericEnum.ApplicationStatus.APPROVE.ToString())
            {
                return Ok(new { icon = "success", message = "Mission Application " + Messages.Approve});
            }
            else if(returnvalue == GenericEnum.ApplicationStatus.DECLINE.ToString())
            {
                return Ok(new { icon = "success", message = "Mission Application " + Messages.Decline });
            }
            else
            {
                return Ok(new { icon = "error", message = Messages.Error });
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
        public IActionResult ApproveOrDeclineStory(long StoryId, long StoryStatus)
        {
            string returnvalue = _adminApproval.ApproveDeclineStory(StoryId, StoryStatus);
            if (returnvalue == GenericEnum.StoryStatus.PUBLISHED.ToString())
            {
                return Ok(new { icon = "success", message = "Story Published Successfully!!" });
            }
            else if (returnvalue == GenericEnum.StoryStatus.DECLINED.ToString())
            {
                return Ok(new { icon = "success", message = "Story Declined Successfully!!" });
            }
            else
            {
                return Ok(new { icon = "error", message = Messages.Error });
            }
        }

        public IActionResult StoryDetailsAdmin(long MissionId)
        {
            AdminApprovalViewModel vm = new();
            vm.AdminStoryDetails =  _adminApproval.GetStoryDetailsAdmin(MissionId);
            return PartialView("_StoryDetailsPartial", vm);
        }

        public IActionResult DeleteStory()
        {
            long StoryId = long.TryParse(Request.Form["HiddenStoryId"], out long result) ? result : 0;
            bool returnvalue = _adminApproval.IsStoryDeleted(StoryId);
            if (returnvalue)
            {
                TempData["succes"] = "Story " + Messages.Delete;
                return RedirectToAction("Story");
            }
            else
            {
                TempData["error"] = Messages.Error ;
                return RedirectToAction("Story");
            }
        }

        //------------------------------------------  Story Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Mission Section Starts ---------------------------------------------------------- 

        public IActionResult AdminMission()
        {
            AdminMissionViewModel missionvm = new();
            missionvm.GetMissionList = _adminMission.MissionList();
            return View(missionvm);
        }

        //add or update mission form get method to load the add and edit forms 
        [HttpGet]
        public IActionResult AddOrUpdateMission()
        {
            AdminMissionViewModel missionvm = new();
            missionvm.CityList = _filter.AllCityList().ToList();
            missionvm.CountryList = _filter.CountryList().ToList();
            missionvm.ThemeList = _filter.ThemeList().ToList();
            missionvm.SkillsList = _filter.SkillList().ToList();
            if(missionvm.OrganizationDetail==null)
            {
                missionvm.OrganizationDetail = "";
            }
            return PartialView("_AddOrUpdateMission", missionvm);
        }

        [HttpPost]
        public IActionResult AddOrUpdateMissionPost(AdminMissionViewModel missionvm)
        {
            //to add
            if (missionvm.MissionId == 0)
            {
                string returnvalue = _adminMission.MissionAdd(missionvm);
                if (returnvalue == "Added")
                {
                    return Ok(new { icon = "success", message = "Mission " + Messages.Add });
                }
                else if (returnvalue == "Exists")
                {
                    return Ok(new { icon = "error", message = "Mission " + Messages.Exists});
                }
                else
                {
                    return Ok(new { icon = "error", message = Messages.Error }); 
                }
            }
            else
            {
                string returnvalue = _adminMission.MissionEdit(missionvm);
                if (returnvalue  == "Updated")
                {
                    return Ok(new { icon = "success", message = "Mission " + Messages.Update });
                }
                else if(returnvalue == "Exists")
                {
                    return Ok(new { icon = "error", message = "Mission " + Messages.Exists });
                }
                else
                {
                    return Ok(new { icon = "error", message = Messages.Error });
                }
            }
        }

        //fetching mission data on edit 
        [HttpGet]
        public IActionResult GetMissionData(long MissionId)
        {
            AdminMissionViewModel missionvm = _adminMission.GetMission(MissionId);
            missionvm.CityList = _filter.AllCityList().ToList();
            missionvm.CountryList = _filter.CountryList().ToList();
            missionvm.ThemeList = _filter.ThemeList().ToList();
            missionvm.SkillsList = _filter.SkillList().ToList();
            missionvm.MissionSkills = _filter.MissionSkillList().ToList();
     
            return PartialView("_AddOrUpdateMission", missionvm);
        }

        [HttpPost]
        public IActionResult DeleteMission()
        {
            long MissionId = long.TryParse(Request.Form["HiddenMissionId"], out long result) ? result : 0;
            bool returnvalue = _adminMission.MissionDelete(MissionId);
            if (returnvalue)
            {
                TempData["succes"] = "Mission " + Messages.Delete;
                return RedirectToAction("AdminMission");
            }
            else
            {
                TempData["error"] = Messages.Error ;
                return RedirectToAction("AdminMission");
            }
        }

        //------------------------------------------  Mission Section Ends ---------------------------------------------------------- 


        //------------------------------------------  Banner Section Starts ---------------------------------------------------------- 
        public IActionResult Banner()
        {
            AdminBannerViewModel bannervm = new();
            bannervm.BannerList = _adminBanner.BannerList();
            return View(bannervm);
        }

        //fetching banner data on edit button click 
        public IActionResult GetBannerData(long BannerId)
        {
            AdminBannerViewModel bannervm = _adminBanner.GetBannerData(BannerId);
            return PartialView("_AddOrUpdateBanner", bannervm);
        }

        //add or update banner form get method to load the add and edit forms 
        [HttpGet]
        public IActionResult AddOrUpdateBanner()
        {
            AdminBannerViewModel bannervm = new();
            return PartialView("_AddOrUpdateBanner", bannervm);
        }

        //add or update privacy pages
        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public IActionResult AddOrUpdateBanner(AdminBannerViewModel bannervm)
        {
            //to add
            if (bannervm.BannerId == 0)
            {
                string returnvalue = _adminBanner.BannerAdd(bannervm);
                if (returnvalue  == "Added")
                {
                    TempData["success"] = " New Banner " + Messages.Add;
                    return RedirectToAction("Banner");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Sort Order " + Messages.Exists ;
                    return RedirectToAction("Banner");
                }
                else
                {
                    TempData["error"] = Messages.Error ;
                    return RedirectToAction("Banner");
                }
            }
            // to Update 
            else
            {
                string returnvalue = _adminBanner.EditBanner(bannervm);
                if (returnvalue == "Updated")
                {
                    TempData["success"] = "Banner " + Messages.Update;
                    return RedirectToAction("Banner");
                }
                else if(returnvalue == "Exists")
                {
                    TempData["error"] = "Sort Order " + Messages.Exists;
                    return RedirectToAction("Banner");
                }
                else
                {
                    TempData["error"] = Messages.Error ;
                    return RedirectToAction("Banner");
                }
            }
        }
        [HttpPost]
        public IActionResult DeleteBanner()
        {
            long BannerId = long.TryParse(Request.Form["HiddenBannerId"], out long result) ? result : 0;
            bool returnvalue = _adminBanner.BannerDelete(BannerId);
            if (returnvalue)
            {
                TempData["succes"] = "Banner " + Messages.Delete;
                return RedirectToAction("Banner");
            }
            else
            {
                TempData["error"] = Messages.Error ;
                return RedirectToAction("Banner");
            }
        }
        //------------------------------------------  Banner  Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Comment Section Starts ---------------------------------------------------------- 

        public IActionResult MissionComments()
        {
            AdminApprovalViewModel vm = new()
            {
                CommentsList = _adminApproval.CommentList()
            };
            return View(vm);
        }

        public IActionResult ApproveOrDeclineComment()
        {
            long CommentId = long.TryParse(Request.Form["HiddenCommentId"], out long result) ? result : 0;
            long Status = long.TryParse(Request.Form["HiddenStatus"], out long resultStatus) ? resultStatus : 0;
            string returnvalue = _adminApproval.ApproveDeclineComments(CommentId, Status);
            if (returnvalue  == GenericEnum.CommentStatus.PUBLISHED.ToString())
            {
                TempData["success"] = "Comment " + Messages.Approve;
                return RedirectToAction("MissionComments");

            }
            else if (returnvalue == GenericEnum.CommentStatus.DECLINED.ToString())
            {
                TempData["success"] = "Comment " + Messages.Decline;
                return RedirectToAction("MissionComments");
            }
            else
            {
                TempData["error"] = Messages.Error ;
                return RedirectToAction("MissionComments");
            }
        }
        //------------------------------------------  Comment Section Ends ---------------------------------------------------------- 

        //------------------------------------------  Timesheet Section Starts ---------------------------------------------------------- 

        public IActionResult Timesheet()
        {
            AdminApprovalViewModel vm = new()
            {
                TimesheetsList = _adminApproval.TimesheetList()
            };
            return View(vm);
        }

        public IActionResult ApproveOrDeclineTimesheet()
        {
            long TimesheetId = long.TryParse(Request.Form["HiddenTimesheetId"], out long result) ? result : 0;
            long Status = long.TryParse(Request.Form["HiddenStatus"], out long resultStatus) ? resultStatus : 0;
            string returnvalue = _adminApproval.ApproveDeclineTimesheets(TimesheetId, Status);
            if (returnvalue == GenericEnum.TimesheetStatus.APPROVED.ToString())
            {
                TempData["success"] = "Timesheet " + Messages.Approve;
                return RedirectToAction("Timesheet");

            }
            else if (returnvalue == GenericEnum.TimesheetStatus.DECLINED.ToString())
            {
                TempData["success"] = "Timesheet " + Messages.Decline;
                return RedirectToAction("Timesheet");
            }
            else
            {
                TempData["error"] = Messages.Error ;
                return RedirectToAction("Timesheet");
            }
        }
        //------------------------------------------  Timesheet Section Ends ---------------------------------------------------------- 
    }

}

