﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IUserRepository
    {
        public User GetUserEmail(string email);
       
        public void UpdatePassword(ResetPasswordValidation obj);

        public bool RegisterUser(RegistrationViewModel obj);

    }
}
