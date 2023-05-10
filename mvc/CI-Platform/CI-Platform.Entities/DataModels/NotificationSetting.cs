using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class NotificationSetting
{
    public long SettingId { get; set; }

    public string? SettingName { get; set; }

    public bool? IsEnabled { get; set; }

    public virtual ICollection<UserSetting> UserSettings { get; } = new List<UserSetting>();
}
