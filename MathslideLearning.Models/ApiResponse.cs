namespace MathslideLearning.Models;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    private ApiResponse(int statusCode, string message, T? data = default)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }

    // Success responses
    public static ApiResponse<T> Response200(T data)
    {
        return new ApiResponse<T>(200, "Success", data);
    }

    public static ApiResponse<T> Response200(string message, T data)
    {
        return new ApiResponse<T>(200, message, data);
    }

    public static ApiResponse<T> Response201(T? data = default)
    {
        return new ApiResponse<T>(201, "Created successfully", data);
    }

    public static ApiResponse<T> Response201(string message, T? data = default)
    {
        return new ApiResponse<T>(201, message, data);
    }

    // Error responses
    public static ApiResponse<T> Response400(string message)
    {
        return new ApiResponse<T>(400, message);
    }

    public static ApiResponse<T> Response400(string message, T? data)
    {
        return new ApiResponse<T>(400, message, data);
    }

    public static ApiResponse<T> Response404(string message)
    {
        return new ApiResponse<T>(404, message);
    }

    public static ApiResponse<T> Response404(string message, T? data)
    {
        return new ApiResponse<T>(404, message, data);
    }

    public static ApiResponse<T> Response401(string message)
    {
        return new ApiResponse<T>(401, message);
    }

    public static ApiResponse<T> Response401(string message, T? data)
    {
        return new ApiResponse<T>(401, message, data);
    }

    public static ApiResponse<T> Response403(string message)
    {
        return new ApiResponse<T>(403, message);
    }

    public static ApiResponse<T> Response403(string message, T? data)
    {
        return new ApiResponse<T>(403, message, data);
    }

    public static ApiResponse<T> Response500(string message)
    {
        return new ApiResponse<T>(500, message);
    }

    public static ApiResponse<T> Response500(string message, T? data)
    {
        return new ApiResponse<T>(500, message, data);
    }
}
