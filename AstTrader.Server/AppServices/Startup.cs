using AstTrader.DbSeeder.Utils;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using MongoDB.Driver;
using AstTrader.Database.DbSetup;

namespace AstTrader.Server.AppServices
{
    internal class Startup
    {
        private ConfigurationManager _configuration;
        private AppConnectionStrings _connStrings = null!;

        public Startup(ConfigurationManager configuration)
        {
            _configuration = configuration;
            _connStrings = new AppConnectionStrings(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //database
            var mongoClient = services.AddDbService(_connStrings);

            //hangire
            services.AddMyHangfire(mongoClient, _connStrings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
        }
    }
}
