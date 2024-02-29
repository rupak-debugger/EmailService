using EmailService.Abstractions;
using EmailService.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Implementations;

public class MyEmailService
    : IEmailService
{
    private readonly EmailSettingOptions _emailSetting;

    public MyEmailService(IOptions<EmailSettingOptions> emailSetting)
    {
        _emailSetting = emailSetting.Value;
    }

    public void SendEmail()
    {
        MimeMessage email = new();
        email.From.Add(MailboxAddress.Parse(_emailSetting.Mail));
        email.To.Add(MailboxAddress.Parse("thora.barton@ethereal.email"));
        email.Subject = "email subject";

        BodyBuilder bodyBuilder = new();
        bodyBuilder.HtmlBody = "<div style=\"background-color:#000000;padding:1em;width:50%\">\r\n<center><img src='_imageSource' /></center>\r\n</div>";
        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("_imageSource", "https://assets.bootstrapemail.com/logos/light/square.png");
        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailSetting.Mail, _emailSetting.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }

}
