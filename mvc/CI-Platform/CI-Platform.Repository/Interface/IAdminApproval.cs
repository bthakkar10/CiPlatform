using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminApproval
    {
        public List<MissionApplication> MissionApplicationList();

        public string ApproveDeclineApplication(long MissionApplicationId, long Status);

        public List<Story> StoryList();

        public string ApproveDeclineStory(long StoryId, long Status);

        public Story GetStoryDetailsAdmin(long MissionId);

        public bool IsStoryDeleted(long StoryId);

        public List<Comment> CommentList();

        public string ApproveDeclineComments(long CommentId, long Status);

        public List<Timesheet> TimesheetList();

        public string ApproveDeclineTimesheets(long TimesheetId, long Status);
    }
}
