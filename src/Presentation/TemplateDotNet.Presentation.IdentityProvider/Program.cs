using System.Diagnostics;
using System.Reflection;
using TemplateDotNet.Application.IdentityProvider;
using TemplateDotNet.Common;
using TemplateDotNet.Common.ConfigurationManagers;
using TemplateDotNet.Common.Modules;
using TemplateDotNet.Common.Options;

var builder = WebApplication.CreateBuilder(args);

var appAssemblies = new Assembly[]
{
    typeof(CommonAssemblyLoader).Assembly,
    typeof(ApplicationAssemblyLoader).Assembly
};

builder.AddVaultProvider();
builder.AddOptions(appAssemblies);
await builder.AddModules(appAssemblies);

builder.Services.AddControllers();
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

WebApplication app = null;

try
{
    app = builder.Build();
    await app.UseModules(appAssemblies);
}
catch (Exception ex)
{
    Debug.WriteLine(ex);
}

app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
