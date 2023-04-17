using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminCmsViewModel
    {
        public string Title { get; set; }   = string.Empty;

        public string CmsDescription { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";
    }
}
