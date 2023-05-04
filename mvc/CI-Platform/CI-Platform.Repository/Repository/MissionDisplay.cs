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
using CI_Platform.Repository.Generic;

namespace CI_Platform.Repository.Repository
{
    public class MissionDisplay : IMissionDisplay
    {
        private readonly CiDbContext _db;
        public MissionDisplay(CiDbContext db)
        {
            _db = db;
        }

        public PageListViewModel.PageList<MissionListViewModel> FilterOnMission(MissionFilterQueryParams queryParams, long UserId)
        {
            var query = _db.Missions.Where(m => m.DeletedAt == null).AsQueryable();
            List<User> user = _db.Users.Where(user => user.DeletedAt == null && user.UserId != UserId).Include(m => m.MissionInviteFromUsers).Include(m => m.MissionInviteToUsers).ToList();
             
            
            var topThemes = _db.Missions
                            .Where(mission => mission.DeletedAt == null && mission.Status == true && mission.MissionTheme.Status == 1 && mission.MissionTheme.DeletedAt == null)
                            .GroupBy(mission => mission.MissionThemeId)
                            .Select(group => new { ThemeId = group.Key, Count = group.Count() })
                            .OrderByDescending(themeCount => themeCount.Count)
                            .Take(5)
                            .Select(themeCount => themeCount.ThemeId);

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
                query = query.Where(m => m.Title.ToLower().Contains(queryParams.SearchText.ToLower()) || m.OrganizationName!.ToLower().Contains(queryParams.SearchText.ToLower()));
            }


            var MissionCardQuery = query.Select(mission => new MissionListViewModel()
            {
                MissionCard = mission,
                CityName = mission.City.CityName,
                ThemeTitle = mission.MissionTheme.Title,
                Applied = mission.MissionApplications.Any(missionApp => missionApp.UserId == UserId && missionApp.User.DeletedAt == null && missionApp.Mission.DeletedAt == null && missionApp.DeletedAt == null && missionApp.ApprovalStatus == GenericEnum.ApplicationStatus.APPROVE.ToString()),
                favMission = mission.FavouriteMissions.Any(favMission => favMission.UserId == UserId && favMission.DeletedAt == null && favMission.User.DeletedAt == null && favMission.Mission.DeletedAt == null),
                avgRating = (float)mission.MissionRatings.Where(rate => rate.User.DeletedAt == null).Average(r => r.Rating),
                seatsLeft = mission.TotalSeats - mission.MissionApplications.Where(missionApp => missionApp.User.DeletedAt == null).Count(m => m.ApprovalStatus.Contains(GenericEnum.ApplicationStatus.APPROVE.ToString())),
                createdAt = mission.CreatedAt,
                startDate = mission.StartDate,
                deadline = mission.Deadline,
                skillName = mission.MissionSkills.Select(ms => ms.Skill.SkillName).ToList()!,
                missionMedia = mission.MissionMedia.Where(mm => mm.Defaultval == true).Select(mm => mm.MediaPath).ToList()!,
                goalMission = mission.GoalMissions.ToList(),
                missionInvites = mission.MissionInvites.Where(mi => mi.DeletedAt == null && mi.Mission.DeletedAt == null && mi.ToUser.DeletedAt == null && mi.FromUser.DeletedAt == null).ToList(),
                IsOngoing = (mission.StartDate < DateTime.Now) && (mission.EndDate > DateTime.Now),
                HasEndDatePassed = mission.EndDate < DateTime.Now,
                HasMissionStarted = mission.StartDate < DateTime.Now,
                HasDeadlinePassed = mission.Deadline < DateTime.Now,
                IsApplicationPending = mission.MissionApplications.Any(missionApp => missionApp.UserId == UserId && missionApp.DeletedAt == null && missionApp.ApprovalStatus == GenericEnum.ApplicationStatus.PENDING.ToString() && missionApp.Mission.DeletedAt == null && missionApp.User.DeletedAt == null),
                CoWorkersList = user.ToList(),
                favouriteMission = mission.FavouriteMissions.Where(favMission => favMission.User.DeletedAt == null).ToList(),
                updatedGoalValue = mission.Timesheets.Where(timesheet => timesheet.MissionId == mission.MissionId && timesheet.DeletedAt == null).Sum(timesheet => timesheet.Action),
            });

