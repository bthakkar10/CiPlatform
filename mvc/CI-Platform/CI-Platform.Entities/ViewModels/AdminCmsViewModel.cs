using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminCmsViewModel
    {
        public List<CmsPage> CmsList { get; set; } = new List<CmsPage>();

        [Required(ErrorMessage = "Title is Required!!")]
        public string Title { get; set; }   = string.Empty;

        [AllowHtml]
        [Required(ErrorMessage ="Description is Required!!")]
        public string CmsDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is Required!!")]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is Required")]
        public bool? Status { get; set; } 

        public AdminCmsViewModel()
        {
        }

        public long CmsId { get; set; } 

        public CmsPage GetCmsData { get; set; } = new CmsPage();

        public AdminCmsViewModel(CmsPage GetCmsData)
        {
            CmsId = GetCmsData.CmsPageId;
            Title = GetCmsData.Title!;   
            CmsDescription = GetCmsData.Description!;
            Slug= GetCmsData.Slug;
            Status = GetCmsData.Status;
        }
    }
}
