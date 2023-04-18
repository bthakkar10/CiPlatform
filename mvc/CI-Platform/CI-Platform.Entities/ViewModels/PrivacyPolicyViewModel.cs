using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class PrivacyPolicyViewModel
    {
        public List<CmsPage> GetCmsPages { get; set; }  = new List<CmsPage>();  
    }
}
