using EmailService.Options;
using Microsoft.Extensions.Options;

namespace EmailService.OptionsSetUp;

public class EmailSettingsOptionsSetUp
    : IConfigureOptions<EmailSettingOptions>
{
    private readonly IConfiguration _configuration;

    public EmailSettingsOptionsSetUp(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EmailSettingOptions options)
    {
        _configuration.GetSection(nameof(EmailSettingOptions))
            .Bind(options);
    }
}
