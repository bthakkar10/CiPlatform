using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
   
    public class ShareStory : IShareStory
    {
        private readonly CiDbContext _db;

        public ShareStory(CiDbContext db)
        {
            _db = db;
        }
        //get mission titles where user has applied
        public List<MissionApplication> GetMissionListofUser(long userId)
        {
            return _db.MissionApplications.Where(m => m.UserId == userId && m.ApprovalStatus == "APPROVE").Include(m => m.Mission).ToList();
        }

        //get the drafted story if there is any
        public Story GetDraftedStory(long userId, long missionId)
        {
            return _db.Stories.Where(s => s.MissionId == missionId && s.UserId == userId && s.Status == "DRAFT").FirstOrDefault();
        }

        //to edit the drafted story
        public void EditDraftedStory(ShareStoryViewModel vm, long userId)
        {
            Story EditDraftStory = GetDraftedStory(userId, vm.MissionId);
            if (EditDraftStory != null)
            {
                EditDraftStory.Title = vm.StoryTitle;
                EditDraftStory.Description = vm.StoryDescription;
                EditDraftStory.Status = "DRAFT";
                EditDraftStory.CreatedAt = DateTime.Now;

                _db.Update(EditDraftStory);
                _db.SaveChanges();
            }
        }

        //to add new story(user is adding a new story)
        public void AddNewStory(ShareStoryViewModel vm, long userId)
        {
            Story newStory = new Story()
            {
                MissionId = vm.MissionId,
                UserId = userId,
                Title = vm.StoryTitle,
                Description = vm.StoryDescription,
                Status = "DRAFT",
                CreatedAt = DateTime.Now,
            };
            _db.Add(newStory);
            _db.SaveChanges();

            Story story = GetDraftedStory(userId, vm.MissionId);
            if(vm.VideoUrls != null)
            {
                AddOrRemoveStoryUrls(story.StoryId, vm.VideoUrls);
            }
        }

        public void AddOrRemoveStoryUrls(long storyId, string[] url)
        {
            var media = _db.StoryMedia.Where(sm => sm.StoryId == storyId && sm.Type == "video");
            if(media.Any())
            {
                _db.RemoveRange(media);
            }
            foreach (var u in url)
            {
                if (u != null)
                {
                    var newMedia = new StoryMedium()
                    {
                        StoryId = storyId,
                        Type = "video",
                        Path = u,
                        CreatedAt = DateTime.Now,
                    };
                    _db.StoryMedia.Add(newMedia);
                }
            }

            _db.SaveChanges();
        }

        //to check if the story is already published 
        public bool isPublishedStory(long userId, long missionId)
        {
            return _db.Stories.Any(s=>s.MissionId == missionId && s.UserId == userId && s.Status == "PUBLISHED");
           
        }
    }
}
