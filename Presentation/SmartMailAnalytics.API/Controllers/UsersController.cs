using Microsoft.AspNetCore.Mvc;
using SmartMailAnalytics.Application.DTOs.UserDtos;
using SmartMailAnalytics.Application.Services.UserServices;


namespace SmartMailAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var values = await _userService.GetUsersAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var value = await _userService.GetByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            await _userService.CreateUserAsync(dto);
            return Ok("User created successfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            await _userService.UpdateUserAsync(dto);
            return Ok("User updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("User deleted successfully");
        }
    }
}
