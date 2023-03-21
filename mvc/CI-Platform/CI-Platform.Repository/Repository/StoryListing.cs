using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class StoryListing : IStoryListing
    {
        private readonly CiDbContext _db;
        public StoryListing(CiDbContext db)
        {
            _db = db;
        }

        public List<Story> DisplayStoryCard(List<long> StoryIds)
        {
            List<Story> StoryList = _db.Stories.Where(m => StoryIds.Contains(m.StoryId))
                .Include(m => m.User)
                .Include(m => m.Mission)
                .ThenInclude(m => m.MissionTheme)
                .ToList();

            return StoryList;
        }

    }
}
