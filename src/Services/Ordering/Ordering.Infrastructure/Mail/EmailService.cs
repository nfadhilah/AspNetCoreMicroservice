using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;

namespace Ordering.Infrastructure.Mail
{
  public class EmailService : IEmailService
  {
    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
      EmailSettings = emailSettings.Value;
      Logger = logger;
    }

    public EmailSettings EmailSettings { get; }
    public ILogger<EmailService> Logger { get; }

    public async Task<bool> SendEmail(Email email)
    {
      MailjetClient client = new MailjetClient(
        EmailSettings.PublicKey,
        EmailSettings.PrivateKey);

      MailjetRequest request = new MailjetRequest
      {
        Resource = Send.Resource,
      };

      // construct your email with builder
      var builder = new TransactionalEmailBuilder()
        .WithFrom(new SendContact(EmailSettings.FromAddress))
        .WithSubject(email.Subject)
        .WithHtmlPart(email.Body)
        .WithTo(new SendContact(email.To))
        .Build();

      // invoke API to send email
      var response = await client.SendTransactionalEmailAsync(builder);
      return response.Messages.Any();
    }
  }
}