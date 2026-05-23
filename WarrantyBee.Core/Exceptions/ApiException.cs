using WarrantyBee.Shared.Core.Exceptions;

namespace WarrantyBee.Shared.Core.Exceptions;

/// <summary>
/// Represents errors that occur during application execution, specifically mapped to API error codes.
/// </summary>
public class ApiException : Exception
{
    /// <summary>
    /// Gets the application error associated with this exception.
    /// </summary>
    public AppError Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified predefined error.
    /// </summary>
    /// <param name="error">The application error details.</param>
    public ApiException(AppError error) : base(error.Message)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a custom message.
    /// </summary>
    /// <param name="error">The base error details.</param>
    /// <param name="customMessage">A custom message that overrides the default error message.</param>
    public ApiException(AppError error, string customMessage) : base(customMessage)
    {
        Error = new AppError(error.Code, customMessage, error.Status);
    }
}
