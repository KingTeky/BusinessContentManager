using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Services;

public class NotificationDispatchWorker(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<NotificationDispatchWorker> logger) : BackgroundService
{
    private readonly TimeSpan dispatchInterval = ResolveDispatchInterval(configuration);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Notification dispatch worker started. Interval: {DispatchIntervalSeconds}s", dispatchInterval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DispatchPendingAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during notification dispatch cycle");
            }

            await Task.Delay(dispatchInterval, stoppingToken);
        }

        logger.LogInformation("Notification dispatch worker stopped");
    }

    internal async Task DispatchPendingAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var sender = scope.ServiceProvider.GetRequiredService<INotificationChannelSender>();

        var pending = await dbContext.Notifications
            .Where(x => x.Status == NotificationStatus.Pending)
            .Include(x => x.ReceiverUser)
            .OrderBy(x => x.CreatedAtUtc)
            .Take(50)
            .ToListAsync(cancellationToken);

        if (pending.Count == 0)
        {
            return;
        }

        foreach (var notification in pending)
        {
            try
            {
                await sender.SendAsync(notification, notification.ReceiverUser, cancellationToken);
                notification.Status = NotificationStatus.Sent;
                notification.SentAtUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                notification.Status = NotificationStatus.Failed;
                logger.LogError(ex, "Failed to dispatch notification {NotificationId}", notification.Id);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static TimeSpan ResolveDispatchInterval(IConfiguration configuration)
    {
        const int defaultSeconds = 15;

        var configuredSeconds = configuration.GetValue<int?>("Notifications:DispatchIntervalSeconds");
        if (!configuredSeconds.HasValue || configuredSeconds.Value <= 0)
        {
            return TimeSpan.FromSeconds(defaultSeconds);
        }

        return TimeSpan.FromSeconds(configuredSeconds.Value);
    }
}
