using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminApprovalViewModel
    {
        public List<MissionApplication> MissionApplicationList { get; set; }   = new List<MissionApplication>();    

        public List<Story> StoryList { get; set; } = new List<Story>();

        public Story AdminStoryDetails { get; set; } = new Story();

        public List<Comment> CommentsList { get; set; } = new List<Comment>();

        public List<Timesheet> TimesheetsList { get; set; } = new List<Timesheet>();


    }
}
