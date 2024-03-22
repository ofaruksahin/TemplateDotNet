namespace TemplateDotNet.Application.Api.Infrastructure.Persistence
{
    public class TemplateDotNetDbContext : DbContext
	{
        private readonly ICurrentUserService _currentUser;

		public TemplateDotNetDbContext(
            ICurrentUserService currentUser,
            DbContextOptions<TemplateDotNetDbContext> options)
            : base(options)
		{
            _currentUser = currentUser;
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
            optionsBuilder.AddInterceptors(new AuditInterceptor(_currentUser));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateDotNetDbContext).Assembly);
        }
    }
}

