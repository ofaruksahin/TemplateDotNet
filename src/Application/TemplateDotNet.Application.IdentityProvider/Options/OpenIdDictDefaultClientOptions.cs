using TemplateDotNet.Common.Options;

namespace TemplateDotNet.Application.IdentityProvider.Options
{
    public class OpenIdDictDefaultClientOptions : IOption
    {
        public string Key => "OpenIdDictDefaultClientOptions";

        public string ClientId { get; set; }
        public string DisplayName { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string RedirectUri { get; set; }
    }
}
