using IdentityDomain.Entities;
using IdentityDomain.Interfaces;
using IdentityService.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IdentityApp.Identity
{
    public class UserStore : IUserTokenProvider<IdentityUser, string>, IUserLoginStore<IdentityUser, string>, IUserClaimStore<IdentityUser, string>, IUserRoleStore<IdentityUser, string>, IUserPasswordStore<IdentityUser, string>, IUserSecurityStampStore<IdentityUser, string>, IUserStore<IdentityUser, string>, IUserEmailStore<IdentityUser, string>, IDisposable
    {
        private readonly SecurityService _security;

        public UserStore(IUnitOfWork unitOfWork)
        {
            _security = new SecurityService(unitOfWork);
        }

        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            user.CreateDate = DateTime.Now;
            var u = getUser(user);

            _security.AddUser(u);
            return _security.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = getUser(user);

            _security.RemoveUser(u);
            return _security.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            var user = _security.FindUserByIdAsync(userId);
            return Task.FromResult<IdentityUser>(getIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _security.FindUserByNameAsync(userName).Result;
            return Task.FromResult<IdentityUser>(getIdentityUser(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentException("user");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            populateUser(u, user);

            _security.UpdateUser(u);
            return _security.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        public Task AddClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = new Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            _security.UpdateUser(u);
            return _security.SaveChangesAsync();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<System.Security.Claims.Claim>>(u.Claims.Select(x => new System.Security.Claims.Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _security.UpdateUser(u);
            return _security.SaveChangesAsync();
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);
            _security.UpdateUser(u);
            return _security.SaveChangesAsync();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var identityUser = default(IdentityUser);

            var l = _security.GetLoginByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = getIdentityUser(l.User);

            return Task.FromResult<IdentityUser>(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);
            _security.UpdateUser(u);
            return _security.SaveChangesAsync();
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            var r = _security.FindByRoleName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            u.Roles.Add(r);
            _security.UpdateUser(u);

            return _security.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.RoleName).ToList());
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<bool>(u.Roles.Any(x => x.RoleName == roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _security.FindUserByIdAsync(user.Email);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var r = u.Roles.FirstOrDefault(x => x.RoleName == roleName);
            u.Roles.Remove(r);

            _security.UpdateUser(u);
            return _security.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        private User getUser(IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;

            var user = new User();
            populateUser(user, identityUser);

            return user;
        }

        private void populateUser(User user, IdentityUser identityUser)
        {
            //user.UserID = identityUser.Email;
            user.Email = identityUser.Email;
            //user.Username = identityUser.UserName;
            user.FirstName = identityUser.FirstName;
            user.LastName = identityUser.LastName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
            user.ConfirmationCode = identityUser.ConfirmationCode;
            user.CreateDate = identityUser.CreateDate;
        }

        private IdentityUser getIdentityUser(User user)
        {
            if (user == null)
                return null;

            var identityUser = new IdentityUser();
            populateIdentityUser(identityUser, user);

            return identityUser;
        }

        private void populateIdentityUser(IdentityUser identityUser, User user)
        {
            //identityUser.Email = user.UserID;
            identityUser.Id = user.Email;
            identityUser.Email = user.Email;
            identityUser.UserName = user.Email;
            identityUser.FirstName = user.FirstName;
            identityUser.LastName = user.LastName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
            identityUser.ConfirmationCode = user.ConfirmationCode;
            identityUser.MailIsConfirmed = user.IsConfirmed;
            identityUser.CreateDate = user.CreateDate;
        }

        //todo: implement interface
        #region UserEmailStore
        public Task SetEmailAsync(IdentityUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            var u = _security.FindUserByIdAsync(user.Email);
            return Task.FromResult(u.IsConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            var u = getUser(user);
            u.IsConfirmed = confirmed;
            _security.UpdateUser(u);
            return Task.FromResult(_security.SaveChangesAsync());
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            var user = _security.FindUserByEmail(email);
            return Task.FromResult<IdentityUser>(getIdentityUser(user));
        }

        public Task<IdentityUser> FindByEmailConfirmationCodeAsync(string token)
        {
            var user = _security.FindUserByConfirmationCodeAsync(token);
            return Task.FromResult<IdentityUser>(getIdentityUser(user));
        }

        public IdentityUser FindByEmailConfirmationCode(string token)
        {
            var user = _security.FindUserByConfirmationCode(token);
            return getIdentityUser(user);
        }

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

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<IdentityUser, string> manager, IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task NotifyAsync(string token, UserManager<IdentityUser, string> manager, IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<IdentityUser, string> manager, IdentityUser user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}