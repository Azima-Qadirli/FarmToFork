using System.Net;
using System.Net.Mail;

namespace FarmToFork.Services;

public class MailService:IMailService
{
    public void SendMail(string to, string subject, string body)
    {
        string from = "azimaqadirli@gmail.com";
        MailMessage mail = new MailMessage();
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.From = new MailAddress(from);
        
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.Port = 587;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(from, "tzwk wwlk ztcf udhz");
        smtpClient.EnableSsl = true;
        
        smtpClient.Send(mail);
    }
}