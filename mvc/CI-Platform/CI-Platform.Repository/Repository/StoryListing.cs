using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CI_Platform.Repository.Repository
{
    public class StoryListing : IStoryListing
    {
        private readonly CiDbContext _db;
        public StoryListing(CiDbContext db)
        {
            _db = db;
        }

      
        public StoryListingViewModel SPStory(MissionFilterQueryParams queryParams)
        {

            string CountryId = queryParams.CountryId;
            string CityIds = queryParams.CityIds;
            string ThemeIds = queryParams.ThemeIds;
            string SkillIds = queryParams.SkillIds;
            string searchText = queryParams.SearchText;
            int pageNo = queryParams.pageNo;
            int pagesize = queryParams.pagesize;

            StoryListingViewModel storyViewModel = new();
            List<Story> stories = new();


            IConfigurationRoot _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spFilterStory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@countryId", SqlDbType.VarChar).Value = CountryId != null ? CountryId : null;
                command.Parameters.Add("@cityId", SqlDbType.VarChar).Value = CityIds != null ? CityIds : null;
                command.Parameters.Add("@themeId", SqlDbType.VarChar).Value = ThemeIds != null ? ThemeIds : null;
                command.Parameters.Add("@skillId", SqlDbType.VarChar).Value = SkillIds != null ? SkillIds : null;
                command.Parameters.Add("@searchText", SqlDbType.VarChar).Value = searchText;

                command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pagesize;
                command.Parameters.Add("@pageNo", SqlDbType.Int).Value = pageNo;
                SqlDataReader reader = command.ExecuteReader();

                List<long> StoryIds = new List<long>();
                while (reader.Read())
                {
                    long totalRecords = reader.GetInt32("TotalRecords");
                    storyViewModel.totalRecords = totalRecords;
                }
                reader.NextResult();

                while (reader.Read())
                {
                    long storyId = reader.GetInt64("story_id");
                    StoryIds.Add(storyId);
                }
                foreach (long storyId in StoryIds)
                {
                    Story story = _db.Stories.Include(m => m.Mission).ThenInclude(m=>m.MissionTheme).Include(m => m.User).Include(s => s.StoryMedia).FirstOrDefault(s => s.StoryId == storyId && s.DeletedAt == null && s.User.DeletedAt == null && s.Mission.DeletedAt == null)!;
                    if (story != null)
                    {
                        stories.Add(story);
                    }
                }
                storyViewModel.DisplayStoryCard = stories;
            }
            return storyViewModel;
        }

    }
}
