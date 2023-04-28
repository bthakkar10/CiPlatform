using Azure.Core;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CI_Platform.Repository.Generic;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Http;

namespace CI_Platform.Repository.Repository
{
    public class StoryDetails : IStoryDetails
    {
        private readonly CiDbContext _db;
        

        public StoryDetails(CiDbContext db )
        {
            _db = db;
           
        }

        public Story GetStoryDetails(long MissionId,long UserId)
        {
            Story story =  _db.Stories.Where(s=>s.DeletedAt == null && s.User.DeletedAt == null && s.Mission.DeletedAt == null).
            Include(s => s.StoryMedia).
            Include(s => s.StoryInvites).
            Include(s => s.User).
            ThenInclude(su => su.City).
            ThenInclude(su => su.Country).
            Where(s => s.MissionId == MissionId && s.UserId == UserId).
            FirstOrDefault()!;
            return story;
        }

        public List<User> UserList(long UserId)
        {
            return _db.Users.Where(u => u.UserId != UserId && u.DeletedAt == null && u.Status == true && u.Role==GenericEnum.Role.user.ToString()).ToList();
        }

        public void IncreaseViewCount(long UserId, long MissionId)
        {
            Story story = _db.Stories.Where(s => s.MissionId == MissionId && s.UserId == UserId && s.Status == GenericEnum.StoryStatus.PUBLISHED.ToString() && s.DeletedAt == null).FirstOrDefault()!;
            if (story != null)
            {
                story.UserVisits = story.UserVisits++;
                story.UpdatedAt = DateTime.Now;
                _db.Update(story);
                _db.SaveChanges();
            }
        }

        

        public async Task SendInvitationToCoWorker(long ToUserId, long FromUserId, StoryDetailsViewModel vm)
        {
            var Email = await _db.Users.Where(u => u.UserId == ToUserId && u.DeletedAt == null && u.Role == GenericEnum.Role.user.ToString()).FirstOrDefaultAsync()!;

            var Sender = await _db.Users.Where(su => su.UserId == FromUserId && su.DeletedAt == null && su.Role == GenericEnum.Role.user.ToString()).FirstOrDefaultAsync()!;

            var fromEmail = new MailAddress("ciplatformdemo@gmail.com");
            var toEmail = new MailAddress(Email.Email);
            var fromEmailPassword = "oretveqrckcgcoog";
            string subject = "Story Invitation";
            string body = "You Have Reciever Story Invitation From " + Sender.FirstName + " " + Sender.LastName + " For:\n\n" + vm.InviteLink;

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
