var builder = WebApplication.CreateBuilder(args);

var appAssemblies = new Assembly[]
{
    typeof(CommonAssemblyLoader).Assembly,
    typeof(ApplicationAssemblyLoader).Assembly
};

builder.AddVaultProvider();
builder.AddOptions(appAssemblies);
await builder.AddModules(appAssemblies);


WebApplication app = null;

try
{
    app = builder.Build();
    await app.UseModules(appAssemblies);
    app.AddOpenIdDictEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Debug.WriteLine(ex);
}


