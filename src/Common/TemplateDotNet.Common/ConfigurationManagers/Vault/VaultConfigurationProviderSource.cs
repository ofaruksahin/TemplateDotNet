using Microsoft.Extensions.Configuration;

namespace TemplateDotNet.Common.ConfigurationManagers.Vault
{
    public class VaultConfigurationProviderSource : IConfigurationSource
    {
        private readonly VaultConfigurationOptions _vaultConfigurationOptions;

        public VaultConfigurationProviderSource(VaultConfigurationOptions vaultConfigurationOptions)
        {
            _vaultConfigurationOptions = vaultConfigurationOptions;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new VaultConfigurationProvider(_vaultConfigurationOptions);
        }
    }
}
