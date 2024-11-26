using Microsoft.AspNetCore.Mvc;
using Projet.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var listUsers = _service.GetUsers();
            if (listUsers != null)
            {
                return new OkObjectResult(listUsers);
            }
            return NotFound();
        }
    }
}
