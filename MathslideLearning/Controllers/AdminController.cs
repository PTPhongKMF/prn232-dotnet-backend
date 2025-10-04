using MathslideLearning.Business.Interfaces;
using MathslideLearning.Controllers.Base;
using MathslideLearning.Models.AccountsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace MathslideLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Api200(users);
        }

        [HttpPut("users/{id}/permissions")]
        public async Task<IActionResult> UpdateUserPermissions(int id, [FromBody] AdminUpdateUserRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Api400<ModelStateDictionary>("Validation failed", ModelState);
            }

            try
            {
                var updatedUser = await _userService.AdminUpdateUserAsync(id, request);
                return Api200(updatedUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return Api404<object>(ex.Message);
                }
                return Api400<object>(ex.Message);
            }
        }
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.SoftDeleteUserAsync(id);
                if (!success)
                {
                    return Api404<object>("User not found");
                }
                return Api200<object>("User deleted successfully", null);
            }
            catch (Exception ex)
            {
                return Api400<object>(ex.Message, new { message = ex.Message });
            }
        }
    }
}