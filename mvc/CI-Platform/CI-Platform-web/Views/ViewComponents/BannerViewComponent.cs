using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace CI_Platform_web.Views.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {

        private readonly IAdminBanner _adminBanner;
         public BannerViewComponent(IAdminBanner adminBanner)
        {
            _adminBanner = adminBanner;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            AdminBannerViewModel vm = new();
            vm.BannerList = _adminBanner.BannerList();
            return View(viewName, vm);
        }

    }
}