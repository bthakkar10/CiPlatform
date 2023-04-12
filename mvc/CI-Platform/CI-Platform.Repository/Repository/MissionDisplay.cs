using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class MissionDisplay : IMissionDisplay
    {
        private readonly CiDbContext _db;
        public MissionDisplay(CiDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Mission> DisplayMissionCardsDemo(List<long> MissionIds)
        {

            return _db.Missions.Where(m => MissionIds.Contains(m.MissionId))
                    .Include(m => m.City)
                    .Include(m => m.Country)
                    .Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill)
                    .Include(m => m.MissionTheme)
                    .Include(m => m.MissionRatings)
                    .Include(m => m.GoalMissions)
                    .Include(m => m.MissionApplications)
                    .Include(m => m.FavouriteMissions)
                    .Include(m => m.MissionMedia)
                    .ToList().OrderBy(ml => MissionIds.IndexOf(ml.MissionId));
        }


        public PageListViewModel.PageList<MissionListViewModel> FilterOnMission(MissionFilterQueryParams queryParams, long UserId)
        {
            List<MissionListViewModel> Mission = new List<MissionListViewModel>();

            var query = _db.Missions.AsQueryable();

            List<User> user = _db.Users.ToList();

            if (!string.IsNullOrEmpty(queryParams.CountryId))
            {
                long.TryParse(queryParams.CountryId, out long ConCountryId);
                query = query.Where(m => m.CountryId == ConCountryId);
            }

            if (!string.IsNullOrEmpty(queryParams.CityIds))
            {
                var cityId = queryParams.CityIds.Split(',').Select(s => long.Parse(s)).ToList();

                query = query.Where(m => cityId.Contains(m.CityId));
            }

            if (!string.IsNullOrEmpty(queryParams.ThemeIds))
            {
                var themeId = queryParams.ThemeIds.Split(',').Select(s => long.Parse(s)).ToList();

                query = query.Where(m => themeId.Contains(m.MissionThemeId));
            }

            if (!string.IsNullOrEmpty(queryParams.SkillIds))
            {
                var skillId = queryParams.SkillIds.Split(',').Select(s => long.Parse(s)).ToList();

                query = query.Where(m => m.MissionSkills.Any(s => skillId.Contains(s.SkillId)));
            }

            if (!string.IsNullOrEmpty(queryParams.SearchText))
            {
                query = query.Where(m => m.Title.ToLower().Contains(queryParams.SearchText.ToLower()) || m.OrganizationName.ToLower().Contains(queryParams.SearchText.ToLower()));
            }


            var MissionCardQuery = query.Select(mission => new MissionListViewModel()
            {
                MissionCard = mission,
                CityName = mission.City.CityName,
                ThemeTitle = mission.MissionTheme.Title,
                Applied = mission.MissionApplications.Any(missionApp => missionApp.UserId == UserId && missionApp.DeletedAt == null),
                favMission = mission.FavouriteMissions.Any(favMission => favMission.UserId == UserId && favMission.DeletedAt == null),
                avgRating = (float)mission.MissionRatings.Average(r => r.Rating),
                seatsLeft = mission.TotalSeats - mission.MissionApplications.Count(m => m.ApprovalStatus.Contains("APPROVE")),
                createdAt = mission.CreatedAt,
                startDate = mission.StartDate,
                skillName = mission.MissionSkills.Select(ms => ms.Skill.SkillName).ToList(),
                missionMedia = mission.MissionMedia.Select(mm => mm.MediaPath).ToList(),    
                goalMission = mission.GoalMissions.ToList(),
                missionInvites = mission.MissionInvites.Where(mi => mi.DeletedAt == null).ToList(),
                IsOngoing = (mission.StartDate < DateTime.Now) && (mission.EndDate > DateTime.Now) , 
                HasEndDatePassed = mission.EndDate < DateTime.Now, 
                HasMissionStarted = mission.StartDate < DateTime.Now,
                HasDeadlinePassed = mission.StartDate.Value.AddDays(-1) < DateTime.Now,
                //UserList = user,
                favouriteMission = mission.FavouriteMissions.ToList(),
            });

            if (queryParams.sortCase != null || queryParams.sortCase != 0)
            {
                switch (queryParams.sortCase)
                {
                    case 1: //newest
                        MissionCardQuery = MissionCardQuery.OrderByDescending(q => q.createdAt);
                        break;
                    case 2: //oldest
                        MissionCardQuery = MissionCardQuery.OrderBy(q => q.createdAt);
                        break;
                    case 3: //lowest seats
                        MissionCardQuery = MissionCardQuery.OrderBy(q => q.seatsLeft);
                        break;
                    case 4: //highest seats
                        MissionCardQuery = MissionCardQuery.OrderByDescending(q => q.seatsLeft);
                        break;
                    case 5: //regsitration deadline
                        MissionCardQuery = MissionCardQuery.OrderBy(q => q.startDate);
                        break;
                    case 6: //fav
                        //MissionCardQuery = MissionCardQuery.OrderByDescending(q=>q.favouriteMission.All(f=>f.UserId == UserId));
                        MissionCardQuery = MissionCardQuery.Where(q => q.favouriteMission.Any(f => f.UserId == UserId)).OrderByDescending(q => q.favouriteMission.Count(f => f.UserId == UserId));
                        break;
                    default:
                        MissionCardQuery = MissionCardQuery;
                        break;
                }
            }

            var totalRecords = MissionCardQuery.Count();

            var records = MissionCardQuery.Skip((queryParams.pageNo - 1 ) * queryParams.pagesize).Take(queryParams.pagesize).ToList();

            //public PageListViewModel.PageList<MissionListViewModel> FilterOnMission(MissionFilterQueryParams queryParams, long UserId)
            return new PageListViewModel.PageList<MissionListViewModel>(records, totalRecords, queryParams.pageNo);
           
        }


    }
}
