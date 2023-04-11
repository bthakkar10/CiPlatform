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
                vm.GetTimesheetData = _timesheet.GetTimesheetData(UserId);
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

        public IActionResult AddOrUpdateTimesheetData(TimesheetViewModel vm)
        {
            var userId = HttpContext.Session.GetString("Id");
            long UserId = Convert.ToInt64(userId);

            //for time based mission entries 
            if (vm.TimeViewModel != null)
            {
                //to check for today's date validation
                if (!vm.TimeViewModel.IsValid())
                    {
                        TempData["error"] = vm.TimeViewModel.TimeErrorMessage;
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                //to add 
                if (vm.TimeViewModel.TimesheetId == 0 || vm.TimeViewModel.TimesheetId == null)
                {
                    
                    if (_timesheet.AddTimeBasedEntry(vm.TimeViewModel, UserId) == "success")
                    {
                        TempData["success"] = "Your Data is entered successfully!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                    else if(_timesheet.AddTimeBasedEntry(vm.TimeViewModel, UserId) == "Exists")
                    {
                        TempData["error"] = "Ypu already have entered timesheet for this date!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                    else
                    {
                        TempData["error"] = "Something went wrong!! Please try again later!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                }
                //to update
                else
                {
                    if (_timesheet.UpdateTimeBasedEntry(vm.TimeViewModel))
                    {
                        TempData["success"] = "Your Data is updated successfully!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                    else
                    {
                        TempData["error"] = "Something went wrong!! Please try again later!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                }
            }
            //for goal based missions timesheet entries
            else if (vm.GoalViewModel != null)
            {
                //to check today's date validation 
                if (!vm.GoalViewModel.IsValid())
                {
                    TempData["error"] = vm.GoalViewModel.TimeErrorMessage;
                    return RedirectToAction("VolunteeringTimesheet");
                }
                //to add 
                if (vm.GoalViewModel.TimesheetId == 0 || vm.GoalViewModel.TimesheetId == null)
                {

                    if (_timesheet.AddGoalBasedEntry(vm.GoalViewModel, UserId) == "success")
                    {
                        TempData["success"] = "Your Data is entered successfully!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                    else if (_timesheet.AddGoalBasedEntry(vm.GoalViewModel, UserId) == "Exists")
                    {
                        TempData["error"] = "You already have entered timesheet for this date!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                    else
                    {
                        TempData["error"] = "Something went wrong!! Please try again later!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                }
                //to update 
                else
                {
                    if (_timesheet.UpdateGoalBasedEntry(vm.GoalViewModel))
                    {
                        TempData["success"] = "Your Data is updated successfully!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                    else
                    {
                        TempData["error"] = "Something went wrong!! Please try again later!!";
                        return RedirectToAction("VolunteeringTimesheet");
                    }
                }
            }
            return RedirectToAction("VolunteeringTimesheet");
        }



        //displays the data when edit button is clicked 
        [HttpGet]
        public IActionResult GetDataOnEdit(long TimeSheetId)
        {
            if (_timesheet.GetDataOnEdit(TimeSheetId) != null)
            {
                Timesheet TsData = _timesheet.GetDataOnEdit(TimeSheetId);
                return Json(TsData);
            }
            else
            {
                return Ok(new { icon = "error", message = "Some error occured!! Please try again later!!" });
            }
        }

        public IActionResult DeleteTimesheetData(long TimeSheetId)
        {
            if(TimeSheetId != null)
            {
                if (_timesheet.DeleteTimeBasedEntry(TimeSheetId))
                {
                    TempData["success"] = "Your Data is deleted successfully!!";
                    return RedirectToAction("VolunteeringTimesheet");
                }
                else
                {
                    TempData["error"] = "Something went wrong!! Please try again later!!";
                    return RedirectToAction("VolunteeringTimesheet");
                }
            }
            else
            {
                TempData["error"] = "Something went wrong!! Please try again later!!";
                return RedirectToAction("VolunteeringTimesheet");
            }

        }

        public IActionResult DeleteGoalTimesheetData(long TimeSheetId)
        {
            if (TimeSheetId != null)
            {
                if (_timesheet.DeleteGoalBasedEntry(TimeSheetId))
                {
                    TempData["success"] = "Your Data is deleted successfully!!";
                    return RedirectToAction("VolunteeringTimesheet");
                }
                else
                {
                    TempData["error"] = "Something went wrong!! Please try again later!!";
                    return RedirectToAction("VolunteeringTimesheet");
                }
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
