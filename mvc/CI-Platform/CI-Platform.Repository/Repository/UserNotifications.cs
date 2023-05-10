using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class UserNotifications : IUserNotifications
    {
        private readonly CiDbContext _db;

        public UserNotifications(CiDbContext db)
        {
            _db = db;
        }

        public IQueryable<NotificationParameters> GetNotificationList(long UserId)
        {
            IQueryable<UserNotification> notifications = _db.UserNotifications.Where(un => un.UpdatedAt == null && un.ToUserId == UserId && un.UserSetting.IsEnabled == true).AsQueryable();
            var list = notifications.Select(n => new NotificationParameters()
            {
                NotificationId = n.NotificationId,
                ToUserId = UserId,
                FromUserId = n.FromUserId.HasValue ? n.FromUserId : 0,
                FromUserName = n.FromUserId.HasValue ? n.FromUser.FirstName + " " + n.FromUser.LastName : "",
                FromUserAvtar = n.FromUserId.HasValue ? n.FromUser.Avtar : "",
                RecommendedMissionId = n.RecommendedMissionId.HasValue ? n.RecommendedMissionId : 0,
                RecommendedMissionTitle = n.RecommendedMissionId.HasValue ? n.RecommendedMission.Title : "",
                RecommendedStoryId = n.RecommendedStoryId.HasValue ? n.RecommendedStoryId : 0,
                RecommendedStoryTitle = n.RecommendedStoryId.HasValue ? n.RecommendedStory.Title : "",
                StoryMissionId = n.RecommendedStoryId.HasValue ? n.RecommendedStory.MissionId : 0,
                StoryUserId = n.RecommendedStoryId.HasValue ? n.RecommendedStory.UserId : 0,
                NewMissionId = n.NewMissionId.HasValue ? n.NewMissionId : 0,
                NewMissionTitle = n.NewMissionId.HasValue ? n.NewMission.Title : "",
                CommentId = n.CommentId.HasValue ? n.CommentId : 0,
                CommentMissionTitle = n.CommentId.HasValue ? n.Comment.Mission.Title : "",
                TimesheetId = n.TimesheetId.HasValue ? n.TimesheetId : 0,
                TimesheetMissionTitle = n.TimesheetId.HasValue ? n.Timesheet.Mission.Title : "",
                TimesheetDate = n.TimesheetId.HasValue ? n.Timesheet.DateVolunteered : DateTime.Now,
                TimesheetStatus = n.TimesheetId.HasValue ? n.Timesheet.Status : "",
                MissionApplicationId = n.MissionApplicationId.HasValue ? n.MissionApplicationId : 0,
                MissionApplicationTitle = n.MissionApplicationId.HasValue ? n.MissionApplication.Mission.Title : string.Empty,
                MissionApplicationStatus = n.MissionApplicationId.HasValue ? n.MissionApplication.ApprovalStatus : string.Empty,
                StoryId = n.StoryId.HasValue ? n.StoryId : 0,
                StoryTitle = n.StoryId.HasValue ? n.Story.Title : "",
                StoryStatus = n.StoryId.HasValue ? n.Story.Status : string.Empty,
                Status = n.Status,
            });
            return list;
        }

        public void ClearNotification(long UserId)
        {
            List<UserNotification> userNotification = _db.UserNotifications.Where(un => un.ToUserId == UserId).ToList();
            foreach (var notification in userNotification)
            {
                notification.UpdatedAt = DateTime.Now;
                _db.UserNotifications.Update(notification);
            }
            _db.SaveChanges();
        }

        public void ChangeNotificationStatus(long NotificationId)
        {
            UserNotification userNotification = _db.UserNotifications.Find(NotificationId);
            if (userNotification != null)
            {
                userNotification.Status = true;
                _db.UserNotifications.Update(userNotification);
                _db.SaveChanges();
            }
        }

        public List<UserSetting> userSettings(long UserId)
        {
            List<UserSetting> userSettings = _db.UserSettings.Where(us => us.UserId == UserId).Include(us => us.Setting).ToList();
            return userSettings;
        }

        public bool NotificationSettings(long[] settingsArr, long UserId)
        {
            UserSetting[] UserSettings = _db.UserSettings.Where(us => us.UserId == UserId).ToArray();
            foreach (var setting in UserSettings)
            {
                if(settingsArr.Contains(setting.SettingId))
                {
                    setting.IsEnabled = true;
                }
                else
                {
                    setting.IsEnabled = false;
                }
            }
            _db.SaveChanges();
            return true;
        }
    }
}
