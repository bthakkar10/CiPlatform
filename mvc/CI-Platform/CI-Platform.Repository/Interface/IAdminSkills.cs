using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminSkills
    {
        public List<Skill> SkillList();

        public string SkillsAdd(AdminSkillsViewModel skillvm);

        public bool SkillDelete(long SkillId);

        public AdminSkillsViewModel GetSkills(long SkillId);

        public string EditSkill(AdminSkillsViewModel skillvm);
    }
}
