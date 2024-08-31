using FMS.Business.Client.Models;
using FMS.Business.DatabaseObjects;
using FMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UsersDAC _usersDAC;

        public UsersController(IConfiguration configuration, UsersDAC usersDAC)
        {
            _configuration = configuration;
            _usersDAC = usersDAC;
        }

        [HttpGet("Get-Admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var admins = await _usersDAC.GetAllAdmins();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete-User")]
        public async Task<IActionResult> DeleteUserByID(int userID)
        {
            try
            {
                if (userID == 0)
                {
                    return Conflict("Invalid user ID.");
                }

                string res = await _usersDAC.DeleteUserByID(userID);

                if (res == "User not found." || res == "You cannot delete Super Admin.")
                {
                    return Conflict(res);
                }

                if (res.Contains("already deleted"))
                {
                    return Conflict(res);
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost("Add-User")]
        public async Task<IActionResult> AddUserAsync(AddUser addUser)
        {
            try
            {
                if(addUser == null)
                {
                    return Conflict("Invalid input.");
                }

                var users = await _usersDAC.AddUserAsync(addUser);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("Get-Employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _usersDAC.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
