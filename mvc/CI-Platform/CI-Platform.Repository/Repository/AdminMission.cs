using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class AdminMission : IAdminMission
    {
        private readonly CiDbContext _db;

        enum MediaType
        {
            img,
            vid
        }
        enum MediaName
        {
            Mission_images,
            Mission_video,
        }
        enum MissionType
        {
            Time,
            Goal
        }

        public AdminMission(CiDbContext db)
        {
            _db = db;
        }
        public List<Mission> MissionList()
        {
            return _db.Missions.Where(mission => mission.DeletedAt == null).ToList();
        }

        public string MissionAdd(AdminMissionViewModel missionvm)
        {
            try
            {
                Mission DoesMissionExist = _db.Missions.Where(mission => mission.Title == missionvm.MissionTitle).FirstOrDefault()!;
                if (DoesMissionExist == null)
                {
                    Mission missionAdd = new()
                    {
                        Title = missionvm.MissionTitle,
                        CountryId = missionvm.CountryId,
                        CityId = missionvm.CityId,
                        MissionThemeId = missionvm.ThemeId,
                        ShortDescription = missionvm.ShortDescription,
                        Description = missionvm.Description,
                        StartDate = missionvm.StartDate,
                        EndDate = missionvm.EndDate,
                        MissionType = missionvm.MissionType,
                        OrganizationName = missionvm.OrganizationName,
                        OrganizationDetail = missionvm.OrganizationDetail,
                        Availability = missionvm.Avaliablity,
                        TotalSeats = missionvm.TotalSeats,
                        Status = missionvm.Status,
                        Deadline = missionvm.Deadline,
                        CreatedAt = DateTime.Now,
                    };
                    _db.Missions.Add(missionAdd);
                    _db.SaveChanges();
                    
                     AddOrRemoveGoalMission(missionAdd.MissionId, missionvm.GoalObjectiveText, missionvm.GoalValue);
                    
                    if (missionvm.ImageList != null)
                    {
                        AddOrRemoveMissionImage(missionAdd.MissionId, missionvm.ImageList, missionvm.DefaultMissionImg);

                    }
                    if (missionvm.YoutubeUrl != null)
                    {
                        AddOrRemoveVideoUrl(missionAdd.MissionId, missionvm.YoutubeUrl);
                    }
                    if (missionvm.DocumentList != null)
                    {
                        AddOrRemoveDocument(missionAdd.MissionId, missionvm.DocumentList);
                    }
                    if (missionvm.UpdatedMissionSKills != null)
                    {
                        AddOrRemoveMissionSkills(missionAdd.MissionId, missionvm.UpdatedMissionSKills);
                    }
                    return "Added";
                }
                else
                {
                    return "Exists";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool AddOrRemoveGoalMission(long MissionId, string GoalObjectiveText, int GoalValue)
        {
            try
            {

                return true;
            }
            catch (Exception) 
            {
                return false;   
            }
        }

        public bool AddOrRemoveMissionImage(long MissionId, List<IFormFile> ImageList, IFormFile DefaultMissionImg)
        {
            try
            {
                var media = _db.MissionMedia.Where(missionmedia => missionmedia.MissionId == MissionId && missionmedia.MediaType != "videos");

                //to remove images if any 
                foreach (var img in media)
                {
                    if (img != null)
                    {
                        var filePath = img.MediaPath;
                        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/Mission", filePath));
                        _db.Remove(img);
                    }
                }


                foreach (var img in ImageList)
                {
                    if (img != null)
                    {
                        var ImgName = img.FileName;
                        var guid = Guid.NewGuid().ToString().Substring(0, 8);
                        var fileName = $"{guid}_{ImgName}"; // getting filename
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/Mission", fileName);

                        MissionMedium newImage = new()
                        {
                            MissionId = MissionId,
                            MediaPath = fileName,
                            MediaType = MediaType.img.ToString(),
                            MediaName = MediaName.Mission_images.ToString(),
                            CreatedAt = DateTime.Now,
                            Defaultval = (DefaultMissionImg != null && DefaultMissionImg.FileName == img.FileName),
                            
                        };
                        //using var stream = 
                        img.CopyTo(new FileStream(filePath, FileMode.Create));

                        _db.MissionMedia.Add(newImage);
                    }
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddOrRemoveVideoUrl(long MissionId, string[] YoutubeUrl)
        {
            try
            {
                var media = _db.MissionMedia.Where(missionmedia => missionmedia.MissionId == MissionId && missionmedia.MediaType == "vid");
                if (media.Any())
                {
                    _db.RemoveRange(media);
                }
                foreach (var url in YoutubeUrl)
                {
                    if (url != null)
                    {
                        MissionMedium newMedia = new()
                        {
                            MissionId = MissionId,
                            MediaType = MediaType.vid.ToString(),
                            MediaPath = url,
                            CreatedAt = DateTime.Now,
                            MediaName = MediaName.Mission_video.ToString(),
                            Defaultval = false
                        };
                        _db.MissionMedia.Add(newMedia);
                    }
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddOrRemoveDocument(long MissionId, List<IFormFile> DocumentList)
        {
            try
            {
                var documents = _db.MissionDocuments.Where(missiondoc => missiondoc.MissionId == MissionId);
                foreach (var documentItem in documents)
                {
                    if (documentItem != null)
                    {
                        var fileName = documentItem.DocumentPath;
                        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", fileName));
                        _db.Remove(documentItem);
                    }
                }

                // Add new documents to the mission
                foreach (var doc in DocumentList)
                {
                    if (doc != null)
                    {
                        var docName = doc.FileName;
                        var guid = Guid.NewGuid().ToString().Substring(0, 8);
                        var fileName = $"{guid}_{docName}"; // getting filename
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", fileName);

                        MissionDocument newDocument = new()
                        {
                            MissionId = MissionId,
                            DocumentPath = fileName,
                            DocumentName = fileName,
                            CreatedAt = DateTime.UtcNow,
                            DocumentType = doc.ContentType,
                        };

                        _db.MissionDocuments.Add(newDocument);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        doc.CopyTo(stream);
                    }
                }
                _db.SaveChanges();  
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool AddOrRemoveMissionSkills(long MissionId, string UpdatedMissionSKills)
        {
            try
            {
                //for skills
                string[] SkillsArr = new string[0];

                SkillsArr = UpdatedMissionSKills.Split(',');

                List<MissionSkill> existingSkills = _db.MissionSkills.Where(missionskill => missionskill.MissionId == MissionId).ToList();
                var SkillsToAdd = _db.Skills.Where(skill => SkillsArr.Contains(skill.SkillName)).Select(skill => skill.SkillId).ToList();
                var SkillsToRemove = existingSkills.Where(missionskill => !SkillsToAdd.Contains(missionskill.SkillId)).ToList();
                _db.MissionSkills.RemoveRange(SkillsToRemove);

                //skills that needs to be added 
                foreach (var AddSkills in SkillsToAdd)
                {
                    if (!existingSkills.Any(missionskill => missionskill.SkillId == AddSkills))
                    {
                        MissionSkill missionskill = new()
                        {
                            MissionId = MissionId,
                            SkillId = AddSkills
                        };
                        _db.Add(missionskill);
                    }
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public AdminMissionViewModel GetMission(long MissionId)
        {
            Mission mission = _db.Missions.Find(MissionId)!;
            AdminMissionViewModel missionvm = new AdminMissionViewModel(mission);
            missionvm.MissionUrlLinks = _db.MissionMedia.Where(missionMedia => missionMedia.MediaType == "vid" && missionMedia.MissionId == MissionId).Select(m => m.MediaPath).ToArray()!;
            missionvm.MissionImagesList = _db.MissionMedia.Where(missionMedia => missionMedia.MediaType != "vid" && missionMedia.MissionId == MissionId).Select(m => m.MediaPath).ToArray()!;
            missionvm.MissionDefaultImage = _db.MissionMedia.FirstOrDefault(missionMedia => missionMedia.MissionId == MissionId && missionMedia.Defaultval == true)?.MediaPath;
            missionvm.MissionDocumentsList = _db.MissionDocuments.Where(missionDocuments => missionDocuments.MissionId == MissionId).Select(m => m.DocumentPath).ToArray()!;
            return missionvm;
        }


    }
}
