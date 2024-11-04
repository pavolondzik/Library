namespace InfrastructureLayer.Email
{
   public class MailServerConfiguration
   {
      public string Hostname { get; set; } = "localhost";
      public int Port { get; set; } = 25;
   }
}
