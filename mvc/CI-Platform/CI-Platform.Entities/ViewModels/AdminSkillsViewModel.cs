using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminSkillsViewModel
    {
        public List<Skill> SkillList { get; set; } = new List<Skill>();

        public AdminSkillsViewModel()
        {
        }
        public long SkillId { get; set; } = 0;

        [Required(ErrorMessage = "Skill Name is Required")]
        public string? SkillName { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public byte Status { get; set; }

        public Skill GetSkillDetail { get; set; } = new Skill();

        public AdminSkillsViewModel(Skill GetSkillDetail)
        {
            SkillId = GetSkillDetail.SkillId;
            SkillName = GetSkillDetail.SkillName;   
            Status = GetSkillDetail.Status; 
        }
    }
}
