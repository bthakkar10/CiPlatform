﻿using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
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

        public bool SkillsAdd(AdminSkillsViewModel skillvm)
        {
            try
            {
                Skill skill = new Skill()
                {
                    SkillName = skillvm.SkillName,
                    Status = skillvm.Status,
                    CreatedAt = DateTime.Now,
                };

                _db.Skills.Add(skill);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SkillDelete(long SkillId)
        {
            try
            {
                Skill skill = _db.Skills.Find(SkillId);
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
            Skill skill = _db.Skills.Find(SkillId);
            AdminSkillsViewModel skillvm = new AdminSkillsViewModel(skill);
            return skillvm;
        }

        public bool EditSkill(AdminSkillsViewModel skillvm)
        {
            Skill skill = _db.Skills.Find(skillvm.SkillId);

            if (skill != null)
            {
                skill.SkillName = skillvm.SkillName;
                skill.Status = skillvm.Status;
                skill.UpdatedAt = DateTime.Now;
                _db.Skills.Update(skill);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}