namespace WarrantyBee.Shared.Infrastructure.Abstractions;

/// <summary>
/// Defines a service for publishing events to the centralized Event Manager.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event with a specific type and data.
    /// </summary>
    /// <param name="eventType">The type of the event.</param>
    /// <param name="data">The event data object.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync(string eventType, object data);
}
