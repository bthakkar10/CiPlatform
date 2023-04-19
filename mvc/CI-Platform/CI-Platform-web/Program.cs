//using AspNetCore;
using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
