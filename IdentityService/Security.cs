using IdentityDomain.Entities;
using IdentityDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Services
{
    public class SecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region UsersActions
        public void AddUser(User user)
        {
            _unitOfWork.UserRepository.Add(user);
        }

        public void RemoveUser(User user)
        {
            _unitOfWork.UserRepository.Remove(user);
        }

        public User FindUserById(object id)
        {
            return _unitOfWork.UserRepository.FindById(id);
        }

        public User FindUserByIdAsync(object id)
        {
            return _unitOfWork.UserRepository.FindByIdAsync(id).Result;
        }

        public User FindUserByName(string userName)
        {
            return _unitOfWork.UserRepository.FindByUserName(userName);
        }

        public Task<User> FindUserByNameAsync(string userName)
        {
            return _unitOfWork.UserRepository.FindByUserNameAsync(userName);
        }

        public User FindUserByEmail(string email)
        {
            return _unitOfWork.UserRepository.FindByEmail(email);
        }

        public User FindUserByConfirmationCode(string token)
        {
            return _unitOfWork.UserRepository.FindByConfirmationCode(token);
        }

        public User FindUserByConfirmationCodeAsync(string token)
        {
            return _unitOfWork.UserRepository.FindByConfirmationCodeAsync(token).Result;
        }

        public void UpdateUser(User user)
        {
            _unitOfWork.UserRepository.Update(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _unitOfWork.UserRepository.GetAll();
        }
        #endregion

        #region RolesActions
        public void AddRole(Role role)
        {
            _unitOfWork.RoleRepository.Add(role);
        }

        public void RemoveRole(Role role)
        {
            _unitOfWork.RoleRepository.Remove(role);
        }

        public Role FindRoleById(object id)
        {
            return _unitOfWork.RoleRepository.FindById(id);
        }

        public Role FindByRoleName(string roleName)
        {
            return _unitOfWork.RoleRepository.FindByName(roleName);
        }

        public void UpdateRole(Role role)
        {
            _unitOfWork.RoleRepository.Update(role);
        }

        public IQueryable<Role> GetAllRoles()
        {
            return _unitOfWork.RoleRepository.GetAll().AsQueryable();
        }
        #endregion

        public ExternalLogin GetLoginByProviderAndKey(string loginProvider, string providerKey)
        {
            return _unitOfWork.ExternalLoginRepository.GetByProviderAndKey(loginProvider, providerKey);
        }

        public Task SaveChangesAsync()
        {
            return _unitOfWork.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _unitOfWork.SaveChanges();
        }
    }
}

