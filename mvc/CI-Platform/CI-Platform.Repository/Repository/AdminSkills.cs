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
    public class AdminSkills : IAdminSkills
    {
        private readonly CiDbContext _db;

        public AdminSkills(CiDbContext db)
        {
            _db = db;
        }
        public List<Skill> SkillList()
        {
            return _db.Skills.Where(skills => skills.DeletedAt == null).ToList();
        }

        public string SkillsAdd(AdminSkillsViewModel skillvm)
        {
            try
            {
                Skill DoesSkillExist = _db.Skills.FirstOrDefault(skill => skill.SkillName!.ToLower() == skillvm.SkillName.ToLower())!;
                if (DoesSkillExist == null)
                {
                    Skill skill = new()
                    {
                        SkillName = skillvm.SkillName,
                        Status = skillvm.Status,
                        CreatedAt = DateTime.Now,
                    };
                    _db.Skills.Add(skill);
                    _db.SaveChanges();
                    return "Added";
                }
                else
                {
                    if (DoesSkillExist != null && DoesSkillExist.DeletedAt == null)
                    {
                        return "Exists";
                    }
                    else
                    {
                        DoesSkillExist.DeletedAt = null;
                        DoesSkillExist.UpdatedAt = DateTime.Now;
                        _db.Update(DoesSkillExist);
                        _db.SaveChanges();
                        return "Added";
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool SkillDelete(long SkillId)
        {
            try
            {
                Skill skill = _db.Skills.Find(SkillId)!;
                if (skill != null)
                {
                    skill.DeletedAt = DateTime.Now;
                    _db.Update(skill);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AdminSkillsViewModel GetSkills(long SkillId)
        {
            Skill skill = _db.Skills.Find(SkillId)!;
            AdminSkillsViewModel skillvm = new(skill);
            return skillvm;
        }

        public string EditSkill(AdminSkillsViewModel skillvm)
        {
            try
            {
                if (_db.Skills.FirstOrDefault(skill => skill.SkillName!.ToLower() == skillvm.SkillName!.ToLower() && skill.SkillId != skillvm.SkillId) != null)
                {
                    return "Exists";
                }
                Skill skill = _db.Skills.Find(skillvm.SkillId)!;
                if (skill != null)
                {
                    skill.SkillName = skillvm.SkillName;
                    skill.Status = skillvm.Status;
                    skill.UpdatedAt = DateTime.Now;
                    _db.Skills.Update(skill);
                    _db.SaveChanges();
                    return "Updated";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
