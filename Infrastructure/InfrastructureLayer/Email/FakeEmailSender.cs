using LibraryCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.Email
{
	/// <summary>
	/// Fake email service, should be named Stub, following this vocabulary: https://martinfowler.com/bliki/TestDouble.html
	/// </summary>
	/// <param name="logger"></param>
	public class FakeEmailSender(ILogger<FakeEmailSender> logger) : IEmailSender
   {
      private readonly ILogger<FakeEmailSender> _logger = logger;
      public Task<bool> SendEmailAsync(string to, string from, string subject, string body)
      {
         _logger.LogInformation("Not actually sending an email to {to} from {from} with subject {subject}", to, from, subject);
         return Task.FromResult(true);
      }
   }
}
