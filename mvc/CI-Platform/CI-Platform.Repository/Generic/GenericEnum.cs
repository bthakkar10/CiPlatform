﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Generic
{
    public class GenericEnum
    {
        public enum StoryStatus
        {
            DRAFT,
            PENDING,
            PUBLISHED,
            DECLINED,
          
        }
         public enum StoryMediaType
        {
            images,
            videos
        }
        public enum Role
        {
            user,
            admin
        }

        public enum ApplicationStatus
        {
            APPROVE, 
            PENDING,
            DECLINE
        }
        public enum CommentStatus
        {
            PUBLISHED,
            PENDING,
            DECLINED
        }
        public enum TimesheetStatus
        {
            APPROVED, 
            PENDING, 
            DECLINED
        }

        public enum notification
        {
            Recommended_Mission = 1,
            Recommended_Story = 2,
            New_Mission = 3,
            Story_Approval = 4,
            Mission_Application_Approval = 5,
            Comment_Approval = 6,
            Timesheet_Approval = 7
        }

        public enum MissionMediaType
        {
            img,
            vid
        }
        public enum MissionMediaName
        {
            Mission_images,
            Mission_video,
        }
        public enum MissionType
        {
            Time,
            Goal
        }
    }
}
