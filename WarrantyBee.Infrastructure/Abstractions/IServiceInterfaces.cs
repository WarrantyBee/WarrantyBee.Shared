using WarrantyBee.Shared.Core.Enums;

namespace WarrantyBee.Shared.Infrastructure.Abstractions;

/// <summary>
/// Represents the payload for a notification to be sent to a user.
/// </summary>
public class NotificationPayload
{
    /// <summary>
    /// Gets or sets the recipient of the notification (e.g., email address).
    /// </summary>
    public string Recipient { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dynamic macros to be replaced in the notification template.
    /// </summary>
    public IDictionary<string, string> DynamicMacros { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the type of notification.
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationPayload"/> class.
    /// </summary>
    public NotificationPayload() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationPayload"/> class with specified details.
    /// </summary>
    /// <param name="recipient">The recipient.</param>
    /// <param name="dynamicMacros">The dynamic macros.</param>
    /// <param name="type">The notification type.</param>
    public NotificationPayload(string recipient, IDictionary<string, string> dynamicMacros, NotificationType type)
    {
        Recipient = recipient;
        DynamicMacros = dynamicMacros;
        Type = type;
    }
}

/// <summary>
/// Defines the contract for email sending services.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email based on the specified notification payload.
    /// </summary>
    /// <param name="notification">The notification payload.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendAsync(NotificationPayload notification);
}

/// <summary>
/// Defines the contract for CAPTCHA validation services.
/// </summary>
public interface ICaptchaService
{
    /// <summary>
    /// Validates a CAPTCHA response.
    /// </summary>
    /// <param name="captchaResponse">The CAPTCHA response to validate.</param>
    /// <returns>True if the CAPTCHA is valid; otherwise, false.</returns>
    Task<bool> ValidateAsync(string captchaResponse);
}

/// <summary>
/// Defines the contract for file storage services.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Uploads a file to the storage service.
    /// </summary>
    /// <param name="fileStream">The stream of the file to upload.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The content type of the file.</param>
    /// <returns>The URL of the uploaded file.</returns>
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);

    /// <summary>
    /// Deletes a file from the storage service by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the file.</param>
    /// <returns>True if the file was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Deletes a file from the storage service by its URL.
    /// </summary>
    /// <param name="url">The URL of the file to delete.</param>
    /// <returns>True if the file was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteByUrlAsync(string url);
}
