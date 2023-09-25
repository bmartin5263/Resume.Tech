using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Identities.Duende;
using ResumeTech.Persistence.EntityFramework.Converter;
using UserClaim = ResumeTech.Identities.Duende.UserClaim;

namespace ResumeTech.Persistence.EntityFramework;

public class EFCoreContext : 
    IdentityDbContext<User, Role, UserId, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
    IPersistedGrantDbContext 
{
    public DbSet<Key> Keys { get; set; } = default!;
    public DbSet<PersistedGrant> PersistedGrants { get; set; } = default!;
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; } = default!;

    private IOptions<OperationalStoreOptions> OperationalStoreOptions { get; }

    public EFCoreContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) 
        : base(options) 
    {
        OperationalStoreOptions = operationalStoreOptions;
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.ConfigurePersistedGrantContext(OperationalStoreOptions.Value);
        
        // Table Setups
        builder.Entity<User>().ToTable("User");
        builder.Entity<Role>().ToTable("Role");
        builder.Entity<RoleClaim>().ToTable("RoleClaim");
        builder.Entity<UserClaim>().ToTable("UserClaim");
        builder.Entity<UserLogin>().ToTable("UserLogin");
        builder.Entity<UserRole>().ToTable("UserRole");
        builder.Entity<UserToken>().ToTable("UserToken");

        // builder.Entity<DateOnlyRange>().HasNoKey();
        
        builder.DefineTable<Job>();
        builder.OneToMany<Job, Position, JobId>(u => u.Positions).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Job>().HasJsonConversion(s => s.Location).HasDefaultValueSql("'{}'::jsonb");

        builder.DefineTable<Position>();
        builder.Entity<Position>().HasJsonConversion(s => s.BulletPoints);
        builder.Entity<Position>().OwnsOne(
            o => o.Dates,
            sa =>
            {
                sa.Property(p => p.Start).HasColumnName("StartDate");
                sa.Property(p => p.End).HasColumnName("EndDate");
            });
        // builder.Entity<Position>().HasJsonConversion(s => s.Dates);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);

        foreach (var (wrapper, wrapee) in WrapperUtils.FindAllWrappedTypes("ResumeTech")) {
            var converter = typeof(WrapperConverter<,>).MakeGenericType(wrapper, wrapee);
            builder.Properties(wrapper)
                .HaveConversion(converter);
        }
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        foreach (var entity in ChangeTracker.Entries()) {
            if (entity.Entity is not IAuditedEntity auditedEntity) {
                continue;
            }
            switch (entity.State) {
                case EntityState.Added:
                    auditedEntity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    auditedEntity.UpdatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges() {
        foreach (var entity in ChangeTracker.Entries()) {
            if (entity.Entity is not IAuditedEntity auditedEntity) {
                continue;
            }
            switch (entity.State) {
                case EntityState.Added:
                    auditedEntity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    auditedEntity.UpdatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }
        return base.SaveChanges();
    }
}