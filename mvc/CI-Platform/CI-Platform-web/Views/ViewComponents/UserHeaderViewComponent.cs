using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Text;
using System.Text.Json;

namespace CI_Platform_web.Views.ViewComponents
{
    public class UserHeaderViewComponent : ViewComponent
    {
      
        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            string Username = "";
            long UserId = 0;
            string Avtar = "";
            string Email = "";
            string Role = "";
            var customClaimForUser = HttpContext.User?.FindFirst("CustomClaimForUser")?.Value;
            if (customClaimForUser != null)
            {
                User UserModel = JsonSerializer.Deserialize<User>(customClaimForUser);
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
