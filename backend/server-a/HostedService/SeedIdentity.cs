using Microsoft.AspNetCore.Identity;
using server_a.Data.Database;

namespace server_a.HostedService;

public class SeedIdentity(IServiceProvider serviceProvider, ILogger<SeedIdentity> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<IdentityUser>>();
        var emailStore = (IUserEmailStore<IdentityUser>)userStore;

        var email = "admin@localhost";
        var username = "admin";
        var user = new IdentityUser();
        await userStore.SetUserNameAsync(user, username, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, "admin123");
        logger.LogInformation("User creation result: {result}", result.ToString());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

