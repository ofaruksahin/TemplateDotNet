namespace TemplateDotNet.Application.Api.Infrastructure.Persistence.Configurations
{
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .Property(property => property.Id)
                .IsRequired();

            builder
                .Property(property => property.Created)
                .IsRequired();

            builder
                .Property(property => property.CreatedBy)
                .IsRequired();

            builder
                .Property(property => property.LastModified);

            builder
                .Property(property => property.LastModifiedBy);

            builder
                .Property(property => property.IsDeleted);
        }
    }
}

