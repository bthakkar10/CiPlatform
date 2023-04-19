using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
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
            var customClaimForUser = HttpContext.User?.FindFirst("CustomClaimForUser")?.Value;
            if (customClaimForUser != null)
            {
                var UserModel = JsonSerializer.Deserialize<User>(customClaimForUser);
                if (UserModel != null)
                {
                    Email = UserModel.Email!;
                    UserId = UserModel.UserId!;
                    Username = UserModel.FirstName + " " + UserModel.LastName;
                    Avtar = UserModel.Avtar!;
                }
            }

            HeaderViewModel vm = new()
            {
                Username = Username,
                UserId = UserId,
                Avtar = Avtar,
                Email = Email
            };
            return View(viewName, vm);  
        }

    }
}
