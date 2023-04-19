﻿using CI_Platform.Entities.DataModels;
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
    }
}
