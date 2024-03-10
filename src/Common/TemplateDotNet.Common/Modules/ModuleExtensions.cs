using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace TemplateDotNet.Common.Modules
{
    public static class ModuleExtensions
    {
        private static IEnumerable<Type> GetModules(params Assembly[] assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(
                    assembly
                        .GetTypes()
                        .Where(type => type.GetInterface(nameof(IModule)) is not null));
            }

            return types;
        }

        public static async Task<WebApplicationBuilder> AddModules(this WebApplicationBuilder @this,params Assembly[] assemblies)
        {
            var modules = GetModules(assemblies);

            foreach (var module in modules)
            {
                var moduleInstance = (IModule)Activator.CreateInstance(module);
                await moduleInstance.AddServices(@this);
            }

            return @this;
        }

        public static async Task<IApplicationBuilder> UseModules(this IApplicationBuilder @this, params Assembly[] assemblies)
        {
            var modules = GetModules(assemblies);

            foreach (var module in modules)
            {
                var moduleInstance = (IModule)Activator.CreateInstance(module);
                await moduleInstance.UseServices(@this);
            }

            return @this;
        }
    }
}
