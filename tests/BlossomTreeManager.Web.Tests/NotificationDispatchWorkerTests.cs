using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Entities;
using BlossomTreeManager.Web.Data.Enums;
using BlossomTreeManager.Web.Services;

namespace BlossomTreeManager.Web.Tests;

public class NotificationDispatchWorkerTests
{
    [Fact]
    public async Task DispatchPendingAsync_MarksPendingAsSent_WhenSenderSucceeds()
    {
        await using var provider = BuildProvider(new SuccessfulSender());
        var notificationId = await SeedPendingNotificationAsync(provider);

        var worker = CreateWorker(provider);
        await worker.DispatchPendingAsync(CancellationToken.None);

        await using var scope = provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var notification = await dbContext.Notifications.SingleAsync(x => x.Id == notificationId);
        Assert.Equal(NotificationStatus.Sent, notification.Status);
        Assert.NotNull(notification.SentAtUtc);
    }

    [Fact]
    public async Task DispatchPendingAsync_MarksPendingAsFailed_WhenSenderThrows()
    {
        await using var provider = BuildProvider(new FailingSender());
        var notificationId = await SeedPendingNotificationAsync(provider);

        var worker = CreateWorker(provider);
        await worker.DispatchPendingAsync(CancellationToken.None);

        await using var scope = provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var notification = await dbContext.Notifications.SingleAsync(x => x.Id == notificationId);
        Assert.Equal(NotificationStatus.Failed, notification.Status);
        Assert.Null(notification.SentAtUtc);
    }

    private static ServiceProvider BuildProvider(INotificationChannelSender sender)
    {
        var services = new ServiceCollection();
        var databaseName = $"notification-worker-{Guid.NewGuid()}";

        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        services.AddScoped<INotificationChannelSender>(_ => sender);

        return services.BuildServiceProvider();
    }

    private static NotificationDispatchWorker CreateWorker(ServiceProvider provider)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Notifications:DispatchIntervalSeconds"] = "1"
            })
            .Build();

        return new NotificationDispatchWorker(
            provider.GetRequiredService<IServiceScopeFactory>(),
            configuration,
            NullLogger<NotificationDispatchWorker>.Instance);
    }

    private static async Task<Guid> SeedPendingNotificationAsync(ServiceProvider provider)
    {
        var notificationId = Guid.NewGuid();
        var receiverId = Guid.NewGuid().ToString();

        await using var scope = provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Users.Add(new ApplicationUser
        {
            Id = receiverId,
            UserName = "receiver@test.local",
            Email = "receiver@test.local",
            FullName = "Receiver",
            IsActive = true,
            EmailConfirmed = true
        });

        dbContext.Notifications.Add(new NotificationMessage
        {
            Id = notificationId,
            ReceiverUserId = receiverId,
            Message = "Hello",
            Channel = NotificationChannel.Email,
            Status = NotificationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        return notificationId;
    }

    private sealed class SuccessfulSender : INotificationChannelSender
    {
        public Task SendAsync(NotificationMessage notification, ApplicationUser receiverUser, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FailingSender : INotificationChannelSender
    {
        public Task SendAsync(NotificationMessage notification, ApplicationUser receiverUser, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Sender failure");
        }
    }
}
