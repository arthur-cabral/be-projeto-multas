using Application.Interfaces;
using Application.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.JWT;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Application.Exceptions;
using Application.Responses;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModelDTO loginModel)
        {
            try
            {
                var login = await _authService.Login(loginModel);
                return Ok(login);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<MessageResponse>> Register([FromBody] RegisterModelDTO registerModel)
        {
            try
            {
                await _authService.Register(registerModel);
                return Ok(new MessageResponse("Usuário criado com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<AccessTokenResponseDTO>> RefreshToken(TokenModelDTO tokenModel)
        {
            try
            {
                var refreshToken = await _authService.RefreshToken(tokenModel);
                return Ok(refreshToken);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("revoke/{username}")]
        public async Task<ActionResult> Revoke(string username)
        {
            try
            {
                await _authService.Revoke(username);
                return NoContent();
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("addUserToRole")]
        public async Task<ActionResult<MessageResponse>> AddUserToRole(string email, string roleName)
        {
            try
            {
                await _authService.AddUserToRole(email, roleName);
                return Ok(new MessageResponse("Usuário " + email + " adicionado a role " + roleName));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("createRole")]
        public async Task<ActionResult<MessageResponse>> CreateRole(string roleName)
        {
            try
            {
                await _authService.CreateRole(roleName);
                return Ok(new MessageResponse("Role " + roleName + " criada com sucesso!"));
            }
            catch (DuplicatedObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
