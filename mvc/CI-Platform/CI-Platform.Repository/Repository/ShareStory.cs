using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
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
        enum StoryStatus
        {
            DRAFT ,
            PENDING ,
            PUBLISHED ,
            DECLINED , 
            APPROVE
        }
        enum MediaType
        {
            images,
            videos 
        }

        public ShareStory(CiDbContext db)
        {
            _db = db;
        }
        //get mission titles where user has applied
        public List<MissionApplication> GetMissionListofUser(long userId)
        {
            return _db.MissionApplications.Where(m => m.UserId == userId && m.ApprovalStatus == StoryStatus.APPROVE.ToString()).Include(m => m.Mission).ToList();
        }

        //get the drafted story if there is any
        public Story GetDraftedStory(long userId, long missionId)
        {
            Story? story =  _db.Stories.Where(s => s.MissionId == missionId && s.UserId == userId && s.Status == StoryStatus.DRAFT.ToString() && s.DeletedAt == null).Include(s => s.StoryMedia).Include(m=>m.Mission).FirstOrDefault();
            return story!;
        }

        //to edit the drafted story
        public void EditDraftedStory(ShareStoryViewModel vm, long userId)
        {
            Story EditDraftStory = GetDraftedStory(userId, vm.MissionId);
            if (EditDraftStory != null)
            {
                EditDraftStory.Title = vm.StoryTitle;
                EditDraftStory.Description = vm.StoryDescription;
                EditDraftStory.Status = StoryStatus.DRAFT.ToString();
                EditDraftStory.PublishedAt = DateTime.Now;

                _db.Update(EditDraftStory);
                _db.SaveChanges();
            }

            Story story = GetDraftedStory(userId, vm.MissionId);
            if (vm.VideoUrls != null)
            {
                AddOrRemoveStoryUrls(story.StoryId, vm.VideoUrls);
            }
            if (vm.Images != null)
            {
                AddOrRemoveStoryImages(story.StoryId, vm.Images);
            }
        }

        //to add new story(user is adding a new story)
        public void AddNewStory(ShareStoryViewModel vm, long userId)
        {
            Story newStory = new()
            {
                MissionId = vm.MissionId,
                UserId = userId,
                Title = vm.StoryTitle,
                Description = vm.StoryDescription,
                Status = StoryStatus.DRAFT.ToString(),
                PublishedAt = DateTime.Now,
            };
            _db.Add(newStory);
            _db.SaveChanges();

            Story story = GetDraftedStory(userId, vm.MissionId);
            if (vm.VideoUrls != null)
            {
                AddOrRemoveStoryUrls(story.StoryId, vm.VideoUrls);
            }

            if (vm.Images != null)
            {
                AddOrRemoveStoryImages(story.StoryId, vm.Images);
            }
        }

        //for story urls updation
        public void AddOrRemoveStoryUrls(long storyId, string[] url)
        {
            var media = _db.StoryMedia.Where(sm => sm.StoryId == storyId && sm.Type == MediaType.videos.ToString());
            if (media.Any())
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
                        Type = MediaType.videos.ToString(),
                        Path = u,
                        CreatedAt = DateTime.Now,
                    };
                    _db.StoryMedia.Add(newMedia);
                }
            }

            _db.SaveChanges();
        }

        //for story images updation
        public void AddOrRemoveStoryImages(long storyId, List<IFormFile> Images)
        {
            var media = _db.StoryMedia.Where(sm => sm.StoryId == storyId && sm.Type == MediaType.images.ToString());
            //to remove if any 
            foreach (var m in media)
            {
                if (m != null)
                {
                    var fileName = m.Path;
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/Story", fileName));
                    _db.Remove(m);
                }
            }
            //to add 
            foreach(var img in Images)
            {
                if( img != null)
                {
                    var ImgName = img.FileName;
                    var guid = Guid.NewGuid().ToString().Substring(0, 8);
                    var fileName = $"{guid}_{ImgName}"; // getting filename
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/Story", fileName);

                    var newImage = new StoryMedium()
                    { 
                        StoryId = storyId,
                        Type = MediaType.images.ToString(),
                        Path = fileName,
                        CreatedAt = DateTime.Now,
                    };

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    _db.StoryMedia.Add(newImage);
                }
            }
            _db.SaveChanges();
        }

        //to check if the story is already published 
        public bool isPublishedStory(long userId, long missionId)
        {
            return _db.Stories.Any(s => s.MissionId == missionId && s.UserId == userId && (s.Status == StoryStatus.PUBLISHED.ToString() || s.Status == StoryStatus.PENDING.ToString()) && s.DeletedAt == null);
        }

        public void SubmitStory(ShareStoryViewModel vm, long userId)
        {
            Story existingStory = GetDraftedStory(userId, vm.MissionId);


            // Modify the properties of the existing story record
            existingStory.UserId = userId;
            existingStory.Title = vm.StoryTitle;
            existingStory.Description = vm.StoryDescription;
            existingStory.Status = StoryStatus.PENDING.ToString();
            existingStory.PublishedAt = vm.Date;
            existingStory.UpdatedAt = DateTime.Now;

            // Update the record in the database
            _db.Update(existingStory);
            _db.SaveChanges();

            if (vm.VideoUrls != null)
            {
                AddOrRemoveStoryUrls(existingStory.StoryId, vm.VideoUrls);
            }

            if (vm.Images != null)
            {
                AddOrRemoveStoryImages(existingStory.StoryId, vm.Images);
            }

        }

        public List<DateTime> GetMissionDates(long MissionId, long UserId)
        {
            List<DateTime> result = new List<DateTime>();

            var mission = _db.Missions.Where(m=>m.MissionId == MissionId && m.DeletedAt == null).Select(m=> new {m.StartDate, m.EndDate}).FirstOrDefault()!;

            if(mission.StartDate != null )
            {
                result.Add(mission.StartDate.Value);
            }
            if(mission.EndDate != null )
            {
                result.Add(mission.EndDate.Value);
            }
            return result;
        }
    }
}
