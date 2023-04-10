using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public  interface IUserProfile
    {
        public UserProfileViewModel GetUserDetails(long UserId);

        public bool ChangePassword(long UserId, string oldPassword, string newPassword);

        public bool ContactUs(long UserId, string ContactSubject, string ContactMessage);


        public bool EditUserProfile(long UserId, UserProfileViewModel vm);
    }
}
