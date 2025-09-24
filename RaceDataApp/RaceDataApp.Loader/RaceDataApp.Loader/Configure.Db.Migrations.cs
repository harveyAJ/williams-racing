using RaceDataApp.Loader.Migrations;
using ServiceStack.Data;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(ConfigureDbMigrations))]

namespace RaceDataApp.Loader;

public class ConfigureDbMigrations : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureAppHost(afterAppHostInit: appHost =>
        {
            var migrator = new Migrator(appHost.Resolve<IDbConnectionFactory>(), typeof(Migration1000).Assembly);
            AppTasks.Register("migrate", _ => migrator.Run());
            AppTasks.Register("migrate.revert", args => migrator.Revert(args[0]));
      
            //Run the migrations
            IDbConnectionFactory ResolveDbFactory() => appHost.Resolve<IDbConnectionFactory>();
            Migrator CreateMigrator() => new(ResolveDbFactory(), typeof(Migration1000).Assembly);
            var result = CreateMigrator().Run();
        });
}