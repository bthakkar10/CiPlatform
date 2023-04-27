using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CI_Platform_web.Utility
{
    public class CountryCityValidationFilter : IActionFilter
    {
        private readonly IUserProfile _userProfile;

        public CountryCityValidationFilter(IUserProfile userProfile)
        {
            _userProfile= userProfile;  
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();

            if((controller == "Auth") || (controller == "User" && (action == "UserProfile" || action == "EditUserProfile")) || action== "GetCitiesByCountry" || (controller=="Home" && action=="MissionDetail") || (controller == "Story" && action == "StoryDetail"))
            {
                // Allow login page to load even if country and city are not set
                return;
            }

            var userId = context.HttpContext.Session.GetString("Id");
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userProfile.GetUserDetails(Convert.ToInt64(userId));
                if (user.CountryId == null || user.CityId == null)
                {
                    // Redirect to the user profile page if country and city are not set
                    if (controller != "User" || action != "UserProfile")
                    {
                        context.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "User", action = "UserProfile", UserId = userId })
                        );
                    }
                }
            }
            else
            {
                // Redirect to login page if user is not logged in and trying to access any page other than the login page
                if (controller != "Auth" || action != "Index")
                {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Auth", action = "Index" })
                    );
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }


    }
}
