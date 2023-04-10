using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform_web.Controllers
{
    public class VolunteeringTimesheetController : Controller
    {
        private readonly IVolunteeringTimesheet _timesheet;
        private readonly IShareStory _shareStory;

        public VolunteeringTimesheetController(IVolunteeringTimesheet timesheet, IShareStory shareStory)
        {
            _timesheet = timesheet;
            _shareStory = shareStory;
        }

        
        public IActionResult VolunteeringTimesheet()
        {
            try
            {
                if (HttpContext.Session.GetString("SEmail") != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("Username") != null)
                {
                    ViewBag.email = HttpContext.Session.GetString("SEmail");
                    ViewBag.UserId = HttpContext.Session.GetString("Id");
                    ViewBag.Username = HttpContext.Session.GetString("Username");
                }

                var userId = HttpContext.Session.GetString("Id");
                long UserId = Convert.ToInt64(userId);
                var vm = new TimesheetViewModel();
                vm.GetMissionTitles = _timesheet.GetMissionTitles(UserId);
                return View(vm);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public IActionResult GetDates(long MissionId)
        {
            var userId = HttpContext.Session.GetString("Id");
            long UserId = Convert.ToInt64(userId);

            List<DateTime> dates = _shareStory.GetMissionDates(MissionId, UserId);
            return Json(dates);
        }

        public IActionResult AddTimeMission(TimeViewModel vm)
        {
            var userId = HttpContext.Session.GetString("Id");
            long UserId = Convert.ToInt64(userId);

            if (_timesheet.AddTimeBasedEntry(vm, UserId))
            {
                TempData["success"] = "Your Data is entered successfully!!";
                return RedirectToAction("VolunteeringTimesheet");
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("VolunteeringTimesheet");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
