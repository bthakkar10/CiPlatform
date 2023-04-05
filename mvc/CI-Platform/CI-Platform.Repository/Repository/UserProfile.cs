using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
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
            User user = _db.Users.Where(u => u.UserId == UserId).Include(u=>u.UserSkills).ThenInclude(us=>us.Skill).FirstOrDefault();
            var vm = new UserProfileViewModel()
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
                CityId= user.CityId,    
                UserSkills = user.UserSkills.ToList(),
                Skills = _db.Skills.ToList(),
                Countries= _db.Countries.ToList(),
                Cities = _db.Cities.ToList(),   
            };

            return vm;
        }

        public bool ChangePassword(long userId, string oldPassword, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user.Password != oldPassword)
            {
                return false;
            }
            else
            {
                user.Password = newPassword;
                user.UpdatedAt= DateTime.Now;   
                _db.Update(user);
                _db.SaveChanges();
                return true;
            }
        }
    }
}
