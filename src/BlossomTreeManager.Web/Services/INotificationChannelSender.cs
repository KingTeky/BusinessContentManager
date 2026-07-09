using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Entities;

namespace BlossomTreeManager.Web.Services;

public interface INotificationChannelSender
{
    Task SendAsync(NotificationMessage notification, ApplicationUser receiverUser, CancellationToken cancellationToken);
}
