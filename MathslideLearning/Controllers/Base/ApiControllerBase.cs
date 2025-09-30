using MathslideLearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace MathslideLearning.Controllers.Base
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult Api200<T>(T data)
        {
            var response = ApiResponse<T>.Response200(data);
            return StatusCode(200, response);
        }

        protected IActionResult Api200<T>(string message, T data)
        {
            var response = ApiResponse<T>.Response200(message, data);
            return StatusCode(200, response);
        }

        protected IActionResult Api201<T>(T? data = default)
        {
            var response = ApiResponse<T>.Response201(data);
            return StatusCode(201, response);
        }

        protected IActionResult Api201<T>(string message, T? data = default)
        {
            var response = ApiResponse<T>.Response201(message, data);
            return StatusCode(201, response);
        }

        protected IActionResult Api400<T>(string message)
        {
            var response = ApiResponse<T>.Response400(message);
            return StatusCode(400, response);
        }

        protected IActionResult Api400<T>(string message, T? data)
        {
            var response = ApiResponse<T>.Response400(message, data);
            return StatusCode(400, response);
        }

        protected IActionResult Api404<T>(string message)
        {
            var response = ApiResponse<T>.Response404(message);
            return StatusCode(404, response);
        }

        protected IActionResult Api404<T>(string message, T? data)
        {
            var response = ApiResponse<T>.Response404(message, data);
            return StatusCode(404, response);
        }

        protected IActionResult Api401<T>(string message)
        {
            var response = ApiResponse<T>.Response401(message);
            return StatusCode(401, response);
        }

        protected IActionResult Api401<T>(string message, T? data)
        {
            var response = ApiResponse<T>.Response401(message, data);
            return StatusCode(401, response);
        }

        protected IActionResult Api403<T>(string message)
        {
            var response = ApiResponse<T>.Response403(message);
            return StatusCode(403, response);
        }

        protected IActionResult Api403<T>(string message, T? data)
        {
            var response = ApiResponse<T>.Response403(message, data);
            return StatusCode(403, response);
        }

        protected IActionResult Api500<T>(string message)
        {
            var response = ApiResponse<T>.Response500(message);
            return StatusCode(500, response);
        }

        protected IActionResult Api500<T>(string message, T? data)
        {
            var response = ApiResponse<T>.Response500(message, data);
            return StatusCode(500, response);
        }
    }
}