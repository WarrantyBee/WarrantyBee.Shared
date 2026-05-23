using System.Threading.Channels;
using WarrantyBee.Shared.Infrastructure.Abstractions;

namespace WarrantyBee.Shared.Infrastructure.Services;

/// <summary>
/// Implementation of <see cref="IBackgroundTaskQueue"/> using <see cref="Channel{T}"/>.
/// </summary>
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public BackgroundTaskQueue(int capacity = 1000)
    {
        // Bounded channel to prevent memory exhaustion under extreme load
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public async ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem)
    {
        if (workItem == null) throw new ArgumentNullException(nameof(workItem));
        await _queue.Writer.WriteAsync(workItem);
    }

    /// <summary>
    /// Executes the primary logic.
    /// </summary>
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
