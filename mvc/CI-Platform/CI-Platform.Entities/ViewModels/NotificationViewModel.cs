using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class NotificationViewModel
    {
        public IQueryable<NotificationParameters>? userNotifications { get; set; } 

        public List<UserSetting> UserNotificationSetting { get; set; }

        public int count { get; set; }
    }

    public class NotificationParameters
    {
        public long NotificationId { get; set; }

        public long? ToUserId { get; set; }

        public long? FromUserId { get; set; }

        public string? FromUserName { get; set; }

        public string? FromUserAvtar { get; set; }

        public long? RecommendedMissionId { get; set; }

        public long? RecommendedStoryId { get; set; }

        public long StoryMissionId { get; set; }

        public long StoryUserId { get; set; }

        public string? RecommendedMissionTitle { get; set; }

        public string? RecommendedStoryTitle { get; set; }

        public long? NewMissionId { get; set; }

        public string? NewMissionTitle { get; set; }

        public long? CommentId { get; set; }

        public string? CommentMissionTitle { get; set; }

        public long? TimesheetId { get; set; }

        public string? TimesheetMissionTitle { get; set; }

        public string? TimesheetStatus { get; set; }

        public DateTime? TimesheetDate { get; set; }

        public long? StoryId { get; set; }

        public string? StoryTitle { get; set; }

        public string? StoryStatus { get; set; }

        public long? MissionApplicationId { get; set; }

        public string? MissionApplicationTitle { get; set; }

        public string? MissionApplicationStatus { get; set; }   

        public bool Status { get; set; }
    }
}
