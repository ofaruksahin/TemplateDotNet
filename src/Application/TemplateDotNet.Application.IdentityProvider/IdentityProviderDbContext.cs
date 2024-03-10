using Microsoft.EntityFrameworkCore;

namespace TemplateDotNet.Application.IdentityProvider
{
    public class IdentityProviderDbContext : DbContext
    {
        public IdentityProviderDbContext(DbContextOptions options)
            :base(options)
        {
            
        }
    }
}
