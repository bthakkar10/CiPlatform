using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class PrivacyPolicyController : Controller
    {
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
       
    }
}
