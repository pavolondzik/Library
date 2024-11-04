namespace LibraryCore.Interfaces
{
   public interface IEmailSender
   {
      Task<bool> SendEmailAsync(string to, string from, string subject, string body);
   }
}
