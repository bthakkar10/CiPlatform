using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;

namespace CI_Platform.Repository.Repository
{
    public class MissionDetail : IMissionDetail
    {


        private readonly CiDbContext _db;
        public MissionDetail(CiDbContext db)
        {
            _db = db;
        }
        public Mission MissionDetails(long MissionId)
        {

            Mission mission = _db.Missions.Where(m => m.DeletedAt == null && m.Status == true && m.City.DeletedAt == null && m.Country.DeletedAt == null)
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.MissionRatings)
                .Include(m => m.MissionTheme)
                .Include(m => m.Timesheets)
                .Include(m => m.MissionSkills)
                .ThenInclude(m => m.Skill)
                .Include(m => m.MissionApplications)
                .Include(m => m.GoalMissions)
                .Include(m => m.FavouriteMissions)
                .Include(m => m.MissionMedia)
                .Include(m => m.MissionDocuments)
                .Include(m => m.Comments)
                .ThenInclude(u => u.User)
                .FirstOrDefault(m => m.MissionId == MissionId)!;
            if(mission != null)
            {
                return mission;
            }
            return new Mission();
        }

        public List<Comment> GetApprovedComments(long MissionId)
        {
            List<Comment> approvedComments = _db.Comments.Where(c => c.MissionId == MissionId && c.User.DeletedAt == null && c.Mission.DeletedAt == null && c.DeletedAt == null &&c.Mission.Status==true && c.ApprovalStatus != GenericEnum.CommentStatus.DECLINED.ToString())
                .Include(c => c.User).ToList();
            return approvedComments;
        }

        public List<MissionApplication> GetRecentVolunteers(long MissionId, long userId)
        {
            var recentVolunteers = _db.MissionApplications.Include(u => u.User).Where(u => u.MissionId == MissionId && u.UserId != userId && u.ApprovalStatus == GenericEnum.ApplicationStatus.APPROVE.ToString() && u.User.DeletedAt == null && u.Mission.DeletedAt == null && u.Mission.Status == true).OrderByDescending(u => u.CreatedAt).ToList();

            return recentVolunteers;
        }

        public List<MissionListViewModel> GetRelatedMissions(long MissionId, long UserId)
        {
            var query = _db.Missions.Where(m => m.MissionId == MissionId && m.DeletedAt == null && m.Status == true).FirstOrDefault()!;
            if (query != null)
            {
                List<User> user = _db.Users.Where(user => user.DeletedAt == null && user.UserId != UserId).Include(m => m.MissionInviteFromUsers).Include(m => m.MissionInviteToUsers).ToList();

                var relatedMissions = _db.Missions
                                    .Where(m => m.MissionId != MissionId && m.DeletedAt == null && m.Status == true && (m.CityId == query.CityId || m.CountryId == query.CountryId || m.MissionThemeId == query.MissionThemeId))
                                    .OrderBy(m => m.CityId == query.CityId ? 0 : 1)
                                    .ThenBy(m => m.CountryId == query.CountryId ? 0 : 1)
                                    .ThenBy(m => m.MissionThemeId == query.MissionThemeId ? 0 : 1)
                                    .Take(3);


                var list = relatedMissions.Select(mission => new MissionListViewModel()
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
                }).ToList();

                return list;
            }
            return new List<MissionListViewModel>();
        }



        public List<User> UserList(long UserId, long MissionId)
        {
            return _db.Users.Where(u => u.UserId != UserId && u.DeletedAt == null && u.Status == true && u.Role == GenericEnum.Role.user.ToString() && !u.MissionApplications.Any(m => m.MissionId == MissionId && m.ApprovalStatus == GenericEnum.ApplicationStatus.APPROVE.ToString())).Include(m => m.MissionInviteFromUsers).Include(m => m.MissionInviteToUsers).ToList();
        }


        public async Task SendInvitationToCoWorker(long ToUserId, long FromUserId, MissionDetailViewModel viewmodel)
        {
            var Email = await _db.Users.Where(u => u.UserId == ToUserId && u.DeletedAt == null).FirstOrDefaultAsync()!;

            var Sender = await _db.Users.Where(su => su.UserId == FromUserId && su.DeletedAt == null).FirstOrDefaultAsync()!;

            var fromEmail = new MailAddress("ciplatformdemo@gmail.com");
            var toEmail = new MailAddress(Email.Email);
            var fromEmailPassword = "pdckerdmuutmdzhz";
            string subject = "Mission Invitation";
            string body = "You Have Reciever Mission Invitation From " + Sender.FirstName + " " + Sender.LastName + " For:\n\n" + viewmodel.link;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            var message = new MailMessage(fromEmail, toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);
        }


    }
}
