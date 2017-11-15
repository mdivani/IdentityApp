using IdentityDomain.Entities;
using IdentityDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityRepository.Repositories
{
    internal class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        //passes singletone context to baserepository
        internal RoleRepository(ApplicationDbContext context)
    : base(context)
        {
        }
        //finds by role, if doesn't exists returns null
        public Role FindByName(string roleName)
        {
            return Set.FirstOrDefault(x => x.RoleName == roleName);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return Set.FirstOrDefaultAsync(x => x.RoleName == roleName);
        }

        public Task<Role> FindByNameAsync(System.Threading.CancellationToken cancellationToken, string roleName)
        {
            return Set.FirstOrDefaultAsync(x => x.RoleName == roleName, cancellationToken);
        }
    }
}

