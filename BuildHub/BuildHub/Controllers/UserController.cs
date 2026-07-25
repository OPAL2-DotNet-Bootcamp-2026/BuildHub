using BuildHub.DTOs;
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

        //path: baseUrl/GetAllUsers
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            List<User> users = _userService.GetAllUsers();
            if (users.Count ==0)
            {
                return NoContent();
            }

            return Ok(users);
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


        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] User user )
        {
            int created = _userService.AddUser(user);
            if (created == 0)
            {
                return BadRequest();
            }

            return Ok(created);
        }

        [HttpPut("UpdateUserEmail")]
        public IActionResult UpdateUserEmail(int id, UserUpdateDTO user)
        {
            UserResponseDTO response = _userService.UpdateUser(id, user);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int id) { 
            bool deleted = _userService.DeleteUser(id);
            
            if (!deleted)
            {
                return NotFound(new { message = $"User with ID {id} was not found." });
            }

            return Ok();
        }
    }
}
