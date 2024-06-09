using Application.Interfaces;
using Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            } 
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<ActionResult> PutUser(ApplicationUser user)
        {
            var updateUser = await _userService.PutUser(user);
            if (updateUser.Errors.Any())
            {
                return BadRequest(updateUser.Errors);
            }
            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var deleteUser = await _userService.DeleteUser(id);
            if (deleteUser.Errors.Any())
            {
                return BadRequest(deleteUser.Errors);
            }
            return NoContent();
        }
    }
}
