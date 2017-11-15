using IdentityDomain.Entities;
using IdentityDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityRepository.Repositories
{
    internal class UserRepository : BaseRepository<User>, IUserRepository
    {
        //passes singletone context to baserepository
        internal UserRepository(ApplicationDbContext context)
    : base(context)
        {
        }
        //finds user by email, if doesn't exists returns null
        public User FindByEmail(string email)
        {
            return Set.FirstOrDefault(x => x.Email == email);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Set.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email)
        {
            return Set.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
        //finds user by username, if doesn't exists returns null
        public User FindByUserName(string username)
        {
            return Set.FirstOrDefault(x => x.Email == username);
        }

        public Task<User> FindByUserNameAsync(string username)
        {
            return Set.FirstOrDefaultAsync(x => x.Email == username);
        }

        public Task<User> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username)
        {
            return Set.FirstOrDefaultAsync(x => x.Email == username, cancellationToken);
        }

        public User FindByConfirmationCode(string confirmationCode)
        {
            return Set.FirstOrDefault(x => x.ConfirmationCode == confirmationCode);
        }

        public Task<User> FindByConfirmationCodeAsync(string confirmationCode)
        {
            return Set.FirstOrDefaultAsync(x => x.ConfirmationCode == confirmationCode);
        }

        public Task<User> FindByConfirmationCodeAsync(System.Threading.CancellationToken cancellationToken, string confirmationCode)
        {
            return Set.FirstOrDefaultAsync(x => x.ConfirmationCode == confirmationCode, cancellationToken);
        }

        public override void Update(User entity)
        {
            var dbEntity = FindById(entity.Email);
            if (dbEntity != null)
            {
                _context.Entry(dbEntity).CurrentValues.SetValues(entity);
                _context.Entry(dbEntity).State = EntityState.Modified;
            }
        }
    }
}

