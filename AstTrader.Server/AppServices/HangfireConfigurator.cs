using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo;
using Hangfire;
using MongoDB.Driver;
using AstTrader.DbSeeder.Utils;

namespace AstTrader.Server.AppServices;

public static class HangfireConfigurator
{
    public static void AddMyHangfire(this IServiceCollection services, IMongoClient mongoClient, AppConnectionStrings connectionStrings)
    {
        var mongoStorageOptions = new MongoStorageOptions
        {
            CheckConnection = true,
            ByPassMigration = true,
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new NoneMongoBackupStrategy()
            },

            // for development - single node setup
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
        };

        var mongoUrl = new MongoUrlBuilder(connectionStrings.MongoDbConnString).ToMongoUrl();
        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrl.DatabaseName, mongoStorageOptions)
                ;
        });

        services.AddHangfireServer();
    }
}
