using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Json;
using ResumeTech.Persistence.EntityFramework.Converter;

namespace ResumeTech.Persistence.EntityFramework; 

public static class DatabaseBuilderUtils {
    
    public static void DefineTable<E>(this ModelBuilder builder) where E : class {
        builder.DefineTable<E>(typeof(E).Name);
    }
    
    public static void DefineTable<E>(this ModelBuilder builder, string tableName) where E : class {
        builder.Entity<E>().ToTable(tableName);
        builder.Entity<E>()
            .HasKey("Id");
        builder.Entity<E>()
            .Property("Id")
            .HasColumnName(tableName + "Id");
    }
    
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) {
        ValueComparer<T?> comparer = new ValueComparer<T?>
        (
            (l, r) => IJsonParser.Default.Write(l) == IJsonParser.Default.Write(r),
            v => v == null ? 0 : IJsonParser.Default.Write(v).GetHashCode(),
            v => IJsonParser.Default.ReadOrElse(IJsonParser.Default.Write(v), default(T))
        );

        propertyBuilder.HasConversion<JsonDbConverter<T>>();
        propertyBuilder.Metadata.SetValueComparer(comparer);
        propertyBuilder.HasColumnType("jsonb");

        return propertyBuilder;
    }
    
    public static PropertyBuilder<TProperty> HasJsonConversion<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> entityBuilder, 
        Expression<Func<TEntity, TProperty>> propertyExpression
    ) 
        where TEntity : class {
        var propertyBuilder = entityBuilder.Property(propertyExpression);
        
        ValueComparer<TProperty?> comparer = new ValueComparer<TProperty?>
        (
            (l, r) => IJsonParser.Default.Write(l) == IJsonParser.Default.Write(r),
            v => v == null ? 0 : IJsonParser.Default.Write(v).GetHashCode(),
            v => IJsonParser.Default.ReadOrElse(IJsonParser.Default.Write(v), default(TProperty))
        );

        propertyBuilder.HasConversion<JsonDbConverter<TProperty>>();
        propertyBuilder.Metadata.SetValueComparer(comparer);
        propertyBuilder.HasColumnType("jsonb");

        return propertyBuilder;
    }
    
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, PKey>(
        this ModelBuilder builder, 
        string foreignKey,
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) where P : class where C : class {
        builder.Entity<C>().Property<PKey>(foreignKey);
        return builder.Entity<P>()
            .HasMany(navigationExpression)
            .WithOne()
            .HasForeignKey(foreignKey);
    }
    
    public static OwnershipBuilder<P, C> OneToManyOwning<P, C, PKey>(
        this ModelBuilder builder, 
        string foreignKey,
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) where P : class where C : class {
        builder.Entity<C>().Property<PKey>(foreignKey);
        return builder.Entity<P>()
            .OwnsMany(navigationExpression)
            .WithOwner()
            .HasForeignKey(foreignKey);
    }
    
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, PId>(
        this ModelBuilder builder, 
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) 
        where P : class, IEntity<PId>
        where PId : IEntityId
        where C : class 
    {
        var foreignKey = typeof(PId).Name;
        builder.Entity<C>().Property<PId>(foreignKey);
        return builder.Entity<P>()
            .HasMany(navigationExpression)
            .WithOne()
            .HasForeignKey(foreignKey);
    }
    
    // No navigation property
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, PId>(
        this ModelBuilder builder
    ) 
        where P : class, IEntity<PId>
        where PId : IEntityId
        where C : class 
    {
        var foreignKey = typeof(PId).Name;
        return builder.OneToMany<P, C, PId>(foreignKey);
    }
    
    // No navigation property
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, PId>(
        this ModelBuilder builder,
        string foreignKey
    ) 
        where P : class, IEntity<PId>
        where PId : IEntityId
        where C : class 
    {
        builder.Entity<C>().Property<PId>(foreignKey);
        return builder.Entity<P>()
            .HasMany<C>()
            .WithOne()
            .HasForeignKey(foreignKey);
    }
    
    public static OwnershipBuilder<P, C> OneToManyOwning<P, C, PId>(
        this ModelBuilder builder, 
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) 
        where P : class, IEntity<PId>
        where PId : IEntityId
        where C : class 
    {
        var foreignKey = typeof(PId).Name;
        builder.Entity<C>().Property<PId>(foreignKey);
        return builder.Entity<P>()
            .OwnsMany(navigationExpression)
            .WithOwner()
            .HasForeignKey(foreignKey);
    }
    
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, PId>(
        this ModelBuilder builder, 
        Expression<Func<C, object?>> foreignKeyExpression,
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) 
        where P : class, IEntity<PId>
        where PId : IEntityId
        where C : class 
    {
        var foreignKey = typeof(PId).Name;
        builder.Entity<C>().Property<PId>(foreignKey);
        return builder.Entity<P>()
            .HasMany(navigationExpression)
            .WithOne()
            .HasForeignKey(foreignKeyExpression);
    }
    
    public static OwnershipBuilder<P, C> OneToManyOwning<P, C, PId>(
        this ModelBuilder builder, 
        Expression<Func<C, object?>> foreignKeyExpression,
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) 
        where P : class, IEntity<PId>
        where PId : IEntityId
        where C : class 
    {
        var foreignKey = typeof(PId).Name;
        builder.Entity<C>().Property<PId>(foreignKey);
        return builder.Entity<P>()
            .OwnsMany(navigationExpression)
            .WithOwner()
            .HasForeignKey(foreignKeyExpression);
    }
}