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
    public class RoleStore : IRoleStore<IdentityRole, int>, IQueryableRoleStore<IdentityRole, int>, IDisposable
    {

        private readonly SecurityService _security;

        public RoleStore(IUnitOfWork unitOfWork)
        {
            _security = new SecurityService(unitOfWork);
        }


        public System.Threading.Tasks.Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var r = getRole(role);

            _security.AddRole(r);
            return _security.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var r = getRole(role);

            _security.RemoveRole(r);
            return _security.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task<IdentityRole> FindByIdAsync(int roleId)
        {
            var role = _security.FindRoleById(roleId);
            return Task.FromResult<IdentityRole>(getIdentityRole(role));
        }

        public System.Threading.Tasks.Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = _security.FindByRoleName(roleName);
            return Task.FromResult<IdentityRole>(getIdentityRole(role));
        }

        public System.Threading.Tasks.Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            var r = getRole(role);
            _security.UpdateRole(r);
            return _security.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        public IQueryable<IdentityRole> Roles
        {
            get
            {
                return _security.GetAllRoles().Select(x => getIdentityRole(x));
            }
        }

        private Role getRole(IdentityRole identityRole)
        {
            if (identityRole == null)
                return null;
            return new Role
            {
                RoleID = identityRole.Id,
                RoleName = identityRole.Name
            };
        }

        private IdentityRole getIdentityRole(Role role)
        {
            if (role == null)
                return null;
            return new IdentityRole
            {
                Id = role.RoleID,
                Name = role.RoleName
            };
        }
    }
}