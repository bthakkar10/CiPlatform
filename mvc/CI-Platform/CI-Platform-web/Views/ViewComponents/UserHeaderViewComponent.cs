using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace CI_Platform_web.Views.ViewComponents
{
    public class UserHeaderViewComponent : ViewComponent
    {

        private readonly CiDbContext _db;

        public UserHeaderViewComponent(CiDbContext db)
        {
            _db= db;    
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            string Username = "";
            long UserId = 0;
            string Avtar = "";
            string Email = "";
            string Role = "";
            //bool? Status = true;
            var customClaimForUser = HttpContext.User?.FindFirst("CustomClaimForUser")?.Value;
            var customClaimValue = JsonSerializer.Deserialize<User>(customClaimForUser);
            UserId = customClaimValue.UserId;

            if (UserId != 0)
            {
                User UserModel = _db.Users.Find(UserId)!;
                if (UserModel != null)
                {
                    Email = UserModel.Email!;
                    UserId = UserModel.UserId!;
                    Username = UserModel.FirstName + " " + UserModel.LastName;
                    Avtar = UserModel.Avtar!;
                    Role = UserModel.Role!;
                   
                }
            }
            HeaderViewModel vm = new()
            {
                Username = Username,
                UserId = UserId,
                Avtar = Avtar,
                Email = Email,
                Role = Role!, 
            };
            return View(viewName, vm);  
        }

    }
}
