using Application.Interfaces;
using Application.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.JWT;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult> Register([FromBody] RegisterModelDTO registerModel)
        {
            try
            {
                await _authService.Register(registerModel);
                return Ok("User created successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult> RefreshToken(TokenModelDTO tokenModel)
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

        [Authorize]
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
    }
}
