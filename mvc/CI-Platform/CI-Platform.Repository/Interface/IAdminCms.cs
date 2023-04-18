using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminCms
    {
        public List<CmsPage> CmsList();

        public bool CmsAdd(AdminCmsViewModel cmsvm);

        public bool CmsDelete(long CmsId);

        public AdminCmsViewModel GetCmsData(long CmsId);

        public bool EditCms(AdminCmsViewModel cmsvm);


    }
}
