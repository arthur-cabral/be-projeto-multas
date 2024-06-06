using Application.DTO.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AccessTokenResponseDTO> Login(LoginModelDTO loginModel);
        Task Register(RegisterModelDTO registerModel);
        Task<AccessTokenResponseDTO> RefreshToken(TokenModelDTO tokenModel);
        Task Revoke(string username);
        Task AddUserToRole(string email, string roleName);
        Task CreateRole(string roleName);
    }
}
