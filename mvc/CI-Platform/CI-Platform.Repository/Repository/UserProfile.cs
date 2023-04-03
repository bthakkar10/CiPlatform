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
                UserSkills = user.UserSkills.ToList(),
            };

            return vm;
        }

    }
}