            if (queryParams.sortCase != 0)
            {
                MissionCardQuery = queryParams.sortCase switch
                {
                    //newest
                    1 => MissionCardQuery.OrderByDescending(q => q.createdAt),
                    //oldest
                    2 => MissionCardQuery.OrderBy(q => q.createdAt),
                    //lowest seats
                    3 => MissionCardQuery.OrderBy(q => q.seatsLeft),
                    //highest seats
                    4 => MissionCardQuery.OrderByDescending(q => q.seatsLeft),
                    //regsitration deadline
                    5 => MissionCardQuery.OrderBy(q => q.deadline),
                    //fav
                    6 => MissionCardQuery.Where(q => q.favouriteMission.Any(f => f.UserId == UserId)).OrderByDescending(q => q.favouriteMission.Count(f => f.UserId == UserId)),
                    _ => MissionCardQuery.OrderBy(q => q.MissionCard.MissionId),
                };
            }
            if (queryParams.ExploreCase != 0)
            {
                switch (queryParams.ExploreCase)
                {
                    case 1://Top themes (select 5 top themes)
                        MissionCardQuery = MissionCardQuery.Where(mission => topThemes.Contains(mission.MissionCard.MissionThemeId));
                        break;
                    case 2: //most rated
                        MissionCardQuery = MissionCardQuery.OrderByDescending(m => m.avgRating);
                        break;
                    case 3: //most favourite missions 
                        MissionCardQuery = MissionCardQuery.OrderByDescending(m => m.MissionCard.FavouriteMissions.Count());
                        break;
                    case 4: // random missions
                        MissionCardQuery = MissionCardQuery.Take(5);
                        break;
                }
            };

            var totalRecords = MissionCardQuery.Count();

            var records = MissionCardQuery.Skip((queryParams.pageNo - 1) * queryParams.pagesize).Take(queryParams.pagesize).ToList();


            return new PageListViewModel.PageList<MissionListViewModel>(records, totalRecords, queryParams.pageNo);

        }

        public string AddToFavourites(long UserId, long MissionId)
        {
            try
            {
                if (_db.FavouriteMissions.Any(fm => fm.MissionId == MissionId && fm.UserId == UserId && fm.User!.DeletedAt == null && fm.Mission!.DeletedAt == null))
                {
                    // Mission is already in favorites, return an error message or redirect back to the mission page
                    var FavouriteMissionId = _db.FavouriteMissions.Where(fm => fm.MissionId == MissionId && fm.UserId == UserId).FirstOrDefault()!;
                    _db.FavouriteMissions.Remove(FavouriteMissionId);
                    _db.SaveChanges();
                    return "Removed";
                }

                // Add the mission to favorites for the user
                var favoriteMission = new FavouriteMission { MissionId = MissionId, UserId = UserId };
                _db.FavouriteMissions.Add(favoriteMission);
                _db.SaveChanges();
                return "Added";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public byte Ratings(byte rating, long MissionId, long UserId)
        {

            var alreadyRated = _db.MissionRatings.SingleOrDefault(mr => mr.MissionId == MissionId && mr.UserId == UserId && mr.DeletedAt == null && mr.User.DeletedAt == null && mr.Mission.DeletedAt == null);

            if (alreadyRated != null)
            {
                alreadyRated.Rating = rating;
                _db.SaveChanges();
            }
            else
            {

                MissionRating newRating = new()
                {
                    UserId = UserId,
                    MissionId = MissionId,
                    Rating = rating
                };
                _db.MissionRatings.Add(newRating);
                _db.SaveChanges();
            }

            return rating;
        }
        public (int rating, int VolunteersRated) GetMissionRating(long missionId)
        {
            int rating = (int)_db.MissionRatings.Where(rate => rate.MissionId == missionId).Average(rate=>rate.Rating);
            int VolunteersRated = (int)_db.MissionRatings.Where(rate => rate.MissionId == missionId).Count();
            return (rating, VolunteersRated);
        }

        public bool AddComment(string comment, long MissionId, long UserId)
        {
            try
            {
                if (comment != null)
                {
                    Comment newComment = new() { UserId = UserId, MissionId = MissionId, CommentText = comment };
                    _db.Comments.Add(newComment);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ApplyToMission(long UserId, long MissionId)
        {
            try
            {
                MissionApplication AlreadyApplied = _db.MissionApplications.FirstOrDefault(m => m.MissionId == MissionId && m.UserId == UserId && m.DeletedAt == null)!;
                if (AlreadyApplied == null)
                {
                    MissionApplication missionApplication = new()
                    {
                        MissionId = MissionId,
                        UserId = UserId,
                        AppliedAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        ApprovalStatus = "PENDING"
                    };
                    _db.Add(missionApplication);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    //if status is declined and user applies again it should be changed to pending again
                    AlreadyApplied.ApprovalStatus = "PENDING";
                    AlreadyApplied.UpdatedAt = DateTime.Now;
                    _db.MissionApplications.Update(AlreadyApplied);
                    _db.SaveChanges();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
