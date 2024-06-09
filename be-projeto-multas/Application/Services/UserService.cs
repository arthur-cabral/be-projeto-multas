using Application.Exceptions;
using Application.Interfaces;
using Application.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            var users = _userManager.Users;
            return users;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }
            return user;
        }

        public async Task<IdentityResult> PutUser(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID {user.Id} not found." });
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            return result;
        }

        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID {userId} not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            return result;
        }
    }
}
