using Microsoft.AspNet.Identity;
using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace IdentityApp.Identity
{
    public class ApplicationUserManager : UserManager<IdentityUser, string>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, string> store) : base(store)
        {
            this.UserTokenProvider = new MyUserTokenProvider();
            this.EmailService = new MyEmailService();
        }

    }

    public class MyUserTokenProvider : IUserTokenProvider<IdentityUser, string>
    {
        public Task<string> GenerateAsync(string purpose, UserManager<IdentityUser, string> manager, IdentityUser user)
        {
            if (purpose == "Confirmation")
            {
                string code = Guid.NewGuid().ToString();
                if (user != null)
                {
                    user.ConfirmationCode = code;
                    manager.UpdateAsync(user);
                }
                return Task.FromResult<string>(code);
            }
            throw new NotImplementedException();
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<IdentityUser, string> manager, IdentityUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException();
            }
            else return Task.FromResult<bool>(manager.SupportsUserEmail);
        }

        public Task NotifyAsync(string token, UserManager<IdentityUser, string> manager, IdentityUser user)
        {
            return Task.FromResult<int>(0);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<IdentityUser, string> manager, IdentityUser user)
        {
              return Task.FromResult<bool>(user.ConfirmationCode == token);
        }
    }

    public class MyEmailService : IIdentityMessageService
    {

        public async Task SendAsync(IdentityMessage message)
        {
            if (DestinationIsMail(message.Destination))
            {
                await this.SendEmailAsync(message);
            }
        }

        private async Task SendEmailAsync(IdentityMessage message)
        {
            SmtpClient Client = new SmtpClient("smtp.gmail.com");
            MailMessage Mail = new MailMessage
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };

            Mail.To.Add(message.Destination);
            Mail.From = new MailAddress("group18test@gmail.com");

            Client.Port = 587;
            Client.Credentials = new System.Net.NetworkCredential("group18test@gmail.com", "testkodi123");
            Client.EnableSsl = true;
            await Client.SendMailAsync(Mail);
        }

        public bool DestinationIsMail(string destination)
        {
            return destination.Contains<Char>('@');
        }
    }
}