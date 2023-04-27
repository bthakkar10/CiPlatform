using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    [AllowAnonymous]
    public class PrivacyPolicyController : Controller
    {
       
        private readonly IAdminCms _adminCms;

        public PrivacyPolicyController(IAdminCms cms)
        {
            _adminCms = cms;
        }

        public IActionResult PrivacyPolicy()
        {
            try
            {
                PrivacyPolicyViewModel policyvm = new PrivacyPolicyViewModel();
                policyvm.GetCmsPages = _adminCms.CmsList();
                return View(policyvm);

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        
        }
       
    }
}
