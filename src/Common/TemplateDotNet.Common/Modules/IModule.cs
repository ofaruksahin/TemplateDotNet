using Microsoft.AspNetCore.Builder;

namespace TemplateDotNet.Common.Modules
{
    public interface IModule
    {
        Task AddServices(WebApplicationBuilder builder);
        Task UseServices(IApplicationBuilder app);
    }
}
