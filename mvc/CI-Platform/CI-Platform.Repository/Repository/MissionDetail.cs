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
            Mission mission = _db.Missions.Where(mission=>mission.DeletedAt == null)
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.MissionRatings)
                .Include(m => m.MissionTheme)
                .Include(m=>m.Timesheets)
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

            return mission;
            

        }

        public List<Comment> GetApprovedComments(long MissionId)
        {
            var approvedComments = _db.Comments.Where(c => c.MissionId == MissionId && c.ApprovalStatus == "PUBLISHED")
                .Include(c => c.User).ToList();

            return approvedComments;
        }

        public List<MissionApplication> GetRecentVolunteers(long MissionId, long userId)
        {
            var recentVolunteers = _db.MissionApplications.Include(u => u.User).Where(u => u.MissionId == MissionId && u.UserId != userId && u.ApprovalStatus == "APPROVE").OrderByDescending(u => u.CreatedAt).ToList();

            return recentVolunteers;
        }

        public List<Mission> GetRelatedMissions(long MissionId)
        {
            var mission = _db.Missions.Where(m => m.MissionId == MissionId && m.DeletedAt == null).FirstOrDefault()!;

            var relatedMissions = new List<Mission>();

            relatedMissions.AddRange(_db.Missions.Where(m => m.MissionId != MissionId && m.CityId == mission.CityId && m.DeletedAt == null)
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.MissionRatings)
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkills).ThenInclude(m => m.Skill)
                .Include(m => m.MissionApplications)
                .Include(m => m.GoalMissions)
                .Include(m => m.FavouriteMissions)
                .Include(m => m.MissionMedia).Take(3));

            if (relatedMissions.Count < 3)
            {
                relatedMissions.AddRange(_db.Missions.Where(m => m.MissionId != MissionId && m.CountryId == mission.CountryId && m.DeletedAt == null)
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.MissionRatings)
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkills).ThenInclude(m => m.Skill)
                .Include(m => m.MissionApplications)
                .Include(m => m.GoalMissions)
                .Include(m => m.FavouriteMissions)
                .Include(m => m.MissionMedia).Take(3 - relatedMissions.Count));
            }

            if (relatedMissions.Count < 3)
            {
                relatedMissions.AddRange(_db.Missions.Where(m => m.MissionId != MissionId && m.MissionThemeId == mission.MissionThemeId && m.DeletedAt == null )
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.MissionRatings)
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkills).ThenInclude(m => m.Skill)
                .Include(m => m.MissionApplications)
                .Include(m => m.GoalMissions)
                .Include(m => m.FavouriteMissions)
                .Include(m => m.MissionMedia).Take(3 - relatedMissions.Count));
            }

            return relatedMissions;
        }

        public List<User> UserList(long UserId, long MissionId)
        {
            return _db.Users.Where(u => u.UserId != UserId && !u.MissionApplications.Any(m => m.MissionId == MissionId && m.ApprovalStatus == "APPROVE")).Include(m=>m.MissionInviteFromUsers).Include(m=>m.MissionInviteToUsers).ToList();
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
