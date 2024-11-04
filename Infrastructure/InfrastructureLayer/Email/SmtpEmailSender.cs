using System.Net.Mail;
using LibraryCore.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfrastructureLayer.Email
{
   public class SmtpEmailSender(ILogger<SmtpEmailSender> logger,
                          IOptions<MailServerConfiguration> mailserverOptions) : IEmailSender
   {
      private readonly ILogger<SmtpEmailSender> _logger = logger;
      private readonly MailServerConfiguration _mailserverConfiguration = mailserverOptions.Value!;

      /// <summary>
      /// Send email
      /// </summary>
      /// <param name="to"></param>
      /// <param name="from"></param>
      /// <param name="subject"></param>
      /// <param name="body"></param>
      /// <returns>True if email has been queued or false if SMTP client returned exception.</returns>
      public async Task<bool> SendEmailAsync(string to, string from, string subject, string body)
      {
         bool success = false;
         var emailClient = new System.Net.Mail.SmtpClient(_mailserverConfiguration.Hostname, _mailserverConfiguration.Port);

         var message = new MailMessage
         {
            From = new MailAddress(from),
            Subject = subject,
            Body = body
         };
         message.To.Add(new MailAddress(to));

         try {
            await emailClient.SendMailAsync(message);
            _logger.LogWarning("Sending email to {to} from {from} with subject {subject} using {type}.", to, from, subject, this.ToString());
				success = true;
			}
         catch(Exception e)
         {
            _logger.LogError(e.Message, e);
         }
         return success;
		}
   }
}