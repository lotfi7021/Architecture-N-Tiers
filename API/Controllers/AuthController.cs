using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Projet.DTOs.Auth;
using Projet.Services.Interfaces;
using LoginRequest = Projet.DTOs.Auth.LoginRequest;

namespace Projet.Service.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userService.LoginAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized();

            var token = _userService.GenerateJwtToken(user, _configuration);

            return Ok(new AuthResponse
            {
                Token = token,
                Role = user.Role.ToString()
            });
        }
    }
}
