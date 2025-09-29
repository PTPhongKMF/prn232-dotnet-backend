namespace MathslideLearning.Models;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public ApiResponse(int statusCode, string message, T data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    // Success responses
    public static ApiResponse<T> Ok(T data, string message = "Success")
    {
        return new ApiResponse<T>(200, message, data);
    }

    public static ApiResponse<T> Created(T data, string message = "Created successfully")
    {
        return new ApiResponse<T>(201, message, data);
    }

    // Error responses
    public static ApiResponse<T> BadRequest(T data = default, string message = "Bad request")
    {
        return new ApiResponse<T>(400, message, data);
    }

    public static ApiResponse<T> NotFound(T data = default, string message = "Not found")
    {
        return new ApiResponse<T>(404, message, data);
    }

    public static ApiResponse<T> Unauthorized(T data = default, string message = "Unauthorized")
    {
        return new ApiResponse<T>(401, message, data);
    }

    public static ApiResponse<T> Forbidden(T data = default, string message = "Forbidden")
    {
        return new ApiResponse<T>(403, message, data);
    }

    public static ApiResponse<T> InternalServerError(T data = default, string message = "Internal server error")
    {
        return new ApiResponse<T>(500, message, data);
    }
}
