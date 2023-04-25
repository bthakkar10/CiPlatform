using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminBanner
    {
        public List<Banner> BannerList();

        public bool BannerAdd(AdminBannerViewModel bannervm);

        public bool BannerDelete(long BannerId);

        public AdminBannerViewModel GetBannerData(long BannerId);

        public bool EditBanner(AdminBannerViewModel bannervm);
    }
}
