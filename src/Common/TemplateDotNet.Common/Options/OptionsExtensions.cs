using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TemplateDotNet.Common.Options
{
    public static class OptionsExtensions
    {
        private static IEnumerable<Type> GetOptions(params Assembly[] assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(
                    assembly
                        .GetTypes()
                        .Where(type => type.GetInterface(nameof(IOption)) is not null));
            }

            return types;
        }

        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder @this,params Assembly[] assemblies)
        {
            var options = GetOptions(assemblies);

            foreach (var option in options)
            {
                var optionInstance = (IOption)Activator.CreateInstance(option);
                @this.Configuration.GetSection(optionInstance.Key).Bind(optionInstance);

                @this.Services.AddSingleton(option, optionInstance);
            }

            return @this;
        }

        public static TOptions GetOptions<TOptions>(this WebApplicationBuilder @this)
            where TOptions : IOption
        {
            var type = typeof(TOptions);
            var instance = (TOptions)Activator.CreateInstance(type);

            @this.Configuration.GetSection(instance.Key).Bind(instance);

            return instance;
        }
    }
}
