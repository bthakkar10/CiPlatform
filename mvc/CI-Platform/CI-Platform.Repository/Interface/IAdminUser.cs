using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminUser
    {
        public List<User> users();

        public bool IsUserDeleted(long UserId);

        public string IsUserAdded(AdminUserViewModel vm);

        public AdminUserViewModel GetDataOnEdit(long UserId);

        public string EditUser(AdminUserViewModel vm);
    }
}
