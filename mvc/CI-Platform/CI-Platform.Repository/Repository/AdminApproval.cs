using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Generic;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CI_Platform.Repository.Repository
{
    public class AdminApproval : IAdminApproval
    {
        private readonly CiDbContext _db;
        public AdminApproval(CiDbContext db)
        {
            _db = db;
        }
      
                
        public List<MissionApplication> MissionApplicationList()
        {
            return _db.MissionApplications.Where(missionapplication => missionapplication.DeletedAt == null && missionapplication.User.DeletedAt == null && missionapplication.Mission.DeletedAt == null).Include(missionapplication => missionapplication.Mission).Include(missionapplication => missionapplication.User).ToList();
        }

        public string ApproveDeclineApplication(long MissionApplicationId, long Status)
        {
            try
            {
                MissionApplication missionApplication = _db.MissionApplications.Find(MissionApplicationId)!;
                if (missionApplication == null)
                {
                    return "error";
                }
                else
                {
                    if (Status == 1)
                    {
                        missionApplication.ApprovalStatus = GenericEnum.ApplicationStatus.APPROVE.ToString();
                        missionApplication.UpdatedAt = DateTime.Now;
                        _db.MissionApplications.Update(missionApplication);
                    }
                    else
                    {
                        missionApplication.ApprovalStatus = GenericEnum.ApplicationStatus.DECLINE.ToString();
                        missionApplication.UpdatedAt = DateTime.Now;
                        _db.MissionApplications.Update(missionApplication);  
                    }

                    UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == missionApplication.UserId && u.SettingId == (long)GenericEnum.notification.Mission_Application_Approval).FirstOrDefault();

                    if (_db.UserNotifications.Any(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.MissionApplicationId == MissionApplicationId))
                    {
                        UserNotification notification = _db.UserNotifications.FirstOrDefault(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.MissionApplicationId == MissionApplicationId);
                        notification.Status = false;
                        notification.CreatedAt = DateTime.Now;
                    }
                    else
                    {
                        UserNotification notification = new()
                        {
                            ToUserId = missionApplication.UserId,
                            MissionApplicationId = MissionApplicationId,
                            Status = false,
                            CreatedAt = DateTime.Now,
                            UserSettingId = userSettingId.UserSettingId
                        };
                        _db.UserNotifications.Add(notification);
                    }
                    _db.SaveChanges();
                    return missionApplication.ApprovalStatus;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public List<Story> StoryList()
        {
            return _db.Stories.Where(story => story.DeletedAt == null && story.Status != GenericEnum.StoryStatus.DRAFT.ToString()  && story.Mission.DeletedAt == null && story.User.DeletedAt == null).Include(story => story.Mission).Include(story => story.User).ToList();
        }

        public string ApproveDeclineStory(long StoryId, long Status)
        {
            try
            {
                Story story = _db.Stories.Find(StoryId)!;
                if (story == null)
                {
                    return "error";
                }
                else
                {
                    if (Status == 1)
                    {
                        story.Status = GenericEnum.StoryStatus.PUBLISHED.ToString();
                        story.PublishedAt = DateTime.Now;
                        _db.Stories.Update(story);
                    }
                    else
                    {
                        story.Status = GenericEnum.StoryStatus.DECLINED.ToString();
                        story.UpdatedAt = DateTime.Now;
                        _db.Stories.Update(story);
                    }

                    UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == story.UserId && u.SettingId == (long)GenericEnum.notification.Story_Approval).FirstOrDefault()!;

                    if (_db.UserNotifications.Any(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.StoryId == StoryId))
                    {
                        UserNotification notification = _db.UserNotifications.FirstOrDefault(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.StoryId == StoryId);
                        notification.Status = false;
                        notification.CreatedAt = DateTime.Now;
                    }
                    else
                    {
                        UserNotification notification = new()
                        {
                            ToUserId = story.UserId,
                            StoryId = StoryId,
                            Status = false,
                            CreatedAt = DateTime.Now,
                            UserSettingId = userSettingId.UserSettingId
                        };
                        _db.UserNotifications.Add(notification);
                    }
                    _db.SaveChanges();
                    return story.Status;

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Story GetStoryDetailsAdmin(long MissionId)
        {
            Story story = _db.Stories.Where(s => s.DeletedAt == null && s.Mission.DeletedAt == null).
            Include(s => s.StoryMedia).Include(s => s.Mission).
            FirstOrDefault(s => s.MissionId == MissionId)!;
            return story;
        }

        public bool IsStoryDeleted(long StoryId)
        {
            try
            {
                Story story = _db.Stories.Find(StoryId)!;
                if (story != null)
                {
                    story.DeletedAt = DateTime.Now;
                    _db.Update(story);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Comment> CommentList()
        {
            return _db.Comments.Where(comment => comment.DeletedAt == null && comment.User.DeletedAt == null && comment.Mission.DeletedAt == null).Include(comment => comment.Mission).Include(comment => comment.User).ToList();
        }

        public string ApproveDeclineComments(long CommentId, long Status)
        {
            try
            {
                Comment comment = _db.Comments.Find(CommentId)!;
                if (comment == null)
                {
                    return "error";
                }
                else
                {
                    if (Status == 1)
                    {
                        comment.ApprovalStatus = GenericEnum.CommentStatus.PUBLISHED.ToString();
                        comment.UpdatedAt = DateTime.Now;
                        _db.Comments.Update(comment);
                    }
                    else
                    {
                        comment.ApprovalStatus = GenericEnum.CommentStatus.DECLINED.ToString();
                        comment.UpdatedAt = DateTime.Now;
                        _db.Comments.Update(comment);
                    }

                    UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == comment.UserId && u.SettingId == (long)GenericEnum.notification.Comment_Approval).FirstOrDefault();

                    if (_db.UserNotifications.Any(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.CommentId == CommentId))
                    {
                        UserNotification notification = _db.UserNotifications.FirstOrDefault(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.CommentId == CommentId);
                        notification.Status = false;
                        notification.CreatedAt = DateTime.Now;
                    }
                    else
                    {
                        UserNotification notification = new()
                        {
                            ToUserId = comment.UserId,
                            CommentId = CommentId,
                            Status = false,
                            CreatedAt = DateTime.Now,
                            UserSettingId = userSettingId.UserSettingId
                        };
                        _db.UserNotifications.Add(notification);
                    }
                    _db.SaveChanges();
                    return comment.ApprovalStatus;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<Timesheet> TimesheetList()
        {
            return _db.Timesheets.Where(timesheet => timesheet.DeletedAt == null && timesheet.Mission.DeletedAt == null && timesheet.User.DeletedAt == null).Include(timesheet => timesheet.Mission).Include(timesheet => timesheet.User).ToList();
        }

        public string ApproveDeclineTimesheets(long TimesheetId, long Status)
        {
            try
            {
                Timesheet timesheet = _db.Timesheets.Find(TimesheetId)!;
                if (timesheet == null)
                {
                    return "error";
                }
                else
                {
                    if (Status == 1)
                    {
                        timesheet.Status = GenericEnum.TimesheetStatus.APPROVED.ToString();
                        timesheet.UpdatedAt = DateTime.Now;
                        _db.Timesheets.Update(timesheet);
                    }
                    else
                    {
                        timesheet.Status = GenericEnum.TimesheetStatus.DECLINED.ToString();
                        timesheet.UpdatedAt = DateTime.Now;
                        _db.Timesheets.Update(timesheet);
                    }

                    UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == timesheet.UserId && u.SettingId == (long)GenericEnum.notification.Timesheet_Approval).FirstOrDefault();

                    if (_db.UserNotifications.Any(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.TimesheetId == TimesheetId))
                    {
                        UserNotification notification = _db.UserNotifications.FirstOrDefault(u => u.UserSettingId == userSettingId.UserSettingId && u.UpdatedAt == null && u.TimesheetId == TimesheetId);
                        notification.Status = false;
                        notification.CreatedAt = DateTime.Now;
                    }
                    else
                    {
                        UserNotification notification = new()
                        {
                            ToUserId = timesheet.UserId,
                            TimesheetId = TimesheetId,
                            Status = false,
                            CreatedAt = DateTime.Now,
                            UserSettingId = userSettingId.UserSettingId
                        };
                        _db.UserNotifications.Add(notification);
                    }
                    _db.SaveChanges();
                    return timesheet.Status;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
