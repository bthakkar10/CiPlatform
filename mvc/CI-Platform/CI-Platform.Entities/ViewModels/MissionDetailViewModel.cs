using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionDetailViewModel
    {
        public Mission MissionDetails { get; set; } = new Mission();

        public List<Comment> ApprovedComments { get; set; } = null!;

        public int totalVolunteers { get; set; }

        //public int nextVolunteers { get; set; }

        public List<MissionApplication>? RecentVolunteers { get; set; }

        public List<MissionListViewModel>? RelatedMissions { get; set; }

        public List<User> UserList { get; set; } = null!;

        public string? link { get; set; }


    }
}
