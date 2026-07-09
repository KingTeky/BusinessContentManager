using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Entities;

namespace BlossomTreeManager.Web.Services;

public class LoggingNotificationChannelSender(ILogger<LoggingNotificationChannelSender> logger) : INotificationChannelSender
{
    public Task SendAsync(NotificationMessage notification, ApplicationUser receiverUser, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Dispatching notification {NotificationId} via {Channel} to user {UserId} ({Email})",
            notification.Id,
            notification.Channel,
            receiverUser.Id,
            receiverUser.Email);

        return Task.CompletedTask;
    }
}
