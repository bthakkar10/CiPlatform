using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IUserNotifications
    {
        public IQueryable<NotificationParameters> GetNotificationList(long UserId);

        public void ClearNotification(long UserId);

        public void ChangeNotificationStatus(long NotificationId);

        public List<UserSetting> userSettings(long UserId);

        public bool NotificationSettings(long[] settingsArr, long UserId);
    }
}
