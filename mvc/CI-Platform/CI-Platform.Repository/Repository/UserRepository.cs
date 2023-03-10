using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly CiDbContext _db;
        public UserRepository(CiDbContext db) : base(db) 
        {
            _db = db;
        }

        public User GetUserEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public void Save()
        {
            _db.SaveChanges();  
        }

        public void UpdatePassword(ResetPasswordValidation obj)
        {
            PasswordReset LastData = _db.PasswordResets.OrderBy(i => i.Id).Last();
            User Change = _db.Users.FirstOrDefault(u => u.Email == LastData.Email);
            Change.Password = obj.Password;
            Change.UpdatedAt = DateTime.Now;
            _db.Update(Change);
            _db.SaveChanges();
        }


    }
}
