using MathslideLearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace MathslideLearning.Controllers.Base
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult ApiOk<T>(T data, string message = "Success")
        {
            return StatusCode(200, ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult ApiCreated<T>(T data, string message = "Created successfully")
        {
            return StatusCode(201, ApiResponse<T>.Created(data, message));
        }

        protected IActionResult ApiBadRequest<T>(T data, string message = "Bad request")
        {
            return StatusCode(400, ApiResponse<T>.BadRequest(data, message));
        }

        protected IActionResult ApiNotFound<T>(string message = "Not found", T data = default)
        {
            return StatusCode(404, ApiResponse<T>.NotFound(data, message));
        }

        protected IActionResult ApiUnauthorized<T>(string message = "Unauthorized", T data = default)
        {
            return StatusCode(401, ApiResponse<T>.Unauthorized(data, message));
        }

        protected IActionResult ApiForbidden<T>(string message = "Forbidden", T data = default)
        {
            return StatusCode(403, ApiResponse<T>.Forbidden(data, message));
        }

        protected IActionResult ApiInternalServerError<T>(string message = "Internal server error", T data = default)
        {
            return StatusCode(500, ApiResponse<T>.InternalServerError(data, message));
        }
    }
}