using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMission
    {
        public List<Mission> MissionList();

        public string MissionAdd(AdminMissionViewModel missionvm);

        public bool AddOrRemoveMissionImage(long MissionId, List<IFormFile> ImageList, IFormFile DefaultMissionImg);

        public bool AddOrRemoveVideoUrl(long MissionId, string[] YoutubeUrl);

        public bool AddOrRemoveDocument(long MissionId, List<IFormFile> DocumentList);

        public bool AddOrRemoveMissionSkills(long MissionId, string UpdatedMissionSKills);

        public AdminMissionViewModel GetMission(long MissionId);
    }
}
