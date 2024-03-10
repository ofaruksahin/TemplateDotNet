using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using TemplateDotNet.Application.IdentityProvider.Options;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace TemplateDotNet.Application.IdentityProvider.Modules.OpenIdDict
{
    public static class OpenIdDictEndpoints
    {
        public static IApplicationBuilder AddOpenIdDictEndpoints(this WebApplication app)
        {
            var openIdDictServerOptions = app.Services.GetRequiredService<OpenIdDictServerOptions>();
            var openIdDictGithubProviderOptions = app.Services.GetRequiredService<OpenIdDictGithubProviderOptions>();

            app.MapMethods(openIdDictGithubProviderOptions.RedirectUri, [HttpMethods.Get, HttpMethods.Post], async (HttpContext context) =>
            {
                var result = await context.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

                var identity = new ClaimsIdentity(
                    authenticationType: "ExternalLogin",
                    nameType: ClaimTypes.Name,
                    roleType: ClaimTypes.Role);

                foreach (var claim in result.Principal.Claims)
                    identity.AddClaim(new Claim(claim.Type, claim.Value));

                var properties = new AuthenticationProperties
                {
                    RedirectUri = result.Properties!.RedirectUri
                };

                return Results.SignIn(new ClaimsPrincipal(identity), properties);
            });

            app.MapGet(openIdDictServerOptions.AuthorizationEndpointUris, async (HttpContext context) =>
            {
                var principal = (await context.AuthenticateAsync())?.Principal;
                if (principal is null)
                {
                    var properties = new AuthenticationProperties
                    {
                        RedirectUri = context.Request.GetEncodedUrl()
                    };

                    return Results.Challenge(properties, [OpenIddictClientAspNetCoreDefaults.AuthenticationScheme]);
                }

                var identifier = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: Claims.Name,
                    roleType: Claims.Role);

                foreach (var claim in principal.Claims)
                    identity.AddClaim(new Claim(claim.Type, claim.Value));

                identity.AddClaim(new Claim(Claims.Subject, identifier));
                identity.AddClaim(new Claim(Claims.Name, identifier).SetDestinations(Destinations.AccessToken));
                identity.AddClaim(new Claim(Claims.PreferredUsername, identifier).SetDestinations(Destinations.AccessToken));

                return Results.SignIn(new ClaimsPrincipal(identity), properties: null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            });

            return app;
        }
    }
}
