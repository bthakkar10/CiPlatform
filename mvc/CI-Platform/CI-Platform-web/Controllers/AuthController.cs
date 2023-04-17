using CI_Platform.Entities;
using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Repository;

namespace CI_Platform_web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _dbUserRepository;
        private readonly IEmailGeneration _emailGeneration;
        private readonly CiDbContext _db;


        public AuthController(ILogger<AuthController> logger, IUserRepository dbUserRepository, IEmailGeneration emailGeneration, CiDbContext db)
        {
            _logger = logger;
            _dbUserRepository = dbUserRepository;
            _emailGeneration= emailGeneration;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
   
        public IActionResult Index(UserLoginViewModel obj, long UserId)
        {
            if (ModelState.IsValid)
            {
                var DoesEmailExists = _dbUserRepository.GetUserEmail(obj.Email);
                if (DoesEmailExists == null)
                {
                    TempData["error"] = "User does not exist.Please register first";
                    return View("Register");
                }
                else
                {
                    if(DoesEmailExists.Password == obj.Password)
                    {
                        TempData["success"] = "Login Successful!!";
                        HttpContext.Session.SetString("SEmail", obj.Email);
                        HttpContext.Session.SetString("Id", DoesEmailExists.UserId.ToString());
                        HttpContext.Session.SetString("Username", DoesEmailExists.FirstName + " " + DoesEmailExists.LastName);

                        return RedirectToAction("HomePage", "Home");

                    }
                    else
                    {
                        TempData["error"] = "Password is Incorrect. Please try again";
                        return View();
                    }
                     
                }
            }
            TempData["error"] = "Something went wrong!!";

            return View();
        }



        public IActionResult ForgetPass()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult ForgetPass(ForgotPasswordValidation obj)
        {
            if (ModelState.IsValid)
            {
                var user = _dbUserRepository.GetUserEmail(obj.Email);
                if (user == null)
                {
                    TempData["error"] = "User does not exists!!";
                    return View(obj);
                }
                else
                {
                    _emailGeneration.GenerateEmail(obj);
                    TempData["success"] = "Link sent successfully!! Please check your email";

                }
            }
            return View(obj);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
    
        public IActionResult Register(RegistrationViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var UserEmail = _dbUserRepository.GetUserEmail(obj.Email);
                if (UserEmail == null)
                {
                    if (_dbUserRepository.RegisterUser(obj))
                    {
                        TempData["success"] = "Registeration successful!!";
                         return RedirectToAction("Index", "Auth");
                    }
                    else
                    {
                        TempData["error"] = "Some error occured!! Please try again later!!";
                        return View(obj);
                    }
                }
                else
                {
                    TempData["error"] = "Email Already Exists!!";
                    return View(obj);
                }
            }
            return View();
        }

        public IActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPass(ResetPasswordValidation obj, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (form["ConfirmPassword"] == obj.Password)
                {
                    _dbUserRepository.UpdatePassword(obj);
                    TempData["success"] = "Password updated successfully!! Please login now";
                    return View("Index");
                }
                else
                {
                    TempData["error"] = "Password does not Match!!";
                    return View(obj);
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            //_db.SignOutAsync();
            //System.Web.Security.FormsAuthentication.SignOut();
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("SEmail");
            HttpContext.Session.Remove("Id");
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}