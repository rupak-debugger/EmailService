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
    private readonly IWebHostEnvironment _hostingEnvironment;

    public MyEmailService(
        IOptions<EmailSettingOptions> emailSetting,
        IWebHostEnvironment hostingEnvironment)
    {
        _emailSetting = emailSetting.Value;
        _hostingEnvironment = hostingEnvironment;
    }

    public void SendEmail()
    {
        string templatePath = Path.Combine(_hostingEnvironment.ContentRootPath, "EmailTemplates/email_template.html");
        string htmlBody = File.ReadAllText(templatePath);

        MimeMessage email = new();
        email.From.Add(new MailboxAddress(_emailSetting.DisplayName, _emailSetting.Mail));
        email.To.Add(MailboxAddress.Parse("spencer.stokes@ethereal.email"));
        email.Subject = "email subject";

        BodyBuilder bodyBuilder = new();
        bodyBuilder.HtmlBody = htmlBody;
        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailSetting.Mail, _emailSetting.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }

}
