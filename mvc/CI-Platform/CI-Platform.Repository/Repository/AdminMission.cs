﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Generic;
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
using System.Web.WebPages;

namespace CI_Platform.Repository.Repository
{
    public class AdminMission : IAdminMission
    {
        private readonly CiDbContext _db;

       

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
                Mission DoesMissionExist = _db.Missions.Where(mission => mission.Title.Trim().ToLower() == missionvm.MissionTitle.Trim().ToLower()).FirstOrDefault()!;
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
                        TotalSeats = (missionvm.MissionType == GenericEnum.MissionType.Time.ToString()) ? missionvm.TotalSeats : null,
                        Status =  missionvm.Status,
                        Deadline = (missionvm.MissionType == GenericEnum.MissionType.Time.ToString()) ? missionvm.Deadline : null,
                        CreatedAt = DateTime.Now,
                    };
                    _db.Missions.Add(missionAdd);
                    _db.SaveChanges();
                    if (GenericEnum.MissionType.Goal.ToString() == missionvm.MissionType)
                    {
                        AddOrRemoveGoalMission(missionAdd.MissionId, missionvm.GoalObjectiveText, missionvm.GoalValue, missionAdd.MissionType);
                    }
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
                    //to add users in user_notification also to send notification about the newly added mission
                    List<long> userIds = _db.Users.Where(u => u.DeletedAt == null && u.Role == GenericEnum.Role.user.ToString()).Select(u => u.UserId).ToList();
                    
                    foreach (long userId in userIds)
                    {
                        UserSetting? userSettingId = _db.UserSettings.Where(u => u.UserId == userId && u.SettingId == (long)GenericEnum.notification.New_Mission).FirstOrDefault()!;
                  
                        UserSkill[] userskills = _db.UserSkills.Where(u=>u.UserId == userId && u.DeletedAt == null).ToArray();
                        MissionSkill[] missionSkills = _db.MissionSkills.Where(ms=>ms.MissionId == missionAdd.MissionId).ToArray();
                        if (userSettingId != null && (userSettingId.User?.Availability ==  missionAdd.Availability || missionSkills.Any(ms => userskills.Any(us => us.SkillId == ms.SkillId))))
                        {
                            UserNotification notification = new()
                            {
                                ToUserId = userId,
                                NewMissionId = missionAdd.MissionId,
                                Status = false,
                                CreatedAt = DateTime.Now,
                                UserSettingId = userSettingId.UserSettingId
                            };
                            _db.UserNotifications.Add(notification);
                            _db.SaveChanges();
                        }
                        
                    }
                   
