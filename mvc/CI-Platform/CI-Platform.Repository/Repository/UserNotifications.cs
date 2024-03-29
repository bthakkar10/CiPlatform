﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Generic;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
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
            IQueryable<UserNotification>? notifications = _db.UserNotifications.Where(un => un.UpdatedAt == null && un.ToUserId == UserId && un.UserSetting.IsEnabled == true ).AsQueryable();
            var list = notifications.Select(n => new NotificationParameters()
            {
                NotificationId = n.NotificationId,
                ToUserId = UserId,
                FromUserId = n.FromUserId.HasValue ? n.FromUserId : 0,
                FromUserName = n.FromUserId.HasValue ? n.FromUser.FirstName + " " + n.FromUser.LastName : string.Empty,
                FromUserAvtar = n.FromUserId.HasValue ? n.FromUser.Avtar : string.Empty,
                RecommendedMissionId = n.RecommendedMissionId.HasValue && n.RecommendedMission.Status == true && n.RecommendedMission.DeletedAt == null ? n.RecommendedMissionId : 0,
                RecommendedMissionTitle = n.RecommendedMissionId.HasValue && n.RecommendedMission.Status == true && n.RecommendedMission.DeletedAt == null ? n.RecommendedMission.Title : string.Empty,
                RecommendedStoryId = n.RecommendedStoryId.HasValue && n.RecommendedStory.Mission.Status == true && n.RecommendedStory.Mission.DeletedAt == null && n.RecommendedStory.DeletedAt ==null && n.RecommendedStory.Status == GenericEnum.StoryStatus.PUBLISHED.ToString()? n.RecommendedStoryId : 0,
                RecommendedStoryTitle = n.RecommendedStoryId.HasValue ? n.RecommendedStory.Title : string.Empty,
                StoryMissionId = n.RecommendedStoryId.HasValue ? n.RecommendedStory.MissionId : 0,
                StoryUserId = n.RecommendedStoryId.HasValue ? n.RecommendedStory.UserId : 0,
                NewMissionId = n.NewMissionId.HasValue  ? n.NewMissionId : 0,
                NewMissionTitle = n.NewMissionId.HasValue  ? n.NewMission.Title : string.Empty,
                CommentId = n.CommentId.HasValue && n.Comment.Mission.DeletedAt == null  && n.Comment.Mission.Status == true? n.CommentId : 0,
                CommentMissionTitle = n.CommentId.HasValue ? n.Comment.Mission.Title : string.Empty,
                TimesheetId = n.TimesheetId.HasValue && n.Timesheet.Mission.Status == true && n.Timesheet.Mission.DeletedAt == null ? n.TimesheetId : 0,
                TimesheetMissionTitle = n.TimesheetId.HasValue ? n.Timesheet.Mission.Title : string.Empty,
                TimesheetDate = n.TimesheetId.HasValue ? n.Timesheet.DateVolunteered : DateTime.UtcNow,
                TimesheetStatus = n.TimesheetId.HasValue ? n.Timesheet.Status : string.Empty,
                MissionApplicationId = n.MissionApplicationId.HasValue && n.MissionApplication.Mission.Status == true && n.MissionApplication.Mission.DeletedAt == null  ? n.MissionApplicationId : 0,
                MissionApplicationTitle = n.MissionApplicationId.HasValue ? n.MissionApplication.Mission.Title : string.Empty,
                MissionApplicationStatus = n.MissionApplicationId.HasValue ? n.MissionApplication.ApprovalStatus : string.Empty,
                StoryId = n.StoryId.HasValue && n.Story.Mission.DeletedAt == null && n.Story.Mission.Status == true && n.Story.DeletedAt == null ? n.StoryId : 0,
                StoryTitle = n.StoryId.HasValue ? n.Story.Title : string.Empty,
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
