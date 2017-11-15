using IdentityDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDomain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User FindByUserName(string username);
        Task<User> FindByUserNameAsync(string username);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);

        User FindByEmail(string email);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email);

        User FindByConfirmationCode(string token);
        Task<User> FindByConfirmationCodeAsync(string token);
        Task<User> FindByConfirmationCodeAsync(CancellationToken cancellationToken, string token);
    }
}
