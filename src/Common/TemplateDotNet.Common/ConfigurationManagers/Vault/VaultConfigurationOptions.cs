namespace TemplateDotNet.Common.ConfigurationManagers.Vault
{
    public class VaultConfigurationOptions : IVaultConfigurationOptions
    {
        public string Key => "TemplateDotNet:ConfigurationProviders:Vault";

        public string Address { get; set; }
        public string Token { get; set; }
        public string Path { get; set; }
    }
}
