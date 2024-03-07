using Microsoft.Extensions.Configuration;

namespace TemplateDotNet.Common.ConfigurationManagers.Vault
{
    public static class VaultConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddVaultConfiguration(this IConfigurationBuilder builder, VaultConfigurationOptions configurationOptions)
        {
            builder.Add(new VaultConfigurationProviderSource(configurationOptions));

            return builder;
        }
    }
}
