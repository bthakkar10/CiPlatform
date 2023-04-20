using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace CI_Platform.Repository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly CiDbContext _db;
        public UserRepository(CiDbContext db) : base(db) 
        {
            _db = db;
        }

        String default_avtar = "profile-user.png";

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
                //string? passwordHash = "";
                if (obj != null) 
                {
                    User user = new User()
                    {
                        Email = obj.Email,
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        PhoneNumber = obj.PhoneNumber,
                        Password = BCrypt.Net.BCrypt.HashPassword(obj.Password),
                        Avtar = default_avtar,
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();  
                }
                return true;    
            }
            catch(Exception) 
            {
                return false;   
            }
        }

        public void UpdatePassword(ResetPasswordValidation obj)
        {
            PasswordReset LastData = _db.PasswordResets.OrderBy(i => i.Id).Last();
            User Change = _db.Users.FirstOrDefault(u => u.Email == LastData.Email)!;
            Change.Password = BCrypt.Net.BCrypt.HashPassword(obj.Password);
            Change.UpdatedAt = DateTime.Now;
            _db.Update(Change);
            _db.SaveChanges();
        }


    }
}
