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
using ResumeTech.Experiences.Educations;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Profiles;
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
        
        builder.DefineTable<Profile>();
        builder.OneToMany<Profile, ContactInfo, ProfileId>(u => u.ContactInfos).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.OneToMany<Profile, Education, ProfileId>(u => u.Educations).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.OneToMany<Profile, Job, ProfileId>(u => u.Jobs).IsRequired().OnDelete(DeleteBehavior.Cascade);
        
        builder.DefineTable<ContactInfo>();
        builder.Entity<ContactInfo>().HasJsonConversion(s => s.Links).HasDefaultValueSql("'[]'::jsonb");
        builder.Entity<ContactInfo>().OwnsOne<Location>(
            o => o.Location,
            sa =>
            {
                sa.Property(p => p.City).HasColumnName("City");
                sa.Property(p => p.State).HasColumnName("State");
                sa.Property(p => p.Country).HasColumnName("Country");
            });
        builder.Entity<ContactInfo>().OwnsOne<PersonName>(
            o => o.Name,
            sa =>
            {
                sa.Property(p => p.FirstName).HasColumnName("FirstName");
                sa.Property(p => p.MiddleName).HasColumnName("MiddleName");
                sa.Property(p => p.LastName).HasColumnName("LastName");
            });
        
        builder.DefineTable<Job>();
        builder.OneToMany<Job, Position, JobId>(u => u.Positions).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Job>().OwnsOne<Location>(
            o => o.Location,
            sa =>
            {
                sa.Property(p => p.City).HasColumnName("City");
                sa.Property(p => p.State).HasColumnName("State");
                sa.Property(p => p.Country).HasColumnName("Country");
            });

        builder.DefineTable<Position>();
        builder.Entity<Position>().HasJsonConversion(s => s.BulletPoints).HasDefaultValueSql("'[]'::jsonb");
        builder.Entity<Position>().OwnsOne<DateOnlyRange>(
            o => o.Dates,
            sa =>
            {
                sa.Property(p => p.Start).HasColumnName("StartDate");
                sa.Property(p => p.End).HasColumnName("EndDate");
            });
        
        builder.DefineTable<Education>();
        builder.Entity<Education>().HasJsonConversion(s => s.BulletPoints);
        builder.Entity<Education>().OwnsOne<Gpa>(
            o => o.Gpa,
            sa =>
            {
                sa.Property(p => p.Scale).HasColumnName("GpaScale");
                sa.Property(p => p.Value).HasColumnName("GpaValue");
            });
        builder.Entity<Education>().OwnsOne<Location>(
            o => o.Location,
            sa =>
            {
                sa.Property(p => p.City).HasColumnName("City");
                sa.Property(p => p.State).HasColumnName("State");
                sa.Property(p => p.Country).HasColumnName("Country");
            });
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        base.ConfigureConventions(builder);

        foreach (var (wrapper, wrapee) in WrapperUtils.FindAllWrappedTypes("ResumeTech")) {
            var converter = typeof(WrapperConverter<,>).MakeGenericType(wrapper, wrapee);
            builder.Properties(wrapper).HaveConversion(converter);
        }
        
        builder.Properties<Enum>().HaveConversion<string>();
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