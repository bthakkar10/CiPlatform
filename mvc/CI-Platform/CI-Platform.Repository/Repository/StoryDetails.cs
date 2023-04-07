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
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Http;

namespace CI_Platform.Repository.Repository
{
    public class StoryDetails : IStoryDetails
    {
        private readonly CiDbContext _db;
        //private readonly HttpRequest _httpRequest;
        //private readonly UrlHelper _urlHelper;

        public StoryDetails(CiDbContext db /*HttpRequest httpRequest, UrlHelper urlHelper*/)
        {
            _db = db;
           //_httpRequest = httpRequest;
           // _urlHelper = urlHelper;
        }

        public Story GetStoryDetails(long MissionId,long UserId)
        {
            Story story =  _db.Stories.
            Include(s => s.StoryMedia).
            Include(s => s.StoryInvites).
            Include(s => s.User).
            ThenInclude(su => su.City).
            ThenInclude(su => su.Country).
            Where(s => s.MissionId == MissionId && s.UserId == UserId).
            FirstOrDefault();
            return story;
        }

        public List<User> UserList(long UserId)
        {
            return _db.Users.Where(u => u.UserId != UserId).ToList();
            //return _db.Users.Where(u => u.UserId != UserId && !u.MissionApplications.Any(m => m.MissionId == MissionId && m.ApprovalStatus == "APPROVE")).ToList();
        }

        public void IncreaseViewCount(long UserId, long MissionId)
        {
            Story story = _db.Stories.Where(s => s.MissionId == MissionId && s.UserId == UserId && s.Status == "PUBLISHED").FirstOrDefault();
            if (story != null)
            {
                story.UserVisits = story.UserVisits++;
                story.UpdatedAt = DateTime.Now;
                _db.Update(story);
                _db.SaveChanges();
            }
        }

        //public async void StoryInvite(long ToUserId, long StoryId, long FromUserId,StoryDetailsViewModel vm)
        //{
        //    var StoryInvite = new StoryInvite()
        //    {
        //        ToUserId = ToUserId, 
        //        FromUserId = FromUserId,            
        //        StoryId = StoryId,
        //    };

        //    _db.StoryInvites.Add(StoryInvite);

        //    var StoryInviteLink = _urlHelper.Action("StoryDetails", "Story", new { StoryId = StoryId }/*, _httpRequest.Scheme*/);
        //    vm.InviteLink = StoryInviteLink;

        //    await _db.SaveChangesAsync();

        //    await _db.SendInvitationToCoWorker(ToUserId, FromUserId, vm);
        //}

        public async Task SendInvitationToCoWorker(long ToUserId, long FromUserId, StoryDetailsViewModel vm)
        {
            var Email = await _db.Users.Where(u => u.UserId == ToUserId).FirstOrDefaultAsync();

            var Sender = await _db.Users.Where(su => su.UserId == FromUserId).FirstOrDefaultAsync();

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
