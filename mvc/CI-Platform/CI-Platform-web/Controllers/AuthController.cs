using CI_Platform.Entities;
using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CI_Platform_web.Controllers
{
    public class AuthController : Controller
    {
        //private readonly ILogger<AuthController> _logger;
        private readonly CiDbContext _db;

        //public AuthController(ILogger<AuthController> logger)
        //{
        //    _logger = logger;
        //}

        public AuthController(CiDbContext db)
        {
            _db = db;
        
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgetPass()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User obj)
        {
            if (ModelState.IsValid)
            {
                var obj2 = _db.Users.Where(a => a.Email.Equals(obj.Email) && a.Password.Equals(obj.Password))
                    .FirstOrDefault();
                if (obj2 != null)
                {

                    return RedirectToAction("Home", "HomePage");
                }
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Auth");
            }
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}