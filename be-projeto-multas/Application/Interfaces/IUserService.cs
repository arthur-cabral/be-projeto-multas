using Application.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IQueryable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string id);
        Task<IdentityResult> PutUser(ApplicationUser user);
        Task<IdentityResult> DeleteUser(string userId);
    }
}
