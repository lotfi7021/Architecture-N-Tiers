using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet.DTOs.Auth;
using Projet.Entities;
using Projet.Services.Interfaces;

namespace Projet.Service.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password
            };

            var createdUser = await _userService.RegisterAsync(user);

            return Ok(new
            {
                createdUser.Id,
                createdUser.Username,
                createdUser.Email,
                createdUser.Role
            });
        }

        [Authorize(Roles = "Client")]
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            return Ok(new
            {
                Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Username = User.Identity?.Name,
                Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }
}
