using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using WarrantyBee.Shared.Core.Configuration;
using WarrantyBee.Shared.Core.Enums;
using WarrantyBee.Shared.Infrastructure.Abstractions;

namespace WarrantyBee.Shared.Infrastructure.Services;

/// <summary>
/// Provides telemetry and logging services, integrating with Better Stack for remote log ingestion using a background queue.
/// </summary>
public class TelemetryService : ITelemetryService
{
    private readonly ILogger<TelemetryService> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly BetterStackConfiguration? _config;
    private readonly RestClient? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="TelemetryService"/> class.
    /// </summary>
    /// <param name="logger">The .NET logger instance.</param>
    /// <param name="config">The application configuration containing Better Stack settings.</param>
    /// <param name="taskQueue">The background task queue for non-blocking ingestion.</param>
    public TelemetryService(ILogger<TelemetryService> logger, IOptions<AppConfiguration> config, IBackgroundTaskQueue taskQueue)
    {
        _logger = logger;
        _taskQueue = taskQueue;
        _config = config.Value.BetterStack;

        if (_config != null && !string.IsNullOrWhiteSpace(_config.Host) && !string.IsNullOrWhiteSpace(_config.AccessToken))
        {
            var host = _config.Host.StartsWith("http") ? _config.Host : $"https://{_config.Host}";
            _client = new RestClient(host);
        }
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public void TrackEvent(string eventName, IDictionary<string, object>? properties = null)
    {
        _logger.LogInformation("Event: {EventName}, Properties: {@Properties}", eventName, properties);

        _taskQueue.QueueBackgroundWorkItemAsync(token =>
        {
            SendToBetterStack(WarrantyBee.Shared.Core.Enums.LogLevel.Info, $"Event: {eventName}", properties);
            return ValueTask.CompletedTask;
        });
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public void Log(WarrantyBee.Shared.Core.Enums.LogLevel level, string message, IDictionary<string, object>? context = null)
    {
        var dotNetLevel = MapLevel(level);
        _logger.Log(dotNetLevel, "Message: {Message}, Context: {@Context}", message, context);

        _taskQueue.QueueBackgroundWorkItemAsync(token =>
        {
            SendToBetterStack(level, message, context);
            return ValueTask.CompletedTask;
        });
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public void Log(WarrantyBee.Shared.Core.Enums.LogLevel level, Exception exception, IDictionary<string, object>? context = null)
    {
        var dotNetLevel = MapLevel(level);
        _logger.Log(dotNetLevel, exception, "Exception, Context: {@Context}", context);

        _taskQueue.QueueBackgroundWorkItemAsync(token =>
        {
            var extendedContext = context ?? new Dictionary<string, object>();
            extendedContext["Exception"] = exception.Message;
            extendedContext["StackTrace"] = exception.StackTrace ?? string.Empty;
            SendToBetterStack(level, exception.Message, extendedContext);
            return ValueTask.CompletedTask;
        });
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public void TrackMetric(string metricName, double value, IDictionary<string, object>? properties = null)
    {
        _logger.LogInformation("Metric: {MetricName}, Value: {Value}, Properties: {@Properties}", metricName, value, properties);

        _taskQueue.QueueBackgroundWorkItemAsync(token =>
        {
            var context = properties ?? new Dictionary<string, object>();
            context["MetricValue"] = value;
            SendToBetterStack(WarrantyBee.Shared.Core.Enums.LogLevel.Info, $"Metric: {metricName}", context);
            return ValueTask.CompletedTask;
        });
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public void Flush() { }

    private Microsoft.Extensions.Logging.LogLevel MapLevel(WarrantyBee.Shared.Core.Enums.LogLevel level) => level switch
    {
        WarrantyBee.Shared.Core.Enums.LogLevel.Info => Microsoft.Extensions.Logging.LogLevel.Information,
        WarrantyBee.Shared.Core.Enums.LogLevel.Warn => Microsoft.Extensions.Logging.LogLevel.Warning,
        WarrantyBee.Shared.Core.Enums.LogLevel.Error => Microsoft.Extensions.Logging.LogLevel.Error,
        WarrantyBee.Shared.Core.Enums.LogLevel.Debug => Microsoft.Extensions.Logging.LogLevel.Debug,
        _ => Microsoft.Extensions.Logging.LogLevel.Information
    };

    private async void SendToBetterStack(WarrantyBee.Shared.Core.Enums.LogLevel level, string message, IDictionary<string, object>? context = null)
    {
        if (_client == null || _config == null) return;

        try
        {
            var request = new RestRequest("/", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_config.AccessToken}");

            var payload = new
            {
                dt = DateTime.UtcNow.ToString("O"),
                level = level.ToString(),
                message = message,
                metadata = context
            };

            request.AddJsonBody(payload);
            await _client.ExecuteAsync(request);
        }
        catch
        {
            // Silent fail for telemetry
        }
    }
}
