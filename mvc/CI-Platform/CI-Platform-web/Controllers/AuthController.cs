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
using CI_Platform.Entities.Auth;
using CI_Platform_web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using CI_Platform.Repository.Generic;
using PublicResXFileCodeGenerator;
namespace CI_Platform_web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _dbUserRepository;
        private readonly IEmailGeneration _emailGeneration;
        private readonly IConfiguration _configuration;
        private readonly CiDbContext _db;
        //private readonly HttpContext _httpContext;

        public AuthController(ILogger<AuthController> logger, IUserRepository dbUserRepository, IEmailGeneration emailGeneration, CiDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _dbUserRepository = dbUserRepository;
            _emailGeneration = emailGeneration;
            _db = db;
            _configuration = configuration;
            //_httpContext = httpContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            Response.Headers.Remove("Authorization");
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("SEmail");
            HttpContext.Session.Remove("Id");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(UserLoginViewModel obj, long UserId, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                User DoesUserExists = _dbUserRepository.GetUserEmail(obj.Email);
                if (DoesUserExists == null)
                {
                    TempData["error"] = "User does not exist.Please register first";
                    return View("Register");
                }
                else if(DoesUserExists.Status != true)
                {
                    TempData["error"] = "User is blocked!!";
                    return View();
                }
                else if( DoesUserExists.DeletedAt != null)
                {
                    TempData["error"] = "User is deleted!!";
                    return View();
                }
                else
                {
                    bool DoesPasswordMatch = BCrypt.Net.BCrypt.Verify(obj.Password, DoesUserExists.Password);
                    if (DoesPasswordMatch)
                    {
                        var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();

                        var token = JwtTokenHelper.GenerateToken(jwtSettings, DoesUserExists);

                        HttpContext.Session.SetString("Token", token);


                        TempData["success"] = "Login Successful!!";

                        HttpContext.Session.SetString("SEmail", obj.Email);
                        HttpContext.Session.SetString("Id", DoesUserExists.UserId.ToString());
                        HttpContext.Session.SetString("Username", DoesUserExists.FirstName + " " + DoesUserExists.LastName);

                        if (DoesUserExists.Role == GenericEnum.Role.user.ToString())
                        {
                            if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else if ((DoesUserExists.CountryId == 0 || DoesUserExists.CountryId == null) && (DoesUserExists.CityId == 0 || DoesUserExists.CityId == null))
                            {
                                TempData["success"] = "Please enter your name, email, country and city first!!!";
                                return RedirectToAction("UserProfile", "User", new {UserId = DoesUserExists.UserId});
                            }
                            else
                            {
                                return RedirectToAction("HomePage", "Home");
                            }
                        }
                        else if(DoesUserExists.Role == GenericEnum.Role.admin.ToString())
                        {
                            return RedirectToAction("User", "Admin");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Password is Incorrect. Please try again";
                        return View();
                    }
                     
                }
            }
            TempData["error"] = Messages.Error ;

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
                User user = _dbUserRepository.GetUserEmail(obj.Email);
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

        [AllowAnonymous]
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
                        TempData["error"] = Messages.Error ;
                        return View(obj);
                    }
                }
                else
                {
                    TempData["error"] = "Email " + Messages.Exists ;
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
                    if(_dbUserRepository.UpdatePassword(obj, HttpContext) == "changed")
                    {
                        TempData["success"] = "Password " + Messages.Update + " Please login now";
                        return View("Index");
                    }
                    else if(_dbUserRepository.UpdatePassword(obj, HttpContext) == "invalid")
                    {
                        TempData["error"] = "Invalid Link!! Please try again!!";
                        return View("Index");
                    }
                    else
                    {
                        TempData["error"] = Messages.Error ;
                        return View("Index");
                    }
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

            //await HttpContext.SignOutAsync("Bearer");

            Response.Headers.Remove("Authorization");
            HttpContext.Session.Clear();    
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("SEmail");
            HttpContext.Session.Remove("Id");
            return RedirectToAction("Index");
        }

        //to check sesssion
        public IActionResult SessionStatus()
        {
            bool sessionExists = (HttpContext.Session.GetString("Token") == null);
            return Json(new { sessionExists });
        }
        [AllowAnonymous]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}