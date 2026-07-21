using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController: ControllerBase 
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // path: url/user/GetUserById/{id}
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            //should replace user with response dto
            User user = _userService.GetUserById(id);
            
            if (user == null)
                return NotFound(new { message = $"User with ID {id} was not found." });
            
            return Ok(user);
        }
    }
}
