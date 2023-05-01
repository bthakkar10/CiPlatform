using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminThemeViewModel
    {

        public List<MissionTheme> ThemeList { get; set; } = new List<MissionTheme>();

        public AdminThemeViewModel()
        {
        }

        public long ThemeId { get; set; } = 0;

        [Required(ErrorMessage = "Theme Name is Required")]
        [MaxLength(120, ErrorMessage = "Maximum 120 characters are allowed!!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetical characters are allowed!!")]
        public string? ThemeName { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public byte Status { get; set; }

        public MissionTheme GetThemeDetails { get; set; } = new MissionTheme();

        public AdminThemeViewModel(MissionTheme GetThemeDetails)
        {
            ThemeId = GetThemeDetails.MissionThemeId;
            ThemeName = GetThemeDetails.Title;
            Status = GetThemeDetails.Status;
        }
    }
}
