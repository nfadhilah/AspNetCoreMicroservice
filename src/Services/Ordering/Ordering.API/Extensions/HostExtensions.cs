using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ordering.API.Extensions
{
  public static class HostExtensions
  {
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder,
      int? retry = 0) where TContext : DbContext
    {
      var retryForAvailability = retry ?? 0;
      using var scope = host.Services.CreateScope();
      var services = scope.ServiceProvider;
      var logger = services.GetRequiredService<ILogger<TContext>>();
      var context = services.GetService<TContext>();
      try
      {
        logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
        InvokeSeeder(seeder, context, services);
        logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
      }
      catch (SqlException e)
      {
        logger.LogError(e, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
        if (retryForAvailability < 50)
        {
          retryForAvailability++;
          Thread.Sleep(2000);
          MigrateDatabase(host, seeder, retryForAvailability);
        }
      }
      return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
      IServiceProvider services) where TContext : DbContext
    {
      context.Database.Migrate();
      seeder(context, services);
    }
  }
}