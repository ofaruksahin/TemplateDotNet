using TemplateDotNet.Common.Options;

namespace TemplateDotNet.Application.IdentityProvider.Options
{
    public class OpenIdDictServerOptions : IOption
    {
        public string Key => "OpenIdDictServerOptions";

        public string AuthorizationEndpointUris { get; set; }
        public string LogoutEndpointUris { get; set; }
        public string TokenEndpointUris { get; set; }
        public string UserInfoEndpointUris { get; set; }
    }
}
