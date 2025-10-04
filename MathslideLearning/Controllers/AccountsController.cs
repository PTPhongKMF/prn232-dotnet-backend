using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models;
using MathslideLearning.Models.AccountsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Runtime.InteropServices;
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
                    return Api401<UserResponseDto>("Invalid token");
                }

                var userId = int.Parse(userIdString);
                var userProfile = await _userService.GetUserProfileAsync(userId);
                return Api200(userProfile);
            }
            catch (Exception ex)
            {
                return Api404<UserResponseDto>(ex.Message);
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
                    return Api401<object>("Invalid token");
                }

                var result = await _userService.SoftDeleteUserAsync(userId);

                if (!result)
                {
                    return Api404<object>("User not found or already deleted");
                }

                return Api200<object?>("Account deleted successfully", null);
            }
            catch
            {
                return Api500<object>("An error occurred while deleting the account");
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    return Api401<UserResponseDto>("Invalid token");
                }

                var updatedUser = await _userService.UpdateUserAsync(userId, request);
                return Api200(updatedUser);
            }
            catch (Exception ex)
            {
                return Api400<UserResponseDto>(ex.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var newUser = await _userService.RegisterAsync(request);
                return Api201(newUser);
            }
            catch (Exception ex)
            {
                return Api400<UserResponseDto>(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<object>("Validation failed", ModelState);
            }

            try
            {
                var response = await _userService.LoginAsync(request);
                return Api200(response);
            }
            catch (Exception ex)
            {
                return Api401<object>(ex.Message);
            }
        }
    }
}