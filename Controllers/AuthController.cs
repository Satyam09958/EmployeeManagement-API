using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MasterRepo _repo;

        public AuthController(MasterRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            var result = _repo.Register(model);

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto model)
        {
            var user = _repo.Login(model);

            if (user == null)
            {
                return BadRequest("Invalid Email Or Password");
            }

            return Ok(user);
        }
    }
}
