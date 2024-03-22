namespace TemplateDotNet.Application.Api.Infrastructure.Persistence.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
	{
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if(eventData.Context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach(var entry in eventData.Context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State != EntityState.Deleted) continue;

                entry.Entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

