using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace IdentityApp.Identity
{
    public class IdentityUser : IUser<string>
    {

        public IdentityUser() { }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<IdentityUser, string> manager)
        {
            var identity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }

        public IdentityUser(string Email)
            : this()
        {
            this.UserName = this.Id = this.Email = Email;
        }

        public IdentityUser(string email, string confirmationCode)
        {
            this.UserName = this.Id = this.Email = email;
            this.ConfirmationCode = confirmationCode;
        }

        public string Id { get; set; }
        public string ConfirmationCode { get; set; }
        public bool MailIsConfirmed { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool RememberMe { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public DateTime CreateDate { get; set; }


    }
}