﻿using CI_Platform.Entities.DataModels;
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
            Story? story =  _db.Stories.Where(s => s.MissionId == missionId && s.UserId == userId && s.Status == "DRAFT").Include(s => s.StoryMedia).FirstOrDefault();
            return story;
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
            var media = _db.StoryMedia.Where(sm => sm.StoryId == storyId && sm.Type == "videos");
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
                        Type = "videos",
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
            var media = _db.StoryMedia.Where(sm => sm.StoryId == storyId && sm.Type == "images");
            //to remove if any 
            foreach (var m in media)
            {
                if (m != null)
                {
                    var fileName = m.Path;
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload", fileName));
                    _db.Remove(m);
                }
            }
            //to add 
            foreach(var img in Images)
            {
                if( img != null)
                {
                    var fileName = img.FileName;

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload", fileName);

                    var newImage = new StoryMedium()
                    { 
                        StoryId = storyId,
                        Type = "images",
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
            return _db.Stories.Any(s => s.MissionId == missionId && s.UserId == userId && (s.Status == "PUBLISHED" || s.Status == "PENDING"));
        }

        public void SubmitStory(ShareStoryViewModel vm, long userId)
        {
            Story existingStory = GetDraftedStory(userId, vm.MissionId);

            // If the story doesn't exist, throw an exception or return an error message
            if (existingStory == null)
            {
                throw new Exception("Story not found");
                // or return an error message to the caller
            }

            // Modify the properties of the existing story record
            existingStory.UserId = userId;
            existingStory.Title = vm.StoryTitle;
            existingStory.Description = vm.StoryDescription;
            existingStory.Status = "PENDING";
            existingStory.CreatedAt = vm.Date;
            existingStory.UpdatedAt = DateTime.Now;

            // Update the record in the database
            _db.Update(existingStory);
            _db.SaveChanges();
            
        }
    }
}
