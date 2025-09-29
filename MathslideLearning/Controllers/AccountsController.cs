using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.AccountsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString))
                {
                    return ApiUnauthorized<object>("Invalid token");
                }

                var userId = int.Parse(userIdString);
                var userProfile = await _userService.GetUserProfileAsync(userId);
                return ApiOk(userProfile);
            }
            catch (Exception ex)
            {
                return ApiNotFound<object>(ex.Message);
            }
        }

        [HttpDelete("profile")]
        [Authorize]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return ApiUnauthorized<object>("Invalid token");
                }

                var result = await _userService.DeleteUserAsync(userId);

                if (!result)
                {
                    return ApiNotFound<object>("User not found or already deleted");
                }

                return ApiOk<object>(null, "Account deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiInternalServerError<object>("An error occurred while deleting the account", new { error = ex.Message });
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return ApiUnauthorized<object>("Invalid token");
                }

                var updatedUser = await _userService.UpdateUserAsync(userId, request);
                return ApiOk(updatedUser);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var newUser = await _userService.RegisterAsync(request);
                return ApiCreated(newUser);
            }
            catch (Exception ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ApiBadRequest<ModelStateDictionary>(ModelState, "Validation failed");
            }

            try
            {
                var token = await _userService.LoginAsync(request);
                return ApiOk(new { token });
            }
            catch (Exception ex)
            {
                return ApiUnauthorized<object>(ex.Message);
            }
        }
    }
}