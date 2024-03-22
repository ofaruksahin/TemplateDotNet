namespace TemplateDotNet.Application.Api.Infrastructure.Persistence.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
	{
        private readonly ICurrentUserService _currentUser;

        public AuditInterceptor(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
                return await base.SavingChangesAsync(eventData, result,cancellationToken);

            var now = DateTime.Now;

            foreach (var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = now;
                        entry.Entity.CreatedBy = _currentUser.Email;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = now;
                        entry.Entity.LastModifiedBy = _currentUser.Email;
                        break;
                }
            }
            
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

