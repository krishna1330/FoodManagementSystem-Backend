using FMS.Business.Client.Models;
using FMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FoodController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly FoodDAC _foodDAC;

        public FoodController(IConfiguration configuration, FoodDAC foodDAC)
        {
            _configuration = configuration;
            _foodDAC = foodDAC;
        }

        [HttpGet("Food-Availability")]
        public async Task<IActionResult> GetFoodAvailabilityDataAsync(int month)
        {
            try
            {
                if(month == 0)
                {
                    return Conflict("Invalid month.");
                }

                var res = await _foodDAC.GetFoodAvailabilityDataAsync(month);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("Menu")]
        public async Task<IActionResult> GetMenuAsync()
        {
            try
            {
                var res = await _foodDAC.GetMenuAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost("Add-Food-Availability")]
        public async Task<IActionResult> AddFoodAvailabilityAsync([FromBody] AddFoodAvailability foodAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input.");
            }

            try
            {
                if (foodAvailability == null)
                {
                    return BadRequest("Food availability data cannot be null.");
                }

                var result = await _foodDAC.AddFoodAvailabilityAsync(foodAvailability);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding food availability: {ex.Message}");
            }
        }

        [HttpGet("User-Selected-Food")]
        public async Task<IActionResult> GetUserSelectedFoodByUserIDAsync(int userID, int month)
        {
            try
            {
                if(userID == 0 || month == 0)
                {
                    return Conflict("Invalid User ID or month");
                }

                var res = await _foodDAC.GetUserSelectedFoodByUserIDAsync(userID, month);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost("Add-User-Food")]
        public async Task<IActionResult> AddUserFoodAsync(AddUserFood food)
        {
            try
            {
                if (food.UserID <= 0 || food.SelectedDate == default || string.IsNullOrWhiteSpace(food.SelectedFood))
                {
                    return Conflict("Invalid User ID, selected date, or selected food.");
                }

                var res = await _foodDAC.AddUserFoodAsync(food);
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("Food-Count")]
        public async Task<IActionResult> GetEmployeeFoodCountAsync(DateTime selectedDate)
        {
            try
            {
                if (selectedDate == DateTime.MinValue)
                {
                    return Conflict("Invalid date");
                }

                var res = await _foodDAC.GetEmployeeFoodCountAsync(selectedDate);

                if (res == null)
                {
                    return NotFound("No food count data found for the selected date.");
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

    }
}
