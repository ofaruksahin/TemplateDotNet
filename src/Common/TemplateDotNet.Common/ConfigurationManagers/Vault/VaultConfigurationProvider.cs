using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;
using Vault;
using Vault.Client;

namespace TemplateDotNet.Common.ConfigurationManagers.Vault
{
    public class VaultConfigurationProvider : ConfigurationProvider
    {
        private readonly VaultConfigurationOptions _vaultOptions;

        public VaultConfigurationProvider(VaultConfigurationOptions vaultOptions)
        {
            _vaultOptions = vaultOptions;
        }

        public override void Load()
        {
            try
            {
                var clientConfig = new VaultConfiguration(_vaultOptions.Address);
                var client = new VaultClient(clientConfig);
                client.SetToken(_vaultOptions.Token);

                var secretKeys = client.Secrets.KvV2List(string.Empty, _vaultOptions.Path);

                foreach (var secretKey in secretKeys.Data.Keys)
                {
                    var kv = client.Secrets.KvV2Read(secretKey, _vaultOptions.Path);
                    var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(kv.Data.Data.ToString());

                    foreach (var keyValuePair in dictionary)
                        Data.Add(string.Concat(secretKey, ":", keyValuePair.Key), keyValuePair.Value.ToString());
                }
            }
            catch (VaultApiException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