                    return "Added";
                }
                else
                {
                    if (DoesMissionExist != null && DoesMissionExist.DeletedAt == null)
                    {
                        return "Exists";
                    }
                    else
                    {
                        DoesMissionExist.DeletedAt = null;
                        DoesMissionExist.UpdatedAt = DateTime.Now;
                        _db.Update(DoesMissionExist);
                        _db.SaveChanges();
                        return "Added";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool AddOrRemoveGoalMission(long MissionId, string GoalObjectiveText, int GoalValue, string MissionType)
        {
            try
            {
                GoalMission goalMission = _db.GoalMissions.Where(goalmission=> goalmission.MissionId == MissionId).FirstOrDefault()!; 
                if (goalMission != null && MissionType == GenericEnum.MissionType.Time.ToString())
                {
                    _db.Remove(goalMission);
                    _db.SaveChanges();
                }
                else
                {
                    if(goalMission.MissionId == MissionId)
                    {
                        goalMission.GoalValue = GoalValue;
                        goalMission.GoalObjectiveText = GoalObjectiveText;
                        _db.Update(goalMission);
                    }
                    else
                    {
                        GoalMission AddGoalMission = new()
                        {
                            MissionId = MissionId,
                            GoalValue = GoalValue,
                            GoalObjectiveText = GoalObjectiveText,
                        };
                        _db.GoalMissions.Add(AddGoalMission);
                    }
                    
                    _db.SaveChanges();
                }
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
                var media = _db.MissionMedia.Where(missionmedia => missionmedia.MissionId == MissionId && missionmedia.MediaType != GenericEnum.MissionMediaType.vid.ToString());

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
                            MediaType = GenericEnum.MissionMediaType.img.ToString(),
                            MediaName = GenericEnum.MissionMediaName.Mission_images.ToString(),
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
                var media = _db.MissionMedia.Where(missionmedia => missionmedia.MissionId == MissionId && missionmedia.MediaType == GenericEnum.MissionMediaType.vid.ToString());
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
                            MediaType = GenericEnum.MissionMediaType.vid.ToString(),
                            MediaPath = url,
                            CreatedAt = DateTime.Now,
                            MediaName = GenericEnum.MissionMediaName.Mission_video.ToString(),
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
            Mission mission = _db.Missions.FirstOrDefault(m=>m.MissionId == MissionId && m.DeletedAt == null)!;
            GoalMission goalMission = _db.GoalMissions.FirstOrDefault(m => m.MissionId == MissionId && m.DeletedAt == null)!;
            AdminMissionViewModel missionvm = new(mission, goalMission)
            {
                MissionUrlLinks = _db.MissionMedia.Where(missionMedia => missionMedia.MediaType == GenericEnum.MissionMediaType.vid.ToString() && missionMedia.MissionId == MissionId).Select(m => m.MediaPath).ToArray()!,
                MissionImagesList = _db.MissionMedia.Where(missionMedia => missionMedia.MediaType != GenericEnum.MissionMediaType.vid.ToString() && missionMedia.MissionId == MissionId).Select(m => m.MediaPath).ToArray()!,
                MissionDefaultImage = _db.MissionMedia.FirstOrDefault(missionMedia => missionMedia.MissionId == MissionId && missionMedia.Defaultval == true)?.MediaPath,
                MissionDocumentsList = _db.MissionDocuments.Where(missionDocuments => missionDocuments.MissionId == MissionId).Select(m => m.DocumentPath).ToArray()!
            };
            return missionvm;
        }

        public string MissionEdit(AdminMissionViewModel missionvm)
        {
            try
            {
                if(_db.Missions.FirstOrDefault(mission => mission.Title!.Trim().ToLower() == missionvm.MissionTitle.Trim().ToLower() && mission.MissionId != missionvm.MissionId) != null)
                {
                    return "Exists";
                }
                else
                {
                    Mission mission = _db.Missions.Find(missionvm.MissionId)!;
                    if(mission != null)
                    {
                        mission.Title = missionvm.MissionTitle;
                        mission.CountryId = missionvm.CountryId;
                        mission.CityId = missionvm.CityId;
                        mission.MissionThemeId = missionvm.ThemeId;
                        mission.ShortDescription = missionvm.ShortDescription;
                        mission.Description = missionvm.Description;
                        mission.StartDate = missionvm.StartDate;
                        mission.EndDate = missionvm.EndDate;
                        mission.MissionType = missionvm.MissionType;
                        mission.OrganizationName = missionvm.OrganizationName;
                        mission.OrganizationDetail = missionvm.OrganizationDetail;
                        mission.Availability = missionvm.Avaliablity;
                        mission.TotalSeats = (missionvm.MissionType == GenericEnum.MissionType.Time.ToString()) ? missionvm.TotalSeats : null;
                        mission.Status = missionvm.Status;
                        mission.Deadline = (missionvm.MissionType == GenericEnum.MissionType.Time.ToString()) ? missionvm.Deadline : null;
                        mission.UpdatedAt = DateTime.Now;
                        _db.Missions.Update(mission);
                        _db.SaveChanges();

                        if (GenericEnum.MissionType.Goal.ToString() == missionvm.MissionType)
                        {
                            AddOrRemoveGoalMission(mission.MissionId, missionvm.GoalObjectiveText, missionvm.GoalValue, mission.MissionType);
                        }

                        if (missionvm.ImageList != null)
                        {
                            AddOrRemoveMissionImage(mission.MissionId, missionvm.ImageList, missionvm.DefaultMissionImg);
                        }
                        if (missionvm.YoutubeUrl != null)
                        {
                            AddOrRemoveVideoUrl(mission.MissionId, missionvm.YoutubeUrl);
                        }
                        if (missionvm.DocumentList != null)
                        {
                            AddOrRemoveDocument(mission.MissionId, missionvm.DocumentList);
                        }
                        if (missionvm.UpdatedMissionSKills != null)
                        {
                            AddOrRemoveMissionSkills(mission.MissionId, missionvm.UpdatedMissionSKills);
                        }
                    }
                    return "Updated";
                }
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }

        public bool MissionDelete(long MissionId)
        {
            try
            {
                Mission mission = _db.Missions.Find(MissionId)!;
                if (mission != null)
                {
                    mission.DeletedAt = DateTime.Now;
                    _db.Update(mission);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
