namespace Ordering.Application.Models
{
  public class EmailSettings
  {
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public string FromAddress { get; set; }
    public string FromName { get; set; }
  }
}