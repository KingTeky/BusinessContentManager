using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Entities;
using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Services;

public class NotificationQueueService(ApplicationDbContext dbContext)
{
    public async Task QueueAsync(Guid? schoolId, string? senderUserId, string receiverUserId, string message, NotificationChannel channel)
    {
        var notification = new NotificationMessage
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            SenderUserId = senderUserId,
            ReceiverUserId = receiverUserId,
            Message = message,
            Channel = channel,
            Status = NotificationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> MarkAllPendingAsSentAsync()
    {
        var pending = await dbContext.Notifications
            .Where(x => x.Status == NotificationStatus.Pending)
            .ToListAsync();

        foreach (var item in pending)
        {
            item.Status = NotificationStatus.Sent;
            item.SentAtUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
        return pending.Count;
    }
}
