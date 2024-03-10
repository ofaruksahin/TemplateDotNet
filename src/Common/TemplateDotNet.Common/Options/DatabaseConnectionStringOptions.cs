namespace TemplateDotNet.Common.Options
{
    public class DatabaseConnectionStringOptions : IOption
    {
        public string Key => "DatabaseConnectionStringOptions";
        public string IdentityProviderDbContext { get; set; }
    }
}
