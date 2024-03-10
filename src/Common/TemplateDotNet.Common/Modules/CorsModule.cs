using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateDotNet.Common.Modules
{
    public class CorsModule : IModule
    {
        public Task AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });

            return Task.CompletedTask;
        }

        public Task UseServices(IApplicationBuilder app)
        {
            app.UseCors();

            return Task.CompletedTask;
        }
    }
}
