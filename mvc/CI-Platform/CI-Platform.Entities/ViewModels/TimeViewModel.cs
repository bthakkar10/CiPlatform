using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class TimeViewModel
    {
        public long MissionId { get; set; } 
        public DateTime TimeDate { get; set; }

        public string? TimeHours { get; set; }

        public string? TimeMinutes { get; set;}

        public TimeSpan? Time
        {
            get
            {
                TimeSpan timeSpanValue;
                if (TimeSpan.TryParseExact($"{TimeHours}:{TimeMinutes}", "h\\:mm", CultureInfo.InvariantCulture, out timeSpanValue))
                {
                    return timeSpanValue;
                }
                else
                {
                    return null;
                }
            }
        }

        public string? TimeMessage { get; set;}
    }
}
