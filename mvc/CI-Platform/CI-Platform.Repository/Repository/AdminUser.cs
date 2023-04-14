using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class AdminUser : IAdminUser
    {
        private readonly CiDbContext _db;
        public AdminUser(CiDbContext db)
        {
            _db = db;
        }

        public List<User> users()
        {
            return _db.Users.Where(U => U.DeletedAt == null).ToList();
        }

        public bool IsDeleted(long UserId)
        {
            try
            {
                User user = _db.Users.FirstOrDefault(u => u.UserId == UserId);
                if (user != null)
                {
                    user.DeletedAt = DateTime.Now;
                    _db.Update(user);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
