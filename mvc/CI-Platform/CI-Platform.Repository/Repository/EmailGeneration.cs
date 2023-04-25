using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace CI_Platform.Repository.Repository
{
    public class EmailGeneration : IEmailGeneration
    {
        private readonly CiDbContext _db;

        public EmailGeneration(CiDbContext db) 
        {
            _db= db;
        }

       
        void IEmailGeneration.GenerateEmail(ForgotPasswordValidation obj)
        {
            Random random = new Random();

            int capitalCharCode = random.Next(65, 91);
            char randomCapitalChar = (char)capitalCharCode;


            int randomint = random.Next();


            int SmallcharCode = random.Next(97, 123);
            char randomChar = (char)SmallcharCode;

            String token = "";
            token += randomCapitalChar.ToString();
            token += randomint.ToString();
            token += randomChar.ToString();

            //string token = WebSecurity.GeneratePasswordResetToken(obj.Email);

            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "localhost";
            uriBuilder.Port = 44350;
            uriBuilder.Path = "Auth/ResetPass";
            uriBuilder.Query = "token=" + token;

            var PasswordResetLink = uriBuilder.ToString();

            var ResetPasswordInfo = new PasswordReset()
            {
                Email = obj.Email,
                Token = token
            };
            _db.Add(ResetPasswordInfo);
            _db.SaveChanges();

            var fromEmail = new MailAddress("ciplatformdemo@gmail.com");
            var toEmail = new MailAddress(obj.Email!);
            var fromEmailPassword = "pdckerdmuutmdzhz";
            string subject = "Reset Password";
            string body = PasswordResetLink;


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            MailMessage message = new MailMessage(fromEmail, toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }
}
