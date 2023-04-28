using System;
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
            APPROVE
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
            PENDING
        }
        public enum CommentStatus
        {
            PUBLISHED,
            PENDING,
            DECLINED
        }
       
    }
}
