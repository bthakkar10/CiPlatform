﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class UserProfile : IUserProfile
    {
        private readonly CiDbContext _db;
       
        public UserProfile(CiDbContext db)
        {
            _db = db;
            
        }
        public UserProfileViewModel GetUserDetails(long UserId)
        {
            User user = _db.Users.Where(u => u.UserId == UserId && u.DeletedAt == null).Include(u => u.UserSkills).ThenInclude(us => us.Skill).FirstOrDefault()!;
            UserProfileViewModel vm = new()
            {
                FirstName = user.FirstName,
                Surname = user.LastName,
                EmployeeId = user.EmployeeId,
                Department = user.Department,
                ProfileText = user.ProfileText,
                WhyIVolunteer = user.WhyIVolunteer,
                LinkedInUrl = user.LinkedInUrl,
                Avatar = user.Avtar,
                CountryId = user.CountryId,
                CityId = user.CityId,
                UserSkills = user.UserSkills.ToList(),
                Skills = _db.Skills.ToList(),
                Countries = _db.Countries.Where(c=>c.DeletedAt == null).ToList(),
                Cities = _db.Cities.Where(c => c.DeletedAt == null).ToList(),
            };

            return vm;
        }

        public bool ChangePassword(long UserId, string oldPassword, string newPassword)
        {
            oldPassword = BCrypt.Net.BCrypt.HashPassword(oldPassword);
            User user = _db.Users.FirstOrDefault(u => u.UserId == UserId)!;
            
            if (user.Password != oldPassword)
            {
                return false;
            }
            else
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.UpdatedAt = DateTime.Now;
                _db.Update(user);
                _db.SaveChanges();
                return true;
            }
        }

        public bool ContactUs(long UserId, string ContactSubject, string ContactMessage) 
        {
            if(ContactSubject != null && ContactMessage != null)
            {
                ContactU contact = new()
                {
                    UserId = UserId,
                    Subject = ContactSubject,
                    Message = ContactMessage
                };
                _db.Add(contact);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string EditUserProfile(long UserId, UserProfileViewModel vm)
        {
            try
            {
                if (_db.Users.FirstOrDefault(user => user.EmployeeId == vm.EmployeeId && user.UserId != vm.UserId) != null)
                {
                    return "Exists";
                }
                User user = _db.Users.FirstOrDefault(u => u.UserId == UserId && u.DeletedAt == null)!;

                user.FirstName = vm.FirstName;
                user.LastName = vm.Surname;
                user.CityId = vm.CityId;
                user.CountryId = vm.CountryId;
                user.EmployeeId = vm.EmployeeId;
                user.Manager = vm.Manager;
                user.Title = vm.Title;
                user.Department = vm.Department;
                user.ProfileText = vm.ProfileText;
                user.WhyIVolunteer = vm.WhyIVolunteer;
                user.Availability = vm.Availibility;
                user.LinkedInUrl = vm.LinkedInUrl;


                if (vm.UpdatedAvatar != null)
                {
                    //var OldImg = user.Avtar;
                    var NewImg = vm.UpdatedAvatar.FileName;
                    var guid = Guid.NewGuid().ToString()[..8];
                    var fileName = $"{guid}_{NewImg}"; // getting filename

                    
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/User", fileName);//for updating new img

                    user.Avtar = fileName;
                    using var stream = new FileStream(filePath, FileMode.Create);
                    vm.UpdatedAvatar.CopyTo(stream);

                }
                //for skills
                string[] SkillsArr = new string[0];
                if (vm.UpdatedUserSkills != null)
                {
                    SkillsArr = vm.UpdatedUserSkills.Split(',');
                }
                List<UserSkill> existingSkills = _db.UserSkills.Where(u => u.UserId == UserId).ToList();
                var SkillsToAdd = _db.Skills.Where(s => SkillsArr.Contains(s.SkillName)).Select(s => s.SkillId).ToList();
                var SkillsToRemove = existingSkills.Where(us => !SkillsToAdd.Contains(us.SkillId)).ToList();
                _db.UserSkills.RemoveRange(SkillsToRemove);

                //skills that needs to be added 
                foreach (var AddSkills in SkillsToAdd)
                {
                    if (!existingSkills.Any(u => u.SkillId == AddSkills))
                    {
                        UserSkill UserSkill = new UserSkill()
                        {
                            UserId = UserId,
                            SkillId = AddSkills
                        };
                        _db.Add(UserSkill);
                    }
                }
                _db.Update(user);
                _db.SaveChanges();
                return "Updated";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
