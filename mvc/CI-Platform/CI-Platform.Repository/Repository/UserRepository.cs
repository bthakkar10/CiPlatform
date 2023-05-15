using Azure.Core;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;


namespace CI_Platform.Repository.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly CiDbContext _db;
        //public readonly HttpContext _httpContext;
        public UserRepository(CiDbContext db) 
        {
            _db = db;
        }

        readonly String default_avtar = "profile-user.png";

        public User GetUserEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email)!;
        }

        public void Save()
        {
            _db.SaveChanges();  
        }

        public bool RegisterUser(RegistrationViewModel obj)
        {
            try
            {
                if (obj != null) 
                {
                    User user = new()
                    {
                        Email = obj.Email,
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        PhoneNumber = obj.PhoneNumber,
                        Password = BCrypt.Net.BCrypt.HashPassword(obj.Password),
                        Avtar = default_avtar,
                        Status = true,
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();

                    for (int i = 1; i <= 7; i++)
                    {
                        UserSetting userSetting = new UserSetting()
                        {
                            SettingId = i,
                            UserId = user.UserId,
                            IsEnabled = true,
                        };
                        _db.Add(userSetting);

                    }
                    _db.SaveChanges();

                }
                return true;    
            }
            catch(Exception) 
            {
                return false;   
            }
        }

        public string UpdatePassword(ResetPasswordValidation obj, HttpContext httpContext)
        {
            string token = httpContext.Request.Query["token"]!;
            PasswordReset LastData = _db.PasswordResets.Where(pr => pr.Token == token && pr.ExpirationTime>DateTime.Now && pr.IsUsed == false).OrderByDescending(pr => pr.Id).FirstOrDefault()!;
            if(LastData != null)
            {
                User user = _db.Users.FirstOrDefault(u => u.Email == LastData.Email)!;
                if(user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(obj.Password);
                    user.UpdatedAt = DateTime.Now;
                    LastData.IsUsed = true;
                    _db.Update(LastData);
                    _db.Update(user);
                    _db.SaveChanges();
                    return "changed";
                }
                else
                {
                    return "error";
                }
            }
            else
            {
                return "invalid";
            }
        }
    }
}
