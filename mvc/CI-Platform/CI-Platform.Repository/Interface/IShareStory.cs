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
    public interface IShareStory
    {
        public List<MissionApplication> GetMissionListofUser( long userId);

        public Story GetDraftedStory(long userId, long missionId);

        public void EditDraftedStory(ShareStoryViewModel vm, long userId);

        public void AddNewStory(ShareStoryViewModel vm, long userId);

        public void AddOrRemoveStoryUrls(long storyId, string[] url);

        public void AddOrRemoveStoryImages(long storyId, List<IFormFile> Images);

        public bool isPublishedStory(long userId, long missionId);

        public void SubmitStory(ShareStoryViewModel vm, long userId);
    }
}
