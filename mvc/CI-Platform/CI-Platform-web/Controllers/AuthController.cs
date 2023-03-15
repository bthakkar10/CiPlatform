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


        public AuthController(ILogger<AuthController> logger, IUserRepository dbUserRepository, IEmailGeneration emailGeneration)
        {
            _logger = logger;
            _dbUserRepository = dbUserRepository;
            _emailGeneration= emailGeneration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string Email, string Password, long UserId)
        {
            if (ModelState.IsValid)
            {
                var cursor = _dbUserRepository.GetUserEmail(Email);
                if (cursor == null)
                {
                    TempData["error"] = "User does not exist.Please register first";
                    return View("Register");
                }
                else
                {
                    if(cursor.Password == Password)
                    {
                        TempData["success"] = "Login Successful!!";
                        HttpContext.Session.SetString("SEmail", Email);
                        HttpContext.Session.SetString("Id", cursor.UserId.ToString());
                        HttpContext.Session.SetString("Username", cursor.FirstName + " " + cursor.LastName);

                        return RedirectToAction("HomePage", "Home");

                    }
                    else
                    {
                        TempData["error"] = "Password is Incorrect. Please try again";
                        return View();
                    }
                     
                }
            }
            TempData["error"] = "invalid model state";

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
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                var UserEmail = _dbUserRepository.GetUserEmail(obj.Email);
                if (UserEmail == null)
                {
                    if (form["ConfirmPassword"] == obj.Password)
                    {
                        _dbUserRepository.Add(obj);
                        _dbUserRepository.Save();
                        TempData["success"] = "Registeration successful!!";
                        return RedirectToAction("HomePage", "Home");
                    }
                    else
                    {
                        TempData["error"] = "Password does not Match!!";
                        return View(obj);
                    }
                }
                else
                {
                    TempData["error"] = "Email Already Exists!!";
                    return View(obj);
                }
            }
            return View(obj);
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
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}