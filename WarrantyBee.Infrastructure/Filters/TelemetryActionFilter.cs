using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using WarrantyBee.Shared.Core.Enums;
using WarrantyBee.Shared.Infrastructure.Abstractions;

namespace WarrantyBee.Shared.Infrastructure.Filters;

/// <summary>
/// A global action filter that tracks request latency and logs endpoint execution metadata.
/// </summary>
public class TelemetryActionFilter : IAsyncActionFilter
{
    private readonly ITelemetryService _telemetry;

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public TelemetryActionFilter(ITelemetryService telemetry)
    {
        _telemetry = telemetry;
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        var actionName = context.ActionDescriptor.DisplayName ?? "UnknownAction";
        var httpMethod = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;

        var telemetryContext = new Dictionary<string, object>
        {
            ["Action"] = actionName,
            ["Method"] = httpMethod,
            ["Path"] = path
        };

        try
        {
            var resultContext = await next();
            stopwatch.Stop();

            telemetryContext["Duration"] = stopwatch.Elapsed.TotalMilliseconds;
            telemetryContext["StatusCode"] = context.HttpContext.Response.StatusCode;

            if (resultContext.Exception != null && !resultContext.ExceptionHandled)
            {
                _telemetry.Log(LogLevel.Error, resultContext.Exception, telemetryContext);
                _telemetry.TrackEvent($"Http.{actionName}.Failure", telemetryContext);
            }
            else
            {
                _telemetry.TrackMetric($"Http.{actionName}.Latency", stopwatch.Elapsed.TotalMilliseconds, telemetryContext);
                _telemetry.TrackEvent($"Http.{actionName}.Success", telemetryContext);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            telemetryContext["Duration"] = stopwatch.Elapsed.TotalMilliseconds;
            _telemetry.Log(LogLevel.Error, ex, telemetryContext);
            throw;
        }
    }
}
