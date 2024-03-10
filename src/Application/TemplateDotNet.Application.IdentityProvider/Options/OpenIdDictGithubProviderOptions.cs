using TemplateDotNet.Common.Options;

namespace TemplateDotNet.Application.IdentityProvider.Options
{
    public class OpenIdDictGithubProviderOptions : IOption
    {
        public string Key => "OpenIdDictGithubProviderOptions";

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}
