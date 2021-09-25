using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
      public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
      {
        services.AddDbContext<OrderContext>(option => option.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.Configure<EmailSettings>(c =>
        {
          var emailSettings = config.GetSection("EmailSettings");
          c.PrivateKey = emailSettings["PrivateKey"];
          c.PublicKey = emailSettings["PublicKey"];
          c.FromAddress = emailSettings["FromAddress"];
          c.FromName = emailSettings["FromName"];
        });
        services.AddTransient<IEmailService, EmailService>();
        return services;
      }
    }
}
