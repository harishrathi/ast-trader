using Hangfire;
using AstTrader.Server.AppServices;

namespace AstTrader.Server;

// this is a startup class for the .net core application server api
internal class Startup
{
    private AppConnectionString _connStrings = null!;
    public Startup(ConfigurationManager configuration)
    {
        _connStrings = AppConnectionString.Init(configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //add sql server database
        

        //database
        var mongoClient = services.AddDbService(_connStrings);

        //hangire
        services.AddMyHangfire(mongoClient, _connStrings);
    }

    public void Configure(IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        ArgumentNullException.ThrowIfNull(env, nameof(env));

        app.UseDefaultFiles(); // for index.html
        app.UseStaticFiles();

        //swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        //hangfire
        app.UseHangfireDashboard();
        if (env.IsDevelopment())
        {
            var backgroundJobs = app.ApplicationServices.GetService<IBackgroundJobClient>();
            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHangfireDashboard();
            endpoints.MapFallbackToFile("/index.html");
        });

        AppLifetimeEvents.RegisterAppLifetimeEvents(app);

    }
}
