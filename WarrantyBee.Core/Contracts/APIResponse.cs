namespace WarrantyBee.Shared.Core.Contracts;

/// <summary>
/// Represents an error detail in an API response.
/// </summary>
public class APIError
{
    /// <summary>
    /// Gets or sets the application-specific error code.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets a human-readable message describing the error.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="APIError"/> class.
    /// </summary>
    public APIError() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="APIError"/> class with specified code and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public APIError(int code, string message)
    {
        Code = code;
        Message = message;
    }
}

/// <summary>
/// Represents a standardized wrapper for all API responses in the WarrantyBee ecosystem.
/// </summary>
/// <typeparam name="T">The type of the data being returned.</typeparam>
public class APIResponse<T>
{
    /// <summary>
    /// Gets or sets the data payload of the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the error information if the request failed.
    /// </summary>
    public APIError? Error { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="APIResponse{T}"/> class.
    /// </summary>
    public APIResponse() { }

    /// <summary>
    /// Initializes a new success response with the specified data.
    /// </summary>
    /// <param name="data">The data payload.</param>
    public APIResponse(T? data) => Data = data;

    /// <summary>
    /// Initializes a new failure response with the specified error.
    /// </summary>
    /// <param name="error">The error detail.</param>
    public APIResponse(APIError error) => Error = error;

    /// <summary>
    /// Initializes a new response with both data and error (rarely used).
    /// </summary>
    /// <param name="data">The data payload.</param>
    /// <param name="error">The error detail.</param>
    public APIResponse(T? data, APIError? error)
    {
        Data = data;
        Error = error;
    }

    /// <summary>
    /// Creates a success response.
    /// </summary>
    /// <param name="data">The data to return.</param>
    /// <returns>A success <see cref="APIResponse{T}"/>.</returns>
    public static APIResponse<T> Success(T? data) => new(data);

    /// <summary>
    /// Creates a failure response.
    /// </summary>
    /// <param name="code">The application error code.</param>
    /// <param name="message">The failure message.</param>
    /// <returns>A failure <see cref="APIResponse{T}"/>.</returns>
    public static APIResponse<T> Failure(int code, string message) => new(new APIError(code, message));
}
