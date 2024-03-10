using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using TemplateDotNet.Application.IdentityProvider.Options;
using TemplateDotNet.Common.Modules;
using TemplateDotNet.Common.Options;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace TemplateDotNet.Application.IdentityProvider.Modules
{
    public class OpenIdDictModule : IModule
    {
        public async Task AddServices(WebApplicationBuilder builder)
        {
            var databaseConnectionStringOptions = builder.GetOptions<DatabaseConnectionStringOptions>();
            var openIdDictGithubProviderOptions = builder.GetOptions<OpenIdDictGithubProviderOptions>();
            var openIdDictServerOptions = builder.GetOptions<OpenIdDictServerOptions>();

            builder
                .Services
                .AddDbContext<IdentityProviderDbContext>(options =>
                {
                    options.UseMySQL(databaseConnectionStringOptions.IdentityProviderDbContext);
                    options.UseOpenIddict();
                });

            builder.Services.AddOpenIddict()
                .AddCore(options =>
                {
                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<IdentityProviderDbContext>();
                }).AddClient(options =>
                {
                    options.AllowAuthorizationCodeFlow();
                    options.AddDevelopmentEncryptionCertificate();
                    options.AddDevelopmentSigningCertificate();

                    options.UseSystemNetHttp();

                    options.UseWebProviders()
                        .AddGitHub(providerOptions =>
                        {
                            providerOptions.SetClientId(openIdDictGithubProviderOptions.ClientId);
                            providerOptions.SetClientSecret(openIdDictGithubProviderOptions.ClientSecret);
                            providerOptions.SetRedirectUri(openIdDictGithubProviderOptions.RedirectUri);
                        });
                }).AddServer(options =>
                {
                    options
                        .SetAuthorizationEndpointUris(openIdDictServerOptions.AuthorizationEndpointUris)
                        .SetTokenEndpointUris(openIdDictServerOptions.TokenEndpointUris)
                        .SetUserinfoEndpointUris(openIdDictServerOptions.UserInfoEndpointUris)
                        .SetLogoutEndpointUris(openIdDictServerOptions.LogoutEndpointUris);

                    options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                    options.AllowAuthorizationCodeFlow()
                        .AllowRefreshTokenFlow();

                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                });
        }

        public async Task UseServices(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityProviderDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            var openIdDictDefaultClientOptions = scope.ServiceProvider.GetRequiredService<OpenIdDictDefaultClientOptions>();
            if (await manager.FindByClientIdAsync(openIdDictDefaultClientOptions.ClientId) is null)
            {
                var defaultClient = new OpenIddictApplicationDescriptor();
                defaultClient.ClientId = openIdDictDefaultClientOptions.ClientId;
                defaultClient.ConsentType = ConsentTypes.Explicit;
                defaultClient.DisplayName = openIdDictDefaultClientOptions.DisplayName;
                defaultClient.ClientType = ClientTypes.Public;

                if (!string.IsNullOrEmpty(openIdDictDefaultClientOptions.PostLogoutRedirectUri))
                {
                    defaultClient.PostLogoutRedirectUris.Add(new Uri(openIdDictDefaultClientOptions.PostLogoutRedirectUri));
                }

                if (!string.IsNullOrEmpty(openIdDictDefaultClientOptions.RedirectUri))
                {
                    defaultClient.RedirectUris.Add(new Uri(openIdDictDefaultClientOptions.RedirectUri));
                }

                defaultClient.Permissions.Add(Permissions.Endpoints.Authorization);
                defaultClient.Permissions.Add(Permissions.Endpoints.Logout);
                defaultClient.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
                defaultClient.Permissions.Add(Permissions.GrantTypes.RefreshToken);
                defaultClient.Permissions.Add(Permissions.ResponseTypes.Code);
                defaultClient.Permissions.Add(Permissions.Scopes.Email);
                defaultClient.Permissions.Add(Permissions.Scopes.Profile);
                defaultClient.Permissions.Add(Permissions.Scopes.Roles);

                defaultClient.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);

                await manager.CreateAsync(defaultClient);
            }
        }
    }
}
