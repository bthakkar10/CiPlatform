using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Entities.ViewModels
{
    public  class AdminBannerViewModel
    {
        public List<Banner> BannerList { get; set; } = new List<Banner>();

        [Required(ErrorMessage = "Title is Required!!")]
        public string? Title { get; set; } = string.Empty;

        [AllowHtml]
        [Required(ErrorMessage = "Description is Required!!")]
        public string? Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sort Order is Required!!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Sort Order should contain only digits!!")]
        public int? sortOrder { get; set; }  

        public IFormFile? UpdatedImg { get; set; } 
        
        public string? Image { get; set; }   

        public long BannerId { get; set; }

        public AdminBannerViewModel() { }

        public Banner GetBannerData { get; set; } = new Banner();

        public AdminBannerViewModel(Banner GetBannerData)
        {
            BannerId= GetBannerData.BannerId;
            sortOrder = GetBannerData.SortOrder;
            Description = GetBannerData.Description;
            Title = GetBannerData.Title;
            Image = GetBannerData.Image; 
        }
    }
}
