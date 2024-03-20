using Hangfire;

namespace AstTrader.Server.AppServices;

public class AppLifetimeEvents
{
    public static void RegisterAppLifetimeEvents(IApplicationBuilder app)
    {
        var events = new AppLifetimeEvents(app);
        events.Register();
    }

    private readonly IApplicationBuilder app;
    public AppLifetimeEvents(IApplicationBuilder app)
    {
        this.app = app;
    }

    private void Register()
    {
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        ArgumentNullException.ThrowIfNull(lifetime, nameof(lifetime));

        lifetime?.ApplicationStarted.Register(OnApplicationStarted);
        lifetime?.ApplicationStopped.Register(OnApplicationStopped);
    }

    private void OnApplicationStarted()
    {
        Console.WriteLine("Application started");

        var recurringJobs = app.ApplicationServices.GetService<IRecurringJobManager>();
        recurringJobs.AddOrUpdate<SyncStockInfoJob>("Sync Stock Info", x => x.Run(), Cron.Minutely);
    }

    private void OnApplicationStopped()
    {
        Console.WriteLine("Application stopped");
    }
}

internal class SyncStockInfoJob
{
    public SyncStockInfoJob()
    {
        
    }

    public void Run()
    {

    }
}
