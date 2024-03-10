using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateDotNet.Common.ConfigurationManagers.Vault;

namespace TemplateDotNet.Common.ConfigurationManagers
{
    public static class ConfigurationManagerExtensions
    {
        public static WebApplicationBuilder AddVaultProvider(this WebApplicationBuilder @this)
        {
            var options = new VaultConfigurationOptions();
            @this.Configuration.GetSection(options.Key).Bind(options);
            @this.Services.AddSingleton(options);

            IConfigurationBuilder configurationBuilder = @this.Configuration;
            configurationBuilder.Add(new VaultConfigurationProviderSource(options));

            return @this;
        }
    }
}
