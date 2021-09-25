using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
  public class OrderContextSeed
  {
    public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
    {
      if (!context.Orders.Any())
      {
        context.Orders.AddRange(GetPreconfiguredOrders());
        await context.SaveChangesAsync();
        logger.LogInformation("Seed database associated with context {DbContextName}", nameof(OrderContext));
      }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
      return new List<Order>
      {
        new()
        {
          UserName = "mnf", FirstName = "Mono", LastName = "Fadhilah", EmailAddress = "halilintar@gmail.com",
          AddressLine = "Penajam", Country = "Indonesia", TotalPrice = 350
        }
      };
    }
  }
}