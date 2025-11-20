using System.Net;
using System.Net.Mail;

namespace TraSuaFireFox.Helpers
{
    public class SendMailService
    {
        // Cấu hình Email gửi đi (Dùng App Password của Gmail nếu dùng Gmail)
        // Nếu chưa có App Password, hãy search "Cách tạo App Password Gmail"
        private static string _fromEmail = "ngocvcl2005@gmail.com";
        private static string _password = "nczx xcbd qvje zenv";

        public static bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromEmail, _password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "Tra Sua FireFox"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi gửi mail: " + ex.Message);
                return false;
            }
        }
    }
}