//using AspNetCore;
using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using CI_Platform_web.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddScoped<CountryCityValidationFilter>();
//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.AddService<CountryCityValidationFilter>();
//});
builder.Services.AddScoped<CountryCityValidationFilter>();
builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.AddService<CountryCityValidationFilter>();
    })
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<CiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailGeneration, EmailGeneration>();
builder.Services.AddScoped<IFilter, Filter>();
builder.Services.AddScoped<IMissionDisplay, MissionDisplay>();
builder.Services.AddScoped<IMissionDetail, MissionDetail>();
builder.Services.AddScoped<IStoryListing, StoryListing>();
builder.Services.AddScoped<IShareStory, ShareStory>();
builder.Services.AddScoped<IStoryDetails, StoryDetails>();
builder.Services.AddScoped<IUserProfile, UserProfile>();
builder.Services.AddScoped<IVolunteeringTimesheet, VolunteeringTimesheet>();
builder.Services.AddScoped<IAdminUser, AdminUser>();
builder.Services.AddScoped<IAdminCms, AdminCms>();
builder.Services.AddScoped<IAdminSkills, AdminSkills>();
builder.Services.AddScoped<IAdminTheme, AdminTheme>();
builder.Services.AddScoped<IAdminApproval, AdminApproval>();
builder.Services.AddScoped<IAdminMission, AdminMission>();
builder.Services.AddScoped<IAdminBanner, AdminBanner>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
        ValidAudience = builder.Configuration["JwtSetting:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:Key"]))
    };
});

//builder.Services.AddMvc().AddSessionStateTempDataProvider();

builder.Services.AddSession();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
//app.UseMiddleware<TokenExpirationMiddleware>();
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrWhiteSpace(token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        // Set the user ID in the HTTP context
        context.Items["UserId"] = userId;
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseStatusCodePages(async context => {
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized || response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        response.Redirect("/Auth/Index");
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
